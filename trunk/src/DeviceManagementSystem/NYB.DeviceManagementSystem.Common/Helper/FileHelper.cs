using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NYB.DeviceManagementSystem.Common.Helper
{
    public class FileHelper
    {

        public static string SaveFile(HttpPostedFileBase fileData, string relativePath, string fileName)
        {
            if (fileData == null || string.IsNullOrWhiteSpace(relativePath) || string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var appPath = SystemInfo.BaseDirectory;
            var absolutePath = Path.Combine(appPath, relativePath);
            var absoluteFullPath = Path.Combine(absolutePath, fileName);
            try
            {
                if (!Directory.Exists(absolutePath))
                {
                    Directory.CreateDirectory(absolutePath);
                }
                if (File.Exists(absoluteFullPath))
                {
                    File.Delete(absoluteFullPath);
                }
                fileData.SaveAs(absoluteFullPath);
                return Path.Combine(relativePath, fileName);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                return string.Empty;
            }
        }

        public static void DelFile(string relativeFullPath)
        {
            if (string.IsNullOrWhiteSpace(relativeFullPath))
            {
                return;
            }

            try
            {
                var absoluteFullPath = Path.Combine(SystemInfo.BaseDirectory, relativeFullPath);
                if (File.Exists(absoluteFullPath))
                {
                    File.Delete(absoluteFullPath);
                }
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
            }
        }
    }
}
