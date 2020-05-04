using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Form
{
    public class DetailFormController : Controller
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
            //Debug.WriteLine("In Validate: " + str);
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
            if (nullable_text == null || nullable_text == "")
            {
                res = null;
            }
            else
            {
                res = nullable_text.Trim();

            }
            return res;
        }

        // GET: DetailForm
        [Auther(Roles = "1")]
        [Route("bieu-mau/chi-tiet-bieu-mau")]
        public ActionResult Index()
        {
            try
            {
                // initialize data
                ViewBag.status = "200";
                ViewBag.status_code = "Success";
                ViewBag.msg = "Load chi tiết biểu mẫu thành công";

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
                return Json(new { status_code = "400", status = "Fail", message = "Không có form_id" }, JsonRequestBehavior.AllowGet);
            }

            return View("/Views/Form/DetailFormView.cshtml");
        }

        // GET: DetailForm
        [Auther(Roles = "1")]
        [Route("bieu-mau/chi-tiet-bieu-mau/GetFormDetail")]
        public ActionResult GetFormDetail()
        {
            FullFormDetail full_form;
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
                        new SqlParameter("FormID", Request["form_id"])).FirstOrDefault();

                    Debug.WriteLine(ft.FormName);
                    full_form = new FullFormDetail();
                    if (ft != null)
                    {
                        // Load image
                        base64_img = LoadImgToB64(ft.FormImageLink);
                        full_form.ft = ft;
                        full_form.image = ft.FormImageLink;
                        Debug.WriteLine("image link: " + full_form.image);
                    }
                }
                Debug.WriteLine("hrere");
                return Json(new { status_code = "200", status = "Success", full_form = ConvertEntJson(full_form)}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception");
                string message = "";
                if (e is ImageNotFoundException)
                {
                    Debug.WriteLine("Image not found while loading !!!");
                    message = "Ảnh không load được";
                }
                else if (e is DbEntityValidationException)
                {
                    LogEFException((DbEntityValidationException)e);
                }
                else
                {
                    Debug.WriteLine(e.Message);
                    message = "Có lỗi. Không lấy được thông tin về biểu mẫu";
                }
                return Json(new { status_code = "400", status = "Fail", message = message }, JsonRequestBehavior.AllowGet);
            }

        }

        // ------------------------------------------------- Delete form -------------------------------------------------------------
        public string ConvertEntJson(FullFormDetail full_form)
        {
            string json_text = "";
            try
            {
                json_text = JsonConvert.SerializeObject(full_form);
                Debug.WriteLine(json_text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return json_text;
        }


        //[Auther(Roles = "1")]
        //[Route("bieu-mau/chi-tiet-bieu-mau/Delete")]
        //[HttpPost]
        //public ActionResult DeleteForm()
        //{
        //    string form_id = Request["form_id"];

        //    if (form_id.Trim() == "")
        //        return Json(new { status_code = "400", status = "Fail", message = "Thiếu ID của biểu mẫu" }, JsonRequestBehavior.AllowGet);
        //    VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        //    using (DbContextTransaction transaction = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Get information of the deleted form
        //            FormTemplate ft;
        //            ft = db.Database.SqlQuery<FormTemplate>("select * from FormTemplate f where f.FormID= @FormID",
        //                new SqlParameter("FormID", LongExtensions.ParseNullableLong(form_id))).FirstOrDefault();

        //            Debug.WriteLine(ft.FormName);

        //            // Load image
        //            string base64_img = "";
        //            if (ft != null)
        //            {
        //                base64_img = LoadImgToB64(ft.FormImageLink);
        //            }

        //            // Load Full Form
        //            FullFormDetail full_form = new FullFormDetail();
        //            full_form.ft = ft;
        //            full_form.image = base64_img;
        //            full_form.action = "delete";

        //            // Send train request to AI Server
        //            Postman pm = new Postman();
        //            string url = "http://localhost:3978";
        //            string json_text = ConvertEntJson(full_form);
        //            pm.SendRequest(url, json_text);

        //            // Delete from database 
        //            string query = "delete from FormTemplate where FormID = @form_id";
        //            db.Database.ExecuteSqlCommand(query, new SqlParameter("form_id", LongExtensions.ParseNullableLong(form_id)));
        //            db.SaveChanges();

        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi xóa biểu mẫu" }, JsonRequestBehavior.AllowGet);
        //        }
        //        transaction.Commit();
        //        return Json(new { status_code = "200", status = "Success", message = "Xoá biểu mẫu thành công" }, JsonRequestBehavior.AllowGet);
        //    }
        //}



        [Auther(Roles = "1")]
        [Route("bieu-mau/chi-tiet-bieu-mau/GetAllFixedValue")]
        [HttpPost]
        public ActionResult GetAllFixedValue()
        {
            string province_id = Request["province_id"];
            string district_id = Request["district_id"];
            string pub_administration_loc_id = Request["pub_administration_loc_id"];
            string profile_id = Request["profile_id"];
            string[] result = new string[4];
            Debug.WriteLine(province_id + ", " + district_id + ", " + pub_administration_loc_id + ", " + profile_id + ", ");
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
                        Debug.WriteLine(result[0]);

                    }
                }

                if (FormatData(district_id) != null)
                {
                    District district = db.Database.SqlQuery<District>("" +
                    "select * " +
                    "from District d " +
                    "where d.PostalDistrictCode = @DistrictID", new SqlParameter("DistrictID", district_id)).FirstOrDefault();
                    if (district != null)
                    {
                        result[1] = FormatData(district.PostalDistrictName) == null ? null : FormatData(district.PostalDistrictName);
                        Debug.WriteLine(result[1]);

                    }
                }

                if (FormatData(pub_administration_loc_id) != null)
                {
                    PublicAdministration pa = db.Database.SqlQuery<PublicAdministration>("" +
                    "select * " +
                    "from PublicAdministration pa " +
                    "where pa.PublicAdministrationLocationID = @pub_admin_loc_id",
                    new SqlParameter("pub_admin_loc_id", LongExtensions.ParseNullableLong(pub_administration_loc_id))).FirstOrDefault();
                    if (pa != null)
                    {
                        result[2] = FormatData(pa.PublicAdministrationName) == null ? null : FormatData(pa.PublicAdministrationName);
                        Debug.WriteLine(result[2]);

                    }
                }

                if (FormatData(profile_id) != null)
                {
                    Profile profile = db.Database.SqlQuery<Profile>("" +
                    "select * " +
                    "from Profile prof " +
                    "where prof.ProfileID = @profile_id", new SqlParameter("profile_id", LongExtensions.ParseNullableLong(profile_id))).FirstOrDefault();
                    if (profile != null)
                    {
                        result[3] = FormatData(profile.ProfileName) == null ? null : FormatData(profile.ProfileName);
                        Debug.WriteLine(result[3]);

                    }
                }


                return Json(new
                {
                    status_code = "200",
                    status = "Success",
                    message = "Lấy thông tin fix value thành công",
                    result_name = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi lấy thông tin về fixed-value và NER" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
    
}