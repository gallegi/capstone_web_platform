using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class ConfirmInforController : Controller
    {
        // GET: ConfirmInfor
        [Route("giay-hen/xac-nhan-thong-tin-giay-hen")]
        public ActionResult Index()
        {
            return View();
        }
    }
}