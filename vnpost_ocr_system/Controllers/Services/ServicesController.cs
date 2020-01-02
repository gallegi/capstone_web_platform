using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Services
{
    public class ServicesController : Controller
    {
        // GET: Services
        [Route("services/thiet-lap-services")]
        public ActionResult Index()
        {
            return View("/Views/Services/Services.cshtml");
        }
    }
}