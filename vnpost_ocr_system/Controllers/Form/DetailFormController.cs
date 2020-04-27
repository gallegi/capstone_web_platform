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
            Debug.WriteLine("In Validate: " + str);
            /* This function is used to check if string is empty or null */
            if (str == null || str == "")
            {
                Debug.WriteLine("Find out empty string: " + str);
                return true;
            }
            return false;
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
                        new SqlParameter("FormID", Request["form_id"])).FirstOrDefault();

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
            catch (ImageNotFoundException e)
            {
                return Json(new { status_code = "400", status = "Fail", message = "Ảnh không load được" }, JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                LogEFException(e);
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Json(new { status_code = "400", status = "Fail", message = "Không có form_id" }, JsonRequestBehavior.AllowGet);
            }

            return Json(full_form);
        }


        [Auther(Roles = "1")]
        [Route("bieu-mau/chi-tiet-bieu-mau/Delete")]
        [HttpPost]
        public ActionResult DeleteForm(string code)
        {
            string form_id = Request["form_id"];
            if (form_id.Trim() == "")
                return Json(new { status_code = "400", status = "Fail", message = "Thiếu ID của biểu mẫu" }, JsonRequestBehavior.AllowGet);
            try
            {
                VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string query = "delete from FormTemplate where FormID = @form_id";
                        db.Database.ExecuteSqlCommand(query, new SqlParameter("form_id", form_id));
                        transaction.Commit();
                        db.SaveChanges();
                        return Json(new { status_code = "200", status = "Success", message = "Xoá biểu mẫu thành công" }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(400);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi xóa biểu mẫu" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
    public class FullForm
    {
        public FullForm() { }
        public FullForm(FormTemplate ft, string image) {
            this.ft = ft;
            this.image = image;
        }
        public FullForm(FormTemplate ft, string image, string action)
        {
            this.action = action;
            this.ft = ft;
            this.image = image;
        }
        public string action{ get; set; }
        public string image{ get; set; }
        public FormTemplate ft { get; set; }
    }
}