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
    public class ProjectBLL
    {
        public CResult<List<WebProject>> GetProjectList(out int totalCount, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<Project, bool>> filter = t => t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Name.ToUpper().Contains(searchInfo));
                }

                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "CreateDate";
                    ascending = false;
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
                var users = context.User.Where(t => t.IsSuperAdminCreate && projectIDs.Contains(t.ProjectID)).Select(user => new WebUser
                {
                    ID = user.UserID,
                    Address = user.Address,
                    Email = user.Email,
                    LoginName = user.LoginName,
                    TelPhone = user.Telephone,
                    Moblie = user.Moblie,
                    UserName = user.Name,
                    ProjectID = user.ProjectID

                }).ToList();

                foreach (var user in users)
                {
                    var project = result.FirstOrDefault(t => t.ID == user.ProjectID);
                    project.WebUser = user;
                }

                LogHelper.Info("result", result);

                return new CResult<List<WebProject>>(result);
            }
        }

        public CResult<bool> InsertProject(WebProject webProject)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webProject", webProject);

            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.Name.ToUpper() == webProject.Name.ToUpper() && t.IsValid))
                {
                    return new CResult<bool>(false, ErrorCode.ProjectNameIsExist);
                }

                var project = new Project();
                project.CreateDate = DateTime.Now;
                project.CreateUserID = webProject.CreateUserID;
                project.ID = Guid.NewGuid().ToString();
                project.Name = webProject.Name;
                project.Note = webProject.Note;
                project.IsValid = true;

                var webUser = webProject.WebUser;
                webUser.CreateUserID = webProject.CreateUserID;
                MembershipCreateStatus status;
                var currentUser = Membership.CreateUser(webUser.LoginName, webUser.Pwd, webUser.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(currentUser.UserName, RoleType.项目管理员.ToString());

                    var entity = new User()
                    {
                        UserID = currentUser.ProviderUserKey.ToString(),
                        LoginName = webUser.LoginName,
                        Name = webUser.UserName,
                        ProjectID = project.ID,
                        Address = webUser.Address,
                        Telephone = webUser.TelPhone,
                        CreateDate = DateTime.Now,
                        CreateUserID = webUser.CreateUserID,
                        Email = webUser.Email,
                        IsValid = true,
                        UserType = (int)RoleType.项目管理员
                    };
                    context.Project.Add(project);
                    context.User.Add(entity);
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
                else if (status == MembershipCreateStatus.DuplicateUserName)
                {
                    return new CResult<bool>(false, ErrorCode.LoginNameIsExist);
                }
                else
                {
                    return new CResult<bool>(false, 1, status.ToString());
                }

                return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
            }
        }

        public CResult<bool> UpdateProjectInfo(WebProject webProject)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("webProject", webProject);

            using (var context = new DeviceMgmtEntities())
            {
                var project = context.Project.FirstOrDefault(t => t.ID == webProject.ID);
                if (project == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                if (context.Project.Any(t => t.Name.ToUpper() == webProject.Name.ToUpper() && t.IsValid && t.ID != webProject.ID))
                {
                    return new CResult<bool>(false, ErrorCode.ProjectNameIsExist);
                }

                project.Name = webProject.Name;
                project.Note = webProject.Note;

                var user = context.User.FirstOrDefault(t => t.UserType == (int)RoleType.项目管理员 && t.ProjectID == webProject.ID);
                if (user == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                var webUser = webProject.WebUser;
                user.Address = webUser.Address;
                user.Email = webUser.Email;
                user.Name = webUser.UserName;
                user.Telephone = webUser.TelPhone;
                user.Moblie = webUser.Moblie;

                context.Entry(project).State = EntityState.Modified;
                return context.Save();
            }
        }

        public CResult<WebProject> GetProjectInfoByID(string projectID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("projectID", projectID);

            using (var context = new DeviceMgmtEntities())
            {
                var project = context.Project.FirstOrDefault(t => t.ID == projectID);
                if (project == null)
                {
                    return new CResult<WebProject>(null, ErrorCode.DataNoExist);
                }

                var webProject = new WebProject()
                {
                    CreateDate = project.CreateDate,
                    ID = project.ID,
                    Note = project.Note,
                    Name = project.Name,
                };

                var user = context.User.FirstOrDefault(t => t.UserType == (int)RoleType.项目管理员 && t.ProjectID == webProject.ID);
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

                webProject.WebUser = webUser;

                LogHelper.Info("result", webProject);

                return new CResult<WebProject>(webProject);
            }
        }

        public CResult<bool> DeleteProject(string projectID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("projectID", projectID);

            using (var context = new DeviceMgmtEntities())
            {
                var project = context.Project.FirstOrDefault(t => t.ID == projectID);
                if (project == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                project.IsValid = false;

                var entity = context.User.FirstOrDefault(t => t.IsValid && t.UserType == (int)RoleType.项目管理员 && t.ProjectID == projectID);
                if (entity != null)
                {
                    //var deleteFlag = Membership.DeleteUser(entity.LoginName);
                    //if (deleteFlag == false)
                    //{
                    //    return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
                    //}

                    entity.IsValid = false;
                }

                return context.Save();
            }
        }
    }
}
