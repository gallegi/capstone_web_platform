using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Home
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Home.cshtml");
            }
            return View("/Views/Home/Home.cshtml");
        }
    }
}