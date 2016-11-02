using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Roles.CreateRole("超级管理员");
            //Roles.GetRolesForUser();


            //Membership.CreateUser("", "");
            //Membership.GetAllUsers();
            //Membership.GetUser("");
            //Membership.ValidateUser("","");
            //Roles.CreateRole("");
            //Roles.AddUsersToRole();

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
