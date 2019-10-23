using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentReceivedDetailController : Controller
    {
        // GET: DocumentReceivedDetail
        [Route("ho-so/chi-tiet-ho-so-da-xu-ly")]
        public ActionResult Index()
        {
            return View("/Views/Document/DocumentReceivedDetail.cshtml");
        }
    }
}