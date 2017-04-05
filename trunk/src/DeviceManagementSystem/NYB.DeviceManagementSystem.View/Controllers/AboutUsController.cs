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
            var result = OrderClientBLL.GetCompanyInfo(this.GetCurrentOrderClientID());

            return View(result.Data);
        }

        public ActionResult GetCompanyInfoJson()
        {
            var result = OrderClientBLL.GetCompanyInfo(this.GetCurrentOrderClientID());

            return JsonContentHelper.GetJsonContent(result);
        }

        public ActionResult UpdateAboutUs(string returnUrl)
        {
            var result = OrderClientBLL.GetCompanyInfo(this.GetCurrentOrderClientID());

            ViewBag.ReturnUrl = returnUrl;
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult UpdateAboutUs(WebOrderClient webOrderClient)
        {
            webOrderClient.ID = this.GetCurrentOrderClientID();

            var cResult = new OrderClientBLL().UpdateCompanyInfo(webOrderClient);

            if (cResult.Code == 0 && cResult.Data)
            {
                var companyInfo = OrderClientBLL.GetCompanyInfo(this.GetCurrentOrderClientID());

                if (companyInfo.Code == 0)
                {
                    Response.Cookies.Add(new HttpCookie("CompanyName", companyInfo.Data.CompanyName));
                }
            }

            return JsonContentHelper.GetJsonContent(cResult);
        }

        [HttpPost]
        public ActionResult UpdateLogo(HttpPostedFileBase fileData)
        {
            var cResult = new OrderClientBLL().UpdateCompayLogo(fileData, this.GetCurrentOrderClientID());

            if (cResult.Code == 0 && cResult.Data)
            {
                var companyInfo = OrderClientBLL.GetCompanyInfo(this.GetCurrentOrderClientID());

                if (companyInfo.Code == 0)
                {
                    Response.Cookies.Add(new HttpCookie("LogoFileUrl", Url.Content(string.Format("~/{0}", companyInfo.Data.LogoFile))));
                }
            }

            return JsonContentHelper.GetJsonContent(cResult);
        }

    }
}

