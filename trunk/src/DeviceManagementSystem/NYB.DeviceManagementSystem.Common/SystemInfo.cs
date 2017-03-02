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
        /// 日期格式yyyyMMddhhmmss
        /// </summary>
        public static string DateFormat { get { return "yyyyMMddHHmmssfff"; } }


        /// <summary>
        /// 上传文件的根目录
        /// </summary>
        public static string UploadFolder { get { return "UploadFile\\"; } }

        /// <summary>
        /// 临时文件文件夹
        /// </summary>
        public static string TempFileFolder { get { return "TempFile\\"; } }

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
    }
}
