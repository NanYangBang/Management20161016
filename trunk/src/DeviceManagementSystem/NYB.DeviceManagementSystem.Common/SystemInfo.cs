using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NYB.DeviceManagementSystem.Common
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// 目前系统LOGO
        /// </summary>
        public static string CurrentSystemLogo { get { return @"UploadFile/Logo/current-logo.png"; } }

        /// <summary>
        /// 默认系统LOGO
        /// </summary>
        public static string DefaultSystemLogo { get { return @"UploadFile/Logo/default-logo.png"; } }

        /// <summary>
        /// 供预览的系统LOGO
        /// </summary>
        public static string PreviewSystemLogo { get { return @"UploadFile/Logo/preview-logo.png"; } }

        /// <summary>
        /// 施工阶段在Session中的Name
        /// </summary>
        public static string ConstructionSectionSessionName { get { return "ConstructionSection"; } }

        public static string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }

        /// <summary>
        /// 更新PickupNewRecordTime的时间间隔(单位：小时)
        /// </summary>
        public static double UpdatePickupNewRecordTimeSpan { get { return 8; } }

        /// <summary>
        /// 日期格式yyyyMMddhhmmss
        /// </summary>
        public static string DateFormat { get { return "yyyyMMddHHmmssfff"; } }

        /// <summary>
        /// 短信发送成功的结果文字-"发送成功"
        /// </summary>
        public static string SmsSendSuccessResult { get { return "发送成功"; } }

        /// <summary>
        /// 上传文件的根目录
        /// </summary>
        public static string UploadFolder { get { return "UploadFile\\"; } }

        public static string NewUploadFolder { get { return "UploadFile/"; } }

        public static string FilesFolder { get { return string.Format("{0}Files/", NewUploadFolder); } }

        /// <summary>
        /// 桩基文件上传目录
        /// </summary>
        public static string PileUploadFolder { get { return string.Format("{0}Pile\\", UploadFolder); } }

        /// <summary>
        /// 进度文件目录
        /// </summary>
        public static string ScheduleFolder { get { return string.Format("{0}Schedule\\", UploadFolder); } }

        /// <summary>
        /// 进度图片文件目录
        /// </summary>
        public static string SchedulePhotoFolder { get { return string.Format("{0}Photo\\", ScheduleFolder); } }

        /// <summary>
        /// 进度图片缩略图文件夹的名称
        /// </summary>
        public static string ScheduleThumbnaiFolderName { get { return "Thumbnai"; } }

        /// <summary>
        /// 周报文件根目录
        /// </summary>
        public static string WeekReportFolder { get { return string.Format("{0}WeekReport\\", UploadFolder); } }

        public static string WeekReportPlanFolder { get { return string.Format("{0}Project", WeekReportFolder); } }

        public static string InformationFileFolder { get { return string.Format("{0}Information\\", UploadFolder); } }

        /// <summary>
        /// 安全管理根目录
        /// </summary>
        public static string SafetyFolder { get { return string.Format("{0}Safety/", NewUploadFolder); } }

        /// <summary>
        /// 安全管理视图模型目录
        /// </summary>
        public static string SafetyModelFolder { get { return string.Format("{0}Model/", SafetyFolder); } }

        /// <summary>
        /// 材料文件夹
        /// </summary>
        public static string MaterialFolder { get { return string.Format("{0}Material/", NewUploadFolder); } }

        /// <summary>
        /// 材料出厂证明文件夹
        /// </summary>
        public static string MaterialMakerCertificateFolder { get { return string.Format("{0}MakerCertificate/", MaterialFolder); } }

        /// <summary>
        /// 材料检验证明文件夹
        /// </summary>
        public static string MaterialVerifyCertificateFolder { get { return string.Format("{0}VerifyCertificate/", MaterialFolder); } }

        /// <summary>
        /// 合同文件夹
        /// </summary>
        public static string ContractFolder { get { return string.Format("{0}Contract/", NewUploadFolder); } }

        /// <summary>
        /// 合同模型文件夹
        /// </summary>
        public static string ContractModelFolder { get { return string.Format("{0}Model/", ContractFolder); } }

        /// <summary>
        /// 变更文件夹
        /// </summary>
        public static string ChangeFolder { get { return string.Format("{0}Change/", NewUploadFolder); } }

        /// <summary>
        /// 变更报价单文件夹
        /// </summary>
        public static string ChangeQuoteFormFolder { get { return string.Format("{0}QuoteForm/", ChangeFolder); } }

        /// <summary>
        /// 变更通知单文件夹
        /// </summary>
        public static string ChangeNoticeFormFolder { get { return string.Format("{0}NoticeForm/", ChangeFolder); } }

        /// <summary>
        /// 变更其他文件文件夹
        /// </summary>
        public static string ChangeOtherFileFolder { get { return string.Format("{0}OtherFile/", ChangeFolder); } }

        /// <summary>
        /// 支付管理文件夹
        /// </summary>
        public static string PaymentFolder { get { return string.Format("{0}Payment/", NewUploadFolder); } }

        /// <summary>
        /// 支付申请文件夹
        /// </summary>
        public static string PaymentApplyFolder { get { return string.Format("{0}PaymentApply/", PaymentFolder); } }

        /// <summary>
        /// 4D形象进度文件夹
        /// </summary>
        public static string RealTime4DFolder { get { return string.Format("{0}RealTime4D/", NewUploadFolder); } }

        /// <summary>
        /// 4D形象进度文件
        /// </summary>
        public static string RealTime4DFile { get { return string.Format("{0}{1}", RealTime4DFolder, "RealTime4D.nwd"); } }

        /// <summary>
        /// 甲供管理文件夹
        /// </summary>
        public static string OwnerSupplyFolder { get { return string.Format("{0}OwnerSupply/", NewUploadFolder); } }

        /// <summary>
        /// 甲供管理-物料使用计划文件夹
        /// </summary>
        public static string OwnerSupplyMaterialUsagePlanFolder { get { return string.Format("{0}UsagePlan/", OwnerSupplyFolder); } }

        /// <summary>
        /// 甲供管理-物料使用计划PDF文件名称
        /// </summary>
        public static string OwnerSupplyMaterialUsagePlanPdfFileName { get { return "计划单.pdf"; } }

        /// <summary>
        /// 甲供管理-物料使用计划模板Excel文件
        /// </summary>
        public static string OwnerSupplyMaterialUsagePlanExcelTemplateFile { get { return string.Format("{0}MaterialUsagePlanTemplate.xlsx", FilesFolder); } }

        /// <summary>
        /// 临时文件文件夹
        /// </summary>
        public static string TempFileFolder { get { return string.Format("{0}TempFile/", NewUploadFolder); } }

        /// <summary>
        /// 设计变更文件夹
        /// </summary>
        public static string DesignChangeAuditFolder { get { return string.Format("{0}DesignChangeAudit/", NewUploadFolder); } }

        /// <summary>
        /// 设计变更附件文件夹
        /// </summary>
        public static string DesignChangeAuditAttachmentFolder { get { return string.Format("{0}Attachment/", DesignChangeAuditFolder); } }

        /// <summary>
        /// 设计变更用户上传文件文件夹
        /// </summary>
        public static string DesignChangeUserUploadAuditFolder { get { return string.Format("{0}UserUpload/", DesignChangeAuditFolder); } }

        /// <summary>
        /// 设计变更自动生成文件文件夹
        /// </summary>
        public static string DesignChangeAutoGenerateAuditFolder { get { return string.Format("{0}AutoGenerate/", DesignChangeAuditFolder); } }

        /// <summary>
        /// 设计变更自动生成文件模板
        /// </summary>
        public static string DesignChangeAutoGenerateAuditTemplateFile { get { return string.Format("{0}DesignChangeAuditTemplate.doc", FilesFolder); } }

        /// <summary>
        /// 设计变更自动生成文件扩展名
        /// </summary>
        public static string DesignChangeAutoGenerateAuditFileExtension { get { return ".doc"; } }

        /// <summary>
        /// 合同工程指令内部审批表文件夹
        /// </summary>
        public static string ProjectDirectiveInternalAuditFolder { get { return string.Format("{0}ProjectDirectiveInternalAudit/", NewUploadFolder); } }

        /// <summary>
        /// 合同工程指令内部审批表附件文件夹
        /// </summary>
        public static string ProjectDirectiveInternalAuditAttachmentFolder { get { return string.Format("{0}Attachment/", ProjectDirectiveInternalAuditFolder); } }

        /// <summary>
        /// 合同工程指令内部审批表用户上传文件文件夹
        /// </summary>
        public static string ProjectDirectiveInternalUserUploadAuditFolder { get { return string.Format("{0}UserUpload/", ProjectDirectiveInternalAuditFolder); } }

        /// <summary>
        /// 合同工程指令内部审批表自动生成文件文件夹
        /// </summary>
        public static string ProjectDirectiveInternalAutoGenerateAuditFolder { get { return string.Format("{0}AutoGenerate/", ProjectDirectiveInternalAuditFolder); } }

        /// <summary>
        /// 合同工程指令内部审批表自动生成文件模板
        /// </summary>
        public static string ProjectDirectiveInternalAutoGenerateAuditTemplateFile { get { return string.Format("{0}ProjectDirectiveInternalAuditTemplate.doc", FilesFolder); } }

        /// <summary>
        /// 合同工程指令内部审批表自动生成文件扩展名
        /// </summary>
        public static string ProjectDirectiveInternalAutoGenerateAuditFileExtension { get { return ".doc"; } }

        /// <summary>
        /// 合同工程指令审批表文件夹
        /// </summary>
        public static string ProjectDirectiveAuditFolder { get { return string.Format("{0}ProjectDirectiveAudit/", NewUploadFolder); } }

        /// <summary>
        /// 合同工程指令审批表用户上传文件文件夹
        /// </summary>
        public static string ProjectDirectiveUserUploadAuditFolder { get { return string.Format("{0}UserUpload/", ProjectDirectiveAuditFolder); } }

        /// <summary>
        /// 合同工程指令审批表自动生成文件文件夹
        /// </summary>
        public static string ProjectDirectiveAutoGenerateAuditFolder { get { return string.Format("{0}AutoGenerate/", ProjectDirectiveAuditFolder); } }

        /// <summary>
        /// 合同工程指令审批表自动生成文件模板
        /// </summary>
        public static string ProjectDirectiveAutoGenerateAuditTemplateFile { get { return string.Format("{0}ProjectDirectiveAuditTemplate.doc", FilesFolder); } }

        /// <summary>
        /// 合同工程指令审批表自动生成文件扩展名
        /// </summary>
        public static string ProjectDirectiveAutoGenerateAuditFileExtension { get { return ".doc"; } }

        /// <summary>
        ///地下连续墙生成的Excel的文件夹
        /// </summary>
        public static string UnderWallConInfoNewExportFileFolder { get { return "UploadFile/UnderWallConInfoNew/"; } }

        /// <summary>
        ///地下连续墙上传的混凝土配合比设计报告文件夹
        /// </summary>
        public static string UnderWallConInfoNewConcreteMixDesignReportFile { get { return "UploadFile/UnderWallConInfoNew/{0}/ConcreteMixDesignReportFile/"; } }

        /// <summary>
        ///地下连续墙Excel模板
        /// </summary>
        public static string UnderWallConInfoNewExportFileTemplate { get { return string.Format("{0}地下连续墙检验批样本.xls", FilesFolder); } }

        /// <summary>
        ///TRD报审表
        ///{0}TRDConInfo/报审表.xls
        /// </summary>
        public static string TRDReportAndTrialFileTemplate { get { return string.Format("{0}TRDConInfo/报审表.xls", FilesFolder); } }

        /// <summary>
        ///TRD水泥土搅拌墙地基工程检验批质量验收记录表
        ///{0}TRDConInfo/水泥土搅拌墙地基工程检验批质量验收记录表.doc
        /// </summary>
        public static string TRDFoundationProjCheckFileTemplate { get { return string.Format("{0}TRDConInfo/水泥土搅拌墙地基工程检验批质量验收记录表.doc", FilesFolder); } }

        /// <summary>
        ///TRDTRD工法型钢等厚度水泥土搅拌墙施工记录表
        /// </summary>
        public static string TRDMethodCementConRecordFileTemplate { get { return string.Format("{0}TRDConInfo/TRD工法型钢等厚度水泥土搅拌墙施工记录表.doc", FilesFolder); } }

        /// <summary>
        ///TRD工法水泥土搅拌墙隐蔽工程验收记录
        /// </summary>
        public static string TRDHiddenProjCheckFileTemplate { get { return string.Format("{0}TRDConInfo/TRD工法水泥土搅拌墙隐蔽工程验收记录.doc", FilesFolder); } }

        /// <summary>
        ///TRD生成的文件的文件夹
        ///UploadFile/TRD/
        /// </summary>
        public static string TRDExportFileFolder { get { return "UploadFile/TRD/"; } }

        public static string PDFExtension { get { return ".PDF"; } }
        public static string WordExtension { get { return ".DOC"; } }
        public static string ExcelExtension { get { return ".XLS"; } }
        public static string JPGExtension { get { return ".JPG"; } }
        public static string JPEGExtension { get { return ".JPEG"; } }
        public static string PNGExtension { get { return ".PNG"; } }
        public static string JIFExtension { get { return ".GIF"; } }
        public static string TxtExtension { get { return ".TXT"; } }
        public static string BmpExtension { get { return ".BMP"; } }

        public static string PdfToPicExtension { get { return ".png"; } }

        public static string FileToHtmlExtension { get { return ".htm"; } }

        public static string FileToHtmlDirectionExtension { get { return ".files"; } }


        //计量单位

        /// <summary>
        /// 米
        /// </summary>
        public static string MeterUnits { get { return "m"; } }
        /// <summary>
        /// 毫米
        /// </summary>
        public static string MillimeterUnits { get { return "mm"; } }

    }
}
