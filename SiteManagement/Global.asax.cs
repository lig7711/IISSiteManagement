using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Automation.Common;
using Spring.Web.Mvc;

namespace SiteManagement
{
    public class MvcApplication : Spring.Web.Mvc.SpringMvcApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //LogHelper初始化
            LogHelper.Init();
            LogHelper.Log(LogHelper.LogLevel.Info, "Application started.");
        }

        public void Application_Error(object send, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            //转到错误页面
            if (ex != null)
            {
                LogHelper.Log(LogHelper.LogLevel.Error, ex.ToString());

                var httpError = ex as HttpException;
                if (httpError != null)
                {
                    //ASP.NET的400与404错误，都以自定义404页面响应
                    var httpCode = httpError.GetHttpCode();
                    if (httpCode == 400 || httpCode == 404)
                    {
                        Response.StatusCode = 404;//在IIS中配置自定义404页面
                        Response.Redirect("~/Content/404.html");
                        Server.ClearError();
                        return;
                    }
                }

                if (ex.TargetSite.ReflectedType == typeof(System.IO.Path))
                {
                    //对于路径错误，都以自定义404页面响应
                    Response.StatusCode = 404;
                    Response.Redirect("~/Content/404.html");
                    Server.ClearError();
                }
                else
                {
                    Response.StatusCode = 500;
                    Response.Redirect("~/Content/500.html");
                    Server.ClearError();
                }
            }
        }
    }
}
