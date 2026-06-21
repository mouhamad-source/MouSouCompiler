using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerMicroservice.Strategies
{
    public class PythonStrategy : IBuildStrategy
    {
        public async Task PrepareAsync(string workDir, List<CodeFile> files, string entryPoint)
        {
            // Write all .py files
            foreach (var file in files)
            {
                var filePath = Path.Combine(workDir, file.Name);
                using (StreamWriter writer = new StreamWriter(filePath, append: false, encoding: Encoding.UTF8))
                {
                    await writer.WriteAsync(file.Code);
                }
            }
        }

        public string[] GetBuildCommand() => null; // No compilation

        public string[] GetRunCommand(string entryPoint, List<string> args, string stdin)
        {
            var cmd = new List<string> { "python", entryPoint };
            if (args != null)
                cmd.AddRange(args);
            return cmd.ToArray();
        }

        public string GetImage() => "python:3.10-slim";
        public string GetLanguageName() => "python";
    }
}