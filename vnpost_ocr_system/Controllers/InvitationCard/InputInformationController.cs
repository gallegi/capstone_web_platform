using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationController : BaseUserController
    {
        //[Auther(Roles = "0")]
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
                return View("/Views/MobileView/InvitationCard/InputInformation.cshtml");
            }
            else
            {
                return View("/Views/InvitationCard/InputInformation.cshtml");
            }

        }


        [Auther(Roles = "0")]
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/Add")]
        [HttpPost]
        public ActionResult Add()
        {
            //if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            var request = Request;
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
                    if (type == null || int.Parse(Request["Type"]) != 1)
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
                            string html = @"
                            <div class='contact" + i + @" info-list-div col s12'>
                                <div class='col s12 m12 l12'>
                                    <p class='FullName content-text highlight col s12'>" + c.FullName + @"</p>
                                    <p class='content-text col s12'><span class='Street' > " + c.Street + "</ span >, <span data-district='1130' class='Address'>" + district.PostalDistrictName + @", " + province.PostalProvinceName + @"</span></p>
                                    <p class='content-text col s12'>Số điện thoại: <span class='Phone'>" + c.Phone + @"</span></p> ";
                            html += i == 1 ? @"
                                    <p class='content-text col s12'>Loại giấy tờ tùy thân: <span data-papertype='" + c.PersonalPaperTypeID + @"' class='PersonalPaperTypeName'>" + (type == null ? "" : type.PersonalPaperTypeName) + @"</span></p>
                                    <p class='content-text col s12'>Số giấy tờ tùy thân: <span class='PersonalPaperNumber'>" + (c.PersonalPaperNumber == null ? "" : c.PersonalPaperNumber) + @"</span></p>
                                    <p class='content-text col s12'>Ngày cấp: <span class='PersonalPaperIssuedDate'>" + PersonalPaperIssuedDateString + @"</span></p>
                                    <p class='content-text col s12'>Nơi cấp: <span class='PersonalPaperIssuedPlace'>" + c.PersonalPaperIssuedPlace + @"</span></p>" : "";
                            html += @"
                                </div>
                                <div class='col s12 m-t-10'>
                                    <div class='col p-t-10'>
                                        <a class='btn waves-effect waves-light bt-color-common contactInfoType" + i + @"' data-contactinfoid='" + c.ContactInfoID + @"'>Sử dụng thông tin này</a>
                                    </div>
                                    <div class='col p-t-10'>
                                        <a class='modal-trigger btn waves-effect waves-light bt-color-common edit' data-contactinfoid='" + c.ContactInfoID + @"' data-type='" + i + @"' href='#myform1'>Chỉnh sửa</a>
                                    </ div >
                                </ div >
                            </ div > ";
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

        public string getMatchResult(string text, string pattern)
        {

            var match = Regex.Match(text, pattern);
            if (match.Success)
                return match.Groups[0].Value.Trim();
            else
                return null;
        }

        public string parseProvince(OCRRaw OCRResponse, FormTemplate form)
        {
            string province = null;
            try
            {
                if (form.ProvinceParseType == 0 && form.ProvinceNERIndex.HasValue)
                {
                    province = OCRResponse.province[form.ProvinceNERIndex.Value];
                }
                else if (form.ProvinceParseType == 1 && form.ProvinceRegex != "")
                {
                    province = getMatchResult(OCRResponse.raw_text, form.ProvinceRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return province;
        }

        public string parseDistrict(OCRRaw OCRResponse, FormTemplate form)
        {
            string district = null;
            try
            {

                if (form.DistrictParseType == 0 && form.DistrictNERIndex.HasValue)
                {
                    district = OCRResponse.district[form.DistrictNERIndex.Value];
                }
                else if (form.DistrictParseType == 1 && form.DistrictRegex != "")
                {
                    district = getMatchResult(OCRResponse.raw_text, form.DistrictRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return district;
        }

        public string parsePublicAdministration(OCRRaw OCRResponse, FormTemplate form)
        {
            string publicAdministration = null;
            try
            {

                if (form.PublicAdministrationParseType == 0 && form.PublicAdministrationNERIndex.HasValue)
                {
                    publicAdministration = OCRResponse.public_administration[form.PublicAdministrationNERIndex.Value];
                }
                else if (form.PublicAdministrationParseType == 1 && form.PublicAdministrationRegex != "")
                {
                    publicAdministration = getMatchResult(OCRResponse.raw_text, form.PublicAdministrationRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return publicAdministration;
        }

        public string parseProfile(OCRRaw OCRResponse, FormTemplate form)
        {
            string profile = null;
            try
            {

                if (form.ProfileParseType == 0 && form.ProfileNERIndex.HasValue)
                {
                    profile = OCRResponse.profile[form.ProfileNERIndex.Value];
                }
                else if (form.ProfileParseType == 1 && form.ProfileRegex != "")
                {
                    profile = getMatchResult(OCRResponse.raw_text, form.ProfileRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return profile;
        }

        public string parseAppointmentLetterCode(OCRRaw OCRResponse, FormTemplate form)
        {
            string appointmentLetterCode = null;
            try
            {

                if (form.AppointmentLetterCodeParseType == 0 && form.AppointmentLetterCodeNERIndex.HasValue)
                {
                    appointmentLetterCode = OCRResponse.appointment_letter_code[form.AppointmentLetterCodeNERIndex.Value];
                }
                else if (form.AppointmentLetterCodeParseType == 1 && form.AppointmentLetterCodeRegex != "")
                {
                    appointmentLetterCode = getMatchResult(OCRResponse.raw_text, form.AppointmentLetterCodeRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return appointmentLetterCode;
        }

        public string parseProcedurerFullName(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerFullName = null;
            try
            {

                if (form.ProcedurerFullNameParseType == 0 && form.ProcedurerFullNameNERIndex.HasValue)
                {
                    procedurerFullName = OCRResponse.name[form.ProcedurerFullNameNERIndex.Value];
                }
                else if (form.ProcedurerFullNameParseType == 1 && form.ProcedurerFullNameRegex != "")
                {
                    procedurerFullName = getMatchResult(OCRResponse.raw_text, form.ProcedurerFullNameRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerFullName;
        }

        public string parseProcedurerPhone(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerPhone = null;
            try
            {

                if (form.ProcedurerPhoneParseType == 0 && form.ProcedurerPhoneNERIndex.HasValue)
                {
                    procedurerPhone = OCRResponse.phone_number[form.ProcedurerPhoneNERIndex.Value];
                }
                else if (form.ProcedurerPhoneParseType == 1 && form.ProcedurerPhoneRegex != "")
                {
                    procedurerPhone = getMatchResult(OCRResponse.raw_text, form.ProcedurerPhoneRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerPhone;
        }

        public string parseProcedurerProvince(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerProvince = null;
            try
            {
                if (form.ProcedurerProvinceParseType == 0 && form.ProcedurerProvinceNERIndex.HasValue)
                {
                    procedurerProvince = OCRResponse.province[form.ProcedurerProvinceNERIndex.Value];
                }
                else if (form.ProcedurerProvinceParseType == 1 && form.ProcedurerProvinceRegex != "")
                {
                    procedurerProvince = getMatchResult(OCRResponse.raw_text, form.ProcedurerProvinceRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerProvince;
        }

        public string parseProcedurerDistrict(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerDistrict = null;
            try
            {

                if (form.ProcedurerDistrictParseType == 0 && form.ProcedurerDistrictNERIndex.HasValue)
                {
                    procedurerDistrict = OCRResponse.district[form.ProcedurerDistrictNERIndex.Value];
                }
                else if (form.ProcedurerDistrictParseType == 1 && form.ProcedurerDistrictRegex != "")
                {
                    procedurerDistrict = getMatchResult(OCRResponse.raw_text, form.ProcedurerDistrictRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerDistrict;
        }

        public string parseProcedurerStreet(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerStreet = null;
            try
            {

                if (form.ProcedurerStreetParseType == 0 && form.ProcedurerStreetNERIndex.HasValue)
                {
                    procedurerStreet = OCRResponse.street[form.ProcedurerStreetNERIndex.Value];
                }
                else if (form.ProcedurerStreetParseType == 1 && form.ProcedurerStreetRegex != "")
                {
                    procedurerStreet = getMatchResult(OCRResponse.raw_text, form.ProcedurerStreetRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerStreet;
        }

        public string parseProcedurerPersonalPaperType(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerPersonalPaperType = null;
            try
            {

                if (form.ProcedurerPersonalPaperTypeParseType == 0 && form.ProcedurerPersonalPaperTypeNERIndex.HasValue)
                {
                    procedurerPersonalPaperType = OCRResponse.personal_paper_type[form.ProcedurerPersonalPaperTypeNERIndex.Value];
                }
                else if (form.ProcedurerPersonalPaperTypeParseType == 1 && form.ProcedurerPersonalPaperTypeRegex != "")
                {
                    procedurerPersonalPaperType = getMatchResult(OCRResponse.raw_text, form.ProcedurerPersonalPaperTypeRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerPersonalPaperType;
        }

        public string parseProcedurerPersonalPaperNumber(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerPersonalPaperNumber = null;
            try
            {

                if (form.ProcedurerPersonalPaperNumberParseType == 0 && form.ProcedurerPersonalPaperNumberNERIndex.HasValue)
                {
                    procedurerPersonalPaperNumber = OCRResponse.personal_paper_number[form.ProcedurerPersonalPaperNumberNERIndex.Value];
                }
                else if (form.ProcedurerPersonalPaperNumberParseType == 1 && form.ProcedurerPersonalPaperNumberRegex != "")
                {
                    procedurerPersonalPaperNumber = getMatchResult(OCRResponse.raw_text, form.ProcedurerPersonalPaperNumberRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerPersonalPaperNumber;
        }

        public string parseProcedurerIssuedDate(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerIssuedDate = null;
            try
            {

                if (form.ProcedurerPersonalPaperIssuedDateParseType == 0 && form.ProcedurerPersonalPaperIssuedDateNERIndex.HasValue)
                {
                    procedurerIssuedDate = OCRResponse.issued_date[form.ProcedurerPersonalPaperIssuedDateNERIndex.Value];
                }
                else if (form.ProcedurerPersonalPaperIssuedDateParseType == 1 && form.ProcedurerPersonalPaperIssuedDateRegex != "")
                {
                    procedurerIssuedDate = getMatchResult(OCRResponse.raw_text, form.ProcedurerPersonalPaperIssuedDateRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerIssuedDate;
        }

        public string parseProcedurerIssuedPlace(OCRRaw OCRResponse, FormTemplate form)
        {
            string procedurerIssuedPlace = null;
            try
            {

                if (form.ProcedurerPersonalPaperIssuedPlaceParseType == 0 && form.ProcedurerPersonalPaperIssuedPlaceNERIndex.HasValue)
                {
                    procedurerIssuedPlace = OCRResponse.issued_place[form.ProcedurerPersonalPaperIssuedPlaceNERIndex.Value];
                }
                else if (form.ProcedurerPersonalPaperIssuedPlaceParseType == 1 && form.ProcedurerPersonalPaperIssuedPlaceRegex != "")
                {
                    procedurerIssuedPlace = getMatchResult(OCRResponse.raw_text, form.ProcedurerPersonalPaperIssuedPlaceRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return procedurerIssuedPlace;
        }

        public string parseSenderFullName(OCRRaw OCRResponse, FormTemplate form)
        {
            string SenderFullName = null;
            try
            {

                if (form.SenderFullNameParseType == 0 && form.SenderFullNameNERIndex.HasValue)
                {
                    SenderFullName = OCRResponse.name[form.SenderFullNameNERIndex.Value];
                }
                else if (form.SenderFullNameParseType == 1 && form.SenderFullNameRegex != "")
                {
                    SenderFullName = getMatchResult(OCRResponse.raw_text, form.SenderFullNameRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return SenderFullName;
        }

        public string parseSenderPhone(OCRRaw OCRResponse, FormTemplate form)
        {
            string senderPhone = null;
            try
            {
                if (form.SenderPhoneParseType == 0 && form.SenderPhoneNERIndex.HasValue)
                {
                    senderPhone = OCRResponse.phone_number[form.SenderPhoneNERIndex.Value];
                }
                else if (form.SenderPhoneParseType == 1 && form.SenderPhoneRegex != "")
                {
                    senderPhone = getMatchResult(OCRResponse.raw_text, form.SenderPhoneRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return senderPhone;
        }

        public string parseSenderProvince(OCRRaw OCRResponse, FormTemplate form)
        {
            string SenderProvince = null;
            try
            {
                if (form.SenderProvinceParseType == 0 && form.SenderProvinceNERIndex.HasValue)
                {
                    SenderProvince = OCRResponse.province[form.SenderProvinceNERIndex.Value];
                }
                else if (form.SenderProvinceParseType == 1 && form.ProcedurerProvinceRegex != "")
                {
                    SenderProvince = getMatchResult(OCRResponse.raw_text, form.ProcedurerProvinceRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return SenderProvince;
        }

        public string parseSenderDistrict(OCRRaw OCRResponse, FormTemplate form)
        {
            string SenderDistrict = null;
            try
            {
                if (form.SenderDistrictParseType == 0 && form.SenderDistrictNERIndex.HasValue)
                {
                    SenderDistrict = OCRResponse.district[form.SenderDistrictNERIndex.Value];
                }
                else if (form.SenderDistrictParseType == 1 && form.SenderDistrictRegex != "")
                {
                    SenderDistrict = getMatchResult(OCRResponse.raw_text, form.SenderDistrictRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return SenderDistrict;
        }

        public string parseSenderStreet(OCRRaw OCRResponse, FormTemplate form)
        {
            string SenderStreet = null;
            try
            {
                if (form.SenderStreetParseType == 0 && form.SenderStreetNERIndex.HasValue)
                {
                    SenderStreet = OCRResponse.street[form.SenderStreetNERIndex.Value];
                }
                else if (form.SenderStreetParseType == 1 && form.SenderStreetRegex != "")
                {
                    SenderStreet = getMatchResult(OCRResponse.raw_text, form.SenderStreetRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return SenderStreet;
        }


        public string parseReceiverFullName(OCRRaw OCRResponse, FormTemplate form)
        {
            string ReceiverFullName = null;
            try
            {
                if (form.ReceiverFullNameParseType == 0 && form.ReceiverFullNameNERIndex.HasValue)
                {
                    ReceiverFullName = OCRResponse.name[form.ReceiverFullNameNERIndex.Value];
                }
                else if (form.ReceiverFullNameParseType == 1 && form.ReceiverFullNameRegex != "")
                {
                    ReceiverFullName = getMatchResult(OCRResponse.raw_text, form.ReceiverFullNameRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return ReceiverFullName;
        }

        public string parseReceiverPhone(OCRRaw OCRResponse, FormTemplate form)
        {
            string receiverPhone = null;
            try
            {
                if (form.ReceiverPhoneParseType == 0 && form.ReceiverPhoneNERIndex.HasValue)
                {
                    receiverPhone = OCRResponse.phone_number[form.ReceiverPhoneNERIndex.Value];
                }
                else if (form.ReceiverPhoneParseType == 1 && form.ReceiverPhoneRegex != "")
                {
                    receiverPhone = getMatchResult(OCRResponse.raw_text, form.ReceiverPhoneRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return receiverPhone;
        }

        public string parseReceiverProvince(OCRRaw OCRResponse, FormTemplate form)
        {
            string ReceiverProvince = null;
            try
            {
                if (form.ReceiverProvinceParseType == 0 && form.ReceiverProvinceNERIndex.HasValue)
                {
                    ReceiverProvince = OCRResponse.province[form.ReceiverProvinceNERIndex.Value];
                }
                else if (form.ReceiverProvinceParseType == 1 && form.ProcedurerProvinceRegex != "")
                {
                    ReceiverProvince = getMatchResult(OCRResponse.raw_text, form.ProcedurerProvinceRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return ReceiverProvince;
        }

        public string parseReceiverDistrict(OCRRaw OCRResponse, FormTemplate form)
        {
            string ReceiverDistrict = null;
            try
            {
                if (form.ReceiverDistrictParseType == 0 && form.ReceiverDistrictNERIndex.HasValue)
                {
                    ReceiverDistrict = OCRResponse.district[form.ReceiverDistrictNERIndex.Value];
                }
                else if (form.ReceiverDistrictParseType == 1 && form.ReceiverDistrictRegex != "")
                {
                    ReceiverDistrict = getMatchResult(OCRResponse.raw_text, form.ReceiverDistrictRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return ReceiverDistrict;
        }

        public string parseReceiverStreet(OCRRaw OCRResponse, FormTemplate form)
        {
            string ReceiverStreet = null;
            try
            {
                if (form.ReceiverStreetParseType == 0 && form.ReceiverStreetNERIndex.HasValue)
                {
                    ReceiverStreet = OCRResponse.street[form.ReceiverStreetNERIndex.Value];
                }
                else if (form.ReceiverStreetParseType == 1 && form.ReceiverStreetRegex != "")
                {
                    ReceiverStreet = getMatchResult(OCRResponse.raw_text, form.ReceiverStreetRegex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return ReceiverStreet;
        }

        public void parseAllOther(VNPOST_AppointmentEntities db, OCRParsed parsed, OCRRaw OCRResponse, FormTemplate form)
        {
            parsed.AppointmentLetterCode = parseAppointmentLetterCode(OCRResponse, form);

            //Parse procedure information
            parsed.ProcedurerFullName = parseProcedurerFullName(OCRResponse, form);
            parsed.ProcedurerPhone = parseProcedurerPhone(OCRResponse, form);
            string parsedProcedurerProvince = parseProcedurerProvince(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedProcedurerProvince))
            {
                Province procedurerProvince = db.Provinces.SqlQuery("SELECT TOP 1 * FROM Province ORDER BY dbo.fn_LevenshteinDistance(@province_name, PostalProvinceName, 1) ASC", new SqlParameter("@province_name", parsedProcedurerProvince)).FirstOrDefault();
                parsed.ProcedurerProvincePostalCode = procedurerProvince.PostalProvinceCode;
            }
            string parsedProcedurerDistrict = parseProcedurerDistrict(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedProcedurerDistrict))
            {
                District procedurerDistrict = db.Districts.SqlQuery("SELECT TOP 1 * FROM District ORDER BY dbo.fn_LevenshteinDistance(@district_name, PostalDistrictName, 1) ASC", new SqlParameter("@district_name", parsedProcedurerDistrict)).FirstOrDefault();
                parsed.ProcedurerDistrictPostalCode = procedurerDistrict.PostalDistrictCode;
            }
            parsed.ProcedurerStreet = parseProcedurerStreet(OCRResponse, form);
            string parsedPersonalPaperType = parseProcedurerPersonalPaperType(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedPersonalPaperType))
            {
                PersonalPaperType personalPaperType = db.PersonalPaperTypes.SqlQuery("SELECT TOP 1 * FROM PersonalPaperType ORDER BY dbo.fn_LevenshteinDistance(@personal_paper_name, PersonalPaperTypeName, 1) ASC", new SqlParameter("@personal_paper_name", parsedPersonalPaperType)).FirstOrDefault();
                parsed.ProcedurerPersonalPaperType = personalPaperType.PersonalPaperTypeID;
            }
            parsed.ProcedurerPersonalPaperNumber = parseProcedurerPersonalPaperNumber(OCRResponse, form);
            parsed.ProcedurerPersonalPaperIssuedDate = parseProcedurerIssuedDate(OCRResponse, form);
            parsed.ProcedurerPersonalPaperIssuedPlace = parseProcedurerIssuedPlace(OCRResponse, form);

            //Parse sender information
            parsed.SenderFullName = parseSenderFullName(OCRResponse, form);
            parsed.SenderPhone = parseSenderPhone(OCRResponse, form);
            string parsedSenderProvince = parseSenderProvince(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedSenderProvince))
            {
                Province senderProvince = db.Provinces.SqlQuery("SELECT TOP 1 * FROM Province ORDER BY dbo.fn_LevenshteinDistance(@province_name, PostalProvinceName, 1) ASC", new SqlParameter("@province_name", parsedSenderProvince)).FirstOrDefault();
                parsed.SenderProvincePostalCode = senderProvince.PostalProvinceCode;
            }
            string parsedSenderDistrict = parseSenderDistrict(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedSenderDistrict))
            {
                District senderDistrict = db.Districts.SqlQuery("SELECT TOP 1 * FROM District ORDER BY dbo.fn_LevenshteinDistance(@district_name, PostalDistrictName, 1) ASC", new SqlParameter("@district_name", parsedSenderDistrict)).FirstOrDefault();
                parsed.SenderDistrictPostalCode = senderDistrict.PostalDistrictCode;
            }
            parsed.SenderStreet = parseSenderStreet(OCRResponse, form);

            //Parse receiver information
            parsed.ReceiverFullName = parseReceiverFullName(OCRResponse, form);
            parsed.ReceiverPhone = parseReceiverPhone(OCRResponse, form);
            string parsedReceiverProvince = parseReceiverProvince(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedReceiverProvince))
            {
                Province receiverProvince = db.Provinces.SqlQuery("SELECT TOP 1 * FROM Province ORDER BY dbo.fn_LevenshteinDistance(@province_name, PostalProvinceName, 1) ASC", new SqlParameter("@province_name", parsedReceiverProvince)).FirstOrDefault();
                parsed.ReceiverProvincePostalCode = receiverProvince.PostalProvinceCode;
            }
            string parsedReceiverDistrict = parseReceiverDistrict(OCRResponse, form);
            if (!string.IsNullOrEmpty(parsedReceiverDistrict))
            {
                District receiverDistrict = db.Districts.SqlQuery("SELECT TOP 1 * FROM District ORDER BY dbo.fn_LevenshteinDistance(@district_name, PostalDistrictName, 1) ASC", new SqlParameter("@district_name", parsedReceiverDistrict)).FirstOrDefault();
                parsed.ReceiverDistrictPostalCode = receiverDistrict.PostalDistrictCode;
            }
            parsed.ReceiverStreet = parseReceiverStreet(OCRResponse, form);
        }

        public OCRParsed parseOCRReusult(VNPOST_AppointmentEntities db, OCRRaw OCRResponse, FormTemplate form)
        {
            OCRParsed OCRParsed = new OCRParsed();
            //Parse first screen combo box
            switch (form.FormScopeLevel)
            {
                case 4:
                    OCRParsed.PostalProvinceCode = form.PostalProvinceCode;
                    OCRParsed.PostalDistrictCode = form.PostalDistrictCode;
                    OCRParsed.PublicAdministrationLocationID = form.PublicAdministrationLocationID;
                    OCRParsed.ProfileID = form.ProfileID;
                    break;
                case 3:
                    OCRParsed.PostalProvinceCode = form.PostalProvinceCode;
                    OCRParsed.PostalDistrictCode = form.PostalDistrictCode;
                    OCRParsed.PublicAdministrationLocationID = form.PublicAdministrationLocationID;
                    Query_Scope_3_Result scope3Result = db.Query_Scope_3(
                        OCRParsed.PublicAdministrationLocationID,
                        parseProfile(OCRResponse, form)
                        ).FirstOrDefault();
                    if (scope3Result != null)
                    {
                        OCRParsed.ProfileID = scope3Result.ProfileID;
                    }
                    break;
                case 2:
                    OCRParsed.PostalProvinceCode = form.PostalProvinceCode;
                    OCRParsed.PostalDistrictCode = form.PostalDistrictCode;
                    Query_Scope_2_Result scope2Result = db.Query_Scope_2(
                        OCRParsed.PostalDistrictCode,
                        parsePublicAdministration(OCRResponse, form),
                        3,
                        parseProfile(OCRResponse, form),
                        3
                        ).FirstOrDefault();
                    if (scope2Result != null)
                    {
                        OCRParsed.PublicAdministrationLocationID = scope2Result.PublicAdministrationLocationID;
                        OCRParsed.ProfileID = scope2Result.ProfileID;
                    }
                    break;
                case 1:
                    OCRParsed.PostalProvinceCode = form.PostalProvinceCode;
                    Query_Scope_1_Result scope1Result = db.Query_Scope_1(
                        OCRParsed.PostalProvinceCode,
                        parseDistrict(OCRResponse, form),
                        3,
                        parsePublicAdministration(OCRResponse, form),
                        3,
                        parseProfile(OCRResponse, form),
                        3
                        ).FirstOrDefault();
                    if (scope1Result != null)
                    {
                        OCRParsed.PostalDistrictCode = scope1Result.PostalDistrictCode;
                        OCRParsed.PublicAdministrationLocationID = scope1Result.PublicAdministrationLocationID;
                        OCRParsed.ProfileID = scope1Result.ProfileID;
                    }

                    break;
                case 0:
                    string province = parseProvince(OCRResponse, form);
                    string district = parseDistrict(OCRResponse, form);
                    string publicAdministration = parsePublicAdministration(OCRResponse, form);
                    string profile = parseProfile(OCRResponse, form);

                    if (district != null)
                    {
                        Query_Scope_0_Result scope0Result = db.Query_Scope_0(
                        parseProvince(OCRResponse, form),
                        3,
                        parseDistrict(OCRResponse, form),
                        3,
                        parsePublicAdministration(OCRResponse, form),
                        3,
                        parseProfile(OCRResponse, form),
                        3
                        ).FirstOrDefault();
                        if (scope0Result != null)
                        {
                            OCRParsed.PostalProvinceCode = scope0Result.PostalProvinceCode;
                            OCRParsed.PostalDistrictCode = scope0Result.PostalDistrictCode;
                            OCRParsed.PublicAdministrationLocationID = scope0Result.PublicAdministrationLocationID;
                            OCRParsed.ProfileID = scope0Result.ProfileID;
                        }
                    } else
                    {
                        Query_Scope_0_Without_District_Result scope0WithoutDistrictResult = db.Query_Scope_0_Without_District(
                        parseProvince(OCRResponse, form),
                        3,
                        parsePublicAdministration(OCRResponse, form),
                        3,
                        parseProfile(OCRResponse, form),
                        3
                        ).FirstOrDefault();
                        if (scope0WithoutDistrictResult != null)
                        {
                            OCRParsed.PostalProvinceCode = scope0WithoutDistrictResult.PostalProvinceCode;
                            OCRParsed.PostalDistrictCode = scope0WithoutDistrictResult.PostalDistrictCode;
                            OCRParsed.PublicAdministrationLocationID = scope0WithoutDistrictResult.PublicAdministrationLocationID;
                            OCRParsed.ProfileID = scope0WithoutDistrictResult.ProfileID;
                        }
                    }
                    
                    break;
            }

            //Parse all other information
            parseAllOther(db, OCRParsed, OCRResponse, form);
            return OCRParsed;
        }



        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/GetOCRResult")]
        [HttpPost]
        public ActionResult GetOCRResult(OCRRaw OCRResponse)
        {
            OCRParsed OCRParsed = new OCRParsed();
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                FormTemplate form = db.FormTemplates.Where(record => record.FormID == OCRResponse.form_id).FirstOrDefault();
                if (form != null)
                {
                    OCRParsed = parseOCRReusult(db, OCRResponse, form);
                    return Json(OCRParsed);
                }
            }
            return Json(new { error = true, message = "FormID not found" });
        }

        private class district
        {
            public string PostalDistrictCode { get; set; }
            public string PostalDistrictName { get; set; }
        }
    }
}