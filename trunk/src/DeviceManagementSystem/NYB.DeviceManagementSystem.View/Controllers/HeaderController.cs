using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class HeaderController : Controller
    {
        //
        // GET: /Header/

        public ActionResult Header()
        {
            var role = this.GetCurrentRole();
            if (role != RoleType.超级管理员)
            {
                var projectID = this.GetCurrentOrderClientID();

                //var clientInfo = OrderClientBLL.GetCompanyInfo(projectID);
                //if (clientInfo.Code == 0)
                //{
                //    ViewBag.LogoFile = clientInfo.Data.LogoFile;
                //    ViewBag.CompanyName = clientInfo.Data.CompanyName;
                //}

                if (role == RoleType.操作员 || role == RoleType.管理员 || role == RoleType.项目管理员)
                {
                    var info = DeviceBLL.GetMaintainCount(projectID);

                    if (info.Code == 0)
                    {
                        ViewBag.MaintainCount = info.Data;
                    }
                }
            }

            return PartialView();
        }

    }
}
