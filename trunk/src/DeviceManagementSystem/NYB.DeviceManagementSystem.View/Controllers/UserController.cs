using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            List<WebUser> userList = new List<WebUser>();

            return View(userList);
        }

        public ActionResult AddUser()
        {
            return View();
        }

    }
}
