using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.DAL;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace NYB.DeviceManagementSystem.BLL
{
    public class DatabaseInitHelper
    {
        public void InitDB()
        {
            return;

            Membership.DeleteUser("SuperAdmin");

            using (var context = new DeviceMgmtEntities())
            {
                foreach (var item in Enum.GetNames(typeof(RoleType)))
                {
                    if (Roles.RoleExists(item) == false)
                    {
                        Roles.CreateRole(item);
                    }
                }

                User superAdmin;
                if (Membership.FindUsersByName("SuperAdmin").Count == 0)
                {
                    Membership.CreateUser("SuperAdmin", "111111");
                    Roles.AddUserToRole("SuperAdmin", RoleType.超级管理员.ToString());
                    superAdmin = new User()
                   {
                       Address = "",
                       CreateDate = DateTime.Now,
                       CreateUserID = "",
                       Email = "",
                       IsSuperAdminCreate = false,
                       IsValid = true,
                       LoginName = "SuperAdmin",
                       ProjectID = "",
                       UserID = Guid.NewGuid().ToString(),
                       Name = "SuperAdmin",
                       Telephone = "",
                   };

                    context.User.Add(superAdmin);
                }
                else
                {
                    superAdmin = context.User.FirstOrDefault(t => t.LoginName == "SuperAdmin");
                }

                var project = context.Project.FirstOrDefault(t => t.Name == "DefaultProject");
                if (project == null)
                {
                    project = new Project()
                    {
                        CreateDate = DateTime.Now,
                        CreateUserID = superAdmin.UserID,
                        ID = Guid.NewGuid().ToString(),
                        Name = "DefaultProject",
                        Note = "DefaultProject",
                        IsValid = true
                    };
                    context.Project.Add(project);
                    context.SaveChanges();

                    var projectUser = new WebUser()
                    {
                        Address = "",
                        CreateDate = DateTime.Now,
                        CreateUserID = superAdmin.UserID,
                        Email = "",
                        ID = "BD3C6C02-8F92-4627-BC5E-A9606CCEF94D",
                        LogoName = "DefaultProjectAdmin",
                        ProjectID = project.ID,
                        Pwd = "111111",
                        Role = RoleType.管理员.ToString(),
                        TelPhone = "",
                        UserName = "DefaultProjectAdmin",
                    };
                    new UserBLL().AddUser(projectUser, true);
                }
            }
        }
    }
}
