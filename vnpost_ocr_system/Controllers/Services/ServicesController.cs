using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Services
{
    public class ServicesController : Controller
    {
        // GET: Services
        [Route("services/thiet-lap-services")]
        public ActionResult Index()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            ServiceTimer st = new ServiceTimer();
            List<ServiceTimer> list = db.ServiceTimers.ToList();
            ViewBag.time = list;
            return View("/Views/Services/Services.cshtml");
        }

        public JsonResult Save(string[] hours, string[] minutes, string[] active, string[] id)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string sqlInsert = "insert into ServiceTimer values(@hours,@minutes,@active)";
            string sqlUpdate = "update ServiceTimer set ServiceTimeHour = @hours, ServiceTimeMinute = @minutes, isActive = @active where ServiceTimeID = @id";
            using (DbContextTransaction tran = db.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        if(hours[i].Equals(""))
                        {
                            hours[i] = "0";
                        }
                        if(minutes[i].Equals(""))
                        {
                            minutes[i] = "0";
                        }
                        if (id[i].Equals(""))
                        {
                            db.Database.ExecuteSqlCommand(sqlInsert, new SqlParameter("hours", hours[i]),
                                                                     new SqlParameter("minutes", minutes[i]),
                                                                     new SqlParameter("active", active[i]));
                        }
                        else
                        {
                            db.Database.ExecuteSqlCommand(sqlUpdate, new SqlParameter("hours", hours[i]),
                                                                     new SqlParameter("minutes", minutes[i]),
                                                                     new SqlParameter("id", id[i]),
                                                                     new SqlParameter("active", active[i]));
                        }
                    }
                    tran.Commit();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    tran.Rollback();
                    return null;
                }
            }
        }
        [Route("delete")]
        public JsonResult Delete(string id)
        {
            try
            {
                VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
                string sql = "delete from ServiceTimer where ServiceTimeID = @id";
                db.Database.ExecuteSqlCommand(sql, new SqlParameter("id", id));
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return null;
            }
      
        }
    }
}