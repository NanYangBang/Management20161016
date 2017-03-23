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
    public class OrderClientManagerController : Controller
    {
        //
        // GET: /OrderClientManager/

        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            var userList = new List<WebOrderClient>();
            int totalCount = 0;
            var cResult = new OrderClientBLL().GetOrderClientList(out totalCount, searchInfo, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                userList = cResult.Data;
            }
            var pageList = new PagedList<WebOrderClient>(userList, pageIndex, pageSize);
            ViewBag.SearchInfo = searchInfo;
            return View(pageList);
        }

        [HttpGet]
        public ActionResult AddOrderClient(string returnUrl)
        {
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = returnUrl;
            WebOrderClient webOrderClient = new WebOrderClient();
            return View(webOrderClient);
        }

        [HttpPost]
        public ActionResult AddOrderClient(WebOrderClient webOrderClient)
        {
            OrderClientBLL OrderClientBLL = new OrderClientBLL();
            webOrderClient.CreateUserID = this.GetCurrentUserID();
            CResult<bool> cResult = OrderClientBLL.InsertOrderClient(webOrderClient);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpGet]
        public ActionResult UpdateOrderClient(string OrderClientID, string returnUrl)
        {
            OrderClientBLL OrderClientBLL = new OrderClientBLL();
            var result = OrderClientBLL.GetOrderClientInfoByID(OrderClientID);
            WebOrderClient webOrderClient = null;
            if (result.Code == 0)
            {
                webOrderClient = result.Data;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(webOrderClient);
        }

        [HttpPost]
        public ActionResult UpdateOrderClient(WebOrderClient webOrderClient, string returnUrl)
        {
            OrderClientBLL OrderClientBLL = new OrderClientBLL();

            CResult<bool> cResult = OrderClientBLL.UpdateOrderClientInfo(webOrderClient);

            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var result = new OrderClientBLL().DeleteOrderClient(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
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

    }
}
