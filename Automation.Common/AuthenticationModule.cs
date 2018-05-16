using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Automation.Common
{
    public class AuthenticationModule : IHttpModule
    {
        protected string _loginUrl = "";
        protected string LoginUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_loginUrl))
                    _loginUrl = FormsAuthentication.LoginUrl;

                return _loginUrl;
            }
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        protected virtual void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (!context.User.Identity.IsAuthenticated && string.IsNullOrEmpty(context.Request.CurrentExecutionFilePathExtension))
            {
                string path = context.Request.Url.AbsolutePath;
                if (path.ToLower().IndexOf(LoginUrl.ToLower()) == -1)
                {
                    var url = LoginUrl;
                    Regex regex = new Regex(string.Format("^{0}/?$", context.Request.ApplicationPath));
                    if (!string.IsNullOrEmpty(path) && !regex.IsMatch(path))
                    {
                        url += "?ReturnUrl=" + HttpUtility.UrlEncode(context.Request.Url.PathAndQuery);
                    }
                    context.Response.Redirect(url);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
