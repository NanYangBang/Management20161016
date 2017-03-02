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
    public class ManufactureManagerController : Controller
    {
        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            try
            {
                var bll = new ManufacturerBLL();
                int totalCount = 0;
                var list = new List<WebManufacturer>();
                var cResult = bll.GetManufacturerList(out totalCount, this.GetCurrentProjectID(), searchInfo, pageIndex, pageSize, orderBy, ascending);
                if (cResult.Code == 0)
                {
                    list = cResult.Data;
                }
                var pageList = new PagedList<WebManufacturer>(list, pageIndex, pageSize);
                ViewBag.SearchInfo = searchInfo;
                return View(pageList);
            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult Details(string manufacturerID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Action = "Details";
            ViewBag.IsView = "True";
            try
            {
                var result = new ManufacturerBLL().GetManufacturerByID(manufacturerID);
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
            return View(new WebManufacturer());
        }

        [HttpPost]
        public ActionResult Create(WebManufacturer WebManufacturer)
        {
            try
            {
                WebManufacturer.ProjectID = this.GetCurrentProjectID();
                WebManufacturer.CreateUserID = this.GetCurrentUserID();

                var result = new ManufacturerBLL().InsertManufacturer(WebManufacturer);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string manufacturerID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            try
            {
                var result = new ManufacturerBLL().GetManufacturerByID(manufacturerID);
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
        public ActionResult Edit(WebManufacturer webManufacturer)
        {
            try
            {
                var result = new ManufacturerBLL().UpdateManufacturer(webManufacturer);
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
                var result = new ManufacturerBLL().DeleteManufacturer(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }
    }
}
