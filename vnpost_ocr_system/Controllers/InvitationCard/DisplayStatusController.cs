using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class DisplayStatusController : BaseUserController
    {
        [Route("giay-hen/trang-thai-giay-hen")]
        [HttpPost]
        public ActionResult Display(string id)
        {
            string cusid = "";
            if (Session["userID"] != null)
            {
                cusid = Session["userID"].ToString();
            }
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Order od = null;
            Order odb = null;

            odb = db.Database.SqlQuery<Order>("select o.* from [Order] o where o.OrderID = @id", new SqlParameter("id", id)).FirstOrDefault();

            if (odb == null)
            {
                SearchStatusController ssc = new SearchStatusController();
                ssc.SetMessage("2");
                return Redirect("/giay-hen/tim-giay-hen");
            }
            else
            {
                if (cusid != "")
                {
                    string checksql = "select o.*," +
                        "(case when o.StatusID = 0 then 0 else 1 end) as 'active' " +
                        "from [Order] o inner join Customer c on o.CustomerID = c.CustomerID " +
                        "where c.CustomerID = @cusid and o.OrderID = @oid " +
                        "order by active desc";
                    od = db.Database.SqlQuery<Order>(checksql, new SqlParameter("cusid", cusid), new SqlParameter("oid", id)).FirstOrDefault();
                }
                if (od == null) ViewBag.ck = 1;
                else ViewBag.ck = 0;

                string query = "select distinct YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd',CAST(CreatedTime AS date) as 'CreatedTime'  from OrderStatusDetail where OrderID = @id order by CreatedTime desc";
                List<OrderByDay> list = db.Database.SqlQuery<OrderByDay>(query, new SqlParameter("id", odb.OrderID)).ToList();
                query = "select os.*,s.StatusName,DATEPART(HOUR, CreatedTime) as 'h',DATEPART(MINUTE, CreatedTime) as 'mi',p.PosName,YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd' from OrderStatusDetail os inner join Status s on os.StatusID = s.StatusID left outer join PostOffice p on os.PosCode = p.PosCode where OrderID = @id order by CreatedTime desc";
                List<MyOrderDetail> listOrderDB = db.Database.SqlQuery<MyOrderDetail>(query, new SqlParameter("id", odb.OrderID)).ToList();
                foreach (var item in list)
                {
                    item.listOrder = new List<MyOrderDetail>();
                    foreach (var x in listOrderDB)
                    {
                        x.hour = x.h + ":" + x.mi;
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
                string sql = @"select distinct o.*, pa.PublicAdministrationName, pa.Phone, pa.Address, pr.ProfileName, p.PosName, p.Address as 'Address_BC', p.Phone as 'Phone_BC', s.StatusName, ppt.PersonalPaperTypeName,
                            (case when o.StatusID = 0 then 0 else 1 end) as 'active'
                            from [Order] o 
                            inner join Profile pr on pr.ProfileID = o.ProfileID
                            inner join PublicAdministration pa on pr.PublicAdministrationLocationID = pa.PublicAdministrationLocationID
                            inner join PostOffice p on pa.PosCode = p.PosCode
                            inner join PersonalPaperType ppt on ppt.PersonalPaperTypeID = o.ProcedurerPersonalPaperTypeID
                            inner join District d on d.DistrictCode = p.DistrictCode
							inner join OrderStatusDetail os on o.OrderID = os.OrderID
							inner join Status s on o.StatusID = s.StatusID
                            where o.OrderID = @id
                            order by active desc";
                orderDB o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", id)).FirstOrDefault();
                if (o == null)
                {
                    sql = @"select distinct o.*, os.PosCode,
                        (case when o.StatusID = 0 then 0 else 1 end) as 'active'
                        from [Order] o left outer join Status s on o.StatusID = s.StatusID
                        inner join OrderStatusDetail os on o.OrderID = os.OrderID
                        where o.OrderID = @id
                        order by active desc";
                    o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", id)).FirstOrDefault();
                }
                else
                {
                    o.NgayCap = o.ProcedurerPersonalPaperIssuedDate.HasValue ? o.ProcedurerPersonalPaperIssuedDate.Value.ToString("dd/MM/yyyy") : null;
                    o.displayAmount = formatAmount(o.Amount);
                }

                if (o.StatusID == -3) o.step = 0;
                else if (o.StatusID == -2) o.step = 1;
                else if (o.StatusID == 1) o.step = 2;
                else if (o.StatusID == 5) o.step = 4;
                else if (o.StatusID == -1) o.step = 3;
                else o.step = 0;
                ViewBag.order = o;
                if (o.StatusID == 0)
                {
                    ViewBag.checkCancelled = true;
                }
                else
                {
                    ViewBag.checkCancelled = false;
                }

                if (Request.Browser.IsMobileDevice)
                {
                    return View("/Views/MobileView/InvitationCard/DisplayStatus.cshtml");
                }
                else
                {
                    return View("/Views/InvitationCard/DisplayStatus.cshtml");
                }
                
            }



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