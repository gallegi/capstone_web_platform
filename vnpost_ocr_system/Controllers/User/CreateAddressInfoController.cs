using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.User
{
    public class CreateAddressInfoController : BaseUserController
    {
        [Auther(Roles = "0")]
        // GET: CreateAddressInfo
        [Route("tai-khoan/tao-dia-chi-moi")]
        public ActionResult Index()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> listProvince = db.Database.SqlQuery<Province>("select * from Province order by PostalProvinceName").ToList();
            List<PersonalPaperType> listPaperType = db.PersonalPaperTypes.ToList<PersonalPaperType>();
            ViewBag.listProvince = listProvince;
            ViewBag.listPaperType = listPaperType;
            return View("/Views/User/CreateAddressInfo.cshtml");
        }
        [Auther(Roles = "0")]
        [Route("tai-khoan/tao-dia-chi-moi/them")]
        public ActionResult Add(string name, string phone, string province, string district
            , string address, string paperType, string code, string date, string placeOfIssue)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ContactInfo c = new ContactInfo();
                    c.FullName = name;
                    c.Phone = phone;
                    c.PostalDistrictCode = district;
                    c.Street = address;
                    c.PersonalPaperTypeID = paperType.Equals("") ? c.PersonalPaperTypeID = null : c.PersonalPaperTypeID = Convert.ToInt32(paperType);
                    c.PersonalPaperNumber = code;
                    c.PersonalPaperIssuedDate = date.Equals("") ? (DateTime?) null :  DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    c.PersonalPaperIssuedPlace = placeOfIssue;

                    //get customerID from session.
                    c.CustomerID  = Convert.ToInt32(Session["userID"].ToString());
                    db.ContactInfoes.Add(c);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new HttpStatusCodeResult(400);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}