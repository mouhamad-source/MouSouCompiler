using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlineCompilerWebForms.Model.DTOs
{
    public class MicroserviceResponse
    {
        [JsonPropertyName("Output")]
        public string Output { get; set; }

        [JsonPropertyName("Time")]
        public string Time { get; set; }

        [JsonPropertyName("ExitCode")]
        public int ExitCode { get; set; }

        [JsonPropertyName("Errors")]
        public List<ErrorInfo> Errors { get; set; }

        [JsonPropertyName("Metadata")]
        public MetadataInfo Metadata { get; set; }
    }

    public class ErrorInfo
    {
        [JsonPropertyName("File")]
        public string File { get; set; }

        [JsonPropertyName("Line")]
        public int Line { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }

    public class MetadataInfo
    {
        [JsonPropertyName("Language")]
        public string Language { get; set; }

        [JsonPropertyName("ContainerId")]
        public string ContainerId { get; set; }

        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("FilesExecuted")]
        public List<string> FilesExecuted { get; set; }

        [JsonPropertyName("Warnings")]
        public List<string> Warnings { get; set; }

        [JsonPropertyName("ResourceUsage")]
        public ResourceUsageInfo ResourceUsage { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }

    public class ResourceUsageInfo
    {
        [JsonPropertyName("Cpu")]
        public string Cpu { get; set; }

        [JsonPropertyName("Memory")]
        public string Memory { get; set; }
    }
}