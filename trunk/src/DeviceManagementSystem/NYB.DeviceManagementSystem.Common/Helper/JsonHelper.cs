using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NYB.DeviceManagementSystem.Common.Helper
{
    public class JsonHelper
    {
        //private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public static string JsonSerializer<T>(T target)
        {
            var result = JsonConvert.SerializeObject(target);

            return result;
        }
    }
}
