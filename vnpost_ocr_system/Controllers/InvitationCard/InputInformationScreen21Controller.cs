using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            List<Province> provinces = db.Provinces.ToList();
            ViewBag.provinces = provinces;

            List<ContactInfoDB> contactInfos = db.Database.SqlQuery<ContactInfoDB>(@"select ci.FullName, ci.Street, ci.Phone, ppt.PersonalPaperTypeName, ci.PersonalPaperNumber, ci.PersonalPaperIssuedDate, ci.PersonalPaperIssuedPlace
from Customer c inner join ContactInfo ci
on c.CustomerID = ci.CustomerID
inner join PersonalPaperType ppt on ci.PersonalPaperTypeID = ppt.PersonalPaperTypeID
where c.CustomerID = @CustomerID", new SqlParameter("CustomerID", 1)).ToList();
            ViewBag.contactInfos = contactInfos;
            return View("/Views/InvitationCard/InputInformationScreen21.cshtml");
        }
    }
}