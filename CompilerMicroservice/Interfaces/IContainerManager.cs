using System.Threading;
using System.Threading.Tasks;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.Interfaces
{
    public interface IContainerManager
    {
        Task<string> CreateContainerAsync(string workDir, string image, string[] command,
                                          int memoryLimitMB, CancellationToken ct);
        Task StartAndWaitAsync(string containerId, CancellationToken ct);
        Task<string> GetLogsAsync(string containerId);
        Task<(long ExitCode, ResourceUsage Usage)> GetExitCodeAndResourceUsageAsync(string containerId);
        Task RemoveContainerAsync(string containerId);
    }
}