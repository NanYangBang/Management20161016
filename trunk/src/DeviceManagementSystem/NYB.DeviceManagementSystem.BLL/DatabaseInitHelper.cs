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
            //Membership.DeleteUser("SuperAdmin");

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
                    var member = Membership.CreateUser("SuperAdmin", "111111");
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
                       UserID = member.ProviderUserKey.ToString(),
                       Name = "SuperAdmin",
                       Telephone = "",
                       Moblie = ""
                   };

                    context.User.Add(superAdmin);
                }
                else
                {
                    superAdmin = context.User.FirstOrDefault(t => t.LoginName == "SuperAdmin");
                }

                context.SaveChanges();
            }
        }
    }
}
