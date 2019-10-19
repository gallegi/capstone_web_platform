using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class DisplayStatusController : Controller
    {
        // GET: DisplayStatus
        [Route("giay-hen/trang-thai-giay-hen")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/DisplayStatus.cshtml");
        }
    }
}