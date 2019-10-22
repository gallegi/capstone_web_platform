using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedDetailController : Controller
    {
        [Route("ho-so/ho-so-cho-nhan/chi-tiet")]
        // GET: DocumentNotReceivedDetail
        public ActionResult Index()
        {
            return View("/Views/Document/DocumentNotReceivedDetail.cshtml");
        }
    }
}