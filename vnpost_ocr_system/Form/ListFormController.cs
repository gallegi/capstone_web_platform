using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Form
{
    public class ListFormController : Controller
    {
        // GET: ListForm
        [Route("bieu-mau/list-bieu-mau")]
        public ActionResult Index()
        {
            return View("/Views/Form/ListFormView.cshtml");
        }
    }
}