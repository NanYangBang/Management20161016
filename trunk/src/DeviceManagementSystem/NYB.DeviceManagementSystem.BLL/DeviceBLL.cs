using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYB.DeviceManagementSystem.Common.Helper;
using NYB.DeviceManagementSystem.Common.Logger;
using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.ViewModel;
using NYB.DeviceManagementSystem.DAL;
using System.Linq.Expressions;
using System.Data;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Reflection;
using NYB.DeviceManagementSystem.Common.Enum;

namespace NYB.DeviceManagementSystem.BLL
{
    public class DeviceBLL
    {
        public CResult<List<WebDevice>> GetDeviceList(out int totalCount, string projectID, string searchInfo, bool isMaintainSearch = false, DeviceStateEnum? deviceStateEnum = null, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Device, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo)
                        || t.Num.ToUpper().Contains(searchInfo)
                        || t.DeviceType.Name.Contains(searchInfo)
                        || (string.IsNullOrEmpty(t.SupplierID) == false && t.Supplier.Name.ToUpper().Contains(searchInfo))
                        || (string.IsNullOrEmpty(t.ManufacturerID) == false && t.Manufacturer.Name.ToUpper().Contains(searchInfo)));
                }

                if (isMaintainSearch)
                {
                    var today = DateTime.Now.Date;
                    filter = filter.And(t => t.DeviceState != (int)DeviceStateEnum.报废 && t.MaintainDate.HasValue && t.MaintainDate.Value <= today);
                }

                if (deviceStateEnum.HasValue)
                {
                    filter = filter.And(t => t.DeviceState == (int)deviceStateEnum.Value);
                }

                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "CreateDate";
                    ascending = false;
                }

                var temp = context.Device.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebDevice()
                {
                    ID = t.ID,
                    Num = t.Num,
                    Note = t.Note,
                    Name = t.Name,
                    DeviceState = (DeviceStateEnum)t.DeviceState,

                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = t.User.Name,
                    ProjectID = t.ProjectID,
                    DeviceTypeID = t.DeviceTypeID,
                    DeviceTypeName = t.DeviceType.Name,
                    MaintainDate = t.MaintainDate,
                    ManufacturerID = t.ManufacturerID,
                    ManufacturerName = string.IsNullOrEmpty(t.ManufacturerID) ? "" : t.Manufacturer.Name,
                    ProductDate = t.ProductDate,
                    SupplierID = t.SupplierID,
                    SupplierName = string.IsNullOrEmpty(t.SupplierID) ? "" : t.Supplier.Name
                }).ToList();

                LogHelper.Info("result", result);

                return new CResult<List<WebDevice>>(result);
            }
        }

        public CResult<bool> InsertDevice(WebDevice model)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ProjectID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.IsValid && t.ID == model.ProjectID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.ProjectNotExist);
                }

                if (context.Device.Any(t => t.Num.ToUpper() == model.Num.ToUpper() && t.ProjectID == model.ProjectID && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceNumIsExist);
                }

                var entity = new Device();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.Num = model.Num;
                entity.Name = model.Name;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.DeviceTypeID = model.DeviceTypeID;
                entity.MaintainDate = model.MaintainDate;
                entity.ManufacturerID = model.ManufacturerID;
                entity.ProductDate = model.ProductDate;
                entity.SupplierID = model.SupplierID;
                entity.DeviceState = (int)model.DeviceState;

                context.Device.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateDevice(WebDevice model)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.Device.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                if (context.Device.Any(t => t.Num.ToUpper() == model.Num.ToUpper() && t.ProjectID == entity.ProjectID && t.IsValid && t.ID != model.ID))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceNumIsExist);
                }

                entity.Num = model.Num;
                entity.Name = model.Name;
                entity.Note = model.Note;
                entity.ProductDate = model.ProductDate;
                entity.MaintainDate = model.MaintainDate;
                entity.DeviceState = (int)model.DeviceState;

                entity.DeviceTypeID = model.DeviceTypeID;
                entity.ManufacturerID = model.ManufacturerID;
                entity.SupplierID = model.SupplierID;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebDevice> GetDeviceByID(string DeviceID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("DeviceID", DeviceID);

            if (string.IsNullOrEmpty(DeviceID))
            {
                return new CResult<WebDevice>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.Device.FirstOrDefault(t => t.ID == DeviceID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebDevice>(null, ErrorCode.DataNoExist);
                }

                var model = new WebDevice()
                {
                    ID = entity.ID,
                    Num = entity.Num,
                    Note = entity.Note,
                    Name = entity.Name,
                    DeviceState = (DeviceStateEnum)entity.DeviceState,

                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID,
                    DeviceTypeID = entity.DeviceTypeID,
                    DeviceTypeName = entity.DeviceType.Name,
                    MaintainDate = entity.MaintainDate,
                    ManufacturerID = entity.ManufacturerID,
                    ManufacturerName = string.IsNullOrEmpty(entity.ManufacturerID) ? "" : entity.Manufacturer.Name,
                    ProductDate = entity.ProductDate,
                    SupplierID = entity.SupplierID,
                    SupplierName = string.IsNullOrEmpty(entity.SupplierID) == false ? "" : entity.Supplier.Name
                };

                LogHelper.Info("result", model);

                return new CResult<WebDevice>(model);
            }
        }

        public CResult<bool> DeleteDevice(string DeviceID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("DeviceID", DeviceID);

            if (string.IsNullOrEmpty(DeviceID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.Device.FirstOrDefault(t => t.ID == DeviceID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.IsValid = false;

                return context.Save();
            }
        }

        public CResult<bool> DeleteDeviceBatch(List<string> DeviceIDList)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("DeviceIDList", DeviceIDList);

            if (DeviceIDList == null || DeviceIDList.Count == 0)
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entityList = context.Device.Where(t => DeviceIDList.Contains(t.ID)).ToList();

                if (entityList.Count != DeviceIDList.Count)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                foreach (var entity in entityList)
                {
                    entity.IsValid = false;                    
                }

                return context.Save();
            }
        }

        public CResult<bool> ImportDeviceFromExcel(HttpPostedFileBase file, string projectID, string operatorUserID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            if (string.IsNullOrEmpty(projectID) || string.IsNullOrEmpty(operatorUserID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
            var filePath = FileHelper.SaveFile(file, SystemInfo.TempFileFolder, fileName);
            if (string.IsNullOrEmpty(filePath))
            {
                return new CResult<bool>(false, ErrorCode.SystemError);
            }

            var dataTable = ExcelHelper.ExcelToDataTable(filePath, 0);
            if (dataTable.Rows.Count == 0)
            {
                return new CResult<bool>(false, ErrorCode.FileContainNoData);
            }

            LogHelper.Info("读取数据行数为" + dataTable.Rows.Count);

            var webDeviceList = new List<WebDevice>();
            foreach (DataRow row in dataTable.Rows)
            {
                int i = 0;
                var webDevice = new WebDevice();
                webDevice.Num = row[i++].ToString();
                webDevice.Name = row[i++].ToString();
                webDevice.DeviceTypeName = row[i++].ToString();
                DeviceStateEnum state;
                if (Enum.TryParse<DeviceStateEnum>(row[i++].ToString(), out state))
                {
                    webDevice.DeviceState = state;
                }
                else
                {
                    webDevice.DeviceState = DeviceStateEnum.未使用;
                }
                webDevice.SupplierName = row[i++].ToString();
                webDevice.ManufacturerName = row[i++].ToString();

                DateTime tempTime;
                if (DateTime.TryParse(row[i++].ToString(), out tempTime))
                {
                    webDevice.ProductDate = tempTime;
                }

                if (DateTime.TryParse(row[i++].ToString(), out tempTime))
                {
                    webDevice.MaintainDate = tempTime;
                }
                webDevice.Note = row[i++].ToString();

                webDeviceList.Add(webDevice);
            }

            var deviceTypeNameList = webDeviceList.Select(t => t.DeviceTypeName).Distinct().ToList();
            var supplierNameList = webDeviceList.Select(t => t.SupplierName).Distinct().ToList();
            var manufacturerNameList = webDeviceList.Select(t => t.ManufacturerName).Distinct().ToList();
            var deviceNumList = webDeviceList.Select(t => t.Num).Distinct().ToList();

            if (deviceNumList.Count < webDeviceList.Count)
            {
                return new CResult<bool>(false, ErrorCode.DeviceNumIsExist);
            }


            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.IsValid && t.ID == projectID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.ProjectNotExist);
                }

                if (context.User.Any(t => t.IsValid && t.UserID == operatorUserID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.UserNotExist);
                }

                if (context.Device.Any(t => t.ProjectID == projectID && t.IsValid && deviceNumList.Contains(t.Num)))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceNumIsExist);
                }

                var deviceTypeList = context.DeviceType.Where(t => t.IsValid && t.ProjectID == projectID && deviceTypeNameList.Contains(t.Name)).Select(t => new { t.ID, t.Name }).ToList();
                if (deviceTypeList.Count < deviceTypeNameList.Count)
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeNotExist);
                }

                var supplierList = context.Supplier.Where(t => t.IsValid && t.ProjectID == projectID && supplierNameList.Contains(t.Name)).Select(t => new { t.ID, t.Name }).ToList();
                if (supplierList.Count < supplierNameList.Count)
                {
                    return new CResult<bool>(false, ErrorCode.SupplierNotExist);
                }

                var manufacturerList = context.Manufacturer.Where(t => t.IsValid && t.ProjectID == projectID && manufacturerNameList.Contains(t.Name)).Select(t => new { t.ID, t.Name }).ToList();
                if (manufacturerList.Count < manufacturerNameList.Count)
                {
                    return new CResult<bool>(false, ErrorCode.ManufacturerNotExist);
                }

                var currentTime = DateTime.Now;
                foreach (var webDevice in webDeviceList)
                {
                    var device = new Device()
                    {
                        CreateDate = currentTime,
                        CreateUserID = operatorUserID,
                        DeviceTypeID = deviceTypeList.FirstOrDefault(t => t.Name == webDevice.DeviceTypeName).ID,
                        ID = Guid.NewGuid().ToString(),
                        IsValid = true,
                        MaintainDate = webDevice.MaintainDate,
                        ManufacturerID = manufacturerList.FirstOrDefault(t => t.Name == webDevice.ManufacturerName).ID,
                        Num = webDevice.Num,
                        Name = webDevice.Name,
                        DeviceState = (int)webDevice.DeviceState,
                        Note = webDevice.Note,
                        ProductDate = webDevice.ProductDate,
                        ProjectID = projectID,
                        SupplierID = supplierList.FirstOrDefault(t => t.Name == webDevice.SupplierName).ID,
                    };

                    context.Device.Add(device);
                }

                LogHelper.Info("importList", webDeviceList);

                return context.Save();
            }
        }

        public CResult<string> ExportDeviceToExcel(string projectID, string searchInfo, DeviceStateEnum? deviceStateEnum = null, bool isMaintainSearch = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            int totalCount;

            var result = GetDeviceList(out totalCount, projectID, searchInfo, isMaintainSearch, deviceStateEnum, 1, -1);
            if (result.Code > 0)
            {
                return new CResult<string>("", result.Code);
            }

            var list = result.Data;

            var dataTable = new DataTable();
            dataTable.Columns.Add("编号");
            dataTable.Columns.Add("名称");
            dataTable.Columns.Add("设备类型");
            dataTable.Columns.Add("设备状态");
            dataTable.Columns.Add("供应商");
            dataTable.Columns.Add("生产厂商");
            dataTable.Columns.Add("生产日期", typeof(DateTime));
            dataTable.Columns.Add("保养日期", typeof(DateTime));
            dataTable.Columns.Add("备注");

            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                int i = 0;

                row[i++] = item.Num;
                row[i++] = item.Name;
                row[i++] = item.DeviceTypeName;
                row[i++] = item.DeviceState.ToString();
                row[i++] = item.SupplierName;
                row[i++] = item.ManufacturerName;
                if (item.ProductDate.HasValue)
                {
                    row[i++] = item.ProductDate;
                }
                else
                {
                    row[i++] = DBNull.Value;
                }
                if (item.MaintainDate.HasValue)
                {
                    row[i++] = item.MaintainDate;
                }
                else
                {
                    row[i++] = DBNull.Value;
                }
                row[i++] = item.Note;

                dataTable.Rows.Add(row);
            }

            var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), ".xls");
            var relativePath = Path.Combine(SystemInfo.TempFileFolder, fileName);

            var isSuccess = ExcelHelper.DataTableToExcel(dataTable, relativePath);

            if (isSuccess)
            {
                return new CResult<string>(relativePath);
            }
            else
            {
                return new CResult<string>("", ErrorCode.SystemError);
            }
        }

        public static CResult<int> GetMaintainCount(string projectID)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                var today = DateTime.Now.Date;

                var result = context.Device.Count(t => t.ProjectID == projectID && t.IsValid && t.DeviceState != (int)DeviceStateEnum.报废 && t.MaintainDate.HasValue && t.MaintainDate.Value <= today);

                LogHelper.Info("result", result);

                return new CResult<int>(result);
            }
        }
    }
}
