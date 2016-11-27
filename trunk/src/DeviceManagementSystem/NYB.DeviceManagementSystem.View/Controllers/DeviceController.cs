using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NYB.DeviceManagementSystem.ViewModel;
using Webdiyer.WebControls.Mvc;
using NYB.DeviceManagementSystem.BLL;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class DeviceController : Controller
    {
        //
        // GET: /Device/

        public ActionResult Index(string searchInfo, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            List<WebDevice> device = new List<WebDevice>();
            int totalCount = 0;
            DeviceBLL deviceBLL = new DeviceBLL();
            var cResult = deviceBLL.GetDeviceList(out totalCount, this.GetCurrentProjectID(), searchInfo, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                device = cResult.Data;
            }
            var pageList = new PagedList<WebDevice>(device, pageIndex, pageSize);

            return View(pageList);
        }

        [HttpGet]
        public ActionResult CreateDevice(string returnUrl)
        {
            DeviceTypeBLL deviceBLL = new DeviceTypeBLL();
            ManufacturerBLL manufacturBLL = new ManufacturerBLL();
            SupplierBLL supplierBLL = new SupplierBLL();

            var deviceType = deviceBLL.GetDeviceTypeDir(this.GetCurrentProjectID());

            ViewBag.ManufacturList = manufacturBLL.GetManufacturerDir(this.GetCurrentProjectID()).Data;
            ViewBag.SupperList = supplierBLL.GetSupplierDir(this.GetCurrentProjectID()).Data;
            ViewBag.DeviceType = deviceType.Data;
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = returnUrl;

            WebDevice webDevice = new WebDevice();
            return View(webDevice);
        }

        [HttpPost]
        public ActionResult CreateDevice(WebDevice webDevice)
        {
            DeviceBLL deviceBLL = new DeviceBLL();
            var cResult = deviceBLL.InsertDevice(webDevice);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        public ActionResult EditDevice(string returnUrl)
        {
            DeviceBLL deviceBLL = new DeviceBLL();
            var result = deviceBLL.GetDeviceByID(this.GetCurrentProjectID());
            WebDevice webDevice = null;
            if (result.Code == 0)
            {
                webDevice = result.Data;
            }
            ViewBag.Action = "Update";
            ViewBag.ReturnUrl = returnUrl;
            return View(webDevice);
        }



    }
}
