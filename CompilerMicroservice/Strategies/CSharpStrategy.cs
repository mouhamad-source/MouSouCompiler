using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerMicroservice.Strategies
{
    public class CSharpStrategy : IBuildStrategy
    {
        public async Task PrepareAsync(string workDir, List<CodeFile> files, string entryPoint)
        {
            // Write all .cs files to workDir
            foreach (var file in files)
            {
                var filePath = Path.Combine(workDir, file.Name);
                using (StreamWriter writer = new StreamWriter(filePath, append: false, encoding: Encoding.UTF8))
                {
                    await writer.WriteAsync(file.Code);
                }
            }
        }

        // No separate build step – compilation is part of execution
        public string[] GetBuildCommand() => null;

        public string[] GetRunCommand(string entryPoint, List<string> args, string stdin)
        {
            // Compile all .cs files to /tmp/Program.exe, then run it.
            // Arguments are appended after the mono command.
            var cmd = "mcs -out:/tmp/Program.exe /app/*.cs && mono /tmp/Program.exe";
            if (args != null && args.Count > 0)
                cmd += " " + string.Join(" ", args);
            return new[] { "bash", "-c", cmd };
        }

        public string GetImage() => "mono:latest";
        public string GetLanguageName() => "csharp";
    }
}