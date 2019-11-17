using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        //Tuấn: Tôi tạm comment đoạn này đang bị lỗi lại để ae rebuild được, ô fix nhanh nhé
        //GET: DocumentNotReceivedDetail
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
        public ActionResult Update(string itemCode, string status, string note, string id)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction con = db.Database.BeginTransaction())
            {
                try
                {
                    int conId = Convert.ToInt32(id);
                    OrderStatusDetail osd = new OrderStatusDetail();
                    osd.OrderID = Convert.ToInt64(id);
                    osd.StatusID = Convert.ToInt32(status);
                    osd.Note = note;
                    osd.CreatedTime = DateTime.Today;
                    db.OrderStatusDetails.Add(osd);
                    db.SaveChanges();
                    Order o = db.Orders.Where(x => x.OrderID == conId).FirstOrDefault();
                    o.ItemCode = itemCode;
                    o.StatusID = Convert.ToInt32(status);
                    db.Entry(o).State = EntityState.Modified;
                    db.SaveChanges();
                    con.Commit();
                    return Redirect("/ho-so/ho-so-cho-nhan");
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    con.Rollback();
                    return Redirect("/ho-so/ho-so-cho-nhan");
                }
            }
        }
    }
}