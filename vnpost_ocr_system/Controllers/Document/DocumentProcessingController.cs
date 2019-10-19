using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentProcessingController : Controller
    {
        // GET: DocumentProcessing
        [Route("ho-so/ho-so-dang-xu-ly")]
        public ActionResult Index()
        {
            return View();
            //test 
            Console.WriteLine(1);
        }
    }
}