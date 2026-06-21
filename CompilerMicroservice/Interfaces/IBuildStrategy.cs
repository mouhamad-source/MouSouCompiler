using CompilerMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompilerMicroservice.Interfaces
{
    public interface IBuildStrategy
    {
        Task PrepareAsync(string workDir, List<CodeFile> files, string entryPoint);
        string[] GetBuildCommand();    // null if interpreted
        string[] GetRunCommand(string entryPoint, List<string> args, string stdin);
        string GetImage();
        string GetLanguageName();
    }
}