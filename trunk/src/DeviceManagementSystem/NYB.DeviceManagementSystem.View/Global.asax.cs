using NYB.DeviceManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NYB.DeviceManagementSystem.View
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //    "API", // 路由名称
            //    "api/{controller}/{action}/{id}", // 带有参数的 URL
            //    new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }, // 参数默认值
            //    new { controller = ".*Api$" }
            //);

            routes.MapRoute(
                "noparam", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Pile", // 路由名称
                "Pile/{action}/{id}", // 带有参数的 URL
                new { controller = "PileConInfo", action = "Index", id = UrlParameter.Optional }// 参数默认值				
            );
            routes.MapRoute(
                "ConstructionInformation", // 路由名称
                "ConstructionInformation/{action}/{id}", // 带有参数的 URL
                new { controller = "PileConInfo", action = "Index", id = UrlParameter.Optional }// 参数默认值				
            );

            routes.MapRoute(
                "Schedule", // 路由名称
                "Schedule/{action}/{id}", // 带有参数的 URL
                new { controller = "ScheduleManage", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "User", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleTable.EnableOptimizations = false;

            DelTempFile();
        }

        private static void DelTempFile()
        {
            NYB.DeviceManagementSystem.Common.Logger.LogHelper.Info("删除temp文件夹");

            var needDeletePath = Path.Combine(SystemInfo.BaseDirectory, SystemInfo.TempFileFolder);
            if (Directory.Exists(needDeletePath) == false)
            {
                return;
            }
            var files = Directory.GetFiles(needDeletePath);
            foreach (var item in files)
            {
                File.Delete(item);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        #region 添加Uploadify的Cookie值
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    //flash 上传组件Session 值恢复
        //    //如果网站中还用到了Membership的FormsAuthentication验证，则还需要把AUTHID也按照SessionID的方法进行处理
        //    //'scriptData': {"ASPSESSID": "<%=Session.SessionID %>","AUTHID" : "<%=Request.Cookies[FormsAuthentication.FormsCookieName].Value%>"}
        //    try {
        //        string session_param_name = "ASPSESSID";
        //        string session_cookie_name = "ASP.NET_SESSIONID";
        //        if (HttpContext.Current.Request.Form[session_param_name] != null) {
        //            UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
        //        } else if (HttpContext.Current.Request.QueryString[session_param_name] != null) {
        //            UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
        //        }

        //        string auth_param_name = "AUTHID";
        //        string auth_cookie_name = FormsAuthentication.FormsCookieName;
        //        if (HttpContext.Current.Request.Form[auth_param_name] != null) {
        //            UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
        //        } else if (HttpContext.Current.Request.QueryString[auth_param_name] != null) {
        //            UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
        //        }

        //    } catch (Exception ex) { }
        //}

        //void UpdateCookie(string cookie_name, string cookie_value)
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
        //    if (cookie == null) {
        //        cookie = new HttpCookie(cookie_name);
        //        HttpContext.Current.Request.Cookies.Add(cookie);
        //    }
        //    cookie.Value = cookie_value;
        //    HttpContext.Current.Request.Cookies.Set(cookie);
        //}
        #endregion
    }
}