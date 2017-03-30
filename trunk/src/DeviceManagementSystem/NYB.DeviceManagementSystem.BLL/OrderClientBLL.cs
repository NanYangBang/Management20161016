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
using System.Web;
using System.IO;

namespace NYB.DeviceManagementSystem.BLL
{
    public class OrderClientBLL
    {
        public CResult<List<WebOrderClient>> GetOrderClientList(out int totalCount, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<OrderClient, bool>> filter = t => t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.OrderClientName.ToUpper().Contains(searchInfo));
                }

                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "CreateDate";
                    ascending = false;
                }

                var temp = context.OrderClient.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebOrderClient()
                {
                    CreateDate = t.CreateDate,
                    ID = t.ID,
                    Note = t.Note,
                    Name = t.OrderClientName,
                }).ToList();

                var ids = result.Select(t => t.ID).ToList();
                var users = context.User.Where(t => t.Role == (int)RoleType.客户管理员 && ids.Contains(t.OrderClientID)).Select(user => new WebUser
                {
                    ID = user.UserID,
                    Address = user.Address,
                    Email = user.Email,
                    LoginName = user.LoginName,
                    TelPhone = user.Telephone,
                    Moblie = user.Moblie,
                    UserName = user.Name,
                    OrderClientID = user.OrderClientID,
                }).ToList();

                foreach (var user in users)
                {
                    var project = result.FirstOrDefault(t => t.ID == user.OrderClientID);
                    project.WebUser = user;
                }

                LogHelper.Info("result", result);

                return new CResult<List<WebOrderClient>>(result);
            }
        }

        public CResult<bool> InsertOrderClient(WebOrderClient webOrderClient)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webOrderClient", webOrderClient);

            using (var context = new DeviceMgmtEntities())
            {
                if (context.OrderClient.Any(t => t.OrderClientName.ToUpper() == webOrderClient.Name.ToUpper() && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.OrderClientNameIsExist);
                }

                var orderClient = new OrderClient();
                orderClient.CreateDate = DateTime.Now;
                orderClient.CreateUserID = webOrderClient.CreateUserID;
                orderClient.ID = Guid.NewGuid().ToString();
                orderClient.OrderClientName = webOrderClient.Name;
                orderClient.Note = webOrderClient.Note;
                orderClient.IsValid = true;

                var webUser = webOrderClient.WebUser;
                webUser.CreateUserID = webOrderClient.CreateUserID;

                var userLoginName = webUser.LoginName.ToUpper();
                if (context.User.Any(t => t.LoginName.ToUpper() == userLoginName))
                {
                    return new CResult<bool>(false, ErrorCode.LoginNameIsExist);
                }

                var entity = new User()
                {
                    UserID = Guid.NewGuid().ToString(),
                    LoginName = webUser.LoginName,
                    Password = webUser.Pwd,
                    Name = webUser.UserName,
                    ProjectID = string.Empty,
                    OrderClientID = orderClient.ID,
                    Address = webUser.Address,
                    Telephone = webUser.TelPhone,
                    CreateDate = DateTime.Now,
                    CreateUserID = webUser.CreateUserID,
                    Email = webUser.Email,
                    IsValid = true,
                    Role = (int)RoleType.客户管理员
                };
                context.OrderClient.Add(orderClient);
                context.User.Add(entity);

                return context.Save();
            }
        }

        public CResult<bool> UpdateOrderClientInfo(WebOrderClient webOrderClient)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webOrderClient", webOrderClient);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.OrderClient.FirstOrDefault(t => t.ID == webOrderClient.ID);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                var name = webOrderClient.Name.ToUpper();
                if (context.OrderClient.Any(t => t.OrderClientName.ToUpper() == name && t.IsValid && t.ID != webOrderClient.ID))
                {
                    return new CResult<bool>(false, ErrorCode.OrderClientNameIsExist);
                }

                entity.OrderClientName = webOrderClient.Name;
                entity.Note = webOrderClient.Note;

                var user = context.User.FirstOrDefault(t => t.Role == (int)RoleType.客户管理员 && t.OrderClientID == webOrderClient.ID);
                if (user == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                var webUser = webOrderClient.WebUser;
                user.Address = webUser.Address;
                user.Email = webUser.Email;
                user.Name = webUser.UserName;
                user.Telephone = webUser.TelPhone;
                user.Moblie = webUser.Moblie;

                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebOrderClient> GetOrderClientInfoByID(string orderClientID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("orderClientID", orderClientID);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.OrderClient.FirstOrDefault(t => t.ID == orderClientID);
                if (entity == null)
                {
                    return new CResult<WebOrderClient>(null, ErrorCode.DataNoExist);
                }

                var webOrderClient = new WebOrderClient()
                {
                    CreateDate = entity.CreateDate,
                    ID = entity.ID,
                    Note = entity.Note,
                    Name = entity.OrderClientName,
                };

                var user = context.User.FirstOrDefault(t => t.Role == (int)RoleType.客户管理员 && t.OrderClientID == webOrderClient.ID);
                var webUser = new WebUser
                {
                    ID = user.UserID,
                    Address = user.Address,
                    Email = user.Email,
                    LoginName = user.LoginName,
                    TelPhone = user.Telephone,
                    Moblie = user.Moblie,
                    UserName = user.Name,
                };

                webOrderClient.WebUser = webUser;

                LogHelper.Info("result", webOrderClient);

                return new CResult<WebOrderClient>(webOrderClient);
            }
        }

        public CResult<bool> DeleteOrderClient(string orderClientID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("orderClientID", orderClientID);

            using (var context = new DeviceMgmtEntities())
            {
                var orderClient = context.OrderClient.FirstOrDefault(t => t.ID == orderClientID);
                if (orderClient == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                orderClient.IsValid = false;

                var entity = context.User.FirstOrDefault(t => t.Role == (int)RoleType.客户管理员 && t.OrderClientID == orderClientID);
                if (entity != null)
                {
                    var projects = context.Project.Where(t => t.IsValid && t.CreateUserID == entity.UserID).ToList();
                    foreach (var project in projects)
                    {
                        project.IsValid = false;
                    }

                    var projectIDs = projects.Select(t => t.ID).ToList();
                    var users = context.User.Where(t => t.IsValid && projectIDs.Contains(t.ProjectID));
                    foreach (var user in users)
                    {
                        user.IsValid = false;
                    }

                    entity.IsValid = false;
                }

                return context.Save();
            }
        }

        public static CResult<WebOrderClient> GetCompanyInfo(string orderClientID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("orderClientID", orderClientID);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.OrderClient.FirstOrDefault(t => t.ID == orderClientID);
                if (entity == null)
                {
                    return new CResult<WebOrderClient>(null, ErrorCode.DataNoExist);
                }

                var webOrderClient = new WebOrderClient()
                {
                    LogoFile = entity.LogoFile,
                    CompanyContact = entity.CompanyContact,
                    CompanyDescribe = entity.CompanyDescribe,
                    CompanyName = entity.CompanyName,
                    ID = entity.ID,
                };

                LogHelper.Info("result", webOrderClient);

                return new CResult<WebOrderClient>(webOrderClient);
            }
        }

        public CResult<bool> UpdateCompanyInfo(WebOrderClient companyInfo)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("companyInfo", companyInfo);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.OrderClient.FirstOrDefault(t => t.ID == companyInfo.ID);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.CompanyContact = companyInfo.CompanyContact;
                entity.CompanyDescribe = companyInfo.CompanyDescribe;
                entity.CompanyName = companyInfo.CompanyName;

                context.Entry(entity).State = EntityState.Modified;

                return context.Save();
            }
        }

        public CResult<bool> UpdateCompayLogo(HttpPostedFileBase file, string orderClientID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("orderClientID", orderClientID);

            using (var context = new DeviceMgmtEntities())
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                var filePath = FileHelper.SaveFile(file, Path.Combine(SystemInfo.UploadFolder, date), fileName);
                if (string.IsNullOrEmpty(filePath) == false)
                {
                    var orderClient = context.OrderClient.FirstOrDefault(t => t.IsValid && t.ID == orderClientID);
                    if (string.IsNullOrEmpty(orderClient.LogoFile) == false)
                    {
                        FileHelper.DelFile(filePath);
                    }
                    orderClient.LogoFile = filePath;
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.SaveFileFailed);
                }

                if (context.SaveChanges() > 0)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    FileHelper.DelFile(filePath);
                }
                return new CResult<bool>(true);
            }
        }
    }
}
