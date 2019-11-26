using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class DisplayStatusController : Controller
    {
        // GET: 
        [Route("giay-hen/trang-thai-giay-hen")]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userID"] == null)
            {
                SearchStatusController ssc = new SearchStatusController();
                ssc.getMess("Phải đăng nhập trước");
                //return Redirect("/giay-hen/tim-giay-hen");
            }
            else
            {
                //return View("/Views/InvitationCard/DisplayStatus.cshtml");
            }
            return Redirect("/giay-hen/tim-giay-hen");

        }

        [Route("giay-hen/trang-thai-giay-hen")]
        [HttpPost]
        public ActionResult Display(string id_raw)
        {
            string cusid = "";
            if (Session["userID"] != null)
            {
                cusid = Session["userID"].ToString();
            }
            int id = Convert.ToInt32(id_raw);
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Order od = null;
            if (cusid != "")
            {
                string checksql = "select o.* " +
                    "from [Order] o inner join Customer c on o.CustomerID = c.CustomerID " +
                    "where c.CustomerID = @cusid and o.OrderID = @oid";
                od = db.Database.SqlQuery<Order>(checksql, new SqlParameter("cusid", cusid), new SqlParameter("oid", id)).FirstOrDefault();
            }
            if (od == null) ViewBag.ck = 1;
            else ViewBag.ck = 0;

            string query = "select distinct YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd'  from OrderStatusDetail where OrderID = @id  order by y,m,d desc";
            List<OrderByDay> list = db.Database.SqlQuery<OrderByDay>(query, new SqlParameter("id", id)).ToList();
            query = "select os.*,s.StatusName,p.PosName,YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd' from OrderStatusDetail os inner join Status s on os.StatusID = s.StatusID left outer join PostOffice p on os.PosCode = p.PosCode where OrderID = @id  order by y,m,d desc";
            List<MyOrderDetail> listOrderDB = db.Database.SqlQuery<MyOrderDetail>(query, new SqlParameter("id", id)).ToList();
            foreach (var item in list)
            {
                item.listOrder = new List<MyOrderDetail>();
                foreach (var x in listOrderDB)
                {
                    x.hour = x.CreatedTime.ToString("HH:MM");
                    if (x.y == item.y && x.m == item.m && x.d == item.d)
                    {
                        if (x.StatusID == 2) x.display = "Đã xác nhận đi khỏi bưu cục - " + x.PosCode + " - " + x.PosName;
                        else if (x.StatusID == 3) x.display = "Đã xác nhận đến bưu cục - " + x.PosCode + " - " + x.PosName;
                        else x.display = x.StatusName;

                        if (x.Note != null) x.display += " (" + x.Note + ")";
                        item.listOrder.Add(x);
                        item.dayOfWeek = x.CreatedTime.ToString("ddd");
                    }
                }
                switch (item.dayOfWeek)
                {
                    case "Mon":
                        item.dayOfWeek = "Thứ hai";
                        break;
                    case "Tue":
                        item.dayOfWeek = "Thứ ba";
                        break;
                    case "Wed":
                        item.dayOfWeek = "Thứ tư";
                        break;
                    case "Thu":
                        item.dayOfWeek = "Thứ năm";
                        break;
                    case "Fri":
                        item.dayOfWeek = "Thứ sáu";
                        break;
                    case "Sat":
                        item.dayOfWeek = "Thứ bảy";
                        break;
                    case "Sun":
                        item.dayOfWeek = "Chủ nhật";
                        break;
                }
            }
            ViewBag.list = list;
            string sql = "select distinct o.*, pa.PublicAdministrationName, pa.Phone, pa.Address, pr.ProfileName, p.PosName, p.Address as 'Address_BC', p.Phone as 'Phone_BC', s.StatusName " +
                "from [Order] o inner join Status s on o.StatusID = s.StatusID " +
                "inner join OrderStatusDetail os on o.OrderID = os.OrderID " +
                "inner join PostOffice p on os.PosCode = p.PosCode " +
                "inner join PublicAdministration pa on p.PosCode = pa.PosCode " +
                "inner join District d on d.DistrictCode = p.DistrictCode " +
                "inner join Profile pr on pr.ProfileID = o.ProfileID " +
                "where o.OrderID = @id";
            orderDB o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", id)).FirstOrDefault();
            if (o == null)
            {
                o = new orderDB();
            }
            else
            {
                o.NgayCap = o.ProcedurerPersonalPaperIssuedDate.ToString("dd/MM/yyyy");
                o.displayAmount = formatAmount(o.Amount);
            }

            ViewBag.order = o;
            if (o.StatusID == -3) o.step = 0;
            else if (o.StatusID == -2) o.step = 1;
            else if (o.StatusID == 1) o.step = 2;
            else if (o.StatusID == 5) o.step = 4;
            else o.step = 3;
            return View("/Views/InvitationCard/DisplayStatus.cshtml");




        }

        public string formatAmount(double? i)
        {
            string s = "";
            if (i != null)
            {
                s = i + "";
                int temp = Convert.ToInt32(s);
                s = temp.ToString("C0");
                s = s.Substring(1);
            }
            return s;
        }
    }
}