using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.API
{
    public class EditAPIController : Controller
    {
        // GET: EditAPI
        [Route("api/chinh-sua-api")]
        public ActionResult Index()
        {
            return View("/Views/API/EditAPI.cshtml");
        }
    }
}