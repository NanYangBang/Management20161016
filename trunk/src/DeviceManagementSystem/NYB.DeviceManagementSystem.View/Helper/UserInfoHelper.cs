using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class UserInfoHelper
    {
        public static string GetCurrentProjectID(this Controller controller)
        {
            return controller.Request.Cookies["CurrentProjectIDStr"].Value;
        }

        public static string GetCurrentUserID(this Controller controller)
        {
            return controller.Request.Cookies["CurrentUserID"].Value;
        }
    }
}