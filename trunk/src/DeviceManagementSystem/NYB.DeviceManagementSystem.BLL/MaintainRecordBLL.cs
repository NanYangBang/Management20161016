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
using System.Web;
using System.IO;

namespace NYB.DeviceManagementSystem.BLL
{
    public class MaintainRecordBLL
    {
        public CResult<List<WebMaintainRecord>> GetMaintainRecordList(out int totalCount, string projectID, string searchInfo, string deviceID = "", int pageIndex = 1, int pageSize = 10, string orderby = null, bool ascending = false)
        {
            using (DeviceMgmtEntities context = new DeviceMgmtEntities())
            {
                Expression<Func<MaintainRecord, bool>> filter = t => t.ProjectID == projectID && t.IsValid == true;

                if (string.IsNullOrWhiteSpace(searchInfo) == false)
                {
                    searchInfo = searchInfo.Trim().ToUpper();
                    filter = filter.And(t => t.Note.ToUpper().Contains(searchInfo));
                }
                if (!string.IsNullOrWhiteSpace(deviceID))
                {
                    filter = filter.And(t => t.DeviceID == deviceID);
                }

                var temp = context.MaintainRecord.Where(filter).Page(out totalCount, pageIndex, pageSize, orderby, ascending, true);

                var result = temp.Select(entity => new WebMaintainRecord()
                {
                    ID = entity.ID,
                    Note = entity.Note,
                    DeviceID = entity.DeviceID,
                    DeviceName = entity.Device.Name,
                    Operator = entity.Operator,
                    MaintainDate = entity.MaintainDate,
                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID,
                }).ToList();

                return new CResult<List<WebMaintainRecord>>(result);
            }
        }

        public CResult<bool> InsertMaintainRecord(WebMaintainRecord model, List<HttpPostedFileBase> files)
        {
            if (string.IsNullOrEmpty(model.ProjectID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.IsValid && t.ID == model.ProjectID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.ProjectNotExist);
                }

                if (context.Device.Any(t => t.ID == model.DeviceID) == false)
                {
                    return new CResult<bool>(false, ErrorCode.DeviceNotExist);
                }

                var entity = new MaintainRecord();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.MaintainDate = model.MaintainDate;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.Operator = model.Operator;
                entity.DeviceID = model.DeviceID;

                context.MaintainRecord.Add(entity);

                var attachments = new List<Attachment>();
                foreach (var fileData in files)
                {
                    var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(fileData.FileName));
                    var filePath = FileHelper.SaveFile(fileData, SystemInfo.UploadFolder, fileName);
                    if (string.IsNullOrEmpty(filePath) == false)
                    {
                        attachments.Add(new Attachment()
                        {
                            DisplayName = fileData.FileName,
                            FilePath = filePath,
                            ID = Guid.NewGuid().ToString(),
                            Note = "",
                            RelationID = entity.ID
                        });
                    }
                    else
                    {
                        foreach (var item in attachments)
                        {
                            FileHelper.DelFile(item.FilePath);
                        }

                        return new CResult<bool>(false, ErrorCode.SaveFileFailed);
                    }
                }

                foreach (var attach in attachments)
                {
                    context.Attachment.Add(attach);
                }

                if (context.SaveChanges() > 0)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    foreach (var item in attachments)
                    {
                        FileHelper.DelFile(item.FilePath);
                    }

                    return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
                }
            }
        }

        public CResult<string> InsertMaintainRecord(WebMaintainRecord model)
        {
            if (string.IsNullOrEmpty(model.ProjectID))
            {
                return new CResult<string>("", ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                if (context.Project.Any(t => t.IsValid && t.ID == model.ProjectID) == false)
                {
                    return new CResult<string>("", ErrorCode.ProjectNotExist);
                }

                if (context.Device.Any(t => t.ID == model.DeviceID) == false)
                {
                    return new CResult<string>("", ErrorCode.DeviceNotExist);
                }

                var entity = new MaintainRecord();
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = model.CreateUserID;
                entity.ID = Guid.NewGuid().ToString();
                entity.MaintainDate = model.MaintainDate;
                entity.IsValid = true;
                entity.Note = model.Note;
                entity.ProjectID = model.ProjectID;
                entity.Operator = model.Operator;
                entity.DeviceID = model.DeviceID;

                context.MaintainRecord.Add(entity);

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

        public CResult<bool> AddmaintainRecordFile(HttpPostedFileBase file, string maintainRecordID)
        {
            using (var context = new DeviceMgmtEntities())
            {
                var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                var filePath = FileHelper.SaveFile(file, SystemInfo.UploadFolder, fileName);
                if (string.IsNullOrEmpty(filePath) == false)
                {
                    var fileMode = new Attachment()
                    {
                        DisplayName = file.FileName,
                        FilePath = filePath,
                        ID = Guid.NewGuid().ToString(),
                        Note = "",
                        RelationID = maintainRecordID,
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

        public CResult<bool> UpdateMaintainRecord(WebMaintainRecord model, List<HttpPostedFileBase> files, List<string> deleteFiles)
        {
            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainRecord.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<bool>(false, ErrorCode.DataNoExist);
                }

                entity.Note = model.Note;
                entity.Operator = model.Operator;
                entity.MaintainDate = model.MaintainDate;
                context.Entry(entity).State = EntityState.Modified;

                var needDelete = context.Attachment.Where(t => deleteFiles.Contains(t.ID)).ToList();
                foreach (var item in needDelete)
                {
                    context.Attachment.Remove(item);
                    FileHelper.DelFile(item.FilePath);
                }

                var attachments = new List<Attachment>();
                foreach (var fileData in files)
                {
                    var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(fileData.FileName));
                    var filePath = FileHelper.SaveFile(fileData, SystemInfo.UploadFolder, fileName);
                    if (string.IsNullOrEmpty(filePath) == false)
                    {
                        attachments.Add(new Attachment()
                        {
                            DisplayName = fileData.FileName,
                            FilePath = filePath,
                            ID = Guid.NewGuid().ToString(),
                            Note = "",
                            RelationID = entity.ID
                        });
                    }
                    else
                    {
                        foreach (var item in attachments)
                        {
                            FileHelper.DelFile(item.FilePath);
                        }

                        return new CResult<bool>(false, ErrorCode.SaveFileFailed);
                    }
                }

                foreach (var attach in attachments)
                {
                    context.Attachment.Add(attach);
                }

                if (context.SaveChanges() > 0)
                {
                    return new CResult<bool>(true);
                }
                else
                {
                    foreach (var item in attachments)
                    {
                        FileHelper.DelFile(item.FilePath);
                    }

                    return new CResult<bool>(false, ErrorCode.SaveDbChangesFailed);
                }
            }
        }

        public CResult<string> UpdateMaintainRecord(WebMaintainRecord model, List<string> deleteFiles)
        {
            if (string.IsNullOrEmpty(model.ID))
            {
                return new CResult<string>("", ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainRecord.FirstOrDefault(t => t.ID == model.ID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<string>("", ErrorCode.DataNoExist);
                }

                entity.Note = model.Note;
                entity.Operator = model.Operator;
                entity.MaintainDate = model.MaintainDate;
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
                if (context.SaveChanges() > 0)
                {
                    return new CResult<string>("");
                }
                else
                {
                    return new CResult<string>("", ErrorCode.SaveDbChangesFailed);
                }
            }
        }

        public CResult<WebMaintainRecord> GetMaintainRecordByID(string maintainRecordID)
        {
            if (string.IsNullOrEmpty(maintainRecordID))
            {
                return new CResult<WebMaintainRecord>(null, ErrorCode.ParameterError);
            }

            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainRecord.FirstOrDefault(t => t.ID == maintainRecordID && t.IsValid);
                if (entity == null)
                {
                    return new CResult<WebMaintainRecord>(null, ErrorCode.DataNoExist);
                }

                var model = new WebMaintainRecord()
                {
                    ID = entity.ID,
                    Note = entity.Note,
                    DeviceID = entity.DeviceID,
                    DeviceName = entity.Device.Name,
                    Operator = entity.Operator,
                    MaintainDate = entity.MaintainDate,
                    CreateDate = entity.CreateDate,
                    CreateUserID = entity.CreateUserID,
                    CreateUserName = entity.User.Name,
                    ProjectID = entity.ProjectID
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

                return new CResult<WebMaintainRecord>(model);
            }
        }

        public CResult<bool> DeleteMaintainRecord(string maintainRecordID)
        {
            if (string.IsNullOrEmpty(maintainRecordID))
            {
                return new CResult<bool>(false, ErrorCode.ParameterError);
            }
            using (var context = new DeviceMgmtEntities())
            {
                var entity = context.MaintainRecord.FirstOrDefault(t => t.ID == maintainRecordID && t.IsValid);
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
