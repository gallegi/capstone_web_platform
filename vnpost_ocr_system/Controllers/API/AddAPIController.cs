using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.CustomCSS.API
{
    public class AddAPIController : Controller
    {
        // GET: AddAPI
        [Route("api/them-moi-api")]
        public ActionResult Index()
        {
            return View("/Views/API/AddAPI.cshtml");
        }
    }
}