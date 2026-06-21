using System.Collections.Generic;
using CompilerMicroservice.Models;

namespace CompilerMicroservice.Interfaces
{
    public interface IErrorParser
    {
        List<ErrorDetail> ParseErrors(string output, string language);
    }
}