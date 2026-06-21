using JavaScriptEngineSwitcher.Core; 
using React;
using System;
using System.Web.Optimization;
using System.Web.Routing;
//*
//
//The control with ID 'upReview' requires a ScriptManager on the page. The ScriptManager must appear before any controls that need it.
//*/
namespace OnlineCompilerWebForms
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Run DB Migrations
            try
            {
                var migrationService = new OnlineCompilerWebForms.Services.MigrationService();
                migrationService.RunMigrations();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Migration failed: " + ex.Message);
            }

            //FormsAuthentication.Timeout = System.TimeSpan.FromMinutes(30);
            RouteManager.RegisterDynamicRoutes(RouteTable.Routes); 
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Log errors
            Exception ex = Server.GetLastError();

            // Log to event log or database
            // System.Diagnostics.EventLog.WriteEntry("OnlineCompiler", ex.ToString(), System.Diagnostics.EventLogEntryType.Error);

            // Clear error and redirect to error page
            Server.ClearError();
            Response.Redirect("~/error");
        }
        protected void Session_Start(object sender, System.EventArgs e) { }
        protected void Session_End(object sender, System.EventArgs e) { }
    
}
}