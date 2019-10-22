using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentRecivedController : Controller
    {
        // GET: DocumentRecived
        [Route("ho-so/ho-so-da-nhan")]
        public ActionResult Index()
        {
            return View("/Views/Document/DocumentRecived.cshtml");
        }
    }
}