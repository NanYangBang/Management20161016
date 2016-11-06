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
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        
        {
            UserBLL userBLL = new UserBLL();
            int totalCount = 0;
            string projectID = "";
            List<WebUser> userList = new List<WebUser>();
            CResult<List<WebUser>> cResult = userBLL.GetUserList(out totalCount, projectID, "", searchInfo, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                userList = cResult.Data;
            }
            var pageList = new PagedList<WebUser>(userList, pageIndex, pageSize);
            return View(pageList);
        }

        [HttpGet]
        public ActionResult AddUser(string returnUrl)
        {
            ViewBag.IsErr = false;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsUpdate = false;
            ViewBag.ErrMsg = "";
            ViewBag.Action = "Add";
            return View(new WebUser());
        }

        [HttpPost]
        public ActionResult AddUser(WebUser webUser, string returnUrl)
        {
            UserBLL userBLL = new UserBLL();
            CResult<bool> cResult = userBLL.AddUser(webUser, false);
            return JsonContentHelper.GetJsonContent(cResult);
            //if (cResult.Code == 0)
            //{
                
            //}
            //else
            //{
            //    ViewBag.IsErr = true;
            //    ViewBag.ErrMsg = cResult.Msg;
            //    return View(webUser);
            //}
        }

        [HttpGet]
        public ActionResult UpdateUser(string userID, string returnUrl)
        {
            ViewBag.IsUpdate = true;
            ViewBag.Action = "Update";
            UserBLL userBLL = new UserBLL();
            CResult<WebUser> cResult = userBLL.GetUserInfoByUserID(userID);
            if (cResult.Code == 0)
            {
                return View(cResult.Data);
            }
            else
            {
                return View(new WebUser());
            }
        }

        public ActionResult UpdateUser()
        {
            return View();
        }

    }
}
