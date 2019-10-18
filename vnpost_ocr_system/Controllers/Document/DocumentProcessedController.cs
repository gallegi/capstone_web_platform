using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace vnpost_ocr_system.Controllers.Document
{
   
    public class DocumentProcessedController : Controller
    {
        // GET: DocumentProcessed
        public ActionResult DocumentProcessed()
        {
            return View("/Views/Document/DocumentProcessed.cshtml");
            //This is Bach
        }
    }
}