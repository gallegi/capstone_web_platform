﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;
using System.IO;
using Newtonsoft.Json;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen21Controller : Controller
    {
        [Auther(Roles = "0")]
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        public ActionResult Index()
        {
            if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                District UserDistricts = db.Database.SqlQuery<District>("select d.* from District d inner join Customer c on d.PostalDistrictCode = c.PostalDistrictID where c.CustomerID = @CustomerID",
                    new SqlParameter("CustomerID", Session["userID"].ToString())).FirstOrDefault();
                ViewBag.UserProvinceCode = UserDistricts == null ? "" : UserDistricts.PostalProvinceCode;
                ViewBag.UserDistrictsID = UserDistricts == null ? "" : UserDistricts.PostalDistrictCode;

                List<Province> provinces = db.Provinces.OrderBy(x => x.PostalProvinceName).ToList();
                ViewBag.provinces = provinces;

                List<District> districts = db.Database.SqlQuery<District>("select * from District where PostalProvinceCode = @PostalProvinceCode",
                    new SqlParameter("PostalProvinceCode", provinces.First().PostalProvinceCode)).ToList();
                string select = "";
                foreach (District item in districts)
                {
                    select += "<option value=" + item.PostalDistrictCode + ">" + item.PostalDistrictName + "</option>";
                }
                ViewBag.select = select;

                List<ContactInfoDB> contactInfos = db.Database.SqlQuery<ContactInfoDB>(@"select ci.*, ppt.PersonalPaperTypeName, d.PostalDistrictName, p.PostalProvinceName 
                    from Customer c inner join ContactInfo ci on c.CustomerID = ci.CustomerID
                    left join PersonalPaperType ppt on ci.PersonalPaperTypeID = ppt.PersonalPaperTypeID
					inner join District d on ci.PostalDistrictCode = d.PostalDistrictCode
					inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode
                    where c.CustomerID = @CustomerID", new SqlParameter("CustomerID", Session["userID"].ToString())).ToList();
                ViewBag.contactInfos = contactInfos;

                List<PersonalPaperType> papertypes = db.PersonalPaperTypes.ToList();
                ViewBag.papertypes = papertypes;
            }
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/InvitationCard/InputInformationScreen21.cshtml");
            }
            else
            {
                return View("/Views/InvitationCard/InputInformationScreen21.cshtml");
            }

        }


        [Auther(Roles = "0")]
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/Add")]
        [HttpPost]
        public ActionResult Add()
        {
            //if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            string FullName = Request["FullName"];
            string Phone = Request["Phone"];
            string PostalDistrictCode = Request["PostalDistrictCode"];
            string Street = Request["Street"];
            if (FullName.Trim() == "")
                return Json(new { success = false, message = "Vui lòng nhập Họ và tên" });
            if (Phone.Trim() == "" || PostalDistrictCode == "")
                return Json(new { success = false, message = "Không được để trống" });
            if (!Regex.IsMatch(Phone, "^0[35789]\\d{8}$"))
                return Json(new { success = false, message = "Số điện thoại không hợp lệ" });

            string PersonalPaperTypeID = Request["PersonalPaperTypeID"];
            string PersonalPaperNumber = Request["PersonalPaperNumber"];
            string PersonalPaperIssuedDateString = Request["PersonalPaperIssuedDateString"];
            string PersonalPaperIssuedPlace = Request["PersonalPaperIssuedPlace"];
            string ContactInfoID = Request["ContactInfoID"];
            try
            {
                using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
                {
                    ContactInfo c = ContactInfoID == "" ? new ContactInfo() : db.ContactInfoes.Find(int.Parse(ContactInfoID));
                    if (ContactInfoID != "" && !c.CustomerID.Equals(long.Parse(Session["userID"].ToString()))) return Json(new { success = false, message = "Có lỗi xảy ra" });
                    c.FullName = FullName;
                    c.Phone = Phone;
                    District district = db.Districts.Find(PostalDistrictCode);
                    if (district == null)
                        return Json(new { success = false, message = "Quận/huyện không tồn tại" });
                    Province province = db.Provinces.Find(district.PostalProvinceCode);
                    c.PostalDistrictCode = PostalDistrictCode;
                    c.Street = Street;
                    PersonalPaperType type = PersonalPaperTypeID == "" ? null : db.PersonalPaperTypes.Find(int.Parse(PersonalPaperTypeID));
                    if (type == null)
                    {
                        c.PersonalPaperTypeID = null;
                        c.PersonalPaperNumber = null;
                        c.PersonalPaperIssuedDate = null;
                        c.PersonalPaperIssuedPlace = null;
                    }
                    else
                    {
                        c.PersonalPaperTypeID = int.Parse(PersonalPaperTypeID);
                        c.PersonalPaperNumber = PersonalPaperNumber;
                        DateTime temp;
                        if (!DateTime.TryParseExact(PersonalPaperIssuedDateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out temp))
                            c.PersonalPaperIssuedDate = null;
                        else
                            c.PersonalPaperIssuedDate = DateTime.ParseExact(PersonalPaperIssuedDateString, "dd/MM/yyyy", null);
                        c.PersonalPaperIssuedPlace = PersonalPaperIssuedPlace;
                    }
                    if (ContactInfoID == "")
                    {
                        c.CustomerID = long.Parse(Session["userID"].ToString());
                        db.ContactInfoes.Add(c);
                        db.SaveChanges();
                        //1 có đủ thông tin, 2 và 3 thiếu giấy tờ tùy thân
                        List<string> list = new List<string>();
                        for (int i = 1; i < 4; i++)
                        {
                            string html = "<div id='contact" + i + @"' class='info-list-div col s12'>
                                                <div class='col s8 m8 l8'>
                                                    <p id='FullName' class='content-text highlight col s12'>" + c.FullName + @"</p>
                                                    <p class='content-text col s12'><span id='Street'>" + c.Street + @"</span>, <span data-district='" + c.PostalDistrictCode + @"' id='Address'>" + district.PostalDistrictName + @", " + province.PostalProvinceName + @"</span></p>
                                                    <p class='content-text col s12'>Số điện thoại: <span id='Phone'>" + c.Phone + @"</span></p>";
                            html += i == 1 ? @"
                                                    <p class='content-text col s12'>Loại giấy tờ tùy thân: <span data-papertype='" + c.PersonalPaperTypeID + @"' id='PersonalPaperTypeName'>" + (type == null ? "" : type.PersonalPaperTypeName) + @"</span></p>
                                                    <p class='content-text col s12'>Số giấy tờ tùy thân: <span id='PersonalPaperNumber'>" + (c.PersonalPaperNumber == null ? "" : c.PersonalPaperNumber) + @"</span></p>
                                                    <p class='content-text col s12'>Ngày cấp: <span id='PersonalPaperIssuedDate'>" + PersonalPaperIssuedDateString + @"</span></p>
                                                    <p class='content-text col s12'>Nơi cấp: <span id='PersonalPaperIssuedPlace'>" + c.PersonalPaperIssuedPlace + @"</span></p>"
                                        : "";
                            html += @"
                                                </div>
                                                <div class='col l4 s4 m4 is-check-step-" + (i + 1) + "' id='isCheckStep" + (i + 1) + "-" + c.ContactInfoID + @"'>
                                                </div>
                                                <div class='col s12 m-t-10'>
                                                    <div class='col p-t-10'>
                                                        <a class='btn waves-effect waves-light bt-color-common' data-profile1='" + c.ContactInfoID + @"' id='profile" + i + @"'>Sử dụng thông tin này</a>
                                                    </div>
                                                    <div class='col p-t-10'>
                                                        <a class='modal-trigger btn waves-effect waves-light bt-color-common' data-profile1='" + c.ContactInfoID + @"' data-type='" + i + @"' id='edit' href='#myform1'>Chỉnh sửa</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <p class='col s12 big-heading m-b-15'>
                                                Bạn muốn sử dụng thông tin khác?
                                                <a class='modal-trigger light-blue-text hover-underline' href='#myform1' id='add' data-type='" + i + @"'>
                                                    Nhập thông tin mới
                                                </a>
                                            </p>";
                            list.Add(html);
                        }
                        return Json(new { success = true, add = true, message = "Thêm mới thành công", html = list });
                    }
                    db.SaveChanges();
                    return Json(new { success = true, add = false, message = "Chỉnh sửa thành công" });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [Auther(Roles = "0")]
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/GetContactInfo")]
        [HttpPost]
        public ActionResult GetContactInfo(int id)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                ContactInfoDB info = db.Database.SqlQuery<ContactInfoDB>(@"select c.*, p.PostalProvinceCode from ContactInfo c inner join District d on c.PostalDistrictCode = d.PostalDistrictCode 
                        inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where ContactInfoID = @ContactInfoID",
                    new SqlParameter("ContactInfoID", id)).FirstOrDefault();
                if (!info.CustomerID.Equals(long.Parse(Session["userID"].ToString()))) return null;
                if (info == null) return null;

                List<district> districts = db.Database.SqlQuery<district>("select d.PostalDistrictCode, d.PostalDistrictName from District d inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where p.PostalProvinceCode = @PostalProvinceCode", new SqlParameter("PostalProvinceCode", info.PostalProvinceCode)).ToList();

                info.PersonalPaperIssuedDateString = info.PersonalPaperIssuedDate == null ? "" : info.PersonalPaperIssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                return Json(new { info = info, list = districts });
            }
        }
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/CheckAppointmentLetterCode")]
        [HttpPost]
        public ActionResult CheckAppointmentLetterCode(string AppointmentLetterCode)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                Order o = db.Orders.Where(x => x.AppointmentLetterCode.Equals(AppointmentLetterCode.Trim()) && x.StatusID != 0).FirstOrDefault();
                if (o == null)
                    return Json(true);
                else
                    return Json(false);
            }
        }

        public string getMatchResult(string text, dynamic pattern)
        {

            var match = Regex.Match(text, (string) pattern);
            if (match.Success)
                return match.Groups[1].Value;
            else
                return null;
        }


        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/GetOCRResult")]
        [HttpPost]
        public ActionResult GetOCRResult(OCRRaw OCRResponse)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                FormTemplate form = db.FormTemplates.Where(record => record.FormID == OCRResponse.form_id).FirstOrDefault();
                if (form != null)
                {
                    switch (form.FormScopeLevel)
                    {
                        case 4:

                            break;
                        case 3:
                            break;
                        case 2:
                            break;
                        case 1:
                            break;
                        case 0:
                            break;
                    }
                }
                return Json(new { Hello = form.PostalProvinceCode });
            }

            //return Json(new
            //{
            //    AppointmentLetterCode = AppointmentLetterCode,
            //    ProcedurerFullName = ProcedurerFullName,
            //    ProcedurerPhone = ProcedurerPhone,
            //    ProcedurerPostalProvince = ProcedurerPostalProvince,
            //    ProcedurerPostalDistrict = ProcedurerPostalDistrict,
            //    ProcedurerStreet = ProcedurerStreet,
            //    ProcedurerPersonalPaperType = ProcedurerPersonalPaperType,
            //    ProcedurerPersonalPaperNumber = ProcedurerPersonalPaperNumber,
            //    ProcedurerPersonalPaperIssuedDate = ProcedurerPersonalPaperIssuedDate,
            //    ProcedurerPersonalPaperIssuedPlace = ProcedurerPersonalPaperIssuedPlace
            //});
        }


        public class OCRParsed
        {
            public int PostalProvinceCode { get; set; }
            public int PostalDistrictCode { get; set; }
            public int PublicAdministrationLocationID { get; set; }
            public int ProfileID { get; set; }
            public string AppointmentLetterCode { get; set; }
            public string ProcedurerFullName { get; set; }
            public string ProcedurerPhone { get; set; }
            public string ProcerdurerProvince { get; set; }
            public string ProcedurerDistrict { get; set; }
            public string ProcedurerStreet { get; set; }
            public int ProcedurerPersonalPaperType { get; set; }
            public string ProcedurerPersonalPaperNumber { get; set; }
            public string ProcedurerPersonalPaperIssuedDate { get; set; }
            public string ProcedurerPersonalPaperIssuedPlace { get; set; }
            public string SenderFullName { get; set; }
            public string SenderPhone { get; set; }
            public string SenderrProvince { get; set; }
            public string SenderDistrict { get; set; }
            public string SenderStreet { get; set; }
            public int SenderPersonalPaperType { get; set; }
            public string SenderPersonalPaperNumber { get; set; }
            public string SenderPersonalPaperIssuedDate { get; set; }
            public string SenderPersonalPaperIssuedPlace { get; set; }
            public string ReceiverFullName { get; set; }
            public string ReceiverPhone { get; set; }
            public string ReceiverrProvince { get; set; }
            public string ReceiverDistrict { get; set; }
            public string ReceiverStreet { get; set; }
            public int ReceiverPersonalPaperType { get; set; }
            public string ReceiverPersonalPaperNumber { get; set; }
            public string ReceiverPersonalPaperIssuedDate { get; set; }
            public string ReceiverPersonalPaperIssuedPlace { get; set; }
        }

        public class OCRRaw
        {
            public int form_id { get; set; }
            public string raw_text { get; set; }
            public string[] province { get; set; }
            public string[] district { get; set; }
            public string[] public_administration { get; set; }
            public string[] profile { get; set; }
            public string[] appointment_letter_code { get; set; }
            public string[] name { get; set; }
            public string[] phone_number { get; set; }
            public string[] street { get; set; }
            public string[] personal_paper_type { get; set; }
            public string[] personal_paper_number { get; set; }
            public string[] issued_date { get; set; }
            public string[] issued_place { get; set; }
        }


        private class district
        {
            public string PostalDistrictCode { get; set; }
            public string PostalDistrictName { get; set; }
        }
    }
}