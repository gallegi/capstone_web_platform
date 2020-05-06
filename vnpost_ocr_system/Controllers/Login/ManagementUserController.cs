using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;
using XCrypt;

namespace vnpost_ocr_system.Controllers.Login
{
    public class ManagementUserController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: ManagementUser
        [Auther(Roles = "1,2,3")]
        [Route("phan-quyen-tai-khoan")]
        public ActionResult Index()
        {
            return View("/Views/Login/ManagementUser.cshtml");
        }

        [HttpPost]
        public ActionResult getData(string province, string role, string active, string username, string name)
        {
            string query = "";
            if (province.Equals("0"))
            {
                if (Convert.ToInt32(Session["adminRole"]) == 1 || Convert.ToInt32(Session["adminRole"]) == 2)
                    query = @"select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT, a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from
                             Admin a inner join Province p on a.PostalProvinceCode = p.PostalProvinceCode
                             inner join AdminRole ar on a.Role = ar.AdminRoleID
                             and a.IsActive like @active and a.Role > @currrole and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username
                             and a.Role = 2";
                if (Convert.ToInt32(Session["adminRole"]) == 3)
                    query = @"select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT, a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from
                             Admin a inner join Province p on a.PostalProvinceCode = p.PostalProvinceCode
                             inner join AdminRole ar on a.Role = ar.AdminRoleID
                             and a.IsActive like @active and a.Role > @currrole and a.PostalProvinceCode = @currprovince and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username";
            }
            else
            {
                if (Convert.ToInt32(Session["adminRole"]) == 1 || Convert.ToInt32(Session["adminRole"]) == 2)
                    query = @"select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT, a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from
                             Admin a inner join Province p on a.PostalProvinceCode = p.PostalProvinceCode
                             inner join AdminRole ar on a.Role = ar.AdminRoleID
                             and a.IsActive like @active and a.Role > @currrole and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username";
                if (!string.IsNullOrEmpty(province)) query += " and a.PostalProvinceCode = @province";
                if (Convert.ToInt32(Session["adminRole"]) == 3)
                    query = @"select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT, a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from
                             Admin a inner join Province p on a.PostalProvinceCode = p.PostalProvinceCode
                             inner join AdminRole ar on a.Role = ar.AdminRoleID
                             and a.IsActive like @active and a.Role > @currrole and a.PostalProvinceCode = @currprovince and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username";
            }
            query += " order by a.Role,a.PostalProvinceCode";
            List<Admindb> searchList = null;
            List<Admindb> append = new List<Admindb>();
            int stt = 0;
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
                    new SqlParameter("province", province),
                    new SqlParameter("role", '%' + role + '%'),
                    new SqlParameter("active", '%' + active + '%'),
                    new SqlParameter("currrole", Convert.ToInt32(Session["adminRole"])),
                    new SqlParameter("currprovince", Convert.ToInt32(Session["adminPro"])),
                    new SqlParameter("name", '%' + name + '%'),
                    new SqlParameter("username", '%' + username + '%')
                    ).ToList();
                if (Convert.ToInt32(Session["adminRole"]) == 1)
                {
                    append = db.Database.SqlQuery<Admindb>(@"select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT, a.AdminID,a.AdminName,a.AdminUsername,a.PostalProvinceCode,ar.AdminRoleName,a.IsActive from Admin a,AdminRole ar where a.Role=ar.AdminRoleID and a.Role in (2) and
                                                        a.IsActive like @active and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username",
                                        new SqlParameter("active", '%' + active + '%'),
                                        new SqlParameter("role", '%' + role + '%'),
                                        new SqlParameter("name", '%' + name + '%'),
                                        new SqlParameter("username", '%' + username + '%')).ToList();
                    append.AddRange(searchList);
                    foreach (Admindb a in append.ToList())
                    {
                        if (a.AdminRoleName.Equals("Tổng công ty") && !province.Equals("0") && !string.IsNullOrEmpty(province)) { append.Remove(a); }
                        else
                        {
                            if (a.AdminRoleName.Equals("Tổng công ty"))
                            {
                                a.PostalProvinceName = "Tổng công ty";
                            }
                        }
                        a.STT = ++stt;
                    }
                }
                else append.AddRange(searchList);

                db.Configuration.LazyLoadingEnabled = false;
                totalrows = append.Count;
                totalrowsafterfiltering = append.Count;
                append = append.Skip(start).Take(length).ToList<Admindb>();
            }
            catch (Exception e)
            {

                e.Message.ToString();

            }
            return Json(new { data = append, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }
        [Auther(Roles = "1,2,3")]
        public ActionResult Insert(string name, string username, string password, string province, int role, int active)
        {
            if (Convert.ToInt32(Session["adminRole"]) < role)
            {
                try
                {
                    var check = db.Admins.Where(x => x.AdminUsername.Equals(username)).ToList();
                    if (check.Count > 0) return Json(2, JsonRequestBehavior.AllowGet);
                    Random r = new Random();
                    int ran = r.Next(100000, 999999);
                    password = string.Concat(password, ran);
                    string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pd");
                    Admin a = new Admin();
                    a.AdminName = name;
                    a.AdminUsername = username;
                    a.AdminPasswordHash = passXc;
                    a.AdminPasswordSalt = ran.ToString();
                    a.Role = role;
                    if (province.Equals("0")) province = null;
                    a.PostalProvinceCode = province;
                    a.IsActive = Convert.ToBoolean(active);
                    a.CreatedTime = DateTime.Now;
                    db.Admins.Add(a);
                    db.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
            }
        }
        [Auther(Roles = "1,2,3")]
        public ActionResult Update(int id, string name, string username, string password, string province, int role, int active)
        {
            if (Convert.ToInt32(Session["adminRole"]) < role)
            {
                try
                {
                    var admin = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
                    if (!password.Equals(admin.AdminPasswordHash))
                    {
                        password = string.Concat(password, admin.AdminPasswordSalt.Substring(0, 6));
                        string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pd");
                        admin.AdminPasswordHash = passXc;
                    }
                    admin.AdminName = name;
                    admin.AdminUsername = username;
                    admin.Role = role;
                    if (province.Equals("0")) province = null;
                    admin.PostalProvinceCode = province;
                    admin.IsActive = Convert.ToBoolean(active);
                    admin.ModifiedBy = Convert.ToInt64(Session["useradminID"]);
                    admin.ModifiedTime = DateTime.Now;
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        [Auther(Roles = "1,2,3")]
        public ActionResult getRole()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int adminRole = Convert.ToInt32(Session["adminRole"]);
            var list = db.AdminRoles.Where(x => x.AdminRoleID > adminRole).OrderBy(x => x.AdminRoleID).ToList();
            //AdminRole a = new AdminRole();
            //a.AdminRoleID = 1; a.AdminRoleName = "Super Admin";
            //if (adminRole == 1) list.Insert(0,a);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [Auther(Roles = "1,2,3")]
        public ActionResult GetPro()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listA = new List<Province>();
            if (Convert.ToInt32(Session["adminRole"]) == 3)
            {
                string adminPro = Session["adminPro"].ToString();
                listA = db.Provinces.Where(x => x.PostalProvinceCode.Equals(adminPro)).ToList();
            }
            else listA = db.Provinces.OrderBy(b => b.PostalProvinceName).ToList();
            var listS = db.Provinces.OrderBy(b => b.PostalProvinceName).ToList();
            return Json(new { listsearch = listS, listAE = listA }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEdit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var obj = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
            //string pass = Encrypt.DecryptString(obj.AdminPasswordHash, "PD").Trim();
            //obj.AdminPasswordHash = pass.Remove(pass.Length - 6, 6);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [Auther(Roles = "1,2,3")]
        public ActionResult Delete(int id)
        {
            try
            {
                var admin = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
                if (Convert.ToInt32(Session["adminRole"]) < admin.Role)
                {
                    db.Admins.Remove(admin);
                    db.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenUsername(int id, string province)
        {
            try
            {
                string username = "sample";
                var provincedb = db.Provinces.Where(x => x.PostalProvinceCode.Equals(province)).FirstOrDefault();
                if (id == 2)
                {
                    username = "admin00"; var tct = db.Admins.Where(x => x.Role == 2).ToList();
                    if (tct.Count == 1) { username += "_1"; }
                    if (tct.Count > 1)
                    {
                        string tctt = tct.Last().AdminUsername;
                        username += "_" + (Convert.ToInt32(tctt.Substring(8, tctt.Length - 8)) + 1);
                    }
                }
                else if (id == 3)
                {
                    username = "admin_" + provincedb.ProvinceShortName; var adt = db.Admins.Where(x => x.Role == 3 && x.PostalProvinceCode.Equals(province)).ToList();
                    if (adt.Count == 0) { username += "_1"; }
                    if (adt.Count >= 1)
                    {
                        int ind = username.Length;
                        string adttt = adt.Last().AdminUsername;
                        username += "_" + (Convert.ToInt32(adttt.Substring(ind + 1, adttt.Length - ind - 1)) + 1);
                    }
                }
                else if (id == 4)
                {
                    username = "giaodichvien_" + provincedb.ProvinceShortName; var gdv = db.Admins.Where(x => x.Role == 4 && x.PostalProvinceCode.Equals(province)).ToList();
                    if (gdv.Count == 0) { username += "_1"; }
                    if (gdv.Count >= 1)
                    {
                        int ind = username.Length;
                        string gdvv = gdv.Last().AdminUsername;
                        username += "_" + (Convert.ToInt32(gdvv.Substring(ind + 1, gdvv.Length - ind - 1)) + 1);
                    }
                }
                return Json(username, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
    }
    public class Admindb
    {
        public int STT { get; set; }
        public Int64 AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminUsername { get; set; }
        public string PostalProvinceName { get; set; }
        public string AdminRoleName { get; set; }
        public bool IsActive { get; set; }
    }
}