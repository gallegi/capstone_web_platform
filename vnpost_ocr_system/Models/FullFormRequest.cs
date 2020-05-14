using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class FullFormRequest
    {
        public FullFormRequest() { }
        public FullFormRequest(FormTemplate ft)
        {
            this.ft.FormID = ft.FormID;
            this.ft.FormName = ft.FormName;
            this.ft.FormImageLink = ft.FormImageLink;
            this.ft.APIOutput = ft.APIOutput;

            // 1. Profile
            this.ft.FormScopeLevel = ft.FormScopeLevel;
            this.ft.PostalProvinceCode = ft.PostalProvinceCode;
            this.ft.ProvinceParseType = ft.ProvinceParseType;
            this.ft.ProvinceNERIndex = ft.ProvinceNERIndex;
            this.ft.ProvinceRegex = ft.ProvinceRegex;


            this.ft.PostalDistrictCode = ft.PostalDistrictCode;
            this.ft.DistrictParseType = ft.DistrictParseType;
            this.ft.DistrictNERIndex = ft.DistrictNERIndex;
            this.ft.DistrictRegex = ft.DistrictRegex;

            this.ft.PublicAdministrationParseType = ft.PublicAdministrationParseType;
            this.ft.PublicAdministrationLocationID = ft.PublicAdministrationLocationID;
            this.ft.PublicAdministrationNERIndex = ft.PublicAdministrationNERIndex;
            this.ft.PublicAdministrationRegex = ft.PublicAdministrationRegex;

            this.ft.ProfileParseType = ft.ProfileParseType;
            this.ft.ProfileID = ft.ProfileID;
            this.ft.ProfileNERIndex = ft.ProfileNERIndex;
            this.ft.ProfileRegex = ft.ProfileRegex;

            this.ft.AppointmentLetterCodeParseType = ft.AppointmentLetterCodeParseType;
            this.ft.AppointmentLetterCodeNERIndex = ft.AppointmentLetterCodeNERIndex;
            this.ft.AppointmentLetterCodeRegex = ft.AppointmentLetterCodeRegex;

            // 2. Procedurer
            this.ft.ProcedurerFullNameParseType = ft.ProcedurerFullNameParseType;
            this.ft.ProcedurerFullNameNERIndex = ft.ProcedurerFullNameNERIndex;
            this.ft.ProcedurerFullNameRegex = ft.ProcedurerFullNameRegex;

            this.ft.ProcedurerPhoneParseType = ft.ProcedurerPhoneParseType;
            this.ft.ProcedurerPhoneNERIndex = ft.ProcedurerPhoneNERIndex;
            this.ft.ProcedurerPhoneRegex = ft.ProcedurerPhoneRegex;

            this.ft.ProcedurerProvinceParseType = ft.ProcedurerProvinceParseType;
            this.ft.ProcedurerProvinceNERIndex = ft.ProcedurerProvinceNERIndex;
            this.ft.ProcedurerProvinceRegex = ft.ProcedurerProvinceRegex;

            this.ft.ProcedurerDistrictParseType = ft.ProcedurerDistrictParseType;
            this.ft.ProcedurerDistrictNERIndex = ft.ProcedurerDistrictNERIndex;
            this.ft.ProcedurerDistrictRegex = ft.ProcedurerDistrictRegex;

            this.ft.ProcedurerStreetParseType = ft.ProcedurerStreetParseType;
            this.ft.ProcedurerStreetNERIndex = ft.ProcedurerStreetNERIndex;
            this.ft.ProcedurerStreetRegex = ft.ProcedurerStreetRegex;

            this.ft.ProcedurerPersonalPaperTypeParseType = ft.ProcedurerPersonalPaperTypeParseType;
            this.ft.ProcedurerPersonalPaperTypeNERIndex = ft.ProcedurerPersonalPaperTypeNERIndex;
            this.ft.ProcedurerPersonalPaperTypeRegex = ft.ProcedurerPersonalPaperTypeRegex;

            this.ft.ProcedurerPersonalPaperNumberParseType = ft.ProcedurerPersonalPaperNumberParseType;
            this.ft.ProcedurerPersonalPaperNumberNERIndex = ft.ProcedurerPersonalPaperNumberNERIndex;
            this.ft.ProcedurerPersonalPaperNumberRegex = ft.ProcedurerPersonalPaperNumberRegex;

            this.ft.ProcedurerPersonalPaperIssuedDateParseType = ft.ProcedurerPersonalPaperIssuedDateParseType;
            this.ft.ProcedurerPersonalPaperIssuedDateNERIndex = ft.ProcedurerPersonalPaperIssuedDateNERIndex;
            this.ft.ProcedurerPersonalPaperIssuedDateRegex = ft.ProcedurerPersonalPaperIssuedDateRegex;

            this.ft.ProcedurerPersonalPaperIssuedPlaceParseType = ft.ProcedurerPersonalPaperIssuedPlaceParseType;
            this.ft.ProcedurerPersonalPaperIssuedPlaceNERIndex = ft.ProcedurerPersonalPaperIssuedPlaceNERIndex;
            this.ft.ProcedurerPersonalPaperIssuedPlaceRegex = ft.ProcedurerPersonalPaperIssuedPlaceRegex;

            // 3. Sender
            this.ft.SenderFullNameParseType = ft.SenderFullNameParseType;
            this.ft.SenderFullNameNERIndex = ft.SenderFullNameNERIndex;
            this.ft.SenderFullNameRegex = ft.SenderFullNameRegex;

            this.ft.SenderPhoneParseType = ft.SenderPhoneParseType;
            this.ft.SenderPhoneNERIndex = ft.SenderPhoneNERIndex;
            this.ft.SenderPhoneRegex = ft.SenderPhoneRegex;

            this.ft.SenderProvinceParseType = ft.SenderProvinceParseType;
            this.ft.SenderProvinceNERIndex = ft.SenderProvinceNERIndex;
            this.ft.SenderProvinceRegex = ft.SenderProvinceRegex;

            this.ft.SenderDistrictParseType = ft.SenderDistrictParseType;
            this.ft.SenderDistrictNERIndex = ft.SenderDistrictNERIndex;
            this.ft.SenderDistrictRegex = ft.SenderDistrictRegex;

            this.ft.SenderStreetParseType = ft.SenderStreetParseType;
            this.ft.SenderStreetNERIndex = ft.SenderStreetNERIndex;
            this.ft.SenderStreetRegex = ft.SenderStreetRegex;

            // 4. Receiver
            this.ft.ReceiverFullNameParseType = ft.ReceiverFullNameParseType;
            this.ft.ReceiverFullNameNERIndex = ft.ReceiverFullNameNERIndex;
            this.ft.ReceiverFullNameRegex = ft.ReceiverFullNameRegex;

            this.ft.ReceiverPhoneParseType = ft.ReceiverPhoneParseType;
            this.ft.ReceiverPhoneNERIndex = ft.ReceiverPhoneNERIndex;
            this.ft.ReceiverPhoneRegex = ft.ReceiverPhoneRegex;

            this.ft.ReceiverProvinceParseType = ft.ReceiverProvinceParseType;
            this.ft.ReceiverProvinceNERIndex = ft.ReceiverProvinceNERIndex;
            this.ft.ReceiverProvinceRegex = ft.ReceiverProvinceRegex;

            this.ft.ReceiverDistrictParseType = ft.ReceiverDistrictParseType;
            this.ft.ReceiverDistrictNERIndex = ft.ReceiverDistrictNERIndex;
            this.ft.ReceiverDistrictRegex = ft.ReceiverDistrictRegex;

            this.ft.ReceiverStreetParseType = ft.ReceiverStreetParseType;
            this.ft.ReceiverStreetNERIndex = ft.ReceiverStreetNERIndex;
            this.ft.ReceiverStreetRegex = ft.ReceiverStreetRegex;
        }
        public FullFormRequest(FormTemplate ft, string action)
        {
            this.action = action;
            this.ft = new FormTemplate();
            this.ft.FormID = ft.FormID;
            this.ft.FormName = ft.FormName;
            this.ft.FormImageLink = ft.FormImageLink;
            this.ft.APIOutput = ft.APIOutput;

            // 1. Profile
            this.ft.FormScopeLevel = ft.FormScopeLevel;
            this.ft.PostalProvinceCode = ft.PostalProvinceCode;
            this.ft.ProvinceParseType = ft.ProvinceParseType;
            this.ft.ProvinceNERIndex = ft.ProvinceNERIndex;
            this.ft.ProvinceRegex = ft.ProvinceRegex;


            this.ft.PostalDistrictCode = ft.PostalDistrictCode;
            this.ft.DistrictParseType = ft.DistrictParseType;
            this.ft.DistrictNERIndex = ft.DistrictNERIndex;
            this.ft.DistrictRegex = ft.DistrictRegex;

            this.ft.PublicAdministrationParseType = ft.PublicAdministrationParseType;
            this.ft.PublicAdministrationLocationID = ft.PublicAdministrationLocationID;
            this.ft.PublicAdministrationNERIndex = ft.PublicAdministrationNERIndex;
            this.ft.PublicAdministrationRegex = ft.PublicAdministrationRegex;

            this.ft.ProfileParseType = ft.ProfileParseType;
            this.ft.ProfileID = ft.ProfileID;
            this.ft.ProfileNERIndex = ft.ProfileNERIndex;
            this.ft.ProfileRegex = ft.ProfileRegex;

            this.ft.AppointmentLetterCodeParseType = ft.AppointmentLetterCodeParseType;
            this.ft.AppointmentLetterCodeNERIndex = ft.AppointmentLetterCodeNERIndex;
            this.ft.AppointmentLetterCodeRegex = ft.AppointmentLetterCodeRegex;

            // 2. Procedurer
            this.ft.ProcedurerFullNameParseType = ft.ProcedurerFullNameParseType;
            this.ft.ProcedurerFullNameNERIndex = ft.ProcedurerFullNameNERIndex;
            this.ft.ProcedurerFullNameRegex = ft.ProcedurerFullNameRegex;

            this.ft.ProcedurerPhoneParseType = ft.ProcedurerPhoneParseType;
            this.ft.ProcedurerPhoneNERIndex = ft.ProcedurerPhoneNERIndex;
            this.ft.ProcedurerPhoneRegex = ft.ProcedurerPhoneRegex;

            this.ft.ProcedurerProvinceParseType = ft.ProcedurerProvinceParseType;
            this.ft.ProcedurerProvinceNERIndex = ft.ProcedurerProvinceNERIndex;
            this.ft.ProcedurerProvinceRegex = ft.ProcedurerProvinceRegex;

            this.ft.ProcedurerDistrictParseType = ft.ProcedurerDistrictParseType;
            this.ft.ProcedurerDistrictNERIndex = ft.ProcedurerDistrictNERIndex;
            this.ft.ProcedurerDistrictRegex = ft.ProcedurerDistrictRegex;

            this.ft.ProcedurerStreetParseType = ft.ProcedurerStreetParseType;
            this.ft.ProcedurerStreetNERIndex = ft.ProcedurerStreetNERIndex;
            this.ft.ProcedurerStreetRegex = ft.ProcedurerStreetRegex;

            this.ft.ProcedurerPersonalPaperTypeParseType = ft.ProcedurerPersonalPaperTypeParseType;
            this.ft.ProcedurerPersonalPaperTypeNERIndex = ft.ProcedurerPersonalPaperTypeNERIndex;
            this.ft.ProcedurerPersonalPaperTypeRegex = ft.ProcedurerPersonalPaperTypeRegex;

            this.ft.ProcedurerPersonalPaperNumberParseType = ft.ProcedurerPersonalPaperNumberParseType;
            this.ft.ProcedurerPersonalPaperNumberNERIndex = ft.ProcedurerPersonalPaperNumberNERIndex;
            this.ft.ProcedurerPersonalPaperNumberRegex = ft.ProcedurerPersonalPaperNumberRegex;

            this.ft.ProcedurerPersonalPaperIssuedDateParseType = ft.ProcedurerPersonalPaperIssuedDateParseType;
            this.ft.ProcedurerPersonalPaperIssuedDateNERIndex = ft.ProcedurerPersonalPaperIssuedDateNERIndex;
            this.ft.ProcedurerPersonalPaperIssuedDateRegex = ft.ProcedurerPersonalPaperIssuedDateRegex;

            this.ft.ProcedurerPersonalPaperIssuedPlaceParseType = ft.ProcedurerPersonalPaperIssuedPlaceParseType;
            this.ft.ProcedurerPersonalPaperIssuedPlaceNERIndex = ft.ProcedurerPersonalPaperIssuedPlaceNERIndex;
            this.ft.ProcedurerPersonalPaperIssuedPlaceRegex = ft.ProcedurerPersonalPaperIssuedPlaceRegex;

            // 3. Sender
            this.ft.SenderFullNameParseType = ft.SenderFullNameParseType;
            this.ft.SenderFullNameNERIndex = ft.SenderFullNameNERIndex;
            this.ft.SenderFullNameRegex = ft.SenderFullNameRegex;

            this.ft.SenderPhoneParseType = ft.SenderPhoneParseType;
            this.ft.SenderPhoneNERIndex = ft.SenderPhoneNERIndex;
            this.ft.SenderPhoneRegex = ft.SenderPhoneRegex;

            this.ft.SenderProvinceParseType = ft.SenderProvinceParseType;
            this.ft.SenderProvinceNERIndex = ft.SenderProvinceNERIndex;
            this.ft.SenderProvinceRegex = ft.SenderProvinceRegex;

            this.ft.SenderDistrictParseType = ft.SenderDistrictParseType;
            this.ft.SenderDistrictNERIndex = ft.SenderDistrictNERIndex;
            this.ft.SenderDistrictRegex = ft.SenderDistrictRegex;

            this.ft.SenderStreetParseType = ft.SenderStreetParseType;
            this.ft.SenderStreetNERIndex = ft.SenderStreetNERIndex;
            this.ft.SenderStreetRegex = ft.SenderStreetRegex;

            // 4. Receiver
            this.ft.ReceiverFullNameParseType = ft.ReceiverFullNameParseType;
            this.ft.ReceiverFullNameNERIndex = ft.ReceiverFullNameNERIndex;
            this.ft.ReceiverFullNameRegex = ft.ReceiverFullNameRegex;

            this.ft.ReceiverPhoneParseType = ft.ReceiverPhoneParseType;
            this.ft.ReceiverPhoneNERIndex = ft.ReceiverPhoneNERIndex;
            this.ft.ReceiverPhoneRegex = ft.ReceiverPhoneRegex;

            this.ft.ReceiverProvinceParseType = ft.ReceiverProvinceParseType;
            this.ft.ReceiverProvinceNERIndex = ft.ReceiverProvinceNERIndex;
            this.ft.ReceiverProvinceRegex = ft.ReceiverProvinceRegex;

            this.ft.ReceiverDistrictParseType = ft.ReceiverDistrictParseType;
            this.ft.ReceiverDistrictNERIndex = ft.ReceiverDistrictNERIndex;
            this.ft.ReceiverDistrictRegex = ft.ReceiverDistrictRegex;

            this.ft.ReceiverStreetParseType = ft.ReceiverStreetParseType;
            this.ft.ReceiverStreetNERIndex = ft.ReceiverStreetNERIndex;
            this.ft.ReceiverStreetRegex = ft.ReceiverStreetRegex;
        }
        public string action { get; set; }
        public FormTemplate ft { get; set; }
    }
}