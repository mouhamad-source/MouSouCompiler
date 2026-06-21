using System.Collections.Generic;

namespace OnlineCompilerWebForms.Model.DTOs
{
    public class ExecutionRequestDto
    {
        public string language { get; set; }
        public List<CodeFileDto> files { get; set; }
        public string stdin { get; set; }
        public int timeoutLimit { get; set; }
        public List<string> args { get; set; }
    }

    public class CodeFileDto
    {
        public string name { get; set; }
        public string code { get; set; }
    }
}
