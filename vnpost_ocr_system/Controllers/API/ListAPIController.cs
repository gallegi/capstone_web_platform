using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.API
{
    public class ListAPIController : Controller
    {
        // GET: ListAPI
        [Auther(Roles = "1")]
        [Route("api/danh-sach-api")]
        public ActionResult Index()
        {
            return View("/Views/API/ListAPI.cshtml");
        }
    }
}