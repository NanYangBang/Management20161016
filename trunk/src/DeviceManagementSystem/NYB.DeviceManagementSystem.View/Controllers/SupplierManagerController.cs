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
    public class SupplierManagerController : Controller
    {
        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            try
            {
                var bll = new SupplierBLL();
                int totalCount = 0;
                var list = new List<WebSupplier>();
                var cResult = bll.GetSupplierList(out totalCount, this.GetCurrentProjectID(), searchInfo, pageIndex, pageSize, orderBy, ascending);
                if (cResult.Code == 0)
                {
                    list = cResult.Data;
                }
                var pageList = new PagedList<WebSupplier>(list, pageIndex, pageSize,totalCount);
                ViewBag.SearchInfo = searchInfo;
                return View(pageList);
            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult Details(string supplierID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Action = "Details";

            try
            {
                var result = new SupplierBLL().GetSupplierByID(supplierID);
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
            return View(new WebSupplier());
        }

        [HttpPost]
        public ActionResult Create(WebSupplier webSupplier)
        {
            try
            {
                webSupplier.ProjectID = this.GetCurrentProjectID();
                webSupplier.CreateUserID = this.GetCurrentUserID();

                var result = new SupplierBLL().InsertSupplier(webSupplier);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string supplierID, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            try
            {
                var result = new SupplierBLL().GetSupplierByID(supplierID);
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
        public ActionResult Edit(WebSupplier webSupplier)
        {
            try
            {
                var result = new SupplierBLL().UpdateSupplier(webSupplier);
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
                var result = new SupplierBLL().DeleteSupplier(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }
    }
}
