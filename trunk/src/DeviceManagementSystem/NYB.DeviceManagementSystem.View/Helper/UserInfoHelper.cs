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
            var projectID = controller.Request.Cookies["CurrentProjectIDStr"].Value;
            //if (string.IsNullOrEmpty(projectID))
            //{
            //    throw new Exception("CurrentProjectIDStr is empty , maybe cookie id disable ");
            //}

            return projectID;
        }

        public static string GetCurrentUserID(this Controller controller)
        {
            var userID = controller.Request.Cookies["CurrentUserID"].Value;
            if (string.IsNullOrEmpty(userID))
            {
                throw new Exception("CurrentUserID is empty , maybe cookie id disable ");
            }
            return userID;
        }

        public static string GetCurrentOrderClientID(this Controller controller)
        {
            var OrderClientID = controller.Request.Cookies["OrderClientID"].Value;
            if (string.IsNullOrEmpty(OrderClientID))
            {
                throw new Exception("OrderClientID is empty , maybe cookie id disable ");
            }
            return OrderClientID;
        }

    }
}