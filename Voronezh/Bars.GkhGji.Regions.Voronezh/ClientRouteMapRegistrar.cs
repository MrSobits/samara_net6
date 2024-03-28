namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            #region риск-оринтированный подход
            map.AddRoute(new ClientRoute("kindknddict", "B4.controller.riskorientedmethod.KindKNDDict", requiredPermission: "GkhGji.RiskOrientedMethod.KindKNDDict.View"));
            map.AddRoute(new ClientRoute("effectivekndindex", "B4.controller.riskorientedmethod.EffectiveKNDIndex", requiredPermission: "GkhGji.RiskOrientedMethod.EffectiveKNDIndex.View"));
            map.AddRoute(new ClientRoute("romcategory", "B4.controller.riskorientedmethod.ROMCategory", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("romcalctask", "B4.controller.ROMCalcTask", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("licensecontrolobj", "B4.controller.LicenseWithHouse", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            map.AddRoute(new ClientRoute("housingspvobj", "B4.controller.LicenseWithHouseByType", requiredPermission: "GkhGji.RiskOrientedMethod.ROMCategory.View"));
            #endregion

            map.AddRoute(new ClientRoute("vdgoviolators", "B4.controller.VDGOViolators", requiredPermission: "GkhGji.VDGOViolators.View"));

            map.AddRoute(new ClientRoute("baseomsu", "B4.controller.BaseOMSU", requiredPermission: "GkhGji.Inspection.BaseOMSU.View"));
            //реестр рассылок
            map.AddRoute(new ClientRoute("emaillist", "B4.controller.EmailLists"));
            map.AddRoute(new ClientRoute("amirsimport", "B4.controller.AmirsImport", requiredPermission: "Administration.AMIRS.View"));
            //СМЭВ
            map.AddRoute(new ClientRoute("redevelopment", "B4.controller.SMEVRedevelopment", requiredPermission: "GkhGji.SMEV.SMEVSocialHire.View"));
            map.AddRoute(new ClientRoute("owproperty", "B4.controller.SMEVOwnershipProperty", requiredPermission: "GkhGji.SMEV.SMEVSocialHire.View"));
            map.AddRoute(new ClientRoute("emergencyhouse", "B4.controller.SMEVEmergencyHouse", requiredPermission: "GkhGji.SMEV.SMEVSocialHire.View"));
            map.AddRoute(new ClientRoute("socialhire", "B4.controller.SMEVSocialHire", requiredPermission: "GkhGji.SMEV.SMEVSocialHire.View"));
            map.AddRoute(new ClientRoute("ndfl", "B4.controller.SMEVNDFL", requiredPermission: "GkhGji.SMEV.SMEVNDFL.View"));
            map.AddRoute(new ClientRoute("smevmvd", "B4.controller.SMEVMVD", requiredPermission: "GkhGji.SMEV.SMEVMVD.View"));
            map.AddRoute(new ClientRoute("smevpropertytype", "B4.controller.SMEVPropertyType", requiredPermission: "GkhGji.SMEV.SMEVPropertyType.View"));
            map.AddRoute(new ClientRoute("gisgmp", "B4.controller.GisGmp", requiredPermission: "GkhGji.SMEV.GISGMP.View"));
            map.AddRoute(new ClientRoute("payreg", "B4.controller.PayReg", requiredPermission: "GkhGji.SMEV.PAYREG.View"));
            map.AddRoute(new ClientRoute("smevegrul", "B4.controller.SMEVEGRUL", requiredPermission: "GkhGji.SMEV.SMEVEGRUL.View"));
            map.AddRoute(new ClientRoute("giserp", "B4.controller.GISERP", requiredPermission: "GkhGji.SMEV.GISERP.View"));
            map.AddRoute(new ClientRoute("erknm", "B4.controller.ERKNM", requiredPermission: "GkhGji.SMEV.GISERP.View"));
            map.AddRoute(new ClientRoute("smevegrn", "B4.controller.SMEVEGRN", requiredPermission: "GkhGji.SMEV.SMEVEGRN.View"));
            map.AddRoute(new ClientRoute("smevfnslicrequest", "B4.controller.SMEVFNSLicRequest", requiredPermission: "GkhGji.SMEV.SMEVFNSLicRequest.View"));
            map.AddRoute(new ClientRoute("payreg", "B4.controller.PayReg", requiredPermission: "GkhGji.SMEV.PAYREG.View")); //добавил
            map.AddRoute(new ClientRoute("premises", "B4.controller.SMEVPremises", requiredPermission: "GkhGji.SMEV.SMEVPremises.View"));
            map.AddRoute(new ClientRoute("diskvlic", "B4.controller.SMEVDISKVLIC", requiredPermission: "GkhGji.SMEV.SMEVDISKVLIC.View"));
            map.AddRoute(new ClientRoute("smevsnils", "B4.controller.Exploit", requiredPermission: "GkhGji.SMEV.SMEVDISKVLIC.View"));
            map.AddRoute(new ClientRoute("exploit", "B4.controller.SMEVExploitResolution", requiredPermission: "GkhGji.SMEV.SMEVExploitResolution.View"));
            map.AddRoute(new ClientRoute("changepremisesstate", "B4.controller.Validpassport", requiredPermission: "GkhGji.SMEV.SMEVExploitResolution.View"));
            map.AddRoute(new ClientRoute("validpassport", "B4.controller.SMEVValidPassport", requiredPermission: "GkhGji.SMEV.SMEVValidPassport.View"));
            map.AddRoute(new ClientRoute("livingplace", "B4.controller.SMEVLivingPlace", requiredPermission: "GkhGji.SMEV.SMEVLivingPlace.View"));
            map.AddRoute(new ClientRoute("stayingplace", "B4.controller.SMEVStayingPlace", requiredPermission: "GkhGji.SMEV.SMEVStayingPlace.View"));
            map.AddRoute(new ClientRoute("gasu", "B4.controller.GASU", requiredPermission: "GkhGji.SMEV.SMEVGASU.View"));

            //Справочники
            map.AddRoute(new ClientRoute("prosecutoroffice", "B4.controller.dict.ProsecutorOffice", requiredPermission: "GkhGji.Dict.ProsecutorOffice.View"));
            map.AddRoute(new ClientRoute("appealexecutiontype", "B4.controller.dict.AppealExecutionType", requiredPermission: "GkhGji.Dict.AppealExecutionType.View"));
            map.AddRoute(new ClientRoute("smevegrip", "B4.controller.SMEVEGRIP", requiredPermission: "GkhGji.SMEV.SMEVEGRIP.View")); //егрип
            map.AddRoute(new ClientRoute("smevdo", "B4.controller.SMEVEDO", requiredPermission: "GkhGji.SMEV.SMEVEGRIP.View")); //егрип
            map.AddRoute(new ClientRoute("sstutransferorg", "B4.controller.dict.SSTUTransferOrg", requiredPermission: "GkhGji.Dict.SSTUTransferOrg.View"));
            map.AddRoute(new ClientRoute("fldoctype", "B4.controller.dict.FLDocType", requiredPermission: "GkhGji.Dict.FLDocType.View"));
            map.AddRoute(new ClientRoute("gisgmppayerstatus", "B4.controller.dict.GISGMPPayerStatus", requiredPermission: "GkhGji.SMEV.GISGMP.View"));
            map.AddRoute(new ClientRoute("egrndoctype", "B4.controller.dict.EGRNDocType", requiredPermission: "GkhGji.Dict.EGRNDocType.View"));
            map.AddRoute(new ClientRoute("egrnapplicanttype", "B4.controller.dict.EGRNApplicantType", requiredPermission: "GkhGji.Dict.EGRNApplicantType.View"));
            map.AddRoute(new ClientRoute("egrnobjecttype", "B4.controller.dict.EGRNObjectType", requiredPermission: "GkhGji.Dict.EGRNObjectType.View"));
            map.AddRoute(new ClientRoute("interdepartmentalrequestsdtogrid", "B4.controller.InterdepartmentalRequestsDTO", requiredPermission: "GkhGji.SMEV.DepartmentlRequestsDTOController.View"));

            map.AddRoute(new ClientRoute("courtpractice", "B4.controller.CourtPractice", requiredPermission: "GkhGji.CourtPractice.CourtPracticeRegystry.View"));

            map.AddRoute(new ClientRoute("sstuexporttask", "B4.controller.SSTUExportTask"));
            map.AddRoute(new ClientRoute("tarifimport", "B4.controller.Import.Tarif", requiredPermission: "Import.Tarif.View"));

            map.AddRoute(new ClientRoute("appealorder", "B4.controller.AppealOrder", requiredPermission: "GkhGji.SOPR.Appeal.View"));

            map.AddRoute(new ClientRoute("protocolosprequest", "B4.controller.ProtocolOSPRequest", requiredPermission: "GkhGji.ProtocolOSPRequest.View"));

            map.AddRoute(new ClientRoute("admonition", "B4.controller.Admonition", requiredPermission: "GkhGji.AppealCitizensState.AppealCitsAdmonition.View"));
            map.AddRoute(new ClientRoute("prescriptionfond", "B4.controller.PrescriptionFond", requiredPermission: "GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.View"));
            //Переоформление лицензии
            map.AddRoute(new ClientRoute("licensereissuance", "B4.controller.licensereissuance.LicenseReissuance"));
            map.AddRoute(new ClientRoute("licensereissuanceeditor/{id}/", "B4.controller.licensereissuance.Edit"));
            map.AddRoute(new ClientRoute("baselicensereissuance", "B4.controller.BaseLicenseReissuance", requiredPermission: "GkhGji.Inspection.BaseLicApplicants.View"));

            map.AddRoute(new ClientRoute("fileregister", "B4.controller.FileRegister", requiredPermission: "GkhGji.FileRegister.View"));
        }
    }
}