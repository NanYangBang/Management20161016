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

        public ActionResult Index(string searchInfo = "", int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            UserBLL userBLL = new UserBLL();
            int totalCount = 0;
            string projectID = this.GetCurrentProjectID();
            List<WebUser> userList = new List<WebUser>();
            CResult<List<WebUser>> cResult = userBLL.GetUserList(out totalCount, projectID, this.GetCurrentUserID(), searchInfo, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                userList = cResult.Data;
            }
            var pageList = new PagedList<WebUser>(userList, pageIndex, pageSize);
            ViewBag.SearchInfo = searchInfo;
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
        public ActionResult AddUser(WebUser webUser)
        {
            UserBLL userBLL = new UserBLL();
            webUser.ProjectID = this.GetCurrentProjectID();
            webUser.CreateUserID = this.GetCurrentUserID();
            CResult<bool> cResult = userBLL.AddUser(webUser);
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
            ViewBag.IsErr = false;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrMsg = "";

            UserBLL userBLL = new UserBLL();
            CResult<WebUser> cResult = userBLL.GetUserInfoByUserID(userID);
            if (cResult.Code == 0)
            {
                return View(cResult.Data);
            }
            else
            {
                ViewBag.IsErr = true;
                ViewBag.ErrMsg = cResult.Msg;
                return View(new WebUser());
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(WebUser webUser)
        {
            webUser.CreateUserID = this.GetCurrentUserID();
            UserBLL userBLL = new UserBLL();
            CResult<bool> cResult = userBLL.UpdateUser(webUser);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            var userBLL = new UserBLL();
            var currentUserID = Request.Cookies["CurrentUserID"].Value;
            var result = userBLL.DeleteUserByID(id, currentUserID);
            return JsonContentHelper.GetJsonContent(result);
        }

        [HttpGet]
        public ActionResult ResetPassword(string userID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var webUser = new WebUser();
            webUser.ID = userID;
            return View(webUser);
        }

        [HttpPost]
        public ActionResult ResetPasswordPost(string userID, string newPassword)
        {
            var result = new UserBLL().ResetPassword(newPassword, userID, this.GetCurrentUserID());

            return JsonContentHelper.GetJsonContent(result);
        }

        [HttpPost]
        public ActionResult ChnagePassword(string oldPassword, string newPassword)
        {
            var result = new UserBLL().ChangePassword(oldPassword, newPassword, this.GetCurrentUserID(), this.GetCurrentUserID());
            return JsonContentHelper.GetJsonContent(result);
        }

        [HttpGet]
        public ActionResult ChangePassWord()
        {
            var webUser = new WebUser();
            return PartialView("PartialView/ChangePassWord", webUser);
        }

    }
}
