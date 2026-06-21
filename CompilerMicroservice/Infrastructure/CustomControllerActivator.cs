using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace CompilerMicroservice.Infrastructure
{
    public class CustomControllerActivator : IHttpControllerActivator
    {
        private readonly HttpConfiguration _configuration;

        public CustomControllerActivator(HttpConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var resolver = _configuration.DependencyResolver;
            return resolver.GetService(controllerType) as IHttpController;
        }
    }
}