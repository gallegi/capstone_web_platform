using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.User
{
    public class InvitationManageController : Controller
    {
        // GET: InvitationManage
        [Auther(Roles = "0")]
        [Route("tai-khoan/quan-ly-giay-hen")]
        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Users/InvitationManage.cshtml");
            }
            else
            {
                return View("/Views/User/InvitationManage.cshtml");
            }
        }

        [Route("tai-khoan/quan-ly-giay-hen/danh-sach")]
        public ActionResult getAll()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<MyOrders> listOrder = new List<MyOrders>();
            //int cancelStatus = 0;
            string query = "select o.OrderID, o.CustomerID, p.ProfileName , o.AppointmentLetterCode, o.Amount, o.ItemCode , s.StatusName,  s.StatusID, o.OrderDate "
                    + " from \"Order\" o"
                    + " inner join  \"Status\" s on o.StatusID = s.StatusID "
                    + " inner join \"Profile\" p on o.ProfileID = p.ProfileID "
                    + " where o.CustomerID = @customerID"
                    + " order by " + sortColumnName + " " + sortDirection + " OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            string countQuery = "select OrderID "
                    + " from \"Order\" o"
                    + " inner join  \"Status\" s on o.StatusID = s.StatusID "
                    + " inner join \"Profile\" p on o.ProfileID = p.ProfileID "
                    + " where o.CustomerID = @customerID ";

            string customerID = Session["userID"].ToString();
            listOrder = db.Database.SqlQuery<MyOrders>(query,
                   new SqlParameter("customerID", customerID)).ToList();
            foreach (MyOrders item in listOrder)
            {
                item.stringDate = item.OrderDate.ToString("dd/MM/yyyy");
            }
            int totalrows = db.Database.SqlQuery<MyOrders>(countQuery,new SqlParameter("customerID", customerID)).Count();
            return Json(new { success = true, data = listOrder , draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrows }, JsonRequestBehavior.AllowGet);
        }

        [Route("tai-khoan/quan-ly-giay-hen/huy")]
        public ActionResult cancel(string Code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int cancelStatus = 0;
                    string queryInfo = "select * from OrderStatusDetail where OrderID = @code";
                    OrderStatusDetail oDetail = db.Database.SqlQuery<OrderStatusDetail>(queryInfo, new SqlParameter("code", Code)).FirstOrDefault();

                    string queryUpdateStatus = "insert into OrderStatusDetail values (@code, @status , @note, @posCode, @date)";
                    db.Database.ExecuteSqlCommand(queryUpdateStatus, 
                        new SqlParameter("code", Code),
                        new SqlParameter("status", cancelStatus),
                        new SqlParameter("note", DBNull.Value),
                        new SqlParameter("posCode", oDetail.PosCode == null ? DBNull.Value : (object) oDetail.PosCode),
                        new SqlParameter("date", DateTime.Now));
                    transaction.Commit();
                    db.SaveChanges();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new HttpStatusCodeResult(400);
                }
            }
        }
    }

    public class MyOrders : Order
    {
        public string ProfileName { get; set; }
        public string StatusName { get; set; }
        public string stringDate { get; set; }
    }
}