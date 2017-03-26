using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.Common.Helper;
using NYB.DeviceManagementSystem.Common.Logger;
using NYB.DeviceManagementSystem.DAL;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NYB.DeviceManagementSystem.BLL
{
    public class MaintainItemBLL
    {
        public CResult<List<WebMaintainItem>> GetMaintainItemList(out int totalCount, string projectID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<MaintainItem, bool>> filter = t => t.ProjectID == projectID && t.IsValid;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "CreateDate";
                    ascending = false;
                }

                var temp = context.MaintainItem.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebMaintainItem()
                {
                    ID = t.ID,
                    Note = t.Note,
                    Name = t.Name,

                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = t.User.Name,
                    ProjectID = t.ProjectID,
                }).ToList();

                LogHelper.Info("result", result);

                return new CResult<List<WebMaintainItem>>(result);
            }
        }

        public CResult<List<WebMaintainItem>> GetMaintainItemListByDeviceID(string deviceID)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                var result = (from device in context.Device
                           where device.ID == deviceID
                           join rel in context.DeviceTypeMaintainItemRel on device.DeviceTypeID equals rel.DeviceTypeID
                           join item in context.MaintainItem on rel.MaintainItemID equals item.ID
                           select new WebMaintainItem()
                       {
                           ID = item.ID,
                           Name = item.Name,
                       }).ToList();                

                LogHelper.Info("result", result);

                return new CResult<List<WebMaintainItem>>(result);
            }
        }

        public CResult<bool> InsertMaintainItem(WebMaintainItem model)
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

                if (context.MaintainItem.Any(t => t.Name.ToUpper() == model.Name.ToUpper() && t.ProjectID == model.ProjectID && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.MaintainItemExist);
                }

                var entity = new MaintainItem();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.Name = model.Name;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;

                context.MaintainItem.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateMaintainItem(WebMaintainItem model)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainItem.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                if (context.MaintainItem.Any(t => t.Name.ToUpper() == model.Name.ToUpper() && t.ProjectID == entity.ProjectID && t.IsValid && t.ID != model.ID))
                {
                    return new CResult<bool>(false, ErrorCode.MaintainItemExist);
                }

                entity.Name = model.Name;
                entity.Note = model.Note;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebMaintainItem> GetMaintainItemByID(string maintainItemID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("maintainItemID", maintainItemID);

            if (string.IsNullOrEmpty(maintainItemID))
            {
                return new CResult<WebMaintainItem>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainItem.FirstOrDefault(t => t.ID == maintainItemID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebMaintainItem>(null, ErrorCode.DataNoExist);
                }

                var model = new WebMaintainItem()
                {
                    ID = entity.ID,
                    Note = entity.Note,
                    Name = entity.Name,

                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID,
                };

                LogHelper.Info("result", model);

                return new CResult<WebMaintainItem>(model);
            }
        }

        public CResult<bool> DeleteMaintainItem(string maintainItemID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("maintainItemID", maintainItemID);

            if (string.IsNullOrEmpty(maintainItemID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainItem.FirstOrDefault(t => t.ID == maintainItemID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                if (context.DeviceTypeMaintainItemRel.Any(t=>t.MaintainItemID==entity.ID))
                {
                    return new CResult<bool>(false,ErrorCode.MaintainItemUsed);
                }

                entity.IsValid = false;

                return context.Save();
            }
        }


        public CResult<bool> ImportMaintainItemFromExcel(HttpPostedFileBase file, string projectID, string operatorUserID)
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

            var WebMaintainItemList = new List<WebMaintainItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                int i = 0;
                var WebMaintainItem = new WebMaintainItem();
                WebMaintainItem.Name = row[i++].ToString();

                WebMaintainItem.Note = row[i++].ToString();

                WebMaintainItemList.Add(WebMaintainItem);
            }

            var nameList = WebMaintainItemList.Select(t => t.Name).Distinct().ToList();

            if (nameList.Count < WebMaintainItemList.Count)
            {
                return new CResult<bool>(false, ErrorCode.MaintainItemExist);
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

                if (context.MaintainItem.Any(t => t.ProjectID == projectID && t.IsValid && nameList.Contains(t.Name)))
                {
                    return new CResult<bool>(false, ErrorCode.MaintainItemExist);
                }

                var currentTime = DateTime.Now;
                foreach (var WebMaintainItem in WebMaintainItemList)
                {
                    var item = new MaintainItem()
                    {
                        CreateDate = currentTime,
                        CreateUserID = operatorUserID,
                        ID = Guid.NewGuid().ToString(),
                        IsValid = true,
                        Name = WebMaintainItem.Name,
                        Note = WebMaintainItem.Note,
                        ProjectID = projectID,
                    };

                    context.MaintainItem.Add(item);
                }

                LogHelper.Info("importList", WebMaintainItemList);

                return context.Save();
            }
        }

        public CResult<string> ExportMaintainItemToExcel(string projectID, string searchInfo)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            int totalCount;

            var result = GetMaintainItemList(out totalCount, projectID, searchInfo, 1, -1);
            if (result.Code > 0)
            {
                return new CResult<string>("", result.Code);
            }

            var list = result.Data;

            var dataTable = new DataTable();
            dataTable.Columns.Add("名称");
            dataTable.Columns.Add("备注");

            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                int i = 0;

                row[i++] = item.Name;
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
    }
}