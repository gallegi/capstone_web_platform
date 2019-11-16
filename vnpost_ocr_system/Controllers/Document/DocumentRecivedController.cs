using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentRecivedController : Controller
    {
        // GET: DocumentRecived
        [Route("ho-so/ho-so-da-nhan")]
        public ActionResult Index()
        {
                using(VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<Province> proList = db.Provinces.ToList();
                ViewBag.proList = proList;

            }
                return View("/Views/Document/DocumentRecived.cshtml");
        }


        public class recieve : Order
        {
            public string PostalProvinceName { get; set; }
            public string ProfileName { get; set; }
            public string PublicAdministrationName { get; set; }
            public string Phone { get; set; }
        }

        [Route("da-tiep-nhan")]
        [HttpPost]
        public ActionResult Search(string province, string district, string profile, string )
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<recieve> searchList = null;
            int totalrows = 0;
            int totalrowsafterfiltering = 0;
            string query = "";

            query += "Select * From "

        }
    }
}