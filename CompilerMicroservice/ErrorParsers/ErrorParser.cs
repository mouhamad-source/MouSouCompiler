using System.Collections.Generic;
using System.Text.RegularExpressions;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.ErrorParsers
{
    public class ErrorParser : IErrorParser
    {
        public List<ErrorDetail> ParseErrors(string output, string language)
        {
            var errors = new List<ErrorDetail>();
            if (string.IsNullOrEmpty(output)) return errors;

            if (language == "csharp")
            {
                // C# compiler error pattern: Program.cs(12,23): error CS1002: ; expected
                var regex = new Regex(@"(?<file>[^\(]+)\((?<line>\d+),?\d*\):\s+error\s+CS\d+:\s+(?<message>.+)");
                foreach (Match m in regex.Matches(output))
                {
                    errors.Add(new ErrorDetail
                    {
                        File = m.Groups["file"].Value,
                        Line = int.Parse(m.Groups["line"].Value),
                        Message = m.Groups["message"].Value.Trim()
                    });
                }
            }
            else if (language == "python")
            {
                // Python traceback: File "script.py", line 10, in <module>
                var regex = new Regex(@"File\s+\""(?<file>[^\""]+)\"",\s+line\s+(?<line>\d+)");
                var matches = regex.Matches(output);
                foreach (Match m in matches)
                {
                    // Capture the next line as message? This is simplified.
                    errors.Add(new ErrorDetail
                    {
                        File = m.Groups["file"].Value,
                        Line = int.Parse(m.Groups["line"].Value),
                        Message = "Syntax or runtime error (see output)"
                    });
                }
                if (errors.Count == 0 && output.Contains("Error:"))
                {
                    errors.Add(new ErrorDetail { File = "unknown", Line = 0, Message = output });
                }
            }
            return errors;
        }
    }
}