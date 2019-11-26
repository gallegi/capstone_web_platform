using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class SearchStatusController : Controller
    {
        static string mess = "";
        [Route("giay-hen/tim-giay-hen")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.mess = mess;
            mess = "";
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/InvitationCard/SearchStatus.cshtml");
            }
            else
            {
                return View("/Views/InvitationCard/SearchStatus.cshtml");
            }
            
        }
        [Auther(Roles = "0")]
        [Route("giay-hen/tim-giay-hen")]
        [HttpPost]
        public void Search(string id)
        {
            DisplayStatusController ds = new DisplayStatusController();
            ds.Display(id);
        }

        public void getMess(string s)
        {
            mess = s;
        }
    }
}