using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class MaintainItemManagerController : Controller
    {
        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            try
            {
                var MaintainItemBLL = new MaintainItemBLL();
                int totalCount = 0;
                var list = new List<WebMaintainItem>();
                var cResult = MaintainItemBLL.GetMaintainItemList(out totalCount, this.GetCurrentProjectID(), searchInfo, pageIndex, pageSize, orderBy, ascending);
                if (cResult.Code == 0)
                {
                    list = cResult.Data;
                }
                var pageList = new PagedList<WebMaintainItem>(list, pageIndex, pageSize,totalCount);
                ViewBag.SearchInfo = searchInfo;
                ViewBag.PageSize = pageSize;
                ViewBag.ProjectID = this.GetCurrentProjectID();
                ViewBag.UserID = this.GetCurrentUserID();
                return View(pageList);
            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult Details(string MaintainItemID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Action = "Details";
            ViewBag.IsView = "True";
            try
            {
                var result = new MaintainItemBLL().GetMaintainItemByID(MaintainItemID);
                if (result.Code == 0)
                {
                    return View(result.Data);
                }
            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Action = "Create";
            return View(new WebMaintainItem());
        }

        [HttpPost]
        public ActionResult Create(WebMaintainItem webMaintainItem)
        {
            try
            {
                webMaintainItem.ProjectID = this.GetCurrentProjectID();
                webMaintainItem.CreateUserID = this.GetCurrentUserID();
                webMaintainItem.ProjectID = this.GetCurrentProjectID();

                var result = new MaintainItemBLL().InsertMaintainItem(webMaintainItem);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string MaintainItemID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            try
            {
                var result = new MaintainItemBLL().GetMaintainItemByID(MaintainItemID);
                if (result.Code == 0)
                {
                    return View(result.Data);
                }
            }
            catch (Exception)
            {
            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(WebMaintainItem webMaintainItem)
        {
            try
            {
                var result = new MaintainItemBLL().UpdateMaintainItem(webMaintainItem);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var result = new MaintainItemBLL().DeleteMaintainItem(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DownloadFile(string filePath, string fileName)
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            fileName = Url.Encode(fileName).Replace("+", "%20");
            return File(absolutePath, "application/object", fileName);
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase fileData, string ProjectID = "", string UserID = "")
        {
            var result = new MaintainItemBLL().ImportMaintainItemFromExcel(fileData, ProjectID, UserID);
            return JsonContentHelper.GetJsonContent(result);
        }

        public ActionResult ExportExcel(string searchInfo)
        {
            var result = new MaintainItemBLL().ExportMaintainItemToExcel(this.GetCurrentProjectID(), searchInfo);
            return JsonContentHelper.GetJsonContent(result);

        }

        public ActionResult GetAllMaintainItems()
        {
            int totalCount;
            var result = new MaintainItemBLL().GetMaintainItemList(out totalCount, this.GetCurrentProjectID(), "", 1, -1, "Name", true);
            
            return JsonContentHelper.GetJsonContent(result);
        }
    }
}
