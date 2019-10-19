using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    [Route("giay-hen/ma-giay-hen")]
    public class DisplayCodeController : Controller
    {
        // GET: DisplayCode
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/DisplayCode.cshtml");
        }
    }
}