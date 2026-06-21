using System;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Strategies;

namespace CompilerMicroservice.Factories
{
    public static class BuildStrategyFactory
    {
        public static IBuildStrategy GetStrategy(string language)
        {
            switch (language?.ToLower())
            {
                case "python":
                    return new PythonStrategy();
                case "csharp":
                    return new CSharpStrategy();
                default:
                    throw new NotSupportedException($"Language '{language}' is not supported.");
            }
        }
    }
}