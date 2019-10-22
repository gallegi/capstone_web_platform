using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedController : Controller
    {
        // GET: DocumentNotReceived
        [Route("ho-so/ho-so-cho-nhan")]
        public ActionResult Index()
        {
            return View("/Views/Document/DocumentNotReceived.cshtml");
        }
    }
}