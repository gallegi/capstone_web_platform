using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.User
{
    public class InvitationManageController : Controller
    {
        // GET: InvitationManage
        [Route("tai-khoan/quan-ly-giay-hen")]
        public ActionResult Index()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<MyOrders> listOrder = new List<MyOrders>();
            string query = "select o.OrderID, o.CustomerID, p.ProfileName , o.AppointmentLetterCode, o.Amount, o.ItemCode , s.StatusName, o.CreatedTime "
                    + " from \"Order\" o"
                    + " inner join  \"Status\" s on o.StatusID = s.StatusID "
                    + " inner join \"Profile\" p on o.ProfileID = p.ProfileID "
                    + " where o.CustomerID = @customerID ";
            listOrder = db.Database.SqlQuery<MyOrders>(query, new SqlParameter("customerID", 1)).ToList();
            ViewBag.listOrder = listOrder;
            return View("/Views/User/InvitationManage.cshtml");
        }

        [Route("tai-khoan/quan-ly-giay-hen/danh-sach")]
        public ActionResult getData()
        {
            //VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            //List <MyOrders> listOrder = new List<MyOrders>();
            //string query = "select o.OrderID, o.CustomerID, p.ProfileName , o.AppointmentLetterCode, o.Amount, o.ItemCode , s.StatusName, o.CreatedTime "  
            //        + " from \"Order\" o"
            //        + " inner join  \"Status\" s on o.StatusID = s.StatusID "
            //        + " inner join \"Profile\" p on o.ProfileID = p.ProfileID "
            //        + " where o.CustomerID = @customerID ";
            //listOrder = db.Database.SqlQuery<MyOrders>(query,new SqlParameter("customerID",1)).ToList();
            //ViewBag.listOrder = listOrder;
            return new HttpStatusCodeResult(200);
            //return Json(listOrder);
        }
    }

    public class MyOrders : Order
    {
        public string ProfileName { get; set; }
        public string StatusName { get; set; }
    }
}