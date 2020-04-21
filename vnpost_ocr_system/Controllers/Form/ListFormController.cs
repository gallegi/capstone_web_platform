using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Form
{
    public class ListFormController : Controller
    {

        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: ListForm
        [Route("bieu-mau/list-bieu-mau")]
        public ActionResult Index()
        {
            var listForms = db.FormTemplates.ToList();
            ViewBag.Forms = listForms;
            return View("/Views/Form/ListFormView.cshtml");
        }
    }
}