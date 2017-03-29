using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class BasicInfoManagerController : Controller
    {
        //
        // GET: /BasicInfoManager/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "MaintainItemManager", new { _timepick = DateTime.Now.ToString("yyyyMMddhhmmssff") });
        }
    }
}
