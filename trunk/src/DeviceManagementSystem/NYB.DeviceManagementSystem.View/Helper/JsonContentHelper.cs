using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace NYB.DeviceManagementSystem.View
{
    public class JsonContentHelper
    {
        public static ContentResult GetJsonContent(object result, bool json = true)
        {
            var contentResult = new ContentResult();
            contentResult.Content = JsonConvert.SerializeObject(result, new SinaDateTimeConverter());
            if (json)
            {
                contentResult.ContentType = "application/json";
            }
            contentResult.ContentEncoding = Encoding.UTF8;
            return contentResult;
        }
    }
}