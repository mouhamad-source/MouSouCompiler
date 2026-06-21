using CompilerMicroservice.Factories;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompilerMicroservice.Services
{
    public class CodeExecutionService : ICodeExecutionService, IDisposable
    {
        private readonly IContainerManager _containerManager;
        private readonly ITempFileManager _tempFileManager;
        private readonly IErrorParser _errorParser;

        public CodeExecutionService(IContainerManager containerManager,
                                    ITempFileManager tempFileManager,
                                    IErrorParser errorParser)
        {
            _containerManager = containerManager;
            _tempFileManager = tempFileManager;
            _errorParser = errorParser;
        }

        public async Task<ExecutionResult> ExecuteAsync(CompileRequest request)
        {
            string workDir = null;
            string containerId = null;
            var stopwatch = Stopwatch.StartNew();
            var result = new ExecutionResult();
            var metadata = new Metadata
            {
                Language = request.Language,
                FilesExecuted = request.Files.Select(f => f.Name).ToList(),
                Timestamp = DateTime.UtcNow,
                Warnings = new List<string>()
            };

            try
            {
                // 1. Get build strategy
                var strategy = BuildStrategyFactory.GetStrategy(request.Language);
                metadata.Language = strategy.GetLanguageName();

                // 2. Prepare temp directory and write all files
                workDir = await _tempFileManager.CreateWorkDirectoryAsync();
                await _tempFileManager.WriteFilesAsync(workDir, request.Files);

                // 3. Language-specific preparation (e.g., no-op for Python, write project for C#)
                await strategy.PrepareAsync(workDir, request.Files, request.EntryPoint);

                // 4. Build command if needed (compilation)
                var buildCmd = strategy.GetBuildCommand();
                if (buildCmd != null)
                {
                    var buildResult = await RunContainerAsync(workDir, strategy.GetImage(), buildCmd,
                                                              request.MemoryLimit, request.TimeoutLimit);
                    if (buildResult.ExitCode != 0)
                    {
                        // Compilation failed
                        result.ExitCode = buildResult.ExitCode;
                        result.Output = buildResult.Output;
                        result.Time = $"{stopwatch.ElapsedMilliseconds}ms";
                        result.Errors = _errorParser.ParseErrors(buildResult.Output, request.Language);
                        metadata.Status = "compile_error";
                        metadata.ContainerId = buildResult.ContainerId;
                        metadata.ResourceUsage = buildResult.Usage;
                        result.Metadata = metadata;
                        return result;
                    }
                    // If build succeeded, we may have produced an executable (e.g., /tmp/Program.exe)
                }

                // 5. Run the code with entry point and args
                var runCmd = strategy.GetRunCommand(request.EntryPoint, request.Args, request.Stdin);
                var runResult = await RunContainerAsync(workDir, strategy.GetImage(), runCmd,
                                                        request.MemoryLimit, request.TimeoutLimit, request.Stdin);

                result.ExitCode = runResult.ExitCode;
                result.Output = runResult.Output;
                result.Time = $"{stopwatch.ElapsedMilliseconds}ms";
                result.Errors = _errorParser.ParseErrors(runResult.Output, request.Language);
                metadata.ContainerId = runResult.ContainerId;
                metadata.ResourceUsage = runResult.Usage;
                metadata.Status = runResult.ExitCode == 0 ? "success" : "runtime_error";
                result.Metadata = metadata;

                return result;
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                result.ExitCode = -1;
                result.Output = "";
                result.Time = $"{stopwatch.ElapsedMilliseconds}ms";
                result.Errors = new List<ErrorDetail>();
                metadata.Status = "timeout";
                metadata.Warnings.Add($"Execution timed out after {request.TimeoutLimit} ms");
                result.Metadata = metadata;
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                result.ExitCode = -1;
                result.Output = "";
                result.Time = $"{stopwatch.ElapsedMilliseconds}ms";
                result.Errors = new List<ErrorDetail>();
                metadata.Status = "internal_error";
                metadata.Warnings.Add($"Internal error: {ex.Message}");
                result.Metadata = metadata;
                return result;
            }
            finally
            {
                if (containerId != null)
                    await _containerManager.RemoveContainerAsync(containerId);
                if (workDir != null)
                    _tempFileManager.DeleteWorkDirectory(workDir);
            }
        }

        private async Task<(string ContainerId, string Output, int  ExitCode, ResourceUsage Usage)>
            RunContainerAsync(string workDir, string image, string[] command, int memoryLimitMB,
                              int timeoutMs, string stdin = null)
        {
            using (var cts = new CancellationTokenSource(timeoutMs))
            {
                var containerId = await _containerManager.CreateContainerAsync(workDir, image, command, memoryLimitMB, cts.Token);
                await _containerManager.StartAndWaitAsync(containerId, cts.Token);
                var output = await _containerManager.GetLogsAsync(containerId);
                var (exitCode, usage) = await _containerManager.GetExitCodeAndResourceUsageAsync(containerId);
                await _containerManager.RemoveContainerAsync(containerId); // immediate cleanup
                return (containerId, output, (int) exitCode, usage);
            }
        }

        public void Dispose()
        {
            (_containerManager as IDisposable)?.Dispose();
        }
    }
}