using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.SupportClass;
using vnpost_ocr_system.Models;
using System.Data.SqlClient;
using vnpost_ocr_system.Controllers.CustomController;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class SearchStatusController : BaseUserController
    {
        static string mess = "";
        [Route("giay-hen/tim-giay-hen")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.mess = mess;
            mess = "";
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/InvitationCard/SearchStatus.cshtml");
            }
            else
            {
                return View("/Views/InvitationCard/SearchStatus.cshtml");
            }
            
        }
        //[Auther(Roles = "0")]
        [Route("giay-hen/tim-giay-hen")]
        [HttpPost]
        public ActionResult Search(string id)
        {
            string sql = @"select o.AppointmentLetterCode, o.OrderID, s.StatusName
                        from [Order] o join Status s on o.StatusID = s.StatusID
                        where o.AppointmentLetterCode = @id";
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            var list = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", id)).ToList();
            string mess = "";
            if (list.Count == 0)
            {
                mess = "Không tìm thấy kết quả nào với mã giấy hẹn trên";
            }
            return Json(new { success = true, data = list, mes = mess }, JsonRequestBehavior.AllowGet);
        }

        public void getMess(string s)
        {
            mess = s;
        }
    }
}