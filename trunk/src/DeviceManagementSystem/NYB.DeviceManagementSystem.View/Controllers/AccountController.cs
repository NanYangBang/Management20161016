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
            if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(Pwd))
            {
            }

            var userBLL = new UserBLL();
            var result = userBLL.VerifyPassword(LoginName, Pwd);
            if (result.Code == 0)
            {
                if (result.Data.Role != null)
                {
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

            return View();
        }

    }
}
