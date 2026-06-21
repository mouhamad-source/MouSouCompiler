using System;
using System.Collections.Generic;

namespace CompilerMicroservice.Models
{
    public class ExecutionResult
    {
        public string Output { get; set; }           // Combined stdout + stderr
        public string Time { get; set; }             // Execution time (e.g., "50ms")
        public int ExitCode { get; set; }            // Process exit code
        public List<ErrorDetail> Errors { get; set; } = new List<ErrorDetail>();
        public Metadata Metadata { get; set; }
    }

    public class ErrorDetail
    {
        public string File { get; set; }
        public int Line { get; set; }
        public string Message { get; set; }
    }

    public class Metadata
    {
        public string Language { get; set; }
        public string ContainerId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<string> FilesExecuted { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public ResourceUsage ResourceUsage { get; set; }
        public string Status { get; set; }  // "success", "compile_error", "runtime_error", "timeout", "internal_error"
    }

    public class ResourceUsage
    {
        public string Cpu { get; set; }     // e.g., "10%"
        public string Memory { get; set; }  // e.g., "50MB"
    }
}