using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using System.Globalization;
using XCrypt;
using System.Data.Entity;

namespace vnpost_ocr_system.Controllers.User
{
    public class AccountInfoController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: AccountInfo
        [Route("tai-khoan/thong-tin-tai-khoan")]
        public ActionResult Index()
        {
            return View("/Views/User/AccountInfo.cshtml");
        }
        public ActionResult Info()
        {
            int userID = Convert.ToInt32(Session["userID"].ToString());
            var custom = db.Customers.Where(x => x.CustomerID == userID).ToList().Select(x => new CustomerDB {
                dob = x.DOB.GetValueOrDefault().ToString("dd/MM/yyyy"),
                FullName = x.FullName,
                Phone = x.Phone,
                Email = x.Email,
                Gender = x.Gender
            }).FirstOrDefault();
            //custom.DOB = Convert.ToDateTime(date.ToString("dd/MM/yyyy"));
            //custom.DOB = DateTime.ParseExact(custom.DOB.ToString(), "MM/dd/YYYY HH:mm:ss tt", CultureInfo.InvariantCulture);
            return Json(custom, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string name,string phone,string email,string dob,string gender,string oldpass,string newpass,string repass)
        {
            try
            {
                string oldpassXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(oldpass, "pl");
                int userID = Convert.ToInt32(Session["userID"].ToString());
                var custom = db.Customers.Where(x => x.CustomerID == userID).FirstOrDefault();
                string RenPass = string.Concat(custom.PasswordHash, custom.PasswordSalt);
                DateTime vert = DateTime.ParseExact(dob, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                custom.FullName = name;
                custom.DOB = vert;
                custom.Email = email;
                custom.Gender = Convert.ToInt32(gender);
                custom.Phone = phone;
                if (!string.IsNullOrEmpty(oldpass))
                {
                    if (string.Concat(oldpassXc, custom.PasswordSalt).Equals(RenPass))
                    {
                        custom.PasswordHash = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(newpass, "pl");
                        db.Entry(custom).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    db.Entry(custom).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class CustomerDB : Customer
    {
        public string dob { get; set; }
    }
}