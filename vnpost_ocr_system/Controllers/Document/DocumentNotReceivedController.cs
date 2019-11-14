using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedController : Controller
    {
        // GET: DocumentNotReceived
        [Route("ho-so/ho-so-cho-nhan")]
        public ActionResult Index()
        {
            using(VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<Province> proList = db.Provinces.ToList();
                ViewBag.proList = proList;
                List<District> disList = db.Districts.ToList();
            }
            return View("/Views/Document/DocumentNotReceived.cshtml");
        }
    }
}