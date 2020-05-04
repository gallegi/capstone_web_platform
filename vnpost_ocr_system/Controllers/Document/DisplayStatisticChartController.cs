using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        OrderDashBorad odb_ori;
        OrderDashBorad odb_ori_year;
        public void load()
        {
            string year = DateTime.Now.ToString("yyyy");
            OrderDashBorad odb = new OrderDashBorad();
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Province p = new Province();
            string sql = "select distinct * from Province order by PostalProvinceName";
            string aid = Session["useradminID"].ToString();
            if (!Session["Role"].ToString().Equals("1") && !Session["Role"].ToString().Equals("2"))
            {
                sql = @"select p.*
                        from Admin a join Province p on a.PostalProvinceCode = p.PostalProvinceCode
                        where a.AdminID = @id";

                p = db.Database.SqlQuery<Province>(sql, new SqlParameter("id", aid)).FirstOrDefault();
                provine_ori = p.PostalProvinceName;
                ViewBag.p = p;
            }
            else
            {
                List<Province> listPro = db.Database.SqlQuery<Province>(sql).ToList();
                ViewBag.listPro = listPro;
            }

            sql = "select (case when sum(a.tong_cho) is null then 0 else sum(a.tong_cho) end) as 'total_cho', (case when sum(a.tong_da) is null then 0 else sum(a.tong_da) end) as 'total_da', (case when sum(a.tong_xong) is null then 0 else sum(a.tong_xong) end) as 'total_xong' " +
                "from(select a.date, " +
               "SUM(case when a.StatusID = -3 and day(a.date) <= day(getdate()) then 1 else 0 end) as 'tong_cho',  " +
               "SUM(case when a.StatusID = -2 and day(a.date) = day(getdate()) then 1 else 0 end) as 'tong_da',  " +
               "SUM(case when a.StatusID = 5 and day(a.date) = day(getdate()) then 1 else 0 end) as 'tong_xong' " +
               "from " +
               "  (select distinct CONVERT(date, os.CreatedTime) as 'date', o.StatusID, o.OrderID  " +
               "from[Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               " inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
               " inner join PostOffice po on pa.PosCode = po.PosCode " +
               " inner join District  d on po.DistrictCode = d.DistrictCode " +
               " inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               " inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               "where year(os.CreatedTime) = year(getdate()) and month(os.CreatedTime) = month(getdate()) AND ";
            //"where year(os.CreatedTime) = 2017 and month(os.CreatedTime) = 4 and day(os.CreatedTime) = 26 AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " ) a group by a.date) a";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.odb = odb;
            odb_ori = odb;

            string date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.date = date;
            string date2 = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            ViewBag.date2 = date2;
            string valyear = DateTime.Now.ToString("yyyy");
            ViewBag.y = valyear;
            ViewBag.d = date + " - " + date;

            string[] data = date.Split('/');
            string start = data[2] + "/" + data[1] + "/" + data[0];
            string end = data[2] + "/" + data[1] + "/" + data[0];
            sql = "select (case when sum(a.tong_cho) is null then 0 else sum(a.tong_cho) end) as 'total_cho', (case when sum(a.tong_da) is null then 0 else sum(a.tong_da) end) as 'total_da', (case when sum(a.tong_xong) is null then 0 else sum(a.tong_xong) end) as 'total_xong' " +
                "from(select a.date, " +
               "SUM(case when a.StatusID = -3 then 1 else 0 end) as 'tong_cho', " +
               "SUM(case when a.StatusID = -2 then 1 else 0 end) as 'tong_da', " +
               "SUM(case when a.StatusID = 5 then 1 else 0 end) as 'tong_xong' " +
               "from " +
               " (select distinct CONVERT(date, os.CreatedTime) as 'date', o.StatusID,o.OrderID " +
               "from[Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               " inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
               " inner join PostOffice po on pa.PosCode = po.PosCode " +
               " inner join District  d on po.DistrictCode = d.DistrictCode " +
               " inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               " inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               " where os.CreatedTime between @start and @end AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " ) a group by a.date) a";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("start", start), new SqlParameter("end", end)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.detail = odb;

            sql = @"select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum'
                        from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID
                        inner join Profile p on o.ProfileID = p.ProfileID
                        inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID
                        inner join PostOffice po on pa.PosCode = po.PosCode
                        inner join District  d on po.DistrictCode = d.DistrictCode
                        inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode
                        where o.StatusID = 5 and year(os.CreatedTime) = @year AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_xong = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).ToList();
            string xong = JsonConvert.SerializeObject(list_xong);
            ViewBag.list_xong = xong;

            sql = @"select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum'
                        from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID
                        inner join Profile p on o.ProfileID = p.ProfileID
                        inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID
                        inner join PostOffice po on pa.PosCode = po.PosCode
                        inner join District  d on po.DistrictCode = d.DistrictCode
                        inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode
                        where o.StatusID = -2 and year(os.CreatedTime) = @year AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_da = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).ToList();
            string da = JsonConvert.SerializeObject(list_da);
            ViewBag.list_da = da;

            sql = @"select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum'
                        from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID
                        inner join Profile p on o.ProfileID = p.ProfileID
                        inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID
                        inner join PostOffice po on pa.PosCode = po.PosCode
                        inner join District  d on po.DistrictCode = d.DistrictCode
                        inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode
                        where o.StatusID = -3 and year(os.CreatedTime) = @year AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_chua = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).ToList();
            string chua = JsonConvert.SerializeObject(list_chua);
            ViewBag.list_chua = chua;

            sql = "select " +
                  " (case when a.total_cho is null then 0 else a.total_cho end) as 'total_cho', " +
                  " (case when a.total_da is null then 0 else a.total_da end) as 'total_da', " +
                  " (case when a.total_xong is null then 0 else a.total_xong end) as 'total_xong' " +
                  " from( " +
                  " select " +
                  "  SUM(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
                  "  SUM(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
                  "  SUM(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
                  "  from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                  "  inner join Profile p on o.ProfileID = p.ProfileID " +
                  "  inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                  "  inner join PostOffice po on pa.PosCode = po.PosCode " +
                  "  inner join District d on po.DistrictCode = d.DistrictCode " +
                  "  inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                  "  where year(os.CreatedTime) = @year AND ";
            if (provine_ori != "Tất cả" && provine_ori != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district_ori != "Tất cả" && district_ori != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc_ori != "Tất cả" && hcc_ori != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile_ori != "Tất cả" && profile_ori != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += ") a";
            odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine_ori)
                , new SqlParameter("dis", district_ori)
                , new SqlParameter("pub", hcc_ori)
                , new SqlParameter("file", profile_ori)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();
            ViewBag.odbyear = odb;
            odb_ori_year = odb;

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
        public ActionResult ChangeYear(string year, string provine, string district, string hcc, string profile)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string date = DateTime.Now.ToString("yyyy");
            if (year == "") year = date;

            string sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = 5 and year(os.CreatedTime) = @year AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_xong = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string xong = JsonConvert.SerializeObject(list_xong);

            sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = -2 and year(os.CreatedTime) = @year AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_da = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string da = JsonConvert.SerializeObject(list_da);

            sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = -3 and year(os.CreatedTime) = @year AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime) ";
            List<DataChart> list_chua = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string chua = JsonConvert.SerializeObject(list_chua);

            sql = "select " +
                  " (case when a.total_cho is null then 0 else a.total_cho end) as 'total_cho', " +
                  " (case when a.total_da is null then 0 else a.total_da end) as 'total_da', " +
                  " (case when a.total_xong is null then 0 else a.total_xong end) as 'total_xong' " +
                  " from( " +
                  " select " +
                  "  SUM(case when o.StatusID = -3 then 1 else 0 end) as 'total_cho', " +
                  "  SUM(case when o.StatusID = -2 then 1 else 0 end) as 'total_da', " +
                  "  SUM(case when o.StatusID = 5 then 1 else 0 end) as 'total_xong' " +
                  "  from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                  "  inner join Profile p on o.ProfileID = p.ProfileID " +
                  "  inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                  "  inner join PostOffice po on pa.PosCode = po.PosCode " +
                  "  inner join District d on po.DistrictCode = d.DistrictCode " +
                  "  inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                  "  where year(os.CreatedTime) = @year AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += ") a";
            OrderDashBorad odb = db.Database.SqlQuery<OrderDashBorad>(sql, new SqlParameter("year", year)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).FirstOrDefault();
            if (odb == null) odb = new OrderDashBorad();

            return Json(new { success = true, cxong = xong, cda = da, cchua = chua, xong = odb.total_xong, da = odb.total_da, cho = odb.total_cho }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeMonth(string year, string month, string provine, string district, string hcc, string profile)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string date = DateTime.Now.ToString("yyyy");
            if (year == "") year = date;
            date = DateTime.Now.ToString("MM");
            if (month == "") month = date;
            string sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', DAY(os.CreatedTime) as 'day', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = 5 and year(os.CreatedTime) = @year and MONTH(os.CreatedTime) = @month AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime), DAY(os.CreatedTime) ";
            List<DataChart> list_xong = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year), new SqlParameter("month", month)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string xong = JsonConvert.SerializeObject(list_xong);

            sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', DAY(os.CreatedTime) as 'day', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = -2 and year(os.CreatedTime) = @year and MONTH(os.CreatedTime) = @month AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime), DAY(os.CreatedTime) ";
            List<DataChart> list_da = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year), new SqlParameter("month", month)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string da = JsonConvert.SerializeObject(list_da);

            sql = "select year(os.CreatedTime) as 'year', MONTH(os.CreatedTime) as 'month', DAY(os.CreatedTime) as 'day', count(o.OrderID) as 'sum' " +
                "from [Order] o inner join (select distinct o.OrderID, CONVERT(date, os.CreatedTime) as 'CreatedTime' from [Order] o inner join OrderStatusDetail os on o.OrderID = os.OrderID) os on o.OrderID = os.OrderID " +
                "inner join Profile p on o.ProfileID = p.ProfileID " +
                "inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                "inner join PostOffice po on pa.PosCode = po.PosCode " +
                "inner join District  d on po.DistrictCode = d.DistrictCode " +
                "inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
                "where o.StatusID = -3 and year(os.CreatedTime) = @year and MONTH(os.CreatedTime) = @month AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " group by year(os.CreatedTime), MONTH(os.CreatedTime), DAY(os.CreatedTime) ";
            List<DataChart> list_chua = db.Database.SqlQuery<DataChart>(sql, new SqlParameter("year", year), new SqlParameter("month", month)
                , new SqlParameter("pro", provine)
                , new SqlParameter("dis", district)
                , new SqlParameter("pub", hcc)
                , new SqlParameter("file", profile)).ToList();
            string chua = JsonConvert.SerializeObject(list_chua);

            return Json(new { success = true, cxong = xong, cda = da, cchua = chua }, JsonRequestBehavior.AllowGet);
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
            string sql = "select (case when sum(a.tong_cho) is null then 0 else sum(a.tong_cho) end) as 'total_cho', (case when sum(a.tong_da) is null then 0 else sum(a.tong_da) end) as 'total_da', (case when sum(a.tong_xong) is null then 0 else sum(a.tong_xong) end) as 'total_xong' " +
                "from(select a.date, " +
               "SUM(case when a.StatusID = -3 then 1 else 0 end) as 'tong_cho', " +
               "SUM(case when a.StatusID = -2 then 1 else 0 end) as 'tong_da', " +
               "SUM(case when a.StatusID = 5 then 1 else 0 end) as 'tong_xong' " +
               "from " +
               " (select distinct CONVERT(date, os.CreatedTime) as 'date', o.StatusID,o.OrderID " +
               "from[Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               " inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
               " inner join PostOffice po on pa.PosCode = po.PosCode " +
               " inner join District  d on po.DistrictCode = d.DistrictCode " +
               " inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               " inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               " where os.CreatedTime between @start and @end AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " ) a group by a.date) a";
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
            string sql = "select (case when sum(a.tong_cho) is null then 0 else sum(a.tong_cho) end) as 'total_cho', (case when sum(a.tong_da) is null then 0 else sum(a.tong_da) end) as 'total_da', (case when sum(a.tong_xong) is null then 0 else sum(a.tong_xong) end) as 'total_xong' " +
                "from(select a.date, " +
               "SUM(case when a.StatusID = -3 then 1 else 0 end) as 'tong_cho', " +
               "SUM(case when a.StatusID = -2 then 1 else 0 end) as 'tong_da', " +
               "SUM(case when a.StatusID = 5 then 1 else 0 end) as 'tong_xong' " +
               "from " +
               " (select distinct CONVERT(date, os.CreatedTime) as 'date', o.StatusID,o.OrderID " +
               "from[Order] o  inner join Profile p on o.ProfileID = p.ProfileID " +
               " inner join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
               " inner join PostOffice po on pa.PosCode = po.PosCode " +
               " inner join District  d on po.DistrictCode = d.DistrictCode " +
               " inner join Province pr  on d.PostalProvinceCode = pr.PostalProvinceCode " +
               " inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
               " where os.CreatedTime between @start and @end AND ";
            if (provine != "Tất cả" && provine != "") sql += "pr.PostalProvinceName  = @pro and ";
            if (district != "Tất cả" && district != "") sql += "d.PostalDistrictName  = @dis and ";
            if (hcc != "Tất cả" && hcc != "") sql += "pa.PublicAdministrationName  = @pub and ";
            if (profile != "Tất cả" && profile != "") sql += "p.ProfileName  = @file and ";
            sql = sql.Substring(0, sql.Length - 5);
            sql += " ) a group by a.date) a";
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
            return Json(new { success = true, xong = odb_ori.total_xong, da = odb_ori.total_da, cho = odb_ori.total_cho, xongyear = odb_ori_year.total_xong, dayear = odb_ori_year.total_da, choyear = odb_ori_year.total_cho }, JsonRequestBehavior.AllowGet);
        }

    }
}