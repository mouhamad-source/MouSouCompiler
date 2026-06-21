using System.Collections.Generic;

namespace CompilerMicroservice.Models
{
    public class CompileRequest
    {
        public string Language { get; set; }          // "python" or "csharp"
        public List<CodeFile> Files { get; set; }     // List of code files
        public string EntryPoint { get; set; }        // Filename of entry point (e.g., "Program.cs")
        public List<string> Args { get; set; }        // Command line arguments
        public string Stdin { get; set; }              // Standard input data
        public int TimeoutLimit { get; set; } = 5000;  // ms
        public int MemoryLimit { get; set; } = 256;    // MB
    }

    public class CodeFile
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}