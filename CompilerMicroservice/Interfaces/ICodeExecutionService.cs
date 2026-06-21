using System.Threading.Tasks;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.Interfaces
{
    public interface ICodeExecutionService
    {
        Task<ExecutionResult> ExecuteAsync(CompileRequest request);
    }
}