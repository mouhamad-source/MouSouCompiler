using System;

namespace OnlineCompilerWebForms.Model
{
    public class ProjectFile
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public bool IsEntryPoint { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
