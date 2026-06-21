using Microsoft.AspNet.FriendlyUrls;
using System.Collections.Generic;
using System.Web.Routing;

namespace OnlineCompilerWebForms
{
    public static class RouteManager
    {
        public static void RegisterDynamicRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();

            routes.MapPageRoute("Home", "", "~/pages/Login.aspx");
            routes.MapPageRoute("LoginRoute", "login", "~/pages/Login.aspx");
            routes.MapPageRoute("RegisterRoute", "register", "~/pages/Register.aspx");
            routes.MapPageRoute("ConfirmEmailRoute", "confirm", "~/pages/ConfirmEmail.aspx");
            routes.MapPageRoute("ForgotPasswordRoute", "forgot-password", "~/pages/ForgotPassword.aspx");
            routes.MapPageRoute("ResetPasswordRoute", "reset-password", "~/pages/ResetPassword.aspx");
            routes.MapPageRoute("AccountSettingsRoute", "settings", "~/pages/AccountSettings.aspx");
            routes.MapPageRoute("CompilerRoute", "compiler", "~/pages/Compiler.aspx");
            routes.MapPageRoute("ProjectsRoute", "projects", "~/pages/Projects.aspx");
            routes.MapPageRoute("EditorRoute", "editor", "~/pages/Editor.aspx");
            routes.MapPageRoute("AboutRoute", "about", "~/pages/About.aspx");
            routes.MapPageRoute("ErrorRoute", "error", "~/pages/Error.aspx");
            routes.MapPageRoute("AdminDashboardRoute", "admin", "~/pages/Admin/Dashboard.aspx");
        }
    }
}