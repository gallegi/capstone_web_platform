using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DisplayStatisticChartController : Controller
    {
        
        // GET: DisplayStatisticChart
        [Route("ho-so/thong-ke-tong-quat")]
        public ActionResult Index()
        {
            OrderDashBorad odb = new OrderDashBorad();
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string sql = "select * from Province order by PostalProvinceName";
            List<Province> listPro = db.Database.SqlQuery<Province>(sql).ToList();
            ViewBag.listPro = listPro;

            sql = "select " +
                "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
                "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
                "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
                "from [Order] o  " +
                "where year(o.ModifiedTime) = year(getdate()) and month(o.ModifiedTime) = month(getdate()) and day(o.ModifiedTime) = day(getdate())";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql).FirstOrDefault();
            if(odb == null) odb = new OrderDashBorad();
            ViewBag.odb = odb;

            string date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.date = date;
            ViewBag.d = date + " - " + date;

            string[] data = date.Split('/');
            string start = data[2] + "/" + data[1] + "/" + data[0];
            string end = data[2] + "/" + data[1] + "/" + data[0];
            sql = "select " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  " +
               "where o.ModifiedTime between @start and @end";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.detail = odb;
            return View("/Views/Document/DisplayStatisticChart.cshtml");
        }

        [HttpPost]
        public ActionResult ChangeDate(string start, string end)
        {
            OrderDashBorad odb = new OrderDashBorad();
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (start == "") start = date;
            if (end == "") end = date;
            date = start + " - " + end;
            string[] data = start.Split('/');
            start = data[2] + "/" + data[1] + "/" + data[0];
            data = end.Split('/');
            end = data[2] + "/" + data[1] + "/" + data[0];
            string sql = "select " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  " +
               "where o.ModifiedTime between @start and @end";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            return Json(new { success = true, date = date, odb = odb }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeDate2(string start, string end)
        {
            OrderDashBorad odb = new OrderDashBorad();
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (start == "") start = date;
            if (end == "") end = date;
            date = start + " - " + end;
            string[] data = start.Split('/');
            start = data[2] + "/" + data[1] + "/" + data[0];
            data = end.Split('/');
            end = data[2] + "/" + data[1] + "/" + data[0];
            string sql = "select " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  " +
               "where o.ModifiedTime between @start and @end";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            return Json(new { success = true, date = date, odb = odb }, JsonRequestBehavior.AllowGet);
        }

    }
}