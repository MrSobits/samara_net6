namespace Bars.GkhGji.Regions.Chelyabinsk{
    using Bars.B4;
    using Enums;
    using B4.Modules.ExtJs;
    using GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums.Egrn;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/CitizensAppealService.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.CitizensAppealService.svc");
            container.Add("WS/MobileApplicationService.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.MobileApplicationService.svc");
            container.Add("WS/pmvGZHIgzhiDublikatIP.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiDublikatIP.svc");
            container.Add("WS/pmvGZHIgzhiDublikatUL.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiDublikatUL.svc");
            container.Add("WS/pmvGZHIgzhiPereofIP.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiPereofIP.svc");
            container.Add("WS/pmvGZHIgzhiPereofUL.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiPereofUL.svc");
            container.Add("WS/pmvGZHIgzhiPredLicIP.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiPredLicIP.svc");
            container.Add("WS/pmvGZHIgzhiPredLicUL.svc", "Bars.GkhGji.Regions.Chelyabinsk.dll/Bars.GkhGji.Regions.Chelyabinsk.Services.pmvGZHIgzhiPredLicUL.svc");

            //енумы

            container.Add("libs/B4/enums/AppealCitsTransferStatus.js", new ExtJsEnumResource<AppealCitsTransferStatus>("B4.enums.AppealCitsTransferStatus"));
            container.Add("libs/B4/enums/AppealCitsTransferType.js", new ExtJsEnumResource<AppealCitsTransferType>("B4.enums.AppealCitsTransferType"));
            container.Add("libs/B4/enums/KindKND.js", new ExtJsEnumResource<KindKND>("B4.enums.KindKND"));
            container.Add("libs/B4/enums/Koefficients.js", new ExtJsEnumResource<Koefficients>("B4.enums.Koefficients"));
          
            container.Add("libs/B4/enums/TypeBaseOMSU.js", new ExtJsEnumResource<TypeBaseOMSU>("B4.enums.TypeBaseOMSU"));
            container.Add("libs/B4/enums/TypeLicenseRequest.js", new ExtJsEnumResource<TypeLicenseRequest>("B4.enums.TypeLicenseRequest"));
            container.Add("libs/B4/enums/MVDTypeAddress.js", new ExtJsEnumResource<MVDTypeAddress>("B4.enums.MVDTypeAddress"));
            container.Add("libs/B4/enums/IdentifierType.js", new ExtJsEnumResource<IdentifierType>("B4.enums.IdentifierType"));
            container.Add("libs/B4/enums/PayerType.js", new ExtJsEnumResource<PayerType>("B4.enums.PayerType"));
            container.Add("libs/B4/enums/GisGmpPaymentsType.js", new ExtJsEnumResource<GisGmpPaymentsType>("B4.enums.GisGmpPaymentsType"));
            container.Add("libs/B4/enums/GisGmpPaymentsKind.js", new ExtJsEnumResource<GisGmpPaymentsKind>("B4.enums.GisGmpPaymentsKind"));
            container.Add("libs/B4/enums/InnOgrn.js", new ExtJsEnumResource<InnOgrn>("B4.enums.InnOgrn"));
            container.Add("libs/B4/enums/RequestType.js", new ExtJsEnumResource<RequestType>("B4.enums.RequestType"));
            container.Add("libs/B4/enums/GisGmpChargeType.js", new ExtJsEnumResource<GisGmpChargeType>("B4.enums.GisGmpChargeType"));

            //административная практика
            container.Add("libs/B4/enums/CourtMeetingResult.js", new ExtJsEnumResource<CourtMeetingResult>("B4.enums.CourtMeetingResult"));
            container.Add("libs/B4/enums/CourtPracticeState.js", new ExtJsEnumResource<CourtPracticeState>("B4.enums.CourtPracticeState"));
            container.Add("libs/B4/enums/DisputeCategory.js", new ExtJsEnumResource<DisputeCategory>("B4.enums.DisputeCategory"));
            container.Add("libs/B4/enums/DisputeType.js", new ExtJsEnumResource<DisputeType>("B4.enums.DisputeType"));
            container.Add("libs/B4/enums/LawyerInspector.js", new ExtJsEnumResource<LawyerInspector>("B4.enums.LawyerInspector"));

            if (container.Resources.ContainsKey("~/libs/B4/enums/GisGkhTypeRequest.js"))
            {
                container.Resources.Remove("~/libs/B4/enums/GisGkhTypeRequest.js");
            }
            container.Add("libs/B4/enums/GisGkhTypeRequest.js", new ExtJsEnumResource<GisGkhTypeRequest>("B4.enums.GisGkhTypeRequest"));

            //ГИС ЕРП
            container.Add("libs/B4/enums/ERPAddressType.js", new ExtJsEnumResource<ERPAddressType>("B4.enums.ERPAddressType"));
            container.Add("libs/B4/enums/ERPInspectionType.js", new ExtJsEnumResource<ERPInspectionType>("B4.enums.ERPInspectionType"));
            container.Add("libs/B4/enums/ERPNoticeType.js", new ExtJsEnumResource<ERPNoticeType>("B4.enums.ERPNoticeType"));
            container.Add("libs/B4/enums/ERPObjectType.js", new ExtJsEnumResource<ERPObjectType>("B4.enums.ERPObjectType"));
            container.Add("libs/B4/enums/ERPReasonType.js", new ExtJsEnumResource<ERPReasonType>("B4.enums.ERPReasonType"));
            container.Add("libs/B4/enums/ERPRiskType.js", new ExtJsEnumResource<ERPRiskType>("B4.enums.ERPRiskType"));
            container.Add("libs/B4/enums/ERPVLawSuitType.js", new ExtJsEnumResource<ERPVLawSuitType>("B4.enums.ERPVLawSuitType"));
            container.Add("libs/B4/enums/GisErpRequestType.js", new ExtJsEnumResource<GisErpRequestType>("B4.enums.GisErpRequestType"));



            container.RegisterExtJsEnum<TypeFactInspection>();
            container.RegisterExtJsEnum<TypeBase>();
            container.RegisterExtJsEnum<ERKNMRequestType>();
            container.RegisterExtJsEnum<SeverityGroup>();
            container.RegisterExtJsEnum<ProbabilityGroup>();
        }
    }
    
}