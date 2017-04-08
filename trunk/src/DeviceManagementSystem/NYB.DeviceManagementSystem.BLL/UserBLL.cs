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
using System.Web.Security;
using System.Data;
using System.Data.Entity.Validation;
using System.Reflection;

namespace NYB.DeviceManagementSystem.BLL
{
    public class UserBLL
    {
        private BusinessModelEnum _businessModel = BusinessModelEnum.用户;
        public CResult<List<WebUser>> GetUserList(out int totalCount, string projectID, string userID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            totalCount = 0;
            if (string.IsNullOrWhiteSpace(projectID))
            {
                return new Common.CResult<List<WebUser>>(new List<WebUser>());
            }

            Expression<Func<User, bool>> filter = t => t.IsValid && t.ProjectID == projectID && t.UserID != userID;

            if (string.IsNullOrWhiteSpace(searchInfo) == false)
            {
                searchInfo = searchInfo.Trim().ToUpper();
                filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo) || t.LoginName.ToUpper().Contains(searchInfo));
            }

            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "CreateDate";
                ascending = false;
            }

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                var userList = context.User.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);
                var result = userList.Select(t => new WebUser()
                {
                    ID = t.UserID,
                    Address = t.Address,
                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = context.User.FirstOrDefault(u => u.UserID == u.UserID).Name,
                    Email = t.Email,
                    LoginName = t.LoginName,
                    TelPhone = t.Telephone,
                    UserName = t.Name,
                    Role = (RoleType)t.Role
                }).ToList();

                return new CResult<List<WebUser>>(result);
            }
        }

        public CResult<WebUser> GetUserInfoByUserID(string userID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("userID", userID);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.IsValid && t.UserID == userID);
                if (entity != null)
                {
                    var webUser = new WebUser()
                    {
                        ID = entity.UserID,
                        Address = entity.Address,
                        Email = entity.Email,
                        LoginName = entity.LoginName,
                        TelPhone = entity.Telephone,
                        UserName = entity.Name,
                        Moblie = entity.Moblie,
                        Role = (RoleType)entity.Role,
                        OrderClientID = entity.OrderClientID,
                    };

                    LogHelper.Info("result", webUser);

                    return new CResult<WebUser>(webUser);
                }
                else
                {
                    return new CResult<WebUser>(null, ErrorCode.DataNoExist);
                }
            }
        }

        public CResult<bool> AddUser(WebUser webUser)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webUser", webUser);

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.ID == webUser.ProjectID && t.IsValid == true) == false)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                var userLoginName = webUser.LoginName.ToUpper();
                if (context.User.Any(u => u.LoginName.ToUpper() == userLoginName && u.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.LoginNameIsExist);
                }

                var entity = new User()
                {
                    UserID = Guid.NewGuid().ToString(),
                    Password = webUser.Pwd,
                    LoginName = webUser.LoginName,
                    Name = webUser.UserName,
                    ProjectID = webUser.ProjectID,
                    Address = webUser.Address,
                    Telephone = webUser.TelPhone,
                    Moblie = webUser.Moblie,
                    CreateDate = DateTime.Now,
                    CreateUserID = webUser.CreateUserID,
                    Email = webUser.Email,
                    IsValid = true,
                    Role = (int)webUser.Role,
                    OrderClientID = webUser.OrderClientID,
                };
                context.User.Add(entity);

                //LoggerBLL.AddLog(context, webUser.CreateUserID, webUser.ProjectID, OperatTypeEnum.添加, _businessModel, "用户名：" + webUser.LoginName);
                return context.Save();
            }
        }

        public CResult<bool> DeleteUserByID(string userID, string operatorUserID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("userID", userID);

            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(operatorUserID))
            {
                return new Common.CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.IsValid && t.UserID == userID);
                if (entity != null)
                {
                    if (entity.Role == (int)RoleType.项目管理员)
                    {
                        return new CResult<bool>(false, ErrorCode.ProjectAdminCannotDelete);
                    }

                    entity.IsValid = false;

                    LoggerBLL.AddLog(context, userID, entity.ProjectID, OperatTypeEnum.删除, _businessModel, "用户名：" + entity.LoginName);

                    return context.Save();
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }
            }
        }

        public CResult<bool> UpdateUser(WebUser webUser)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webUser", webUser);

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.UserID == webUser.ID);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.Address = webUser.Address;
                entity.Email = webUser.Email;
                entity.Name = webUser.UserName;
                entity.Telephone = webUser.TelPhone;
                entity.Moblie = webUser.Moblie;
                entity.Role = (int)webUser.Role;

                context.Entry(entity).State = EntityState.Modified;
                LoggerBLL.AddLog(context, webUser.CreateUserID, entity.ProjectID, OperatTypeEnum.修改, _businessModel, "用户名：" + entity.LoginName);

                return context.Save();
            }
        }

        public CResult<bool> ChangePassword(string oldPassword, string newPassword, string userID, string operatorUserID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(oldPassword))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.UserID == userID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.UserNotExist);
                }

                if (string.Equals(oldPassword, entity.Password))
                {
                    entity.Password = newPassword;
                    return context.Save();
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.ChangePasswordError);
                }

                //LoggerBLL.AddLog(context, operatorUserID, entity.ProjectID, OperatTypeEnum.修改, _businessModel, "用户名：" + entity.LoginName);


            }
        }

        public CResult<bool> ResetPassword(string newPassword, string userID, string operatorUserID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(userID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.UserID == userID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.UserNotExist);
                }

                entity.Password = newPassword;
                context.Entry(entity).State = EntityState.Modified;

                //   LoggerBLL.AddLog(context, operatorUserID, entity.ProjectID, OperatTypeEnum.修改, _businessModel, "用户名：" + userID);
                return context.Save();
            }
        }

        public CResult<WebUser> VerifyPassword(string userName, string password)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            using (var context = new DeviceMgmtEntities())
            {
                var user = context.User.FirstOrDefault(t => t.IsValid && t.LoginName == userName && t.Password == password);
                if (user == null)
                {
                    return new CResult<WebUser>(null, ErrorCode.UserNameOrPasswordWrong);
                }

                WebUser webUser = new WebUser()
                {
                    ID = user.UserID,
                    Address = user.Address,
                    Email = user.Email,
                    LoginName = user.LoginName,
                    TelPhone = user.Telephone,
                    Moblie = user.Moblie,
                    UserName = user.Name,
                    Role = (RoleType)user.Role,
                    OrderClientID = user.OrderClientID,
                };

                if (webUser.Role == RoleType.超级管理员 || webUser.Role == RoleType.客户管理员)
                {
                }
                else
                {
                    var project = context.Project.FirstOrDefault(t => t.IsValid && t.ID == user.ProjectID);

                    if (project == null)
                    {
                        return new CResult<WebUser>(null, ErrorCode.UserNameOrPasswordWrong);
                    }

                    webUser.ProjectID = project.ID;
                }

                LogHelper.Info("result", webUser);

                return new CResult<WebUser>(webUser);
            }
        }
    }
}
