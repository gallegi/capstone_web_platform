using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputSenderInfoController : Controller
    {
        // GET: InputSenderInfo
        [Route("giay-hen/nhap-giay-hen/thong-tin-nguoi-gui")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputSenderInfo.cshtml");
        }
    }
}