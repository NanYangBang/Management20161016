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

namespace NYB.DeviceManagementSystem.BLL
{
    public class ProjectBLL
    {
        public CResult<List<WebProject>> GetProjectList(out int totalCount, string userID, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool acending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Project, bool>> filter = t => true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                context.Project.Where(filter);


                totalCount = 0;

                var result = new List<WebProject>();

                return new CResult<List<WebProject>>(result);
            }
        }

        public CResult<bool> AddUser(WebUser webUser)
        {
            if (Roles.RoleExists(webUser.Role) == false)
            {
                return new CResult<bool>(false, ErrorCode.AddUserFault);
            }

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                MembershipCreateStatus status;
                var currentUser = Membership.CreateUser(webUser.LogoName, webUser.Pwd, webUser.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(currentUser.UserName, webUser.Role);

                    var entity = new User()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        Name = webUser.UserName,
                        ProjectID = webUser.ProjectID,
                        Address = webUser.Address,
                        Telephone = webUser.TelPhone,
                         CreateDate=DateTime.Now,
                          //CreateUser=webUser.CreateUserID,
                           
                    };

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

        //public CResult<bool> IsUserNameExist(string userLoginName)
        //{
        //    if (string.IsNullOrWhiteSpace(userLoginName))
        //    {
        //        return new CResult<bool>(false, ErrorCode.ParameterError);
        //    }

        //    userLoginName = userLoginName.Trim();

        //    using (var context = new DeviceMgmtEntities())
        //    {
        //        if (context.User.Any(u=>u.UserID))
        //        {
                    
        //        }
        //    }
        //}
    }
}
