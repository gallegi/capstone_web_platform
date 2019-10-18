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
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/ConfirmInfor.cshtml");
        }
    }
}