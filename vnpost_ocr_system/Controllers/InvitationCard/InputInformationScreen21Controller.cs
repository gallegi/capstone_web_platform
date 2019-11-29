using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen21Controller : Controller
    {
        [Auther(Roles = "0")]
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        public ActionResult Index()
        {
            //if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
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
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        [HttpPost]
        public ActionResult Add()
        {
            //if (Session["userID"] == null) return Redirect("~/khach-hang/dang-nhap");
            string FullName = Request["FullName"];
            string Phone = Request["Phone"];
            string PostalDistrictCode = Request["PostalDistrictCode"];
            string Street = Request["Street"];
            if (FullName == "" || Phone == "" || PostalDistrictCode == "")
                return Json(new { success = false, message = "Không được để trống" });

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
                                                    <p class='content-text col s12'>Số giấy tờ tùy thân: <span id='PersonalPaperNumber'>" + (c.PersonalPaperNumber == null ? "-1" : c.PersonalPaperNumber) + @"</span></p>
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
                ContactInfoDB info = db.Database.SqlQuery<ContactInfoDB>(@"select * from ContactInfo c inner join District d on c.PostalDistrictCode = d.PostalDistrictCode 
                        inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where ContactInfoID = @ContactInfoID",
                    new SqlParameter("ContactInfoID", id)).FirstOrDefault();
                if (!info.CustomerID.Equals(long.Parse(Session["userID"].ToString()))) return null;
                if (info == null) return null;

                List<District> districts = db.Database.SqlQuery<District>("select d.* from District d inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where p.PostalProvinceCode = @PostalProvinceCode", new SqlParameter("PostalProvinceCode", info.PostalProvinceCode)).ToList();

                info.PersonalPaperIssuedDateString = info.PersonalPaperIssuedDate == null ? "" : info.PersonalPaperIssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                return Json(new { info = info, list = districts });
            }
        }
    }
}