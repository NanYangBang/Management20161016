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
using System.Web;
using System.IO;

namespace NYB.DeviceManagementSystem.BLL
{
    public class RepairRecordBLL
    {
        public CResult<List<WebRepairRecord>> GetRepairRecordList(string deviceID, out int totalCount, string projectID, string searchInfo, DateTime? startTime = null, DateTime? endTime = null, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("deviceID", deviceID);
            LogHelper.Info("projectID", projectID);

            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<RepairRecord, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Operator.ToUpper().Contains(searchInfo) || t.Device.Name.Contains(searchInfo));

                    //filter = filter.And(t => t.Note.ToUpper().Contains(searchInfo));
                }
                if (!string.IsNullOrWhiteSpace(deviceID))
                {
                    filter = filter.And(t => t.DeviceID == deviceID);
                }

                if (startTime.HasValue)
                {
                    filter = filter.And(t => t.RepairDate >= startTime);
                }
                if (endTime.HasValue)
                {
                    filter = filter.And(t => t.RepairDate <= endTime);
                }

                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "CreateDate";
                    ascending = false;
                }

                var temp = context.RepairRecord.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(t => new WebRepairRecord()
                {
                    ID = t.ID,
                    Note = t.Note,
                    DeviceID = t.DeviceID,
                    DeviceName = t.Device.Name,
                    Operator = t.Operator,
                    RepairDate = t.RepairDate,
                    CreateDate = t.CreateDate,
                    CreateUserID = t.CreateUserID,
                    CreateUserName = t.User.Name,
                    ProjectID = t.ProjectID
                }).ToList();

                LogHelper.Info("result", result);

                return new CResult<List<WebRepairRecord>>(result);
            }
        }

        //public CResult<List<WebRepairRecord>> GetRepairRecordListByDeviceID(string deviceID, out int totalCount, string searchInfo, int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        //{
        //    LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
        //    LogHelper.Info("deviceID", deviceID);

        //    using (DeviceMgmtEntities context = new DeviceMgmtEntities())
        //    {
        //        Expression<Func<RepairRecord, bool>> filter = t => t.DeviceID == deviceID && t.IsValid == true;

        //        if (string.IsNullOrWhiteSpace(searchInfo) == false)
        //        {
        //            searchInfo = searchInfo.Trim().ToUpper();
        //            filter = filter.And(t => t.Note.ToUpper().Contains(searchInfo));
        //        }

        //        var temp = context.RepairRecord.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

        //        var result = temp.Select(t => new WebRepairRecord()
        //        {
        //            ID = t.ID,
        //            Note = t.Note,
        //            DeviceID = t.DeviceID,
        //            DeviceName = t.Device.Name,
        //            Operator = t.Operator,
        //            RepairDate = t.RepairDate,
        //            CreateDate = t.CreateDate,
        //            CreateUserID = t.CreateUserID,
        //            CreateUserName = t.User.Name,
        //            ProjectID = t.ProjectID
        //        }).ToList();

        //        LogHelper.Info("result", result);

        //        return new CResult<List<WebRepairRecord>>(result);
        //    }
        //}

        public CResult<string> InsertRepairRecord(WebRepairRecord model)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ProjectID))
            {
                return new CResult<string>(string.Empty, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.IsValid && t.ID == model.ProjectID) == false)
                {
                    return new CResult<string>(string.Empty, ErrorCode.ProjectNotExist);
                }

                if (context.Device.Any(t => t.ID == model.DeviceID) == false)
                {
                    return new CResult<string>(string.Empty, ErrorCode.DeviceNotExist);
                }

                var entity = new RepairRecord();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.RepairDate = model.RepairDate;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.Operator = model.Operator;
                entity.DeviceID = model.DeviceID;
                entity.Solution = model.Solution;
                entity.Describe = model.Describe;

                context.RepairRecord.Add(entity);

                if (context.SaveChanges() > 0)
                {
                    return new CResult<string>(entity.ID);
                }
                else
                {
                    return new CResult<string>("", ErrorCode.SaveDbChangesFailed);
                }
            }
        }

        public CResult<bool> AddRepairRecordFile(HttpPostedFileBase file, string repairRecordID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("repairRecordID", repairRecordID);

            using (var context = new DeviceMgmtEntities())
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                var filePath = FileHelper.SaveFile(file, Path.Combine(SystemInfo.UploadFolder, date), fileName);
                if (string.IsNullOrEmpty(filePath) == false)
                {
                    var fileMode = new Attachment()
                    {
                        DisplayName = file.FileName,
                        FilePath = filePath,
                        ID = Guid.NewGuid().ToString(),
                        Note = "",
                        RelationID = repairRecordID,
                    };
                    context.Attachment.Add(fileMode);
                }
                else
                {
                    return new CResult<bool>(false, ErrorCode.SaveFileFailed);
                }

                if (context.SaveChanges() > 0)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    FileHelper.DelFile(filePath);
                }
                return new CResult<bool>(true);
            }
        }

        public CResult<bool> UpdateRepairRecord(WebRepairRecord model, List<string> deleteFiles)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("model", model);

            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.RepairRecord.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.Note = model.Note;
                entity.Operator = model.Operator;
                entity.RepairDate = model.RepairDate;
                entity.Solution = model.Solution;
                entity.Describe = model.Describe;

                context.Entry(entity).State = EntityState.Modified;

                if (deleteFiles != null && deleteFiles.Count() > 0)
                {
                    var needDelete = context.Attachment.Where(t => deleteFiles.Contains(t.ID)).ToList();
                    foreach (var item in needDelete)
                    {
                        context.Attachment.Remove(item);
                        FileHelper.DelFile(item.FilePath);
                    }
                }

                return context.Save();
            }
        }

        public CResult<WebRepairRecord> GetRepairRecordByID(string repairRecordID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("repairRecordID", repairRecordID);

            if (string.IsNullOrEmpty(repairRecordID))
            {
                return new CResult<WebRepairRecord>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.RepairRecord.FirstOrDefault(t => t.ID == repairRecordID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebRepairRecord>(null, ErrorCode.DataNoExist);
                }

                var model = new WebRepairRecord()
                {
                    ID = entity.ID,
                    Note = entity.Note,
                    DeviceID = entity.DeviceID,
                    DeviceName = entity.Device.Name,
                    Operator = entity.Operator,
                    RepairDate = entity.RepairDate,
                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID,
                    Solution = entity.Solution,
                    Describe = entity.Describe,
                };

                var attachments = context.Attachment.Where(a => a.RelationID == entity.ID).ToList();
                foreach (var attachment in attachments)
                {
                    model.Attachments.Add(new WebAttachment()
                    {
                        DisplayName = attachment.DisplayName,
                        FilePath = attachment.FilePath,
                        ID = attachment.ID,
                        Note = attachment.Note
                    });
                }

                LogHelper.Info("result", model);

                return new CResult<WebRepairRecord>(model);
            }
        }

        public CResult<bool> DeleteRepairRecord(string repairRecordID)
        {
            LogHelper.Info(MethodBase.GetCurrentMethod().ToString());
            LogHelper.Info("repairRecordID", repairRecordID);

            if (string.IsNullOrEmpty(repairRecordID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.RepairRecord.FirstOrDefault(t => t.ID == repairRecordID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.IsValid = false;

                var attachments = context.Attachment.Where(a => a.RelationID == entity.ID).ToList();
                foreach (var attachment in attachments)
                {
                    FileHelper.DelFile(attachment.FilePath);
                }

                return context.Save();
            }
        }
    }
}
