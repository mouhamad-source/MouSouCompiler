using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.Services
{
    public class DockerContainerManager : IContainerManager, IDisposable
    {
        private readonly DockerClient _client;
        private bool _disposed;

        public DockerContainerManager()
        {
            var uri = new Uri("npipe://./pipe/docker_engine"); // Windows
            // For Linux: new Uri("unix:///var/run/docker.sock")
            _client = new DockerClientConfiguration(uri).CreateClient();
        }

        public async Task<string> CreateContainerAsync(string workDir, string image, string[] command,
                                                       int memoryLimitMB, CancellationToken ct)
        {
            var memoryBytes = memoryLimitMB * 1024 * 1024;
            var nanoCpus = 500_000_000; // 0.5 CPU

            var createParams = new CreateContainerParameters
            {
                Image = image,
                NetworkDisabled = true,
                User = "1000:1000",   // non-root
                HostConfig = new HostConfig
                {
                    Memory = memoryBytes,
                    NanoCPUs = nanoCpus,
                    PidsLimit = 50,
                    ReadonlyRootfs = true,
                    Mounts = new List<Mount>
                    {
                        new Mount
                        {
                            Source = workDir,
                            Target = "/app",
                            ReadOnly = true,
                            Type = "bind"
                        }
                    },
                    Tmpfs = new Dictionary<string, string> { { "/tmp", "size=10m" } }
                },
                Cmd = command,
                WorkingDir = "/app"
            };

            var response = await _client.Containers.CreateContainerAsync(createParams, ct);
            return response.ID;
        }

        public async Task StartAndWaitAsync(string containerId, CancellationToken ct)
        {
            await _client.Containers.StartContainerAsync(containerId, null, ct);
            await _client.Containers.WaitContainerAsync(containerId, ct);
        }

        public async Task<string> GetLogsAsync(string containerId)
        {
            var parameters = new ContainerLogsParameters
            {
                ShowStdout = true,
                ShowStderr = true,
                Follow = false
            };

            using (var stream = await _client.Containers.GetContainerLogsAsync(containerId, false, parameters))
            using (var memory = new MemoryStream())
            {
                await stream.CopyOutputToAsync(null, memory, memory, CancellationToken.None);
                memory.Position = 0;
                using (var reader = new StreamReader(memory, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<(long ExitCode, ResourceUsage Usage)> GetExitCodeAndResourceUsageAsync(string containerId)
        {
            var inspect = await _client.Containers.InspectContainerAsync(containerId);
            long exitCode = inspect.State.ExitCode;

            // Simple resource usage – we could call docker stats, but we report configured limits
            var usage = new ResourceUsage
            {
                Cpu = "0.5 core (limit)",
                Memory = $"{inspect.HostConfig?.Memory / (1024 * 1024)} MB"
            };
            return (exitCode, usage);
        }

        public async Task RemoveContainerAsync(string containerId)
        {
            try
            {
                await _client.Containers.RemoveContainerAsync(containerId,
                    new ContainerRemoveParameters { Force = true });
            }
            catch { /* best effort */ }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _client?.Dispose();
                _disposed = true;
            }
        }
    }
}