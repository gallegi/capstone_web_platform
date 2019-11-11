using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen21Controller : Controller
    {
        // GET: InputInformationScreen21
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputInformationScreen21.cshtml");
        }
    }
}