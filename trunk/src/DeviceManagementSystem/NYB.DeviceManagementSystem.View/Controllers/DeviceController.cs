using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NYB.DeviceManagementSystem.ViewModel;
using Webdiyer.WebControls.Mvc;
using NYB.DeviceManagementSystem.BLL;
using System.IO;
using System.Diagnostics;
using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.Common.Enum;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class DeviceController : Controller
    {
        //
        // GET: /Device/

        public ActionResult Index(string searchInfo, DeviceStateEnum? deviceStateEnum = null, int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            List<WebDevice> device = new List<WebDevice>();
            int totalCount = 0;
            DeviceBLL deviceBLL = new DeviceBLL();
            var cResult = deviceBLL.GetDeviceList(out totalCount, this.GetCurrentProjectID(), searchInfo, deviceStateEnum, pageIndex, pageSize, orderBy, ascending);
            if (cResult.Code == 0)
            {
                device = cResult.Data;
            }
            var pageList = new PagedList<WebDevice>(device, pageIndex, pageSize);
            ViewBag.SearchInfo = searchInfo;
            ViewBag.DeviceStateEnum = deviceStateEnum;
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
            webDevice.CreateUserID = this.GetCurrentUserID();
            webDevice.ProjectID = this.GetCurrentProjectID();
            DeviceBLL deviceBLL = new DeviceBLL();
            var cResult = deviceBLL.InsertDevice(webDevice);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpGet]
        public ActionResult EditDevice(string deviceTypeID, string returnUrl)
        {
            DeviceBLL deviceBLL = new DeviceBLL();
            var result = deviceBLL.GetDeviceByID(deviceTypeID);
            WebDevice webDevice = null;
            if (result.Code == 0)
            {
                webDevice = result.Data;
            }
            ManufacturerBLL manufacturBLL = new ManufacturerBLL();
            SupplierBLL supplierBLL = new SupplierBLL();

            DeviceTypeBLL deviceTypeBll = new DeviceTypeBLL();
            var deviceType = deviceTypeBll.GetDeviceTypeDir(this.GetCurrentProjectID());

            ViewBag.ManufacturList = manufacturBLL.GetManufacturerDir(this.GetCurrentProjectID()).Data;
            ViewBag.SupperList = supplierBLL.GetSupplierDir(this.GetCurrentProjectID()).Data;
            ViewBag.DeviceType = deviceType.Data;
            ViewBag.Action = "Update";
            ViewBag.ReturnUrl = returnUrl;
            return View(webDevice);
            
            
        }

        [HttpPost]
        public ActionResult EditDevice(WebDevice webDevice)
        {
            DeviceBLL deviceBLL = new DeviceBLL();
            var cResult = deviceBLL.UpdateDevice(webDevice);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        public ActionResult Detail(string deviceTypeID, string returnUrl)
        {
            DeviceBLL deviceBLL = new DeviceBLL();
            var result = deviceBLL.GetDeviceByID(deviceTypeID);
            WebDevice webDevice = null;
            if (result.Code == 0)
            {
                webDevice = result.Data;
            }
            ManufacturerBLL manufacturBLL = new ManufacturerBLL();
            SupplierBLL supplierBLL = new SupplierBLL();

            DeviceTypeBLL deviceTypeBll = new DeviceTypeBLL();
            var deviceType = deviceTypeBll.GetDeviceTypeDir(this.GetCurrentProjectID());

            ViewBag.ManufacturList = manufacturBLL.GetManufacturerDir(this.GetCurrentProjectID()).Data;
            ViewBag.SupperList = supplierBLL.GetSupplierDir(this.GetCurrentProjectID()).Data;
            ViewBag.DeviceType = deviceType.Data;
            ViewBag.Action = "Detail";
            ViewBag.ReturnUrl = returnUrl;
            return View(webDevice);
        }

        [HttpGet]
        public ActionResult AddRepairRecord(string deviceID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = deviceID;
            ViewBag.OperateAction = "Add";
            ViewBag.LeftName = "设备信息";
            WebRepairRecord webRepairRecord = new WebRepairRecord();
            webRepairRecord.RepairDate = DateTime.Now.Date;
            return View("RepairRecord/AddRepairRecord", webRepairRecord);
        }

        [HttpPost]
        public ActionResult AddRepairRecord(WebRepairRecord webRepairRecord)
        {
            RepairRecordBLL repairRecord = new RepairRecordBLL();
            webRepairRecord.CreateDate = DateTime.Now;
            webRepairRecord.CreateUserID = this.GetCurrentUserID();
            webRepairRecord.ProjectID = this.GetCurrentProjectID();
            var cResult = repairRecord.InsertRepairRecord(webRepairRecord);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        public ActionResult RepairRecordList(DateTime? startTime, DateTime? endTime, string deviceID = "", string returnUrl = "", string searchInfo = "", int pageIndex = 1, int pageSize = 10, string orderBy = "", bool ascending = false)
        {
            RepairRecordBLL repairRecord = new RepairRecordBLL();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            int totalCount = 0;
            var cResult = repairRecord.GetRepairRecordList(deviceID, out totalCount, this.GetCurrentProjectID(), searchInfo, startTime, endTime, pageIndex, pageSize, orderBy, ascending);

            List<WebRepairRecord> webRepairRecordList = new List<WebRepairRecord>();
            if (cResult.Code == 0)
            {
                webRepairRecordList = cResult.Data;
            }
            ViewBag.SearchInfo = searchInfo;
            var pageList = new PagedList<WebRepairRecord>(webRepairRecordList, pageIndex, pageSize, totalCount);
            return View("RepairRecord/RepairRecordList", pageList);
        }

        [HttpGet]
        public ActionResult EditRepairRecord(string repairRecordID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = "";
            ViewBag.RepairRecordID = repairRecordID;
            ViewBag.OperateAction = "Update";
            ViewBag.LeftName = "维修记录信息";
            WebRepairRecord webRepairRecord = new WebRepairRecord();
            RepairRecordBLL repairRecordBLL = new RepairRecordBLL();
            webRepairRecord = repairRecordBLL.GetRepairRecordByID(repairRecordID).Data;

            return View("RepairRecord/AddRepairRecord", webRepairRecord);
        }

        [HttpGet]
        public ActionResult ViewRepairRecord(string repairRecordID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = "";
            ViewBag.RepairRecordID = repairRecordID;
            ViewBag.OperateAction = "View";
            ViewBag.LeftName = "维修记录信息";
            ViewBag.IsView = "True";
            WebRepairRecord webRepairRecord = new WebRepairRecord();
            RepairRecordBLL repairRecordBLL = new RepairRecordBLL();
            webRepairRecord = repairRecordBLL.GetRepairRecordByID(repairRecordID).Data;

            return View("RepairRecord/PartialView/AddRepairRecordPartialView", webRepairRecord);
        }

        [HttpPost]
        public ActionResult EditRepairRecord(WebRepairRecord webRepairRecord)
        {
            RepairRecordBLL repairRecordBLL = new RepairRecordBLL();
            var cResult = repairRecordBLL.UpdateRepairRecord(webRepairRecord, null);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DelRepairRecord(string repairRecordID)
        {
            RepairRecordBLL repairRecordBLL = new RepairRecordBLL();
            var cResult = repairRecordBLL.DeleteRepairRecord(repairRecordID);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpGet]
        public ActionResult AddMainTainRecord(string deviceID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = deviceID;
            ViewBag.OperateAction = "Add";
            ViewBag.IsAddRecord = "True";
            ViewBag.LeftName = "设备信息";
            WebMaintainRecord webMaintainRecord = new WebMaintainRecord();
            webMaintainRecord.MaintainDate = DateTime.Now.Date;
            return View("MaintainRecord/AddMainTainRecord", webMaintainRecord);
        }

        [HttpPost]
        public ActionResult AddMainTainRecord(WebMaintainRecord webMaintainRecord, List<string> delIDList)
        {
            MaintainRecordBLL maintainRecordBLL = new MaintainRecordBLL();
            webMaintainRecord.CreateUserID = this.GetCurrentUserID();
            webMaintainRecord.ProjectID = this.GetCurrentProjectID();
            var cResult = maintainRecordBLL.InsertMaintainRecord(webMaintainRecord);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpPost]
        public ActionResult AddMaintainRecordFile(HttpPostedFileBase fileData, string maintainRecordID)
        {
            MaintainRecordBLL maintainRecordBLL = new MaintainRecordBLL();
            var cResult = maintainRecordBLL.AddmaintainRecordFile(fileData, maintainRecordID);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpGet]
        public ActionResult EditMaintainRecord(string maintainRecordID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = "";
            ViewBag.OperateAction = "Update";
            ViewBag.IsAddRecord = "False";
            ViewBag.LeftName = "保养记录信息";
            MaintainRecordBLL maintainRecordBLL = new MaintainRecordBLL();
            var cResult = maintainRecordBLL.GetMaintainRecordByID(maintainRecordID);
            return View("MaintainRecord/AddMainTainRecord", cResult.Data);
        }

        [HttpGet]
        public ActionResult ViewMaintainRecord(string maintainRecordID, string deviceName, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DeviceName = deviceName;
            ViewBag.DeviceID = "";
            ViewBag.OperateAction = "Update";
            ViewBag.IsAddRecord = "False";
            ViewBag.LeftName = "保养记录信息";
            ViewBag.IsView = "True";
            MaintainRecordBLL maintainRecordBLL = new MaintainRecordBLL();
            var cResult = maintainRecordBLL.GetMaintainRecordByID(maintainRecordID);
            return View("MaintainRecord/PartialView/AddEditMaintainRecordView", cResult.Data);
        }

        [HttpPost]
        public ActionResult EditMaintainRecord(WebMaintainRecord webMaintainRecord, List<string> delIDList)
        {
            MaintainRecordBLL maintainRecordBLL = new MaintainRecordBLL();
            var cResult = maintainRecordBLL.UpdateMaintainRecord(webMaintainRecord, delIDList);
            return JsonContentHelper.GetJsonContent(cResult);
        }

        public ActionResult MaintainRecordList(DateTime? startTime, DateTime? endTime, string deviceID = "", string returnUrl = "", string searchInfo = "", int pageIndex = 1, int pageSize = 10, string orderBy = ""
            , bool ascending = false)
        {
            ViewBag.ReturnUrl = returnUrl;
            MaintainRecordBLL maintainRecord = new MaintainRecordBLL();
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            int totalCount = 0;
            var cResult = maintainRecord.GetMaintainRecordList(out totalCount, this.GetCurrentProjectID(), searchInfo, startTime, endTime, deviceID, pageIndex, pageSize, orderBy, ascending);

            List<WebMaintainRecord> webRepairRecordList = new List<WebMaintainRecord>();
            if (cResult.Code == 0)
            {
                webRepairRecordList = cResult.Data;
            }
            var pageList = new PagedList<WebMaintainRecord>(webRepairRecordList, pageIndex, pageSize, totalCount);
            ViewBag.SearchInfo = searchInfo;
            return View("MaintainRecord/MaintainRecordList", pageList);
        }

        [HttpGet]
        public void ViewPicture(string filePath = "")
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            Process ardProcess = new Process();
            ardProcess.StartInfo.FileName = absolutePath;
            //ardProcess.StartInfo.Verb = "Print";
            ardProcess.StartInfo.CreateNoWindow = true;
            //ardProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ardProcess.Start();
        }

        public ActionResult DownloadFile(string filePath, string fileName)
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            fileName = Url.Encode(fileName).Replace("+", "%20");
            return File(absolutePath, "application/object", fileName);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var result = new DeviceBLL().DeleteDevice(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteMaintainRecord(string id)
        {
            try
            {
                var result = new MaintainRecordBLL().DeleteMaintainRecord(id);
                return JsonContentHelper.GetJsonContent(result);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase fileData)
        {
            var result = new DeviceBLL().ImportDeviceFromExcel(fileData, this.GetCurrentProjectID(), this.GetCurrentUserID());
            return JsonContentHelper.GetJsonContent(result);
        }

        public ActionResult ExportExcel(string searchInfo)
        {
            var result = new DeviceBLL().ExportDeviceToExcel(this.GetCurrentProjectID(), searchInfo);
            return JsonContentHelper.GetJsonContent(result);

        }

    }
}
