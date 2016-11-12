using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.Common;
using NYB.DeviceManagementSystem.DAL;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.BLL
{
    public class LoggerBLL
    {
        public static void AddLog(DeviceMgmtEntities context, string userID, string projectID, OperatTypeEnum operatType, BusinessModelEnum businessModel, string content)
        {
            var entity = new Log()
            {
                BusinessModel = (int)businessModel,
                ID = Guid.NewGuid().ToString(),
                LogContent = content,
                OperatType = (int)operatType,
                UserID = userID,
                OperatDate = DateTime.Now,
                ProjectID = projectID
            };

            context.Log.Add(entity);
        }

        public CResult<List<WebLog>> GetLogList(out int totalCount, string projectID, OperatTypeEnum? operatType = null, BusinessModelEnum? businessModel = null, string searchInfo = "", int pageIndex = 1, int pageSize = 10)
        {
            Expression<Func<Log, bool>> filter = t => t.ProjectID == projectID;

            if (operatType.HasValue)
            {
                filter = filter.And(t => t.OperatType == (int)operatType.Value);
            }

            if (businessModel.HasValue)
            {
                filter = filter.And(t => t.BusinessModel == (int)businessModel.Value);
            }

            if (string.IsNullOrWhiteSpace(searchInfo) == false)
            {
                searchInfo = searchInfo.Trim().ToUpper();
                filter = filter.And(t => t.LogContent.ToUpper().Contains(searchInfo));
            }

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                var result = context.Log.Where(filter).Page(out totalCount, pageIndex, pageSize, t => t.OperatDate, false).Select(t => new WebLog()
                  {
                      BusinessModel = (BusinessModelEnum)t.BusinessModel,
                      Content = t.LogContent,
                      LoginName = t.User.LoginName,
                      Name = t.User.Name,
                      OperatDate = t.OperatDate,
                      OperatType = (OperatTypeEnum)t.OperatType
                  }).ToList();

                return new CResult<List<WebLog>>(result);
            }
        }
    }
}
