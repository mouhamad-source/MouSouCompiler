using CompilerMicroservice.Infrastructure;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace CompilerMicroservice
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Set custom dependency resolver
            var resolver = new CustomDependencyResolver();
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            // Replace default controller activator with our custom one
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new CustomControllerActivator(GlobalConfiguration.Configuration));
        }
    }
}