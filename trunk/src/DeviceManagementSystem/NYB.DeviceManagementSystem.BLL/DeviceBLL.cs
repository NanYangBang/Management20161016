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
    public class DeviceBLL
    {
        public CResult<List<WebDevice>> GetDeviceList(out int totalCount, string projectID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Device, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                var temp = context.Device.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebDevice()
                {
                    ID = t.ID,
                    Name = t.Name,
                    Note = t.Note,

                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = t.User.Name,
                    ProjectID = t.ProjectID,
                    DeviceTypeID = t.DeviceTypeID,
                    DeviceTypeName = t.DeviceType.Name,
                    MaintainDate = t.MaintainDate,
                    ManufacturerID = t.ManufacturerID,
                    ManufacturerName = t.Manufacturer == null ? "" : t.Manufacturer.Name,
                    ProductDate = t.ProductDate,
                    SupplierID = t.SupplierID,
                    SupplierName = t.Supplier == null ? "" : t.Supplier.Name
                }).ToList();

                return new CResult<List<WebDevice>>(result);
            }
        }

        public CResult<bool> InsertDevice(WebDevice model)
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

                var entity = new Device();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.Name = model.Name;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.DeviceTypeID = model.DeviceTypeID;
                entity.MaintainDate = model.MaintainDate;
                entity.ManufacturerID = model.ManufacturerID;
                entity.ProductDate = model.ProductDate;
                entity.SupplierID = model.SupplierID;

                context.Device.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateDevice(WebDevice model)
        {
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

                entity.Name = model.Name;
                entity.Note = model.Note;
                entity.ProductDate = model.ProductDate;
                entity.MaintainDate = model.MaintainDate;

                entity.DeviceTypeID = model.DeviceTypeID;
                entity.ManufacturerID = model.ManufacturerID;
                entity.SupplierID = model.SupplierID;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebDevice> GetDeviceByID(string DeviceID)
        {
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
                    Name = entity.Name,
                    Note = entity.Note,

                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID,
                    DeviceTypeID = entity.DeviceTypeID,
                    DeviceTypeName = entity.DeviceType.Name,
                    MaintainDate = entity.MaintainDate,
                    ManufacturerID = entity.ManufacturerID,
                    ManufacturerName = entity.Manufacturer == null ? "" : entity.Manufacturer.Name,
                    ProductDate = entity.ProductDate,
                    SupplierID = entity.SupplierID,
                    SupplierName = entity.Supplier == null ? "" : entity.Supplier.Name
                };

                return new CResult<WebDevice>(model);
            }
        }

        public CResult<bool> DeleteDevice(string DeviceID)
        {
            if (string.IsNullOrEmpty(DeviceID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == DeviceID && t.IsValid);
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
