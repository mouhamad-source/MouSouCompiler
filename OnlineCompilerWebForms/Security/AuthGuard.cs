using System.Web;
using System.Web.UI;

namespace OnlineCompilerWebForms.Security
{
    public static class AuthGuard
    {
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.Session["UserId"] != null;
        }

        public static bool IsAdmin()
        {
            return HttpContext.Current.Session["Role"]?.ToString() == "Admin";
        }

        public static void RequireAuthentication(Page page)
        {
            if (!IsAuthenticated())
            {
                page.Response.Redirect("~/Login.aspx" , false );
            }
        }

        public static void RequireAdmin(Page page)
        {
            if (!IsAdmin())
            {
                page.Response.Redirect("~/Login.aspx" , false );
            }
        }
    }
}
