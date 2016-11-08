using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class ProjectManagerController : Controller
    {
        //
        // GET: /ProjectManager/

        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 1, string orderBy = "", bool ascending = false)
        {
            List<WebProject> userList = new List<WebProject>();

            userList.Add(
                new WebProject()
                {
                    Name = "测试项目"
                }
                );
            userList.Add(
                new WebProject()
                {
                    Name = "测试项目"
                }
                );

            var pageList = new PagedList<WebProject>(userList, pageIndex, pageSize);
            return View(pageList);
        }

        [HttpGet]
        public ActionResult AddProject(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProject(WebProject webUser)
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateProject(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProject(WebProject webUser)
        {
            return View();
        }

    }
}
