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

namespace NYB.DeviceManagementSystem.BLL
{
    public class DeviceTypeBLL
    {
        public CResult<List<WebDeviceType>> GetDeviceTypeList(out int totalCount, string projectID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<DeviceType, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                var temp = context.DeviceType.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebDeviceType()
                {
                    CreateDate = t.CreateDate,
                    ID = t.ID,
                    Name = t.Name,
                    Note = t.Note,
                    CreateUserName = t.User.Name,
                    CreateUserID = t.CreateUserID,
                    ProjectID = t.ProjectID
                }).ToList();

                return new CResult<List<WebDeviceType>>(result);
            }
        }

        public CResult<bool> InsertDeviceType(WebDeviceType model)
        {
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
            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == model.ID);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.Name = model.Name;
                entity.Note = model.Note;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebDeviceType> GetDeviceTypeByID(string deviceTypeID)
        {
            if (string.IsNullOrEmpty(deviceTypeID))
            {
                return new CResult<WebDeviceType>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == deviceTypeID);
                if (entity == null)
                {
                    return new CResult<WebDeviceType>(null, ErrorCode.DataNoExist);
                }

                var model = new WebDeviceType()
                {
                    CreateDate = entity.CreateDate,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjectID = entity.ProjectID,
                    Note = entity.Note
                };

                return new CResult<WebDeviceType>(model);
            }
        }

        public CResult<bool> DeleteDeviceType(string deviceTypeID)
        {
            if (string.IsNullOrEmpty(deviceTypeID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == deviceTypeID);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.IsValid = false;

                return context.Save();
            }
        }
    }
}
