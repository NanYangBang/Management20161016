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

            Expression<Func<User, bool>> filter = t => t.IsValid && t.ProjectID == projectID && t.UserID != userID && t.IsSuperAdminCreate == false;

            if (string.IsNullOrWhiteSpace(searchInfo) == false)
            {
                searchInfo = searchInfo.Trim().ToUpper();
                filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo) || t.LoginName.ToUpper().Contains(searchInfo));
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
                }).ToList();

                foreach (var item in result)
                {
                    item.Role = Roles.GetRolesForUser(item.LoginName).FirstOrDefault();
                }

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

        public CResult<WebUser> GetUserInfoByUserName(string userName)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("userName", userName);

            if (string.IsNullOrWhiteSpace(userName))
            {
                return new CResult<WebUser>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.User.FirstOrDefault(t => t.IsValid && t.LoginName == userName);
                if (entity != null)
                {
                    var webUser = new WebUser()
                    {
                        ID = entity.UserID,
                        Address = entity.Address,
                        Email = entity.Email,
                        LoginName = entity.LoginName,
                        TelPhone = entity.Telephone,
                        Moblie = entity.Moblie,
                        UserName = entity.Name,
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

        public CResult<bool> AddUser(WebUser webUser, bool isSuperAdminCreate)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webUser", webUser);
            LogHelper.Info("isSuperAdminCreate", isSuperAdminCreate);

            if (Roles.RoleExists(webUser.Role) == false)
            {
                return new CResult<bool>(false, ErrorCode.AddUserFault);
            }

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

                MembershipCreateStatus status;
                var currentUser = Membership.CreateUser(webUser.LoginName, webUser.Pwd, webUser.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(currentUser.UserName, webUser.Role);

                    var entity = new User()
                    {
                        UserID = currentUser.ProviderUserKey.ToString(),
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
                        IsSuperAdminCreate = isSuperAdminCreate
                    };
                    context.User.Add(entity);

                    LoggerBLL.AddLog(context, webUser.CreateUserID, webUser.ProjectID, OperatTypeEnum.添加, _businessModel, "用户名：" + webUser.LoginName);

                    try
                    {
                        if (context.SaveChanges() > 0)
                        {
                            return new CResult<bool>(true);
                        }
                        else
                        {
                            Membership.DeleteUser(currentUser.UserName, true);
                            return new CResult<bool>(false, ErrorCode.AddUserFault);
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        Membership.DeleteUser(currentUser.UserName, true);
                        return new CResult<bool>(false, ErrorCode.AddUserFault);
                    }
                }
            }

            return new CResult<bool>(false, ErrorCode.ParameterError);
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
                    var deleteFlag = Membership.DeleteUser(entity.LoginName);
                    if (deleteFlag == false)
                    {
                        return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
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

                var user = Membership.GetUser(entity.LoginName);
                if (user == null)
                {
                    return new CResult<bool>(false, ErrorCode.UserNotExist);
                }

                var flag = user.ChangePassword(oldPassword, newPassword);

                LoggerBLL.AddLog(context, operatorUserID, entity.ProjectID, OperatTypeEnum.修改, _businessModel, "用户名：" + entity.LoginName);
                context.SaveChanges();

                if (flag)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
                }
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

                var user = Membership.GetUser(entity.LoginName);
                if (user == null)
                {
                    return new CResult<bool>(false, ErrorCode.UserNotExist);
                }

                var randomPwd = user.ResetPassword();
                var flag = user.ChangePassword(randomPwd, newPassword);

                LoggerBLL.AddLog(context, operatorUserID, entity.ProjectID, OperatTypeEnum.修改, _businessModel, "用户名：" + userID);
                context.SaveChanges();

                if (flag)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
                }
            }
        }

        public CResult<WebUser> VerifyPassword(string userName, string password)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            var isExist = Membership.ValidateUser(userName, password);
            if (isExist == false)
            {
                return new Common.CResult<WebUser>(null, ErrorCode.UserNameOrPasswordWrong);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var user = context.User.FirstOrDefault(t => t.IsValid == true && t.LoginName == userName);
                if (user == null)
                {
                    return new CResult<WebUser>(null, ErrorCode.UserNameOrPasswordWrong);
                }

                var role = Roles.GetRolesForUser(userName).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(role))
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
                    Role = role,
                };

                if (role == RoleType.超级管理员.ToString())
                {
                }
                else
                {
                    var project = context.Project.FirstOrDefault(t => t.IsValid == true && t.ID == user.ProjectID);

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

        public CResult<bool> IsUserNameExist(string userLoginName)
        {
            if (string.IsNullOrWhiteSpace(userLoginName))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            userLoginName = userLoginName.Trim();

            using (var context = new DeviceMgmtEntities())
            {
                userLoginName = userLoginName.ToUpper();
                if (context.User.Any(u => u.LoginName.ToUpper() == userLoginName && u.IsValid))
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    return new CResult<bool>(false);
                }
            }
        }
    }
}
