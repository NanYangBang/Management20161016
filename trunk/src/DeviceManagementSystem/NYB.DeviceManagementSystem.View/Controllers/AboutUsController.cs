using NYB.DeviceManagementSystem.BLL;
using NYB.DeviceManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYB.DeviceManagementSystem.View.Controllers
{
    public class AboutUsController : Controller
    {
        //
        // GET: /AboutUs/

        public ActionResult Index()
        {
            var result = new OrderClientBLL().GetCompanyInfo(this.GetCurrentOrderClientID());

            return View(result.Data);
        }

        public ActionResult UpdateAboutUs(string returnUrl)
        {
            var result = new OrderClientBLL().GetCompanyInfo(this.GetCurrentOrderClientID());

            ViewBag.ReturnUrl = returnUrl;
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult UpdateAboutUs(WebOrderClient webOrderClient)
        {
            webOrderClient.ID = this.GetCurrentOrderClientID();

            var cResult = new OrderClientBLL().UpdateCompanyInfo(webOrderClient);

            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpPost]
        public ActionResult UpdateLogo(HttpPostedFileBase fileData)
        {
            var cResult = new OrderClientBLL().UpdateCompayLogo(fileData, this.GetCurrentOrderClientID());

            return JsonContentHelper.GetJsonContent(cResult);
        }

    }
}

