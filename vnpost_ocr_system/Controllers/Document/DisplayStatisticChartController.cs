using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DisplayStatisticChartController : Controller
    {
        string provine_ori = "";
        string district_ori = "";
        string hcc_ori = "";
        string profile_ori = "";
        public void load()
        {
            OrderDashBorad odb = new OrderDashBorad();
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string sql = "select distinct * from Province order by PostalProvinceName";
            List<Province> listPro = db.Database.SqlQuery<Province>(sql).ToList();
            ViewBag.listPro = listPro;

            sql = "select distinct " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               "inner join PublicAdministration pa on p.PublicAdministrationLocationID  = pa.PublicAdministrationLocationID " +
               "inner join PostOffice po on pa.PosCode = po.PosCode " +
               "inner join District  d on po.DistrictCode = d.DistrictCode " +
               "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               "inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               "where year(os.CreatedTime) = year(getdate()) and month(os.CreatedTime) = month(getdate()) and day(os.CreatedTime) = day(getdate()) AND ";
            if (provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5); 
            odb = db.Database.SqlQuery<OrderDashBorad>(sql
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.odb = odb;

            string date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.date = date;
            ViewBag.d = date + " - " + date;

            string[] data = date.Split('/');
            string start = data[2] + "/" + data[1] + "/" + data[0];
            string end = data[2] + "/" + data[1] + "/" + data[0];
            sql = "select distinct " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               "inner join PublicAdministration pa on p.PublicAdministrationLocationID  = pa.PublicAdministrationLocationID " +
               "inner join PostOffice po on pa.PosCode = po.PosCode " +
               "inner join District  d on po.DistrictCode = d.DistrictCode " +
               "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               "inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               "where os.CreatedTime between @start and @end AND ";
            if (provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.detail = odb;
        }

        // GET: DisplayStatisticChart
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/thong-ke-tong-quat")]
        public ActionResult Index()
        {
            load();
            return View("/Views/Document/DisplayStatisticChart.cshtml");
        }

        [HttpPost]
        public ActionResult ChangeDate(string start, string end, string provine, string district, string hcc, string profile)
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
            string sql = "select distinct " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               "inner join PublicAdministration pa on p.PublicAdministrationLocationID  = pa.PublicAdministrationLocationID " +
               "inner join PostOffice po on pa.PosCode = po.PosCode " +
               "inner join District  d on po.DistrictCode = d.DistrictCode " +
               "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               "inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               "where os.CreatedTime between @start and @end AND ";
            if (provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            return Json(new { success = true, date = date, odb = odb }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeDate2(string start, string end, string provine, string district, string hcc, string profile)
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
            string sql = "select distinct " +
               "(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
               "(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
               "(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
               "from [Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               "inner join PublicAdministration pa on p.PublicAdministrationLocationID  = pa.PublicAdministrationLocationID " +
               "inner join PostOffice po on pa.PosCode = po.PosCode " +
               "inner join District  d on po.DistrictCode = d.DistrictCode " +
               "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               "inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               "where os.CreatedTime between @start and @end AND ";
            if (provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            return Json(new { success = true, date = date, odb = odb }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeLocation(string provine, string district, string hcc, string profile)
        {
            provine_ori = provine;
            district_ori = district;
            hcc_ori = hcc;
            profile_ori = profile;
            load();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}