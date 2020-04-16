//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vnpost_ocr_system.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormTemplate
    {
        public long FormID { get; set; }
        public string FormName { get; set; }
        public string FormImageLink { get; set; }
        public string APIOutput { get; set; }
        public Nullable<int> FormScopeLevel { get; set; }
        public string PostalProvinceCode { get; set; }
        public Nullable<int> ProvinceParseType { get; set; }
        public Nullable<int> ProvinceNERIndex { get; set; }
        public string ProvinceRegex { get; set; }
        public string PostalDistrictCode { get; set; }
        public Nullable<int> DistrictParseType { get; set; }
        public Nullable<int> DistrictNERIndex { get; set; }
        public string DistrictRegex { get; set; }
        public Nullable<int> PublicAdministrationParseType { get; set; }
        public Nullable<long> PublicAdministrationLocationID { get; set; }
        public Nullable<int> PublicAdministrationNERIndex { get; set; }
        public string PublicAdministrationRegex { get; set; }
        public Nullable<int> ProfileParseType { get; set; }
        public Nullable<long> ProfileID { get; set; }
        public Nullable<int> ProfileNERIndex { get; set; }
        public string ProfileRegex { get; set; }
        public Nullable<int> AppointmentLetterCodeParseType { get; set; }
        public Nullable<int> AppointmentLetterCodeNERIndex { get; set; }
        public string AppointmentLetterCodeRegex { get; set; }
        public Nullable<int> ProcedurerFullNameParseType { get; set; }
        public Nullable<int> ProcedurerFullNameNERIndex { get; set; }
        public string ProcedurerFullNameRegex { get; set; }
        public Nullable<int> ProcedurerPhoneParseType { get; set; }
        public Nullable<int> ProcedurerPhoneNERIndex { get; set; }
        public string ProcedurerPhoneRegex { get; set; }
        public Nullable<int> ProcedurerProvinceParseType { get; set; }
        public Nullable<int> ProcedurerProvinceNERIndex { get; set; }
        public string ProcerdurerProvinceRegex { get; set; }
        public Nullable<int> ProcedurerDistrictParseType { get; set; }
        public Nullable<int> ProcedurerDistrictNERIndex { get; set; }
        public string ProcedurerDistrictRegex { get; set; }
        public Nullable<int> ProcedurerStreetParseType { get; set; }
        public Nullable<int> ProcedurerStreetNERIndex { get; set; }
        public string ProcedurerStreetRegex { get; set; }
        public Nullable<int> ProcedurerPersonalPaperTypeParseType { get; set; }
        public Nullable<int> ProcedurerPersonalPaperTypeNERIndex { get; set; }
        public string ProcedurerPersonalPaperTypeRegex { get; set; }
        public Nullable<int> ProcedurerPersonalPaperNumberParseType { get; set; }
        public Nullable<int> ProcedurerPersonalPaperNumberNERIndex { get; set; }
        public string ProcedurerPersonalPaperNumberRegex { get; set; }
        public Nullable<int> ProcedurerPersonalPaperIssuedDateParseType { get; set; }
        public Nullable<int> ProcedurerPersonalPaperIssuedDateNERIndex { get; set; }
        public string ProcedurerPersonalPaperIssuedDateRegex { get; set; }
        public Nullable<int> ProcedurerPersonalPaperIssuedPlaceParseType { get; set; }
        public Nullable<int> ProcedurerPersonalPaperIssuedPlaceNERIndex { get; set; }
        public string ProcedurerPersonalPaperIssuedPlaceRegex { get; set; }
        public Nullable<int> SenderFullNameParseType { get; set; }
        public Nullable<int> SenderFullNameNERIndex { get; set; }
        public string SenderFullNameRegex { get; set; }
        public Nullable<int> SenderPhoneParseType { get; set; }
        public Nullable<int> SenderPhoneNERIndex { get; set; }
        public string SenderPhoneRegex { get; set; }
        public Nullable<int> SenderProvinceParseType { get; set; }
        public Nullable<int> SenderProvinceNERIndex { get; set; }
        public string SenderProvinceRegex { get; set; }
        public Nullable<int> SenderDistrictParseType { get; set; }
        public Nullable<int> SenderDistrictNERIndex { get; set; }
        public string SenderDistrictRegex { get; set; }
        public Nullable<int> SenderStreetParseType { get; set; }
        public Nullable<int> SenderStreetNERIndex { get; set; }
        public string SenderStreetRegex { get; set; }
        public Nullable<int> ReceiverFullNameParseType { get; set; }
        public Nullable<int> ReceiverFullNameNERIndex { get; set; }
        public string ReceiverFullNameRegex { get; set; }
        public Nullable<int> ReceiverPhoneParseType { get; set; }
        public Nullable<int> ReceiverPhoneNERIndex { get; set; }
        public string ReceiverPhoneRegex { get; set; }
        public Nullable<int> ReceiverProvinceParseType { get; set; }
        public Nullable<int> ReceiverProvinceNERIndex { get; set; }
        public string ReceiverProvinceRegex { get; set; }
        public Nullable<int> ReceiverDistrictParseType { get; set; }
        public Nullable<int> ReceiverDistrictNERIndex { get; set; }
        public string ReceiverDistrictRegex { get; set; }
        public Nullable<int> ReceiverStreetParseType { get; set; }
        public Nullable<int> ReceiverStreetNERIndex { get; set; }
        public string ReceiverStreetRegex { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
    
        public virtual District District { get; set; }
        public virtual Province Province { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual PublicAdministration PublicAdministration { get; set; }
    }
}
