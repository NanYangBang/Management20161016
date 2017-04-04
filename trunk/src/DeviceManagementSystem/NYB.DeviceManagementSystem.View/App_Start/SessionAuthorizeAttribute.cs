using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NYB.DeviceManagementSystem.View.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SessionAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                return;
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account")
            {
                return;
            }

            var session = filterContext.HttpContext.Session["CompanyName"];
            if (session == null)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl, true);
                return;
            }
        }
    }
}