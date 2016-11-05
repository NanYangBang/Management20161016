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
            return new CResult<bool>(false);

            //Membership 
        }
    }
}
