using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.Common;
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
            int totalCount = 0;
            ProjectBLL projectBLL = new ProjectBLL();
            var cResult = projectBLL.GetProjectList(out totalCount, searchInfo, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                userList = cResult.Data;
            }
            var pageList = new PagedList<WebProject>(userList, pageIndex, pageSize);
            return View(pageList);
        }

        [HttpGet]
        public ActionResult AddProject(string returnUrl)
        {
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = returnUrl;
            WebProject webProject = new WebProject();
            return View(webProject);
        }

        [HttpPost]
        public ActionResult AddProject(WebProject webProject)
        {
            //webProject.ID = Guid.NewGuid().ToString();
            //webProject.WebUser.ID = Guid.NewGuid().ToString();
            //webProject.WebUser.Role = RoleType.超级管理员.ToString();
            //webProject.WebUser.CreateUserID = "640C01A6-1B8F-423C-96CA-162C6A5C4034";
            //webProject.CreateUserID = "640C01A6-1B8F-423C-96CA-162C6A5C4034";
            //webProject.WebUser.ProjectID = webProject.ID;
            //webProject.WebUser.CreateUserName = "SuperAdmin";
            ProjectBLL projectBLL = new ProjectBLL();

            CResult<bool> cResult = projectBLL.InsertProject(webProject);

            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpGet]
        public ActionResult UpdateProject(string projectID, string returnUrl)
        {
            ProjectBLL projectBLL = new ProjectBLL();
            var result = projectBLL.GetProjectInfoByID(projectID);
            WebProject webProject = null;
            if (result.Code == 0)
            {
                webProject = result.Data;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(webProject);
        }

        [HttpPost]
        public ActionResult UpdateProject(WebProject webProject, string returnUrl)
        {
            ProjectBLL projectBLL = new ProjectBLL();

            CResult<bool> cResult = projectBLL.UpdateProjectInfo(webProject);

            return JsonContentHelper.GetJsonContent(cResult);
        }

    }
}
