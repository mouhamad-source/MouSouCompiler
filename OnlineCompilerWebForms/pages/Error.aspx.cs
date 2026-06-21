using System;

namespace OnlineCompilerWebForms.pages
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string statusCode = Request.QueryString["code"];
                
                if (statusCode == "404")
                {
                    litErrorCode.Text = "404";
                    litErrorTitle.Text = "Page Not Found";
                    litErrorMessage.Text = "The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
                }
                else if (statusCode == "403" || statusCode == "401")
                {
                    litErrorCode.Text = statusCode;
                    litErrorTitle.Text = "Unauthorized Access";
                    litErrorMessage.Text = "You do not have permission to view this directory or page using the credentials that you supplied.";
                }
                else if (statusCode == "500")
                {
                    litErrorCode.Text = "500";
                    litErrorTitle.Text = "Internal Server Error";
                    litErrorMessage.Text = "Our servers are currently experiencing issues. Please try again later.";
                }
            }
        }
    }
}
