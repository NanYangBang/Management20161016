using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NYB.DeviceManagementSystem.ViewModel;
using Webdiyer.WebControls.Mvc;
using NYB.DeviceManagementSystem.BLL;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class RepairRecordController : Controller
    {
        //
        // GET: /RepairRecord/

        public ActionResult Index()
        {
            return View();
        }


    }
}
