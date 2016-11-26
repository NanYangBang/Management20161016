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
    public class SupplierBLL
    {
        public CResult<List<WebSupplier>> GetSupplierList(out int totalCount, string projectID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Supplier, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                var temp = context.Supplier.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebSupplier()
                {
                    ID = t.ID,
                    Name = t.Name,
                    Address = t.Address,
                    Contact = t.Contact,
                    Mobile = t.Mobile,
                    Phone = t.Phone,
                    Note = t.Note,
                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = t.User.Name,
                    ProjectID = t.ProjectID
                }).ToList();

                return new CResult<List<WebSupplier>>(result);
            }
        }

        public CResult<bool> InsertSupplier(WebSupplier model)
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

                var entity = new Supplier();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.Name = model.Name;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.Address = model.Address;
                entity.Contact = model.Contact;
                entity.Mobile = model.Mobile;
                entity.Phone = model.Phone;

                context.Supplier.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateSupplier(WebSupplier model)
        {
            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.Supplier.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.Name = model.Name;
                entity.Note = model.Note;
                entity.Address = model.Address;
                entity.Contact = model.Contact;
                entity.Mobile = model.Mobile;
                entity.Phone = model.Phone;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebSupplier> GetSupplierByID(string SupplierID)
        {
            if (string.IsNullOrEmpty(SupplierID))
            {
                return new CResult<WebSupplier>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.Supplier.FirstOrDefault(t => t.ID == SupplierID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebSupplier>(null, ErrorCode.DataNoExist);
                }

                var model = new WebSupplier()
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    Address = entity.Address,
                    Contact = entity.Contact,
                    Mobile = entity.Mobile,
                    Phone = entity.Phone,
                    Note = entity.Note,
                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID
                };

                return new CResult<WebSupplier>(model);
            }
        }

        public CResult<bool> DeleteSupplier(string SupplierID)
        {
            if (string.IsNullOrEmpty(SupplierID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.DeviceType.FirstOrDefault(t => t.ID == SupplierID && t.IsValid);
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
