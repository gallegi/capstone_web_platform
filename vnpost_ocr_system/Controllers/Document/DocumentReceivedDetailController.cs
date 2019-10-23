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
        [Route("ho-so/ho-so-da-nhan/chi-tiet")]
        public ActionResult Index()
        {
            return View("/Views/Document/DocumentReceivedDetail.cshtml");
        }
    }
}