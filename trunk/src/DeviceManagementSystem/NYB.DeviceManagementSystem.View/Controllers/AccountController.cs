using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult LogOn()
        {
            //new DatabaseInitHelper().InitDB();

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string LoginName, string Pwd)
        {
            var errorInfo = "用戶名或密码错误";
            if (ModelState.IsValid)
            {
                var userBLL = new UserBLL();
                var result = userBLL.VerifyPassword(LoginName, Pwd);
                if (result.Code == 0)
                {
                    if (result.Data.Role != null)
                    {
                        Response.Cookies.Add(new HttpCookie("CurrentUserID", result.Data.ID));
                        Response.Cookies.Add(new HttpCookie("CurrentUserName", result.Data.UserName));
                        Response.Cookies.Add(new HttpCookie("CurrentProjectID", result.Data.ProjectID));

                        FormsAuthentication.SetAuthCookie(LoginName, false);

                        errorInfo = result.Msg;

                        if (result.Data.Role == RoleType.超级管理员.ToString())
                        {
                            return RedirectToAction("Index", "ProjectManager");
                        }
                        else
                        {
                            return RedirectToAction("Index", "SystemManager");
                        }
                    }
                }
            }

            ModelState.AddModelError("", errorInfo);
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn");
        }
    }
}
