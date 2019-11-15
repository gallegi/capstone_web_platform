using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen21Controller : Controller
    {
        // GET: InputInformationScreen21
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        public ActionResult Index()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> list = db.Provinces.ToList();
            ViewBag.provinces = list;
            return View("/Views/InvitationCard/InputInformationScreen21.cshtml");
        }
    }
}