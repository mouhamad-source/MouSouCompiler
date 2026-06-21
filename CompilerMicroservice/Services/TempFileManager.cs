using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerMicroservice.Services
{
    public class TempFileManager : ITempFileManager
    {
        private readonly string _baseTempPath;

        public TempFileManager()
        {
            _baseTempPath = Path.Combine(Path.GetTempPath(), "CompilerMicroservice");
            Directory.CreateDirectory(_baseTempPath);
        }

        public async Task<string> CreateWorkDirectoryAsync()
        {
            var workDir = Path.Combine(_baseTempPath, Guid.NewGuid().ToString());
            await Task.Run(() => Directory.CreateDirectory(workDir));
            return workDir;
        }

        public async Task WriteFilesAsync(string workDir, List<CodeFile> files)
        {
            foreach (var file in files)
            {
                var filePath = Path.Combine(workDir, file.Name);
                using (StreamWriter writer = new StreamWriter(filePath, append: false, encoding: Encoding.UTF8))
                {
                    await writer.WriteAsync(file.Code);
                }
            }
        }

        public void DeleteWorkDirectory(string workDir)
        {
            if (Directory.Exists(workDir))
            {
                try { Directory.Delete(workDir, true); }
                catch { /* best effort */ }
            }
        }
    }
}