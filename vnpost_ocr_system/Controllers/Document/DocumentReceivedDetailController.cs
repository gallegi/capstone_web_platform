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
    public class DocumentReceivedDetailController : Controller
    {

        // GET: DocumentReceivedDetail
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/ho-so-da-nhan/chi-tiet")]
        [HttpGet]
        public ActionResult Index(string id_raw)
        {
            int id = Convert.ToInt32(id_raw);
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string query = "select distinct YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd'  from OrderStatusDetail where OrderID = @id  order by y,m,d desc";
            List<OrderByDay> list = db.Database.SqlQuery<OrderByDay>(query, new SqlParameter("id", id)).ToList();
            query = "select osd.*,YEAR(osd.CreatedTime) as 'y', MONTH(osd.CreatedTime) as 'm', DAY(osd.CreatedTime) as 'd', s.StatusName from OrderStatusDetail osd join Status s on osd.StatusID = s.StatusID where OrderID = @id order by y,m,d desc";
            List<MyOrderDetail> listOrderDB = db.Database.SqlQuery<MyOrderDetail>(query, new SqlParameter("id", id)).ToList();
            foreach (var item in list)
            {
                item.listOrder = new List<MyOrderDetail>();
                foreach (var x in listOrderDB)
                {
                    x.hour = x.CreatedTime.ToString("HH:MM");
                    if (x.y == item.y && x.m == item.m && x.d == item.d)
                    {
                        item.StatusID = x.StatusID;
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
                    case "Sub":
                        item.dayOfWeek = "Chủ nhật";
                        break;
                }
            }
            ViewBag.list = list;


            string sql = "select distinct o.*,s.StatusName, pa.PublicAdministrationName, pa.Phone, pa.[Address], p.ProfileName, " +
                        " po.PosName, po.[Address] as 'Address_BC', po.Phone as 'Phone_BC' from[Order] o" +
                        " left join[Profile] p on o.ProfileID = p.ProfileID" +
                        " left join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID" +
                        " left join PostOffice po on pa.PosCode = po.PosCode" +
                        " left join District d on po.DistrictCode = d.DistrictCode" +
                        " left join[Status] s on o.StatusID = s.StatusID" +
                        " left join OrderStatusDetail osd on o.OrderID = osd.OrderID" +
                        " where o.OrderID = @id";
            orderDB o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", id)).FirstOrDefault();
            o.NgayCap = o.ProcedurerPersonalPaperIssuedDate.ToString("dd/MM/yyyy");
            o.displayAmount = formatAmount(o.Amount);
            ViewBag.order = o;
            if (o.StatusID == -3) o.step = 0;
            else if (o.StatusID == -2) o.step = 1;
            else if (o.StatusID == 1) o.step = 2;
            else if (o.StatusID == 5) o.step = 4;
            else o.step = 3;
            if(o.StatusID == 0)
            {
                ViewBag.checkCancelled = true;
            }
            else
            {
                ViewBag.checkCancelled = false;
            }
            return View("/Views/Document/DocumentReceivedDetail.cshtml");
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