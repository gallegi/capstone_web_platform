using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class OCRController : Controller
    {
        // GET: OCR
        [Route("giay-hen/nhap-giay-hen/ocr")]
        public ActionResult Index()
        {
            if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            return View("/Views/InvitationCard/OCR.cshtml");
        }
    }
}