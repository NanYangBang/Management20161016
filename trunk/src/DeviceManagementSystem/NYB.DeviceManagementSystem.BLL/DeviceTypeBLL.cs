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
using System.Reflection;

namespace NYB.DeviceManagementSystem.BLL
{
    public class DeviceTypeBLL
    {
        public CResult<List<WebDeviceType>> GetDeviceTypeList(out int totalCount, string projectID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<DeviceType, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo) || t.User.LoginName.ToUpper().Contains(searchInfo));
                }

                var temp = context.DeviceType.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebDeviceType()
                {
                    CreateDate = t.CreateDate,
                    ID = t.ID,
                    Name = t.Name,
                    Note = t.Note,
                    CreateUserName = t.User.LoginName,
                    CreateUserID = t.CreateUserID,
                    ProjectID = t.ProjectID
                }).ToList();

                LogHelper.Info("result", result);

                return new CResult<List<WebDeviceType>>(result);
            }
        }

        public CResult<Dictionary<string, string>> GetDeviceTypeDir(string projectID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("projectID", projectID);

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<DeviceType, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                var temp = context.DeviceType.Where(filter);

                var result = temp.Select(t => new
                {
                    ID = t.ID,
                    Name = t.Name,
                }).ToList().OrderBy(t => t.Name).ToDictionary(t => t.ID, r => r.Name);

                LogHelper.Info("result", result);

                return new CResult<Dictionary<string, string>>(result);
            }
        }

        public CResult<bool> InsertDeviceType(WebDeviceType model)
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

                if (context.DeviceType.Any(t => t.Name.ToUpper() == model.Name.ToUpper() && t.ProjectID == model.ProjectID && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeNameIsExist);
                }

                var entity = new DeviceType();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.Name = model.Name;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;

                context.DeviceType.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateDeviceType(WebDeviceType model)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeNotExist);
                }

                if (context.DeviceType.Any(t => t.Name.ToUpper() == model.Name.ToUpper() && t.ProjectID == model.ProjectID && t.IsValid && t.ID == model.ID))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeNameIsExist);
                }

                entity.Name = model.Name;
                entity.Note = model.Note;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebDeviceType> GetDeviceTypeByID(string deviceTypeID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("deviceTypeID", deviceTypeID);

            if (string.IsNullOrEmpty(deviceTypeID))
            {
                return new CResult<WebDeviceType>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == deviceTypeID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebDeviceType>(null, ErrorCode.DeviceTypeNotExist);
                }

                var model = new WebDeviceType()
                {
                    CreateDate = entity.CreateDate,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjectID = entity.ProjectID,
                    Note = entity.Note
                };

                LogHelper.Info("result", model);

                return new CResult<WebDeviceType>(model);
            }
        }

        public CResult<bool> DeleteDeviceType(string deviceTypeID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("deviceTypeID", deviceTypeID);

            if (string.IsNullOrEmpty(deviceTypeID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == deviceTypeID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeNotExist);
                }

                if (context.Device.Any(t => t.DeviceTypeID == entity.ID && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.DeviceTypeConatinDevice);
                }

                entity.IsValid = false;

                return context.Save();
            }
        }
    }
}
