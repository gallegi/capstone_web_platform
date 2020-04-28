using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;

namespace vnpost_ocr_system.Controllers.Form
{
    public class EditFormController : Controller
    {
        public void LogEFException(DbEntityValidationException e)
        {
            /* This function is used to log EntityFramework Exception for details */
            foreach (var eve in e.EntityValidationErrors)
            {
                Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Debug.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                        ve.PropertyName,
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                        ve.ErrorMessage);
                }
            }
        }

        public string LoadImgToB64(string img_name)
        {
            /* This function is used to convert one image to Based64Sring */
            string base64_string = "";
            String path = Server.MapPath("~/FormImage"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //set the image path
            string img_path = Path.Combine(path, img_name);

            using (Image image = Image.FromFile(img_path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64_string = Convert.ToBase64String(imageBytes);
                    if (EmptyStr(base64_string))
                    {
                        throw new ImageNotFoundException("ảnh không tồn tại hoặc trên server");
                    }
                    else
                    {
                        // append base64 tag to the current image bytes
                        base64_string = string.Concat("data:image/jpg;base64,", base64_string);
                    }
                    return base64_string;
                }
            }
        }

        public bool EmptyStr(string str)
        {
            Debug.WriteLine("In Validate: " + str);
            /* This function is used to check if string is empty or null */
            if (str == null || str == "")
            {
                Debug.WriteLine("Find out empty string: " + str);
                return true;
            }
            return false;
        }
        public string FormatData(string nullable_text)
        {
            /* Remove leading and tailing space */
            string res = "";
            if (EmptyStr(nullable_text))
                res = null;
            else
                res = nullable_text.Trim();

            return res;
        }

        // GET: EditForm
        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau")]
        public ActionResult Index()
        {
            try
            {
                // initialize data
                if (Request["form_id"] == null || Request["form_id"] == "")
                {
                    throw new Exception();
                }
                else
                {
                    ViewBag.form_id = Request["form_id"];
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Json(new { status_code = "400", status = "Fail", message = "Không có form id" }, JsonRequestBehavior.AllowGet);
            }

            return View("/Views/Form/EditFormView.cshtml");
        }

        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetFormDetail")]
        public ActionResult GetFormDetail()
        {
            FullForm full_form;
            string base64_img = "";
            FormTemplate ft;

            try
            {
                if (Request["form_id"] == null || Request["form_id"] == "")
                {
                    throw new Exception();
                }
                using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
                {
                    // Production
                    ft = db.Database.SqlQuery<FormTemplate>(
                        "select * from FormTemplate f where f.FormID= @FormID",
                        new SqlParameter("FormID", LongExtensions.ParseNullableLong(Request["form_id"]))).FirstOrDefault();

                    Debug.WriteLine(ft.FormName);
                    if (ft != null)
                    {
                        // Load image
                        base64_img = LoadImgToB64(ft.FormImageLink);
                    }
                    full_form = new FullForm();
                    full_form.ft = ft;
                    full_form.image = base64_img;
                }
            }
            catch (Exception e)
            {
                if (e is ImageNotFoundException)
                {
                    return Json(new { status_code = "400", status = "Fail", message = "Ảnh không load được" }, JsonRequestBehavior.AllowGet);
                }
                else if (e is DbEntityValidationException)
                {
                    LogEFException((DbEntityValidationException) e);
                }

                Debug.WriteLine(e);
                return Json(new { status_code = "400", status = "Fail", message = "Cõ lỗi xảy ra. Vui lòng thử lại sau"}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status_code = "200", status = "Success", full_form = full_form }, JsonRequestBehavior.AllowGet);
        }

        // Get all province in the database
        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetAllProvince")]
        [HttpPost]
        public ActionResult GetAllProvince()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> list = db.Database.SqlQuery<Province>("select * from Province order by PostalProvinceName asc").ToList()
                .Select(x => new Province
                {
                    PostalProvinceCode = x.PostalProvinceCode,
                    PostalProvinceName = x.PostalProvinceName,
                    ProvinceCode = x.ProvinceCode,
                    ProvinceShortName = x.ProvinceShortName

                }).ToList();
            return Json(list);
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetDistrictByProvCode")]
        [HttpPost]
        public ActionResult GetDistrictByProvCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<District> list = db.Database.SqlQuery<District>("select * " +
                "from District where PostalProvinceCode = @code order by PostalDistrictName asc", new SqlParameter("code", code)).ToList()
                .Select(x => new District
                {
                    PostalDistrictCode = x.PostalDistrictCode,
                    PostalDistrictName = x.PostalDistrictName
                }).ToList();
            return Json(list);
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetPublicAdminsByDistrictCode")]
        [HttpPost]
        public ActionResult GetPubAdminsByDistrictCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<PublicAdministration> list = db.Database.SqlQuery<PublicAdministration>("" +
                "select * " +
                "from PublicAdministration pa inner join PostOffice po on pa.PosCode = po.PosCode " +
                "where po.DistrictCode = @code order by pa.PublicAdministrationName asc", new SqlParameter("code", code)).ToList()
                .Select(x => new PublicAdministration
                {
                    PublicAdministrationLocationID = x.PublicAdministrationLocationID,
                    PublicAdministrationName = x.PublicAdministrationName,
                    Address = x.Address
                }).ToList();
            return Json(list);
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetProfileByPAId")]
        [HttpPost]
        public ActionResult GetProfileByPAId(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Profile> list = db.Database.SqlQuery<Profile>("select * " +
                "from Profile where PublicAdministrationLocationID = @code order by ProfileName asc", new SqlParameter("code", code)).ToList()
                .Select(x => new Profile
                {
                    ProfileID = x.ProfileID,
                    ProfileName = x.ProfileName
                }).ToList();
            return Json(list);
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/GetAllFixedValue")]
        [HttpPost]
        public ActionResult GetAllFixedValue()
        {
            string province_id = Request["province_id"];
            string district_id = Request["district_id"];
            string pub_administration_loc_id = Request["pub_administration_loc_id"];
            string profile_id = Request["profile_id"];
            string[] result = new string[4];
            try
            {
                VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
                if (FormatData(province_id) != null)
                {
                    Province province = db.Database.SqlQuery<Province>("" +
                    "select * " +
                    "from Province p " +
                    "where p.PostalProvinceCode = @postal_province_code", new SqlParameter("postal_province_code", province_id)).FirstOrDefault();
                    if (province != null)
                    {
                        result[0] = FormatData(province.PostalProvinceName) == null ? null : FormatData(province.PostalProvinceName);
                    }
                }

                if (FormatData(district_id) != null)
                {
                    District district = db.Database.SqlQuery<District>("" +
                    "select * " +
                    "from District d " +
                    "where d.District = @DistrictID", new SqlParameter("DistrictID", district_id)).FirstOrDefault();
                    if (district != null)
                    {
                        result[1] = FormatData(district.PostalDistrictName) == null ? null : FormatData(district.PostalDistrictName);
                    }
                }

                if (FormatData(district_id) != null)
                {
                    PublicAdministration pa = db.Database.SqlQuery<PublicAdministration>("" +
                    "select * " +
                    "from PublicAdministration pa " +
                    "where pa.PublicAdministrationLocationID = @pub_admin_loc_id", new SqlParameter("pub_admin_loc_id", pub_administration_loc_id)).FirstOrDefault();
                    if (pa != null)
                    {
                        result[2] = FormatData(pa.PublicAdministrationName) == null ? null : FormatData(pa.PublicAdministrationName);
                    }
                }

                if (FormatData(district_id) != null)
                {
                    Profile profile = db.Database.SqlQuery<Profile>("" +
                    "select * " +
                    "from Profile prof " +
                    "where prof.ProfileID = @profile_id", new SqlParameter("profile_id", profile_id)).FirstOrDefault();
                    if (profile != null)
                    {
                        result[3] = FormatData(profile.ProfileName) == null ? null : FormatData(profile.ProfileName);
                    }
                }

                return Json(new { 
                                    status_code = "200", 
                                    status = "Success", 
                                    message = "Xoá biểu mẫu thành công", 
                                    result_name = result,
                                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi xóa biểu mẫu" }, JsonRequestBehavior.AllowGet);
            }
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chinh-sua-bieu-mau/Edit")]
        [HttpPost]
        public ActionResult EditForm()
        {
            string form_id = Request["form_id"];
            if (form_id.Trim() == "")
                return Json(new { status_code = "400", status = "Fail", message = "Thiếu ID của biểu mẫu" }, JsonRequestBehavior.AllowGet);
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string query = "delete from FormTemplate where FormID = @form_id";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("form_id", form_id));
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi xóa biểu mẫu" }, JsonRequestBehavior.AllowGet);
                }
                transaction.Commit();
                return Json(new { status_code = "200", status = "Success", message = "Xoá biểu mẫu thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}