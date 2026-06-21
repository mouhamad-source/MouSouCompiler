using CompilerMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompilerMicroservice.Interfaces
{
    public interface ITempFileManager
    {
        Task<string> CreateWorkDirectoryAsync();
        Task WriteFilesAsync(string workDir, List<CodeFile> files);
        void DeleteWorkDirectory(string workDir);
    }
}