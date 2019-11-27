using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using XCrypt;
using System.Data.Entity;
using vnpost_ocr_system.SupportClass;

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
            if (Convert.ToInt32(Session["adminRole"]) == 1 || Convert.ToInt32(Session["adminRole"]) == 2) query = "select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT,a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from Admin a,Province p,AdminRole ar where ar.AdminRoleID=a.[Role] and a.PostalProvinceCode = p.PostalProvinceCode and a.IsActive like @active and a.Role > @currrole and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username";
            if (Convert.ToInt32(Session["adminRole"]) == 3) query = "select CAST(ROW_NUMBER() OVER(ORDER BY a.AdminName ASC) as int) AS STT,a.AdminID,a.AdminName,a.AdminUsername,p.PostalProvinceName,ar.AdminRoleName,a.IsActive from Admin a,Province p,AdminRole ar where ar.AdminRoleID=a.[Role] and a.PostalProvinceCode = p.PostalProvinceCode and a.IsActive like @active and a.Role > @currrole and a.PostalProvinceCode = @currprovince and a.Role like @role and a.AdminName like @name and a.AdminUsername like @username";
            if (!string.IsNullOrEmpty(province)) { query += " and a.PostalProvinceCode = @province "; }
            query += " order by a.Role";
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
                    new SqlParameter("province", province),
                    new SqlParameter("role", '%' + role + '%'),
                    new SqlParameter("active", '%' + active + '%'),
                    new SqlParameter("currrole", Convert.ToInt32(Session["adminRole"])),
                    new SqlParameter("currprovince", Convert.ToInt32(Session["adminPro"])),
                    new SqlParameter("name", '%' + name + '%'),
                    new SqlParameter("username", '%' + username + '%')
                    ).ToList();

                foreach (Admindb a in searchList)
                {
                    if (!string.IsNullOrEmpty(province) && Convert.ToInt32(Session["adminRole"]) == 1) { searchList.Remove(a); }
                    else
                    {
                        if (a.AdminRoleName.Equals("Tổng công ty")) a.PostalProvinceName = "Tổng công ty";
                    }
                }
                db.Configuration.LazyLoadingEnabled = false;

                totalrows = searchList.Count;
                totalrowsafterfiltering = searchList.Count;
                //sorting
                //searchList = searchList.OrderBy(sortColumnName + " " + sortDirection).ToList<Admindb>();
                //paging
                searchList = searchList.Skip(start).Take(length).ToList<Admindb>();
            }
            catch (Exception e)
            {

                e.Message.ToString();

            }
            return Json(new { data = searchList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }
        [Auther(Roles = "1,2,3")]
        public ActionResult Insert(string name, string username, string password, int province, int role, int active)
        {
            if (Convert.ToInt32(Session["adminRole"]) < role)
            {
                try
                {
                    var check = db.Admins.Where(x => x.AdminUsername.Equals(username)).ToList();
                    if (check.Count > 0) return Json("Tên tài khoản đã tồn tại. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
                    Random r = new Random();
                    int ran = r.Next(100000, 999999);
                    password = string.Concat(password, ran);
                    string passXc = Encrypt.EncryptString(password, "PD");
                    Admin a = new Admin();
                    a.AdminName = name;
                    a.AdminUsername = username;
                    a.AdminPasswordHash = passXc;
                    a.AdminPasswordSalt = ran.ToString();
                    a.Role = role;
                    if (province == 0) province = 1;
                    a.PostalProvinceCode = province.ToString();
                    a.IsActive = Convert.ToBoolean(active);
                    a.CreatedTime = DateTime.Now;
                    db.Admins.Add(a);
                    db.SaveChanges();
                    return Json("Thêm tài khoản thành công", JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
            }
        }
        [Auther(Roles = "1,2,3")]
        public ActionResult Update(int id, string name, string username, string password, int province, int role, int active)
        {
            if (Convert.ToInt32(Session["adminRole"]) < role)
            {
                try
                {
                    var admin = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
                    password = string.Concat(password, admin.AdminPasswordSalt);
                    string passXc = Encrypt.EncryptString(password, "PD");
                    admin.AdminName = name;
                    admin.AdminUsername = username;
                    admin.AdminPasswordHash = passXc;
                    admin.Role = role;
                    if (province == 0) province = 10;
                    admin.PostalProvinceCode = province.ToString();
                    admin.IsActive = Convert.ToBoolean(active);
                    admin.ModifiedBy = Convert.ToInt64(Session["useradminID"]);
                    admin.ModifiedTime = DateTime.Now;
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Chỉnh sửa tài khoản thành công", JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
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
            string adminPro = Session["adminPro"].ToString();
            if (Convert.ToInt32(Session["adminRole"]) == 3) listA = db.Provinces.Where(x => x.PostalProvinceCode.Equals(adminPro)).ToList();
            else listA = db.Provinces.ToList();
            var listS = db.Provinces.ToList();
            return Json(new { listsearch = listS, listAE = listA }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEdit(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var obj = db.Admins.Where(x => x.AdminID == id).FirstOrDefault();
            string pass = Encrypt.DecryptString(obj.AdminPasswordHash, "PD").Trim();
            obj.AdminPasswordHash = pass.Remove(pass.Length - 6, 6);
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
                    return Json("Thao tác thành công", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Có lỗi xảy ra. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenUsername(int id, string province)
        {
            string username = "sample";
            if (id == 2) { username = "tongcongty"; }
            else if (id == 3) { username = "Admin" + province; }
            else if (id == 4) { username = "giaodichvien"; }
            return Json(username, JsonRequestBehavior.AllowGet);
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