using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputReceiverInformationController : Controller
    {
        // GET: InputReceiverInformation
        [Route("giay-hen/nhap-thong-tin-nguoi-nhan")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputReceiverInfomation.cshtml");
        }
    }
}