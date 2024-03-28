namespace Bars.GkhGji.Regions.Chelyabinsk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("baseomsu", "B4.controller.BaseOMSU", requiredPermission: "GkhGji.Inspection.BaseOMSU.View"));

            #region риск-оринтированный подход
            map.AddRoute(new ClientRoute("kindknddict", "B4.controller.riskorientedmethod.KindKNDDict", requiredPermission: "GkhGji.RiskOrientedMethod.KindKNDDict.View"));
            map.AddRoute(new ClientRoute("effectivekndindex", "B4.controller.riskorientedmethod.EffectiveKNDIndex", requiredPermission: "GkhGji.RiskOrientedMethod.EffectiveKNDIndex.View"));
            map.AddRoute(new ClientRoute("romcategory", "B4.controller.riskorientedmethod.ROMCategory", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("romcalctask", "B4.controller.ROMCalcTask", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("licensecontrolobj", "B4.controller.LicenseWithHouse", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("housingspvobj", "B4.controller.LicenseWithHouseByType", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            #endregion

            //справочники
            map.AddRoute(new ClientRoute("regioncode", "B4.controller.dict.RegionCodeMVD", requiredPermission: "GkhGji.Dict.RegionCode.View"));
            map.AddRoute(new ClientRoute("fldoctype", "B4.controller.dict.FLDocType", requiredPermission: "GkhGji.Dict.FLDocType.View"));
            map.AddRoute(new ClientRoute("gisgmppayerstatus", "B4.controller.dict.GISGMPPayerStatus", requiredPermission: "GkhGji.SMEV.GISGMP.View"));
            map.AddRoute(new ClientRoute("egrndoctype", "B4.controller.dict.EGRNDocType", requiredPermission: "GkhGji.Dict.EGRNDocType.View"));
            map.AddRoute(new ClientRoute("prosecutoroffice", "B4.controller.dict.ProsecutorOffice", requiredPermission: "GkhGji.Dict.ProsecutorOffice.View"));
            map.AddRoute(new ClientRoute("egrnapplicanttype", "B4.controller.dict.EGRNApplicantType", requiredPermission: "GkhGji.Dict.EGRNApplicantType.View"));
            map.AddRoute(new ClientRoute("egrnobjecttype", "B4.controller.dict.EGRNObjectType", requiredPermission: "GkhGji.Dict.EGRNObjectType.View"));

            map.AddRoute(new ClientRoute("sstutransferorg", "B4.controller.dict.SSTUTransferOrg", requiredPermission: "GkhGji.Dict.SSTUTransferOrg.View"));
            map.AddRoute(new ClientRoute("admonition", "B4.controller.Admonition", requiredPermission: "GkhGji.AppealCitizens.View"));
            
            //СМЭВ
            map.AddRoute(new ClientRoute("erknm", "B4.controller.ERKNM", requiredPermission: "GkhGji.SMEV.GISERP.View"));
            map.AddRoute(new ClientRoute("smevmvd", "B4.controller.SMEVMVD", requiredPermission: "GkhGji.SMEV.SMEVMVD.View"));
            map.AddRoute(new ClientRoute("smevegrul", "B4.controller.SMEVEGRUL", requiredPermission: "GkhGji.SMEV.SMEVEGRUL.View"));
            map.AddRoute(new ClientRoute("smevdo", "B4.controller.SMEVEDO", requiredPermission: "GkhGji.SMEV.SMEVEGRIP.View"));
            map.AddRoute(new ClientRoute("smevegrip", "B4.controller.SMEVEGRIP", requiredPermission: "GkhGji.SMEV.SMEVEGRIP.View"));
            map.AddRoute(new ClientRoute("smevegrn", "B4.controller.SMEVEGRN", requiredPermission: "GkhGji.SMEV.SMEVEGRN.View"));
            map.AddRoute(new ClientRoute("gisgmp", "B4.controller.GisGmp", requiredPermission: "GkhGji.SMEV.GISGMP.View"));
            map.AddRoute(new ClientRoute("giserp", "B4.controller.GISERP", requiredPermission: "GkhGji.SMEV.GISERP.View"));
            map.AddRoute(new ClientRoute("payreg", "B4.controller.PayReg", requiredPermission: "GkhGji.SMEV.PAYREG.View"));
            map.AddRoute(new ClientRoute("eaisintegration", "B4.controller.EaisIntegration"));

            //Переоформление лицензии
            map.AddRoute(new ClientRoute("licensereissuance", "B4.controller.licensereissuance.LicenseReissuance"));
            map.AddRoute(new ClientRoute("licensereissuanceeditor/{id}/", "B4.controller.licensereissuance.Edit"));
            map.AddRoute(new ClientRoute("baselicensereissuance", "B4.controller.BaseLicenseReissuance", requiredPermission: "GkhGji.Inspection.BaseLicApplicants.View"));

            //реестр рассылок
            map.AddRoute(new ClientRoute("emaillist", "B4.controller.EmailLists"));

            map.AddRoute(new ClientRoute("courtpractice", "B4.controller.CourtPractice", requiredPermission: "GkhGji.CourtPractice.CourtPracticeRegystry.View"));
            map.AddRoute(new ClientRoute("smevegrnlog", "B4.controller.SMEVEGRNLog", requiredPermission: "GkhGji.SMEV.SMEVEGRN.Log"));

            //ссту
            map.AddRoute(new ClientRoute("sstuexporttask", "B4.controller.SSTUExportTask"));
            
            //СОПР
            map.AddRoute(new ClientRoute("appealorder", "B4.controller.AppealOrder", requiredPermission: "GkhGji.SOPR.Appeal"));
        }
    }
}