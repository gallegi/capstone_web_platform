using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

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

        public bool EmptyStr(string str)
        {
            Debug.WriteLine("In Validate: " + str);
            /* This function is used to check if string is empty or null */
            if (str == null || str == "" || str == "null")
            {
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
        public string FormatImgName(string img_name)
        {
            string[] img_extensions = { ".png", ".jpeg", ".jpg", ".bmp", ".gif" };
            string non_extension_name = img_name;
            foreach (string ext in img_extensions)
            {
                if (!EmptyStr(img_name))
                {
                    int index = non_extension_name.IndexOf(ext, StringComparison.OrdinalIgnoreCase);

                    if (index > 0)
                    { //non_extension_name.Contains(ext)
                        non_extension_name = (index < 0) ? non_extension_name : non_extension_name.Remove(index, ext.Length);
                        non_extension_name = string.Concat(non_extension_name, ext);
                    }
                }
            }
            return non_extension_name;
        }
        public bool SaveImage(Image sourceimage, string ImgName)
        {
            /* This function is used to save image before add new form to DB */
            try
            {
                string path = "/FormImage/";
                if (!Directory.Exists(HostingEnvironment.MapPath(path)))
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath(path));
                }
                if (sourceimage.Size != null)
                {
                    sourceimage.Save(HostingEnvironment.MapPath(path + ImgName));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("There is error while saving image" + e);
                return false;
            }

            return true;
        }
        
        protected string ConvertEntJson<T>(T full_form)
        {
            string json_text = "";
            try
            {
                json_text = JsonConvert.SerializeObject(full_form, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                Debug.WriteLine(json_text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return json_text;
        }
        public bool IsDuplicate(List<FormTemplate> list, string form_name, long form_id )
        {
            /* This function check whether input form_name exist in list or not */
            foreach (FormTemplate ft in list)
            {
                if (form_id != ft.FormID && form_name == ft.FormName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ValidateFormName(string form_name, long form_id)
        {
            /* Validate whether new form name is duplicated in DB or not*/
            try
            {
                VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
                List<FormTemplate> list = db.Database.SqlQuery<FormTemplate>("select * " +
                    "from FormTemplate order by FormName asc").ToList()
                    .Select(x => new FormTemplate
                    {
                        FormID = x.FormID,
                        FormName = x.FormName
                    }).ToList();
                Boolean is_duplicated = IsDuplicate(list, form_name, form_id);
                if (is_duplicated)
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public Tuple<Boolean, string> ValidateInput(string form_id, string form_name, string api_output)
        {
            // 1. Validate non-empty form id
            long l_form_id;
            if (LongExtensions.ParseNullableLong(form_id.Trim()) != null)
            {
                l_form_id = LongExtensions.ParseLong(form_id.Trim());
            }
            else
            {
                Debug.WriteLine("In validate: " + LongExtensions.ParseNullableLong(form_id.Trim()));
                Boolean status = false;
                string msg = "Form ID không được rỗng";
                return Tuple.Create(status, msg);
            }

            // 2. Validate non-empty: form_name, form_img, form_img_link, api_output
            if (EmptyStr(form_name))
            {
                Boolean status = false;
                string msg = "Tên biểu mẫu không được rỗng";
                return Tuple.Create(status, msg);
            }
            else if (EmptyStr(api_output))
            {
                Boolean status = false;
                string msg = "Kết quả OCR không được rỗng";
                return Tuple.Create(status, msg);
            }

            // 3. Validate non-duplicate form_name
            if (ValidateFormName(form_name, l_form_id) == false)
            {
                Boolean status = false;
                string msg = "Tên biểu mẫu đã bị trùng";
                return Tuple.Create(status, msg);
            }

            return Tuple.Create(true, "Không có lỗi với data input");
        }

        Type GetStaticType<T>(T x) { return typeof(T); }

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
                    if (ft == null)
                    {
                        throw new Exception();
                    }
                }
                Debug.WriteLine("JSON data: \n" + ConvertEntJson<FormTemplate>(ft));

                return Json(new { status_code = "200", status = "Success", form_template = ConvertEntJson<FormTemplate>(ft) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                if (e is ImageNotFoundException)
                {
                    return Json(new { status_code = "400", status = "Fail", message = "Ảnh không load được" }, JsonRequestBehavior.AllowGet);
                }
                else if (e is DbEntityValidationException)
                {
                    LogEFException((DbEntityValidationException)e);
                }

                Debug.WriteLine(e);
                return Json(new { status_code = "400", status = "Fail", message = "Cõ lỗi xảy ra. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
            }

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

                return Json(new
                {
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
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                long l_form_id;
                string form_img_link = "";
                FormTemplate ft;
                try
                {
                    // 1. Validate input first
                    Tuple<Boolean, string> validation = ValidateInput(Request["form_id"].Trim(), Request["form_name"].Trim(), Request["api_output"]);
                    if (validation.Item1 == false)
                    {
                        string msg = string.Concat("Bad request.\n", validation.Item2);
                        Debug.WriteLine(msg);
                        return Json(new { status_code = "400", status = "Fail", message = msg }, JsonRequestBehavior.AllowGet);
                    }
                    
                    l_form_id = LongExtensions.ParseLong(Request["form_id"].Trim());
                    ft = db.FormTemplates.Where(f => f.FormID == l_form_id).FirstOrDefault();

                    // 2. Save image
                    if (Request["image_changed"] == "1")
                    {
                        string time_stamp = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ssfff");
                        form_img_link = FormatImgName(string.Concat(Request["form_img_link"].Trim(), time_stamp));
                        Image sourceimage = Image.FromStream(Request.Files["form_img"].InputStream, true, true);
                        bool is_save_success = SaveImage(sourceimage, form_img_link);
                        if (is_save_success == false)
                        {
                            string msg = "Ảnh biểu mẫu chưa được lưu vào database.\nXin vui lòng thử lại sau ít phút";
                            Debug.WriteLine(msg);
                            return Json(new { status_code = "400", status = "Fail", message = msg }, JsonRequestBehavior.AllowGet);
                        }
                        ft.FormImageLink = form_img_link;
                    }

                    //ft = new FormTemplate();
                    // 3. Start updating 
                    // Basic info
                    ft.FormName = Request["form_name"].Trim();
                    ft.APIOutput = Request["api_output"].Trim();

                    // Profile
                    ft.FormScopeLevel = IntegerExtensions.ParseNullableInt(Request["form_scope_level"].Trim());
                    ft.PostalProvinceCode = FormatData(Request["postal_province_code"].Trim());
                    ft.ProvinceParseType = IntegerExtensions.ParseNullableInt(Request["province_parse_type"].Trim());
                    Debug.WriteLine("Province: " + FormatData(Request["postal_province_code"].Trim()) + ", " + Request["province_parse_type"]);
                    ft.ProvinceNERIndex = IntegerExtensions.ParseNullableInt(Request["province_ner_index"].Trim());
                    ft.ProvinceRegex = FormatData(Request["province_regex"]);


                    ft.PostalDistrictCode = FormatData(Request["postal_district_code"].Trim());
                    ft.DistrictParseType = IntegerExtensions.ParseNullableInt(Request["district_parse_type"].Trim());
                    Debug.WriteLine("District: " + FormatData(Request["postal_district_code"].Trim()) + ", " + GetStaticType(Request["postal_district_code"]) + ", " + Request["district_parse_type"]);
                    ft.DistrictNERIndex = IntegerExtensions.ParseNullableInt(Request["district_ner_index"].Trim());
                    ft.DistrictRegex = FormatData(Request["district_regex"]);

                    ft.PublicAdministrationParseType = IntegerExtensions.ParseNullableInt(Request["public_administration_parse_type"].Trim());
                    Debug.WriteLine("PA: " + ft.ProvinceParseType + ", " + Request["public_administration_parse_type"]);
                    ft.PublicAdministrationLocationID = LongExtensions.ParseNullableLong(Request["public_administration_location_id"].Trim());
                    ft.PublicAdministrationNERIndex = IntegerExtensions.ParseNullableInt(Request["public_administration_ner_index"].Trim());
                    ft.PublicAdministrationRegex = FormatData(Request["public_administration_regex"]);

                    ft.ProfileParseType = IntegerExtensions.ParseNullableInt(Request["profile_parse_type"].Trim());
                    Debug.WriteLine("Profile: " + ft.ProvinceParseType + ", " + Request["profile_parse_type"]);
                    ft.ProfileID = LongExtensions.ParseNullableLong(Request["profile_id"].Trim());
                    ft.ProfileNERIndex = IntegerExtensions.ParseNullableInt(Request["profile_ner_index"]);
                    ft.ProfileRegex = FormatData(Request["profile_regex"].Trim());

                    ft.AppointmentLetterCodeParseType = IntegerExtensions.ParseNullableInt(Request["appointment_letter_code_parse_type"].Trim());
                    Debug.WriteLine("AppointmentLetterCodeParseType: " + ft.ProvinceParseType + ", " + Request["appointment_letter_code_parse_type"]);
                    ft.AppointmentLetterCodeNERIndex = IntegerExtensions.ParseNullableInt(Request["appointment_letter_code_ner_index"].Trim());
                    ft.AppointmentLetterCodeRegex = FormatData(Request["appointment_letter_code_regex"]);

                    // Procedurer
                    ft.ProcedurerFullNameParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_full_name_parse_type"].Trim());
                    ft.ProcedurerFullNameNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_full_name_ner_index"].Trim());
                    ft.ProcedurerFullNameRegex = FormatData(Request["procedurer_full_name_regex"]);

                    ft.ProcedurerPhoneParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_phone_parse_type"].Trim());
                    ft.ProcedurerPhoneNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_phone_ner_index"].Trim());
                    ft.ProcedurerPhoneRegex = FormatData(Request["procedurer_phone_regex"]);

                    ft.ProcedurerProvinceParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_province_parse_type"].Trim());
                    ft.ProcedurerProvinceNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_province_ner_index"].Trim());
                    ft.ProcerdurerProvinceRegex = FormatData(Request["procedurer_province_regex"]);

                    ft.ProcedurerDistrictParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_district_parse_type"].Trim());
                    ft.ProcedurerDistrictNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_district_ner_index"].Trim());
                    ft.ProcedurerDistrictRegex = FormatData(Request["procedurer_district_regex"]);

                    ft.ProcedurerStreetParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_street_parse_type"].Trim());
                    ft.ProcedurerStreetNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_street_ner_index"].Trim());
                    ft.ProcedurerStreetRegex = FormatData(Request["procedurer_street_regex"]);

                    ft.ProcedurerPersonalPaperTypeParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_type_parse_type"].Trim());
                    ft.ProcedurerPersonalPaperTypeNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_type_ner_index"].Trim());
                    ft.ProcedurerPersonalPaperTypeRegex = FormatData(Request["procedurer_personal_paper_type_regex"]);

                    ft.ProcedurerPersonalPaperNumberParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_number_parse_type"].Trim());
                    ft.ProcedurerPersonalPaperNumberNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_number_ner_index"].Trim());
                    ft.ProcedurerPersonalPaperNumberRegex = FormatData(Request["procedurer_personal_paper_number_regex"]);

                    ft.ProcedurerPersonalPaperIssuedDateParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_issued_date_parse_type"].Trim());
                    ft.ProcedurerPersonalPaperIssuedDateNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_issued_date_ner_index"].Trim());
                    ft.ProcedurerPersonalPaperIssuedDateRegex = FormatData(Request["procedurer_personal_paper_issued_date_regex"]);

                    ft.ProcedurerPersonalPaperIssuedPlaceParseType = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_issued_place_parse_type"].Trim());
                    ft.ProcedurerPersonalPaperIssuedPlaceNERIndex = IntegerExtensions.ParseNullableInt(Request["procedurer_personal_paper_issued_place_ner_index"].Trim());
                    ft.ProcedurerPersonalPaperIssuedPlaceRegex = FormatData(Request["procedurer_personal_paper_issued_place_regex"]);

                    // Sender
                    ft.SenderFullNameParseType = IntegerExtensions.ParseNullableInt(Request["sender_full_name_parse_type"].Trim());
                    ft.SenderFullNameNERIndex = IntegerExtensions.ParseNullableInt(Request["sender_full_name_ner_index"].Trim());
                    ft.SenderFullNameRegex = FormatData(Request["sender_full_name_regex"]);

                    ft.SenderPhoneParseType = IntegerExtensions.ParseNullableInt(Request["sender_phone_parse_type"].Trim());
                    ft.SenderPhoneNERIndex = IntegerExtensions.ParseNullableInt(Request["sender_phone_ner_index"].Trim());
                    ft.SenderPhoneRegex = FormatData(Request["sender_phone_regex"]);

                    ft.SenderProvinceParseType = IntegerExtensions.ParseNullableInt(Request["sender_province_parse_type"].Trim());
                    ft.SenderProvinceNERIndex = IntegerExtensions.ParseNullableInt(Request["sender_province_ner_index"].Trim());
                    ft.SenderProvinceRegex = FormatData(Request["sender_province_regex"]);

                    ft.SenderDistrictParseType = IntegerExtensions.ParseNullableInt(Request["sender_district_parse_type"].Trim());
                    ft.SenderDistrictNERIndex = IntegerExtensions.ParseNullableInt(Request["sender_district_ner_index"].Trim());
                    ft.SenderDistrictRegex = FormatData(Request["sender_district_regex"]);

                    ft.SenderStreetParseType = IntegerExtensions.ParseNullableInt(Request["sender_street_parse_type"].Trim());
                    ft.SenderStreetNERIndex = IntegerExtensions.ParseNullableInt(Request["sender_street_ner_index"].Trim());
                    ft.SenderStreetRegex = FormatData(Request["sender_street_regex"]);

                    // Receiver
                    ft.ReceiverFullNameParseType = IntegerExtensions.ParseNullableInt(Request["receiver_full_name_parse_type"].Trim());
                    ft.ReceiverFullNameNERIndex = IntegerExtensions.ParseNullableInt(Request["receiver_full_name_ner_index"].Trim());
                    ft.ReceiverFullNameRegex = FormatData(Request["receiver_full_name_regex"]);

                    ft.ReceiverPhoneParseType = IntegerExtensions.ParseNullableInt(Request["receiver_phone_parse_type"].Trim());
                    ft.ReceiverPhoneNERIndex = IntegerExtensions.ParseNullableInt(Request["receiver_phone_ner_index"].Trim());
                    ft.ReceiverPhoneRegex = FormatData(Request["receiver_phone_regex"]);

                    ft.ReceiverProvinceParseType = IntegerExtensions.ParseNullableInt(Request["receiver_province_parse_type"].Trim());
                    ft.ReceiverProvinceNERIndex = IntegerExtensions.ParseNullableInt(Request["receiver_province_ner_index"].Trim());
                    ft.ReceiverProvinceRegex = FormatData(Request["receiver_province_regex"]);

                    ft.ReceiverDistrictParseType = IntegerExtensions.ParseNullableInt(Request["receiver_district_parse_type"].Trim());
                    ft.ReceiverDistrictNERIndex = IntegerExtensions.ParseNullableInt(Request["receiver_district_ner_index"].Trim());
                    ft.ReceiverDistrictRegex = FormatData(Request["receiver_district_regex"]);

                    ft.ReceiverStreetParseType = IntegerExtensions.ParseNullableInt(Request["receiver_street_parse_type"].Trim());
                    ft.ReceiverStreetNERIndex = IntegerExtensions.ParseNullableInt(Request["receiver_street_ner_index"].Trim());
                    ft.ReceiverStreetRegex = FormatData(Request["receiver_street_regex"]);

                    // 4. Time                    
                    ft.LastModifiedTime = DateTime.Now;
                    db.SaveChanges();
                    transaction.Commit();

                    // 5. Send train request to AI Server
                    Postman pm = new Postman();
                    string url = "https://ocr.vnpost.tech/retrain";
                    pm.SendRequest(url, "{\"action\":\"update\"}");
                    
                    return Json(new { status_code = "200", status = "Success", form_id = l_form_id }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    if (e is DbEntityValidationException)
                    {
                        LogEFException((DbEntityValidationException)e);
                    }

                    Debug.WriteLine(e + "\n" + e.Message);
                    transaction.Rollback();
                    return Json(new { status_code = "400", status = "Fail", message = "Có lỗi xảy ra khi thêm biểu mẫu. Vui lòng thử lại sau ít phút" },
                        JsonRequestBehavior.AllowGet);
                }

                
            }
        }
    }

}