using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.API
{
    public class ListAPIController : Controller
    {
        // GET: ListAPI
        [Route("api/danh-sach-api")]
        public ActionResult Index()
        {
            return View("/Views/API/ListAPI.cshtml");
        }
    }
}