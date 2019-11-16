using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedDetailController : Controller
    {
        [Route("ho-so/ho-so-cho-nhan/chi-tiet")]
        // GET: DocumentNotReceivedDetail
        public ActionResult Detail(string id)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Non_revieve_detail order = new Non_revieve_detail();
            string sql = "select *,pa.Phone as 'PAPhone',pa.Address as 'PAAddress', po.Phone as 'POPhone', po.Address as 'POAddress' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on po.DistrictCode = d.DistrictCode " +
                        "join Province pro on d.PostalProvinceCode = pro.PostalProvinceCode " +
                        "join PersonalPaperType ppt on o.ProcedurerPersonalPaperTypeID = ppt.PersonalPaperTypeID " +
                        "where o.AppointmentLetterCode = @id";
            order = db.Database.SqlQuery<Non_revieve_detail>(sql, new SqlParameter("id", id)).FirstOrDefault();
            ViewBag.Order = order;
            return View("/Views/Document/DocumentNotReceivedDetail.cshtml");
        }

        [Route("ho-so/ho-so-cho-nhan/chi-tiet/cap-nhat")]
        [HttpPost]
        public ActionResult Update()
        {
            return View("/Views/Document/DocumentNotReceivedDetail.cshtml");
        }
    }
}