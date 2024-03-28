namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;
    using Enums;
    using B4.Modules.ExtJs;
    using EnumsReplace;
    using Bars.GkhGji.Regions.Voronezh.Enums.Egrn;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/MobileApplicationService.svc", "Bars.GkhGji.Regions.Voronezh.dll/Bars.GkhGji.Regions.Voronezh.Services.MobileApplicationService.svc");
            container.Add("WS/AppealCitService.svc", "Bars.GkhGji.Regions.Voronezh.dll/Bars.GkhGji.Regions.Voronezh.Services.AppealCitService.svc");

            //енумы
         
            container.Add("libs/B4/enums/PreliminaryCheckResult.js", new ExtJsEnumResource<PreliminaryCheckResult>("B4.enums.PreliminaryCheckResult"));
            container.Add("libs/B4/enums/KindKND.js", new ExtJsEnumResource<KindKND>("B4.enums.KindKND"));
            container.Add("libs/B4/enums/Koefficients.js", new ExtJsEnumResource<Koefficients>("B4.enums.Koefficients"));           
            container.Add("libs/B4/enums/GasuMessageType.js", new ExtJsEnumResource<GasuMessageType>("B4.enums.GasuMessageType"));
            container.Add("libs/B4/enums/TypeBaseOMSU.js", new ExtJsEnumResource<TypeBaseOMSU>("B4.enums.TypeBaseOMSU"));
            container.Add("libs/B4/enums/TypeLicenseRequest.js", new ExtJsEnumResource<TypeLicenseRequest>("B4.enums.TypeLicenseRequest"));          
            container.Add("libs/B4/enums/PublicPropertyLevel.js", new ExtJsEnumResource<Enums.PublicPropertyLevel>("B4.enums.PublicPropertyLevel"));
            container.Add("libs/B4/enums/SnilsPlaceType.js", new ExtJsEnumResource<Enums.SnilsPlaceType>("B4.enums.SnilsPlaceType"));
            container.Add("libs/B4/enums/SMEVGender.js", new ExtJsEnumResource<Enums.SMEVGender>("B4.enums.SMEVGender"));
            container.Add("libs/B4/enums/EmergencyTypeSGIO.js", new ExtJsEnumResource<Enums.EmergencyTypeSGIO>("B4.enums.EmergencyTypeSGIO"));
            container.Add("libs/B4/enums/InnOgrn.js", new ExtJsEnumResource<InnOgrn>("B4.enums.InnOgrn"));
            container.Add("libs/B4/enums/MVDTypeAddress.js", new ExtJsEnumResource<MVDTypeAddress>("B4.enums.MVDTypeAddress"));
            container.Add("libs/B4/enums/RequestType.js", new ExtJsEnumResource<RequestType>("B4.enums.RequestType"));
            container.Add("libs/B4/enums/FNSLicRequestType.js", new ExtJsEnumResource<FNSLicRequestType>("B4.enums.FNSLicRequestType"));
            container.Add("libs/B4/enums/FNSLicPersonType.js", new ExtJsEnumResource<FNSLicPersonType>("B4.enums.FNSLicPersonType"));
            container.Add("libs/B4/enums/RequestParamType.js", new ExtJsEnumResource<RequestParamType>("B4.enums.RequestParamType"));
            container.Add("libs/B4/enums/ChangePremisesType.js", new ExtJsEnumResource<ChangePremisesType>("B4.enums.ChangePremisesType"));
            container.Add("libs/B4/enums/SocialHireDocType.js", new ExtJsEnumResource<SocialHireDocType>("B4.enums.SocialHireDocType"));
            container.Add("libs/B4/enums/SocialHireContractType.js", new ExtJsEnumResource<SocialHireContractType>("B4.enums.SocialHireContractType"));
            container.Add("libs/B4/enums/SMEVStayingPlaceDocType.js", new ExtJsEnumResource<SMEVStayingPlaceDocType>("B4.enums.SMEVStayingPlaceDocType"));
            container.Add("libs/B4/enums/QueryTypeType.js", new ExtJsEnumResource<QueryTypeType>("B4.enums.QueryTypeType"));
            if (container.Resources.ContainsKey("~/libs/B4/enums/TypeDocumentGji.js"))
            {
                container.Resources.Remove("~/libs/B4/enums/TypeDocumentGji.js");
            }
            container.RegisterExtJsEnum<EnumsReplace.TypeDocumentGji>();

            //ГИС ЕРП
            container.Add("libs/B4/enums/ERPAddressType.js", new ExtJsEnumResource<ERPAddressType>("B4.enums.ERPAddressType"));
            container.Add("libs/B4/enums/ERPInspectionType.js", new ExtJsEnumResource<ERPInspectionType>("B4.enums.ERPInspectionType"));
            container.Add("libs/B4/enums/ERPNoticeType.js", new ExtJsEnumResource<ERPNoticeType>("B4.enums.ERPNoticeType"));
            container.Add("libs/B4/enums/ERPObjectType.js", new ExtJsEnumResource<ERPObjectType>("B4.enums.ERPObjectType"));
            container.Add("libs/B4/enums/ERPReasonType.js", new ExtJsEnumResource<ERPReasonType>("B4.enums.ERPReasonType"));
            container.Add("libs/B4/enums/FuckingOSSState.js", new ExtJsEnumResource<FuckingOSSState>("B4.enums.FuckingOSSState"));
            container.Add("libs/B4/enums/OSSApplicantType.js", new ExtJsEnumResource<OSSApplicantType>("B4.enums.OSSApplicantType"));
            container.Add("libs/B4/enums/ERPRiskType.js", new ExtJsEnumResource<ERPRiskType>("B4.enums.ERPRiskType"));
            container.Add("libs/B4/enums/GisErpRequestType.js", new ExtJsEnumResource<GisErpRequestType>("B4.enums.GisErpRequestType"));
            container.Add("libs/B4/enums/ERPVLawSuitType.js", new ExtJsEnumResource<ERPVLawSuitType>("B4.enums.ERPVLawSuitType"));

            //ГИС ГМП
            container.Add("libs/B4/enums/IdentifierType.js", new ExtJsEnumResource<IdentifierType>("B4.enums.IdentifierType"));
            container.Add("libs/B4/enums/PayerType.js", new ExtJsEnumResource<PayerType>("B4.enums.PayerType"));
            container.Add("libs/B4/enums/GisGmpPaymentsType.js", new ExtJsEnumResource<GisGmpPaymentsType>("B4.enums.GisGmpPaymentsType"));
            container.Add("libs/B4/enums/GisGmpPaymentsKind.js", new ExtJsEnumResource<GisGmpPaymentsKind>("B4.enums.GisGmpPaymentsKind"));
            container.Add("libs/B4/enums/GisGmpChargeType.js", new ExtJsEnumResource<GisGmpChargeType>("B4.enums.GisGmpChargeType"));

            //административная практика
            container.Add("libs/B4/enums/CourtMeetingResult.js", new ExtJsEnumResource<CourtMeetingResult>("B4.enums.CourtMeetingResult"));
            container.Add("libs/B4/enums/CourtPracticeState.js", new ExtJsEnumResource<CourtPracticeState>("B4.enums.CourtPracticeState"));
            container.Add("libs/B4/enums/DisputeCategory.js", new ExtJsEnumResource<DisputeCategory>("B4.enums.DisputeCategory"));
            container.Add("libs/B4/enums/DisputeType.js", new ExtJsEnumResource<DisputeType>("B4.enums.DisputeType"));
            container.Add("libs/B4/enums/LawyerInspector.js", new ExtJsEnumResource<LawyerInspector>("B4.enums.LawyerInspector"));
            container.RegisterExtJsEnum<ERKNMRequestType>();
            //ГИС ЖКХ
            if (container.Resources.ContainsKey("~/libs/B4/enums/GisGkhTypeRequest.js"))
            {
                container.Resources.Remove("~/libs/B4/enums/GisGkhTypeRequest.js");
            }
            container.Add("libs/B4/enums/GisGkhTypeRequest.js", new ExtJsEnumResource<GisGkhTypeRequest>("B4.enums.GisGkhTypeRequest"));

            if (container.Resources.ContainsKey("~/libs/B4/enums/TypeEntityLogging.js"))
            {
                container.Resources.Remove("~/libs/B4/enums/TypeEntityLogging.js");
            }
            container.RegisterExtJsEnum<TypeEntityLogging>();
        }
    }
    
}