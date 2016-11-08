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
    public class ProjectBLL
    {
        public CResult<List<WebProject>> GetProjectList(out int totalCount, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Project, bool>> filter = t => true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                var temp = context.Project.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebProject()
                {
                    CreateDate = t.CreateDate,
                    ID = t.ID,
                    Note = t.Note,
                    Name = t.Name,
                }).ToList();

                var projectIDs = result.Select(t => t.ID).ToList();
                var users = context.User.Where(t => t.IsSuperAdminCreate && projectIDs.Contains(t.ProjectID)).Select(t => new
                {
                    ProjectID = t.ProjectID,
                    LoginName = t.LoginName,
                    Name = t.Name
                }).ToList();

                foreach (var item in result)
                {
                    var target = users.FirstOrDefault(t => t.ProjectID == item.ID);
                    if (target != null)
                    {
                        item.AdminLoginName = target.LoginName;
                    }
                }

                return new CResult<List<WebProject>>(result);
            }
        }

        public CResult<bool> InsertProject(WebProject webProject)
        {
            using (var context = new DeviceMgmtEntities())
            {
                var project = new Project();
                project.Address = webProject.Address;
                project.Contact = webProject.Contact;
                project.CreateDate = DateTime.Now;
                project.CreateUserID = webProject.CreateUserID;
                project.ID = webProject.ID;
                project.Name = webProject.Name;
                project.Note = webProject.Note;
                project.Phone = webProject.Phone;
                project.IsValid = true;

                MembershipCreateStatus status;
                var currentUser = Membership.CreateUser(webProject.AdminLoginName, webProject.Password, null, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(currentUser.UserName, RoleType.管理员.ToString());

                    var entity = new User()
                    {
                        UserID = currentUser.ProviderUserKey.ToString(),
                        LoginName = webProject.AdminLoginName,
                        Name = webProject.Contact,
                        ProjectID = webProject.ID,
                        Address = webProject.Address,
                        Telephone = webProject.Phone,
                        CreateDate = DateTime.Now,
                        CreateUserID = webProject.CreateUserID,
                        Email = "",
                        IsValid = true,
                        IsSuperAdminCreate = true
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

                return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
            }
        }
        public CResult<bool> UpdateProjectInfo(WebProject webProject)
        {
            using (var context = new DeviceMgmtEntities())
            {
                var project = context.Project.FirstOrDefault(t => t.ID == webProject.ID);
                if (project == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                project.Address = webProject.Address;
                project.Contact = webProject.Contact;
                project.Name = webProject.Name;
                project.Note = webProject.Note;
                project.Phone = webProject.Phone;

                context.Entry(project).State = EntityState.Modified;
                return context.Save();
            }
        }
    }
}
