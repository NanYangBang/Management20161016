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
using NYB.DeviceManagementSystem.Common;
using System.Data;

namespace NYB.DeviceManagementSystem.BLL
{
    public class UserBLL
    {
        private BusinessModelEnum _businessModel = BusinessModelEnum.用户;
        public CResult<List<WebUser>> GetUserList(out int totalCount, string projectID, string userID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            Expression<Func<User, bool>> filter = t => t.ProjectID == projectID && t.UserID != userID && t.IsSuperAdminCreate == false;

            if (string.IsNullOrWhiteSpace(searchInfo) == false)
            {
                searchInfo = searchInfo.Trim().ToUpper();
                filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
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
                    LogoName = t.LoginName,
                    TelPhone = t.Telephone,
                    UserName = t.Name,
                    Role = Roles.GetRolesForUser(t.LoginName).FirstOrDefault(),
                }).ToList();

                return new CResult<List<WebUser>>(result);
            }
        }

        public CResult<WebUser> GetUserInfoByUserID(string userID)
        {
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
                        LogoName = entity.LoginName,
                        TelPhone = entity.Telephone,
                        UserName = entity.Name,
                    };

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
                        LogoName = entity.LoginName,
                        TelPhone = entity.Telephone,
                        UserName = entity.Name,
                    };

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
            if (Roles.RoleExists(webUser.Role) == false)
            {
                return new CResult<bool>(false, ErrorCode.AddUserFault);
            }

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.ID == webUser.ProjectID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                MembershipCreateStatus status;
                var currentUser = Membership.CreateUser(webUser.LogoName, webUser.Pwd, webUser.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(currentUser.UserName, webUser.Role);

                    var entity = new User()
                    {
                        UserID = currentUser.ProviderUserKey.ToString(),
                        LoginName = webUser.LogoName,
                        Name = webUser.UserName,
                        ProjectID = webUser.ProjectID,
                        Address = webUser.Address,
                        Telephone = webUser.TelPhone,
                        CreateDate = DateTime.Now,
                        CreateUserID = webUser.CreateUserID,
                        Email = webUser.Email,
                        IsValid = true,
                        IsSuperAdminCreate = isSuperAdminCreate
                    };

                    LoggerBLL.AddLog(context, webUser.CreateUserID, OperatTypeEnum.添加, _businessModel, "用户名：" + webUser.LogoName);

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
                    catch
                    {
                        Membership.DeleteUser(currentUser.UserName, true);
                        return new CResult<bool>(false, ErrorCode.AddUserFault);
                    }
                }
            }

            return new CResult<bool>(false);
        }
        public CResult<bool> DeleteUserByID(string userID, string operatorUserID)
        {
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

                    LoggerBLL.AddLog(context, userID, OperatTypeEnum.删除, _businessModel, "用户名：" + entity.LoginName);

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

                context.Entry(entity).State = EntityState.Modified;
                LoggerBLL.AddLog(context, webUser.CreateUserID, OperatTypeEnum.修改, _businessModel, "用户名：" + entity.LoginName);

                return context.Save();
            }
        }

        public CResult<bool> ChangePassword(string oldPassword, string newPassword, string loginName, string operatorUserID)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(loginName))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            var user = Membership.GetUser(loginName);
            if (user == null)
            {
                return new CResult<bool>(false, ErrorCode.DataNoExist);
            }

            var flag = user.ChangePassword(oldPassword, newPassword);
            using (var context = new DeviceMgmtEntities())
            {
                LoggerBLL.AddLog(context, operatorUserID, OperatTypeEnum.修改, _businessModel, "用户名：" + loginName);
                context.SaveChanges();
            }

            if (flag)
            {
                return new CResult<bool>(true);
            }
            else
            {
                return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
            }
        }


        //public CResult<bool> IsUserNameExist(string userLoginName)
        //{
        //    if (string.IsNullOrWhiteSpace(userLoginName))
        //    {
        //        return new CResult<bool>(false, ErrorCode.ParameterError);
        //    }

        //    userLoginName = userLoginName.Trim();

        //    using (var context = new DeviceMgmtEntities())
        //    {
        //        if (context.User.Any(u => u.LoginName == userLoginName && u.IsValid))
        //        {
        //            return new CResult<bool>(true);
        //        }
        //        else
        //        {
        //            return new CResult<bool>(false);
        //        }
        //    }
        //}
    }
}
