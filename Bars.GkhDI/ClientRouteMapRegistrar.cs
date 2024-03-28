namespace Bars.GkhDi
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("dictworkto", "B4.controller.dict.WorkTo", requiredPermission: "GkhDi.Dict.WorkTo.View"));
            map.AddRoute(new ClientRoute("dictworkppr", "B4.controller.dict.WorkPpr", requiredPermission: "GkhDi.Dict.Ppr.View"));
            map.AddRoute(new ClientRoute("templateservice", "B4.controller.dict.TemplateService", requiredPermission: "GkhDi.Dict.TemplateService.View"));
            map.AddRoute(new ClientRoute("templateotherservice", "B4.controller.dict.TemplateOtherService", requiredPermission: "GkhDi.Dict.TemplateOtherService.View"));
            map.AddRoute(new ClientRoute("taxsystem", "B4.controller.dict.TaxSystem", requiredPermission: "GkhDi.Dict.TaxSystem.View"));
            map.AddRoute(new ClientRoute("supervisoryorg", "B4.controller.dict.SupervisoryOrg", requiredPermission: "GkhDi.Dict.SupervisoryOrg.View"));
            map.AddRoute(new ClientRoute("periodicitytempserv", "B4.controller.dict.PeriodicityTemplateService", requiredPermission: "GkhDi.Dict.PeriodicityTempServ.View"));
            map.AddRoute(new ClientRoute("perioddi", "B4.controller.dict.PeriodDi", requiredPermission: "GkhDi.Dict.PeriodDi.View"));
            map.AddRoute(new ClientRoute("groupworkto", "B4.controller.dict.GroupWorkTo", requiredPermission: "GkhDi.Dict.GroupWorkTo.View"));
            map.AddRoute(new ClientRoute("groupworkppr", "B4.controller.dict.GroupWorkPpr", requiredPermission: "GkhDi.Dict.GroupPpr.View"));

            map.AddRoute(new ClientRoute("managingorganization", "B4.controller.ManagingOrganization", requiredPermission: "Gkh.Orgs.Managing.View"));
            map.AddRoute(new ClientRoute("infcontracts", "B4.controller.InformationOnContracts"));
            map.AddRoute(new ClientRoute("infoaboutusecommonfac", "B4.controller.InfoAboutUseCommonFacilities"));
            map.AddRoute(new ClientRoute("infoaboutreductionpaym", "B4.controller.InfoAboutReductionPayment"));
            map.AddRoute(new ClientRoute("infoaboutpaymhousing", "B4.controller.InfoAboutPaymentHousing", requiredPermission: "GkhDi.DisinfoRealObj.InfoAboutPaymentHousing.View"));
            map.AddRoute(new ClientRoute("generalinforo", "B4.controller.GeneralInfoRealityObj"));
            map.AddRoute(new ClientRoute("generaldatadi", "B4.controller.GeneralData"));
            map.AddRoute(new ClientRoute("devices", "B4.controller.Devices"));
            map.AddRoute(new ClientRoute("fundsinfo", "B4.controller.FundsInfo"));
            map.AddRoute(new ClientRoute("finactivityrealobj", "B4.controller.FinActivityRealityObj"));
            map.AddRoute(new ClientRoute("finactivity", "B4.controller.FinActivity"));
            map.AddRoute(new ClientRoute("docsrealityobj", "B4.controller.DocumentsRealityObj"));
            map.AddRoute(new ClientRoute("documents", "B4.controller.Documents"));
            map.AddRoute(new ClientRoute("disclosureinfo", "B4.controller.DisclosureInfo"));
            map.AddRoute(new ClientRoute("masspercentcalculation", "B4.controller.MassPercentCalculation", requiredPermission: "GkhDi.MassPercCalc"));
            map.AddRoute(new ClientRoute("masscalcreport", "B4.controller.MassCalcReport731", requiredPermission: "GkhDi.MassGenerateReport"));
            map.AddRoute(new ClientRoute("direalobjclaimwork", "B4.controller.DiRealObjClaimWork", requiredPermission: "GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_View"));
            map.AddRoute(new ClientRoute("pretensionqualitywork", "B4.controller.PretensionQualityWork", requiredPermission: "GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_View"));
            map.AddRoute(new ClientRoute("housemanaging", "B4.controller.HouseManaging", requiredPermission: "GkhDi.DisinfoRealObj.RealtyObjectPassport.HouseManaging_View"));
        }
    }
}