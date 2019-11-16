using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using XCrypt;

namespace vnpost_ocr_system.Controllers.Login
{
    public class ManagementUserController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: ManagementUser
        [Route("phan-quyen-tai-khoan")]
        public ActionResult Index()
        {
            return View("/Views/Login/ManagementUser.cshtml");
        }

        [HttpPost]
        public ActionResult getData(string province,string role,string active)
        {
            string query = "";
            if (string.IsNullOrEmpty(province)) query = "select a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from Admin a,Province p,AdminRole ar where ar.AdminRoleID=a.[Role] and a.PostalProvinceCode = p.PostalProvinceCode and a.[Role] like @role and a.IsActive like @active";
            else query = "select a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from Admin a,Province p,AdminRole ar where ar.AdminRoleID=a.[Role] and a.PostalProvinceCode = p.PostalProvinceCode and a.PostalProvinceCode = @province and a.[Role] like @role and a.IsActive like @active";
            List<Admindb> searchList = null;
            int totalrows = 0;
            int totalrowsafterfiltering = 0;
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                searchList = db.Database.SqlQuery<Admindb>(query,
                    new SqlParameter("province", province ),
                    new SqlParameter("role", '%' + role+ '%'),
                    new SqlParameter("active", '%' + active + '%')
                    ).ToList();

                db.Configuration.LazyLoadingEnabled = false;

                totalrows = searchList.Count;
                totalrowsafterfiltering = searchList.Count;
                //sorting
                searchList = searchList.OrderBy(sortColumnName + " " + sortDirection).ToList<Admindb>();
                //paging
                searchList = searchList.Skip(start).Take(length).ToList<Admindb>();

            }
            catch (Exception e)
            {

                e.Message.ToString();

            }
            return Json(new { data = searchList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Insert(string name,string username,string password,int province,int role,int active)
        {
            try
            {
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password);
                Random r = new Random();
                Admin a = new Admin();
                a.AdminName = name;
                a.AdminUsername = username;
                a.AdminPasswordHash = passXc;
                a.AdminPasswordSalt = r.Next(100000, 999999).ToString();
                a.Role = role;
                a.PostalProvinceCode = province.ToString();
                a.IsActive = Convert.ToBoolean(active);
                a.CreatedTime = DateTime.Now;
                db.Admins.Add(a);
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getRole()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.AdminRoles.OrderBy(x=>x.AdminRoleID).ToList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEdit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var obj = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
            obj.AdminPasswordHash = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Decrypt(obj.AdminPasswordHash);
            return Json(obj,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var admin = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
                db.Admins.Remove(admin);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

    }
    public class Admindb
    {
        public Int64 AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminUsername { get; set; }
        public string PostalProvinceName { get; set; }
        public string AdminRoleName { get; set; }
        public bool IsActive { get; set; }
    }
}