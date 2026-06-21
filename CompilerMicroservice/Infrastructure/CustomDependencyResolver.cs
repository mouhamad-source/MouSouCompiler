using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using CompilerMicroservice.Interfaces;
using CompilerMicroservice.Services;
using CompilerMicroservice.ErrorParsers;

namespace CompilerMicroservice.Infrastructure
{
    public class CustomDependencyResolver : IDependencyResolver, IDependencyScope
    {
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        public CustomDependencyResolver()
        {
            var tempFileManager = new TempFileManager();
            var containerManager = new DockerContainerManager();
            var errorParser = new ErrorParser();
            var execService = new CodeExecutionService(containerManager, tempFileManager, errorParser);

            _singletons[typeof(ITempFileManager)] = tempFileManager;
            _singletons[typeof(IContainerManager)] = containerManager;
            _singletons[typeof(IErrorParser)] = errorParser;
            _singletons[typeof(ICodeExecutionService)] = execService;
        }

        public object GetService(Type serviceType)
        {
            if (_singletons.TryGetValue(serviceType, out var instance))
                return instance;

            if (serviceType.IsClass && !serviceType.IsAbstract && serviceType.Name.EndsWith("Controller"))
                return CreateController(serviceType);

            return null;
        }

        private object CreateController(Type controllerType)
        {
            var ctor = controllerType.GetConstructors()[0];
            var args = new object[ctor.GetParameters().Length];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = GetService(ctor.GetParameters()[i].ParameterType);
                if (args[i] == null)
                    throw new InvalidOperationException($"Cannot resolve {ctor.GetParameters()[i].ParameterType}");
            }
            return Activator.CreateInstance(controllerType, args);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var s = GetService(serviceType);
            return s != null ? new[] { s } : new object[0];
        }

        public IDependencyScope BeginScope() => this;
        public void Dispose() { }
    }
}