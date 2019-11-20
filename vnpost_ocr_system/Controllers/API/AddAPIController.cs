using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.CustomCSS.API
{
    public class AddAPIController : Controller
    {
        // GET: AddAPI
        [Auther(Roles = "1")]
        [Route("api/them-moi-api")]
        public ActionResult Index()
        {
            return View("/Views/API/AddAPI.cshtml");
        }
    }
}