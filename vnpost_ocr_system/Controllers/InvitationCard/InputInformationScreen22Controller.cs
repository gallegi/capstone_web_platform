using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen22Controller : Controller
    {
        // GET: InputInformationScreen22
        [Route("giay-hen/nhap-giay-hen/thong-tin-yeu-cau")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputInformationScreen22.cshtml");
        }
    }
}