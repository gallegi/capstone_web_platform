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
        [Route("ho-so/ho-so-da-xu-ly")]
        public ActionResult DocumentProcessed()
        {
            return View("/Views/Document/DocumentProcessed.cshtml");
            //TriHPHE130589
        }
    }
}