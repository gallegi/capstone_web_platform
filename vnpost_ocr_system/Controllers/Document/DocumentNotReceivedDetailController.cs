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
            Non_revieve order = new Non_revieve();
            string sql = "select o.*, p.*, pro.*, pa.* from [Order] o join" +
                " District d on o.ProcedurerPostalDistrictCode = d.PostalDistrictCode join " +
                "Province p on d.PostalProvinceCode = p.ProvinceCode join " +
                "[Profile] pro on o.ProfileID = pro.ProfileID join " +
                "PublicAdministration pa on pro.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "where o.AppointmentLetterCode = @id";
            order = db.Database.SqlQuery<Non_revieve>(sql, new SqlParameter("id", id)).FirstOrDefault();
            ViewBag.Order = order;
            return View("/Views/Document/DocumentNotReceivedDetail.cshtml");
        }
    }
}