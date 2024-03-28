using Bars.B4;
namespace Bars.Gkh
{
    using System.Collections.Generic;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gkhconfig", "B4.controller.GkhConfig"));
            map.AddRoute(new ClientRoute("taskentrycontroller", "B4.controller.TaskEntryController", requiredPermission: "Administration.TaskEntryController.View"));
            map.AddRoute(new ClientRoute("instructiongroupsmanager",
                "B4.controller.administration.InstructionGroups",
                requiredPermission: "Administration.InstructionGroups.View"));
            map.AddRoute(new ClientRoute("role", "B4.controller.Role", requiredPermission: "B4.Security.Role"));
            map.AddRoute(new ClientRoute("localadminrolesettings", "B4.controller.administration.LocalAdminRoleSettings", requiredPermission: "B4.Security.LocalAdminRoleSettings"));
            map.AddRoute(new ClientRoute("rolepermission", "B4.controller.RolePermission", requiredPermission: "B4.Security.AccessRights"));
            map.AddRoute(new ClientRoute("statepermission", "B4.controller.StatePermission", requiredPermission: "B4.States.StatePermission.View"));

            map.AddRoute(new ClientRoute("printcerthistory", "B4.controller.administration.PrintCertHistory", requiredPermission: "Administration.PrintCertHistory.View"));

            map.AddRoute(new ClientRoute("operatoradministration", "B4.controller.administration.Operator", requiredPermission: "Administration.Operator.View"));
            map.AddRoute(new ClientRoute("profilesettingadministration",
                "B4.controller.administration.ProfileSetting",
                requiredPermission: "Administration.Profile.View"));
            map.AddRoute(new ClientRoute("templatereplacement",
                "B4.controller.administration.TemplateReplacement",
                requiredPermission: "Administration.TemplateReplacement.View")); //??
            map.AddRoute(new ClientRoute("belaymanorgactivity", "B4.controller.BelayManOrgActivity", requiredPermission: "Gkh.BelayManOrgActivity.View"));
            map.AddRoute(new ClientRoute("belayorg", "B4.controller.BelayOrg", requiredPermission: "Gkh.Orgs.Belay.View"));
            map.AddRoute(new ClientRoute("builder", "B4.controller.Builder", requiredPermission: "Gkh.Orgs.Builder.View"));
            
            map.AddRoute(new ClientRoute("belayorgkindactivity",
                "B4.controller.dict.BelayOrgKindActivity",
                requiredPermission: "Gkh.Dictionaries.BelayOrgKindActivity.View"));
            map.AddRoute(new ClientRoute("belayorgkindactivity", "B4.controller.dict.BelayOrgKindActivity", requiredPermission: "Gkh.Dictionaries.BelayOrgKindActivity.View"));
            map.AddRoute(new ClientRoute("capitalgroup", "B4.controller.dict.CapitalGroup", requiredPermission: "Gkh.Dictionaries.CapitalGroup.View"));
            map.AddRoute(new ClientRoute("monitoringtypedict", "B4.controller.dict.MonitoringTypeDict", requiredPermission: "Gkh.Dictionaries.MonitoringTypeDict.View"));
            map.AddRoute(new ClientRoute("constructiveelement",
                "B4.controller.dict.ConstructiveElement",
                requiredPermission: "Gkh.Dictionaries.ConstructiveElement.View"));
            map.AddRoute(new ClientRoute("constructiveelementgroup",
                "B4.controller.dict.ConstructiveElementGroup",
                requiredPermission: "Gkh.Dictionaries.ConstructiveElementGroup.View"));
            map.AddRoute(new ClientRoute("furtheruse", "B4.controller.dict.FurtherUse", requiredPermission: "Gkh.Dictionaries.FurtherUse.View"));
            map.AddRoute(new ClientRoute("inspector", "B4.controller.dict.Inspector", requiredPermission: "Gkh.Dictionaries.Inspector.View"));
            map.AddRoute(new ClientRoute("institutions", "B4.controller.dict.Institutions", requiredPermission: "Gkh.Dictionaries.Institutions.View"));
            map.AddRoute(new ClientRoute("centralheatingstation", "B4.controller.dict.CentralHeatingStation", requiredPermission: "Gkh.Dictionaries.CentralHeatingStation.View"));

            map.AddRoute(new ClientRoute("kindequipment", "B4.controller.dict.KindEquipment", requiredPermission: "Gkh.Dictionaries.KindEquipment.View"));
            map.AddRoute(new ClientRoute("licenseprovideddoc",
                "B4.controller.dict.LicenseProvidedDoc",
                requiredPermission: "Gkh.Dictionaries.LicenseProvidedDoc.View"));
            map.AddRoute(new ClientRoute("kindrisk", "B4.controller.dict.KindRisk", requiredPermission: "Gkh.Dictionaries.KindRisk.View"));
            map.AddRoute(new ClientRoute("videooverwatchtype", "B4.controller.dict.VideoOverwatchType", requiredPermission: "Gkh.Dictionaries.VideoOverwatchType.View"));
            map.AddRoute(new ClientRoute("cscalculation", "B4.controller.CSCalculation", requiredPermission: "Gkh.CSCalculation.Calculate.View"));
            map.AddRoute(new ClientRoute("cscalculationformula", "B4.controller.cscalculation.CSFormula", requiredPermission: "Gkh.CSCalculation.CSFormula.View"));
            map.AddRoute(new ClientRoute("tarifnormative", "B4.controller.dict.TarifNormative", requiredPermission: "Gkh.CSCalculation.CSFormula.View"));
            map.AddRoute(new ClientRoute("mocoefficient", "B4.controller.dict.MOCoefficient", requiredPermission: "Gkh.CSCalculation.CSFormula.View"));
            map.AddRoute(new ClientRoute("categorycsmkd", "B4.controller.dict.CategoryCSMKD", requiredPermission: "Gkh.CSCalculation.CSFormula.View"));
            map.AddRoute(new ClientRoute("cscalculationformula_edit/{id}", "B4.controller.cscalculation.CSFormulaEdit", requiredPermission: "Gkh.CSCalculation.CSFormula.View"));
            map.AddRoute(new ClientRoute("protocolmkdstate", "B4.controller.dict.ProtocolMKDState", requiredPermission: "Gkh.Dictionaries.ProtocolMKDState.View"));
            map.AddRoute(new ClientRoute("protocolmkdsource", "B4.controller.dict.ProtocolMKDSource", requiredPermission: "Gkh.Dictionaries.ProtocolMKDSource.View"));
            map.AddRoute(new ClientRoute("protocolmkdiniciator", "B4.controller.dict.ProtocolMKDIniciator", requiredPermission: "Gkh.Dictionaries.ProtocolMKDIniciator.View"));
            //map.AddRoute(new ClientRoute("meterdevice", "B4.controller.dict.MeteringDevice", requiredPermission: "Gkh.Dictionaries.MeteringDevice.View"));
            map.AddRoute(new ClientRoute("municipality", "B4.controller.dict.Municipality", requiredPermission: "Gkh.Dictionaries.Municipality.View"));
            map.AddRoute(new ClientRoute("normativedoc", "B4.controller.dict.NormativeDoc", requiredPermission: "Gkh.Dictionaries.NormativeDoc.View"));
            map.AddRoute(new ClientRoute("orgform", "B4.controller.dict.OrganizationForm", requiredPermission: "Gkh.Dictionaries.OrganizationForm.View"));
            map.AddRoute(new ClientRoute("period", "B4.controller.dict.Period", requiredPermission: "Gkh.Dictionaries.Period.View"));
            map.AddRoute(new ClientRoute("reasoninexpedient",
                "B4.controller.dict.ReasonInexpedient",
                requiredPermission: "Gkh.Dictionaries.ReasonInexpedient.View"));
            map.AddRoute(new ClientRoute("resettlementprogram",
                "B4.controller.dict.ResettlementProgram",
                requiredPermission: "Gkh.Dictionaries.ResettlementProgram.View"));
            map.AddRoute(new ClientRoute("resettlementprogsource",
                "B4.controller.dict.ResettlementProgramSource",
                requiredPermission: "Gkh.Dictionaries.ResettlementProgramSource.View"));
            map.AddRoute(new ClientRoute("roofingmaterial", "B4.controller.dict.RoofingMaterial", requiredPermission: "Gkh.Dictionaries.RoofingMaterial.View"));
            map.AddRoute(new ClientRoute("speciality", "B4.controller.dict.Specialty", requiredPermission: "Gkh.Dictionaries.Specialty.View"));
            map.AddRoute(new ClientRoute("typeownership", "B4.controller.dict.TypeOwnership", requiredPermission: "Gkh.Dictionaries.TypeOwnership.View"));
            map.AddRoute(new ClientRoute("typeproject", "B4.controller.dict.TypeProject", requiredPermission: "Gkh.Dictionaries.TypeProject.View"));
            map.AddRoute(new ClientRoute("typeservice", "B4.controller.dict.TypeService", requiredPermission: "Gkh.Dictionaries.TypeService.View"));
            map.AddRoute(new ClientRoute("unitmeasure", "B4.controller.dict.UnitMeasure", requiredPermission: "Gkh.Dictionaries.UnitMeasure.View"));
            map.AddRoute(new ClientRoute("wallmaterial", "B4.controller.dict.WallMaterial", requiredPermission: "Gkh.Dictionaries.WallMaterial.View"));
            map.AddRoute(new ClientRoute("work", "B4.controller.dict.Work", requiredPermission: "Gkh.Dictionaries.Work.View"));
            map.AddRoute(new ClientRoute("communalresource", "B4.controller.dict.CommunalResource", requiredPermission: "Gkh.Dictionaries.CommunalResource.View"));
            map.AddRoute(new ClientRoute("stopreason", "B4.controller.dict.StopReason", requiredPermission: "Gkh.Dictionaries.StopReason.View"));
            map.AddRoute(new ClientRoute("managementcontractservice", "B4.controller.dict.contractservice.ManagementContractService", requiredPermission: "Gkh.Dictionaries.ManagementContractService.View"));
            map.AddRoute(new ClientRoute("organizationwork", "B4.controller.dict.OrganizationWork", requiredPermission: "Gkh.Dictionaries.OrganizationWork.View"));
            map.AddRoute(new ClientRoute("contentrepairmkdwork",
                "B4.controller.dict.ContentRepairMkdWork",
                requiredPermission: "Gkh.Dictionaries.ContentRepairMkdWork.View"));
            map.AddRoute(new ClientRoute("multipurposeglossary",
                "B4.controller.dict.MultipurposeGlossary",
                requiredPermission: "Gkh.Dictionaries.Multipurpose.View"));
            map.AddRoute(new ClientRoute("currentworkkindrepair",
                "B4.controller.dict.WorkKindCurrentRepair",
                requiredPermission: "Gkh.Dictionaries.WorkKindCurrentRepair.View"));
            map.AddRoute(new ClientRoute("zonalinspection", "B4.controller.dict.ZonalInspection", requiredPermission: "Gkh.Dictionaries.ZonalInspection.View"));
            map.AddRoute(new ClientRoute("emergencyobject", "B4.controller.EmergencyObj", requiredPermission: "Gkh.EmergencyObject.View"));
            map.AddRoute(new ClientRoute("importlog", "B4.controller.Import.ImportLog", requiredPermission: "Administration.ImportLog.View"));
            map.AddRoute(new ClientRoute("loadidis", "B4.controller.Import.loadidis.LoadIdIs", requiredPermission: "Administration.LoadIdIs.View"));
            map.AddRoute(new ClientRoute("localgovernment", "B4.controller.LocalGovernment", requiredPermission: "Gkh.Orgs.LocalGov.View"));
            map.AddRoute(new ClientRoute("paymentagent", "B4.controller.PaymentAgent", requiredPermission: "Gkh.Orgs.PaymentAgent.View"));
            map.AddRoute(new ClientRoute("managingorganization", "B4.controller.ManagingOrganization", requiredPermission: "Gkh.Orgs.Managing.View"));
            map.AddRoute(new ClientRoute("politicauthority", "B4.controller.PoliticAuthority", requiredPermission: "Gkh.Orgs.PoliticAuth.View"));
            map.AddRoute(new ClientRoute("realityobject", "B4.controller.RealityObject", requiredPermission: "Gkh.RealityObject.View"));
            map.AddRoute(new ClientRoute("realityobjectoutdoor", "B4.controller.realityobj.RealityObjectOutdoor", requiredPermission: "Gkh.RealityObjectOutdoor.View"));
            map.AddRoute(new ClientRoute("housingfundmonitoring", "B4.controller.housingfundmonitoring.Period", requiredPermission: "Gkh.HousingFundMonitoringPeriod.View"));
            map.AddRoute(new ClientRoute("housingfundmonitoringdetail/{id}", "B4.controller.housingfundmonitoring.Detail", requiredPermission: "Gkh.HousingFundMonitoringPeriod.View"));

            map.AddRoute(new ClientRoute("contragentclwedit/{id}", "B4.controller.contragentclw.Navigation", requiredPermission: "Gkh.Orgs.ContragentClw.View"));
            map.AddRoute(new ClientRoute("contragentclwedit/{id}/edit", "B4.controller.contragentclw.Edit", requiredPermission: "Gkh.Orgs.ContragentClw.Edit"));
            map.AddRoute(new ClientRoute("contragentclwedit/{id}/municipality", "B4.controller.contragentclw.Municipality", requiredPermission: "Gkh.Orgs.ContragentClw.Municipality.View"));
            map.AddRoute(new ClientRoute("contragentclw", "B4.controller.ContragentClw", requiredPermission: "Gkh.Orgs.ContragentClw.View"));

            map.AddRoute(new ClientRoute("serviceorganization", "B4.controller.ServiceOrganization", requiredPermission: "Gkh.RealityObject.Register.ServiceOrg.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}", "B4.controller.servorg.Navigation", requiredPermission: "Gkh.Orgs.Serv.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/edit", "B4.controller.servorg.Edit", requiredPermission: "Gkh.Orgs.Serv.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/municipality", "B4.controller.servorg.Municipality", requiredPermission: "Gkh.Orgs.Serv.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/realobj", "B4.controller.servorg.RealityObject", requiredPermission: "Gkh.Orgs.Serv.RealtyObject.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/realobjcontract", "B4.controller.servorg.RealityObjectContract", requiredPermission: "Gkh.Orgs.Serv.RealityObjectContract.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/document", "B4.controller.servorg.Documentation", requiredPermission: "Gkh.Orgs.Serv.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/service", "B4.controller.servorg.Service", requiredPermission: "Gkh.Orgs.Serv.View"));
            map.AddRoute(new ClientRoute("serviceorganizationedit/{id}/activity", "B4.controller.servorg.Activity", requiredPermission: "Gkh.Orgs.Serv.View"));

            map.AddRoute(new ClientRoute("supplyresorg", "B4.controller.SupplyResourceOrg", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}", "B4.controller.supplyresourceorg.Navigation", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/edit", "B4.controller.supplyresourceorg.Edit", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/municipality", "B4.controller.supplyresourceorg.Municipality", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/realobj", "B4.controller.supplyresourceorg.RealtyObject", requiredPermission: "Gkh.Orgs.SupplyResource.RealtyObject.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/realobjcontract", "B4.controller.supplyresourceorg.Contract", requiredPermission: "Gkh.Orgs.SupplyResource.ContractsWithRealObj.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/document", "B4.controller.supplyresourceorg.Documentation", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/service", "B4.controller.supplyresourceorg.Service", requiredPermission: "Gkh.Orgs.SupplyResource.View"));
            map.AddRoute(new ClientRoute("supplyresorgedit/{id}/activity", "B4.controller.supplyresourceorg.Activity", requiredPermission: "Gkh.Orgs.SupplyResource.View"));

            map.AddRoute(new ClientRoute("citizensuggestion",
                "B4.controller.suggestion.CitizenSuggestion",
                requiredPermission: "Gkh.Dictionaries.Suggestion.CitizenSuggestion.View"));
            map.AddRoute(new ClientRoute("rubric", "B4.controller.suggestion.Rubric", requiredPermission: "Gkh.Dictionaries.Suggestion.Rubric.View"));
            map.AddRoute(new ClientRoute("problemplace", "B4.controller.dict.ProblemPlace", requiredPermission: "Gkh.Dictionaries.ProblemPlace.View"));
            map.AddRoute(new ClientRoute("position", "B4.controller.dict.Position", requiredPermission: "Gkh.Dictionaries.Position.View"));
            map.AddRoute(new ClientRoute("executionAction",
                "B4.controller.administration.ExecutionAction",
                requiredPermission: "Administration.ExecutionAction.View"));
            map.AddRoute(new ClientRoute("roimport", "B4.controller.Import.Ro.RoImport", requiredPermission: "Import.RoImport.View"));
            map.AddRoute(new ClientRoute("gkuimport", "B4.controller.Import.Gku", requiredPermission: "Import.Gku.View"));
            map.AddRoute(new ClientRoute("roimportfromfund", "B4.controller.Import.Ro.RoImportFromFund", requiredPermission: "Import.RoImportFromFund.View"));
            map.AddRoute(new ClientRoute("billingimport", "B4.controller.Import.Billing", requiredPermission: "Import.Billing.View"));
            map.AddRoute(new ClientRoute("municipalityTree", "B4.controller.dict.MunicipalityTree", requiredPermission: "Gkh.Dictionaries.MunicipalityTree.View"));
            map.AddRoute(new ClientRoute("oktmodataimport", "B4.controller.import.oktmo.OktmoData", requiredPermission: "Import.Oktmo"));
            map.AddRoute(new ClientRoute("municipalityfiasoktmoimport", "B4.controller.import.oktmo.MunicipalityFiasOktmo", requiredPermission: "Import.MunicipalityFiasOktmo"));
            map.AddRoute(new ClientRoute("gkhparams", "B4.controller.administration.GkhParams", requiredPermission: "Administration.GkhParams.View"));
            map.AddRoute(new ClientRoute("typeliftdict", "B4.controller.dict.TypeLift", requiredPermission: "Gkh.Dictionaries.Position.View"));
            map.AddRoute(new ClientRoute("buildingfeature",
                "B4.controller.dict.BuildingFeature",
                requiredPermission: "Gkh.Dictionaries.BuildingFeature.View"));
            map.AddRoute(new ClientRoute("typecategorycs",
                "B4.controller.dict.TypeCategoryCS",
                requiredPermission: "Gkh.Dictionaries.TypeCategoryCS.View"));
            map.AddRoute(new ClientRoute("builderdocumenttype",
                "B4.controller.dict.BuilderDocumentType",
                requiredPermission: "Gkh.Dictionaries.BuilderDocumentType.View"));

            map.AddRoute(new ClientRoute("importexport",
                "B4.controller.administration.ImportExportController",
                requiredPermission: "Administration.ImportExport.View"));
            map.AddRoute(new ClientRoute("fieldrequirement", "B4.controller.administration.FieldRequirement", requiredPermission: "B4.Security.AccessRights"));
            map.AddRoute(new ClientRoute("importoperator", "B4.controller.Import.Operator", requiredPermission: "Administration.Operator.View"));

            map.AddRoute(new ClientRoute("managingorganizationimport", "B4.controller.Import.ManagingOrganization", requiredPermission: "Import.ManagingOrganization.View"));

            map.AddRoute(new ClientRoute("importorganization", "B4.controller.Import.organization.OrganizationImport", requiredPermission: "Import.OrganizationImport"));
            map.AddRoute(new ClientRoute("managementsysimport",
                "B4.controller.export.ManagementSysImport",
                requiredPermission: "Administration.ExportManagSys.View"));

            map.AddRoute(new ClientRoute("map/{id}", "B4.controller.YandexMap"));
           
            map.AddRoute(new ClientRoute("contragent", "B4.controller.Contragent", requiredPermission: "Gkh.Orgs.Contragent.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}", "B4.controller.contragent.Navi",requiredPermission: "Gkh.Orgs.Contragent.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/edit", "B4.controller.contragent.Edit", requiredPermission: "Gkh.Orgs.Contragent.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/bank", "B4.controller.contragent.Bank", requiredPermission: "Gkh.Orgs.Contragent.Register.Bank.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/casesedit", "B4.controller.contragent.CasesEdit", requiredPermission: "Gkh.Orgs.Contragent.Register.CasesEdit.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/contact", "B4.controller.contragent.Contact", requiredPermission: "Gkh.Orgs.Contragent.Register.Contact.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/municipality", "B4.controller.contragent.Municipality", requiredPermission: "Gkh.Orgs.Contragent.Register.Municipality.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/auditpurpose", "B4.controller.contragent.AuditPurpose", requiredPermission: "Gkh.Orgs.Contragent.Register.AuditPurpose.View"));
            map.AddRoute(new ClientRoute("contragentedit/{id}/risk", "B4.controller.contragent.Risk", requiredPermission: "Gkh.Orgs.Contragent.Register.Risk.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}", "B4.controller.realityobj.Navi", requiredPermission: "Gkh.RealityObject.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/edit", "B4.controller.realityobj.Edit", requiredPermission: "Gkh.RealityObject.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/meteringdevice",
                "B4.controller.realityobj.MeteringDevice",
                requiredPermission: "Gkh.RealityObject.Register.MeteringDevice.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/image",
                "B4.controller.realityobj.Image",
                requiredPermission: "Gkh.RealityObject.Register.Image.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/structelement",
                "B4.controller.realityobj.StructElement",
                requiredPermission: "Gkh.RealityObject.Register.StructElem.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/houseinfo",
                "B4.controller.realityobj.HouseInfo",
                requiredPermission: "Gkh.RealityObject.Register.HouseInfo.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/categorymkd",
              "B4.controller.realityobj.RealityObjectCategoryMKD",
              requiredPermission: "Gkh.RealityObject.Register.CategoryCSMKD.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/room",
                "B4.controller.realityobj.Room",
                requiredPermission: "Gkh.RealityObject.Register.HouseInfo.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/roomedit/{roomid}",
                "B4.controller.realityobj.Room",
                "edit",
                requiredPermission: "Gkh.RealityObject.Register.HouseInfo.Edit"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/land",
                "B4.controller.realityobj.Land",
                requiredPermission: "Gkh.RealityObject.Register.Land.View"));
            /*map.AddRoute(new ClientRoute("realityobjectedit/{id}/belay",
                "B4.controller.realityobj.Belay",
                requiredPermission: "Gkh.RealityObject.Register.Belay.View"));*/
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/managorg",
                "B4.controller.realityobj.ManagOrg",
                requiredPermission: "Gkh.RealityObject.Register.ManagOrg.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/serviceorg",
                "B4.controller.realityobj.ServiceOrg",
                requiredPermission: "Gkh.RealityObject.Register.ServiceOrg.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/resorg",
                "B4.controller.realityobj.ResOrg",
                requiredPermission: "Gkh.RealityObject.Register.ResOrg.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/councilapartmenthouse",
                "B4.controller.realityobj.CouncilApartmentHouse",
                requiredPermission: "Gkh.RealityObject.Register.Councillors.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/constructiveelement",
                "B4.controller.realityobj.ConstructiveElement",
                requiredPermission: "Gkh.RealityObject.Register.ConstructiveElement.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/curentrepair",
                "B4.controller.realityobj.CurentRepair",
                requiredPermission: "Gkh.RealityObject.Register.CurentRepair.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/entrance",
                "B4.controller.realityobj.Entrance",
                requiredPermission: "Gkh.RealityObject.Register.Entrance.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/block",
                "B4.controller.realityobj.Block",
                requiredPermission: "Gkh.RealityObject.Register.Block.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/antenna",
                "B4.controller.realityobj.Antenna",
                requiredPermission: "Gkh.RealityObject.Register.SKPT.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/videcam",
              "B4.controller.realityobj.Videcam",
              requiredPermission: "Gkh.RealityObject.Register.Videcam.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/housekeeper",
              "B4.controller.realityobj.Housekeeper",
              requiredPermission: "Gkh.RealityObject.Register.Housekeeper.View"));

            //map.AddRoute(new ClientRoute("realityobjectedit/{id}/meteringdeviceschecks","B4.controller.realityobj.MeteringDevicesChecks"));



            map.AddRoute(new ClientRoute("livingsquarecost", "B4.controller.dict.LivingSquareCost", requiredPermission: "Gkh.Dictionaries.LivingSquareCost.View"));
            map.AddRoute(new ClientRoute("fiasoktmo", "B4.controller.dict.FiasOktmo", requiredPermission: "Administration.Oktmo.fiasoktmo.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/changevalueshistory", "B4.controller.realityobj.ChangeValuesHistory"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/technicalmonitoring", "B4.controller.realityobj.TechnicalMonitoring", requiredPermission: "Gkh.RealityObject.Register.TechnicalMonitoring.View"));

            map.AddRoute(new ClientRoute("person", "B4.controller.Person", requiredPermission: "Gkh.Person.View"));
            map.AddRoute(new ClientRoute("personedit/{id}", "B4.controller.person.Navi", requiredPermission: "Gkh.Person.View"));
            map.AddRoute(new ClientRoute("personedit/{id}/edit", "B4.controller.person.Edit", requiredPermission: "Gkh.Person.Edit"));
            map.AddRoute(new ClientRoute("personedit/{id}/disqualification",
                "B4.controller.person.DisqualificationInfo",
                requiredPermission: "Gkh.Person.PersonDisqualificationInfo.View"));
            map.AddRoute(new ClientRoute("personedit/{id}/placework", "B4.controller.person.PlaceWork", requiredPermission: "Gkh.Person.PersonPlaceWork.View"));
            map.AddRoute(new ClientRoute("personedit/{id}/{requestId}/{qsId}",
                "B4.controller.person.Edit",
                "editRequest",
                requiredPermission: "Gkh.Person.RequestToExam.Edit"));
           
            map.AddRoute(new ClientRoute("system_version", "B4.controller.system.Version", requiredPermission: "Administration.Version.View"));

            map.AddRoute(new ClientRoute("manorglicenseregister", "B4.controller.manorglicense.License", requiredPermission: "Gkh.ManOrgLicense.License.View"));
            map.AddRoute(new ClientRoute("manorgrequestlicense", "B4.controller.manorglicense.Request", requiredPermission: "Gkh.ManOrgLicense.Request"));
            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}", "B4.controller.manorglicense.Navi"/*, requiredPermission: "Gkh.ManOrgLicense.License.View"*/));
            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}/addlicense",
                "B4.controller.manorglicense.Navi",
                "addlicense",
                requiredPermission: "Gkh.ManOrgLicense.License.Edit"));
            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}/deletelicense",
                "B4.controller.manorglicense.Navi",
                "deletelicense",
                requiredPermission: "Gkh.ManOrgLicense.License.Delete"));
            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}/editrequest",
                "B4.controller.manorglicense.EditRequest",
                requiredPermission: "Gkh.ManOrgLicense.Request.Edit"));
            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}/editlicense",
                "B4.controller.manorglicense.EditLicense",
                requiredPermission: "Gkh.ManOrgLicense.License.Edit"));

            map.AddRoute(new ClientRoute("requesttoexamregister", "B4.controller.RequestToExamRegister", requiredPermission: "Gkh.RequestToExamRegister.View"));

            map.AddRoute(new ClientRoute("datatransferintegration", "B4.controller.administration.DataTransferIntegrationSession", requiredPermission: "Administration.DataTransferIntegration.View"));

            map.AddRoute(new ClientRoute("realestatetype", "B4.controller.RealEstateType", requiredPermission: "Gkh.Dictionaries.RealEstateType.View"));

            map.AddRoute(new ClientRoute("edit_realestatetype/{id}","B4.controller.RealEstateType","edit",requiredPermission: "Gkh.Dictionaries.RealEstateType.Edit"));
           
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}", "B4.controller.claimwork.Navi"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/{docUrl}/aftercreatedoc", "B4.controller.claimwork.Navi", "aftercreatedoc"));

            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}", "B4.controller.claimwork.BuildContractNavi"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/{docId}/{docUrl}/aftercreatedoc", "B4.controller.claimwork.BuildContractNavi", "aftercreatedoc"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/deletedocument", "B4.controller.claimwork.BuildContractNavi", "deletedocument"));

            map.AddRoute(new ClientRoute("operationlog", "B4.controller.administration.LogOperation", requiredPermission: "Administration.OperationLog.View"));

            map.AddRoute(new ClientRoute("tablelock", "B4.controller.TableLock", requiredPermission: "Administration.TableLock.View"));

            map.AddRoute(new ClientRoute("gkhtaskscontroller", "B4.controller.GkhTasksController", requiredPermission: "Administration.GkhTasksController.View"));

            map.AddRoute(new ClientRoute("categoryposts", "B4.controller.dict.CategoryPosts", requiredPermission: "Gkh.Dictionaries.CategoryPosts.View"));

            map.AddRoute(new ClientRoute("managingorganizationedit/{id}", "B4.controller.manorg.Navigation"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/edit", "B4.controller.manorg.Edit"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/municipality", "B4.controller.manorg.Municipality"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/realityObject", "B4.controller.manorg.RealityObject"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/contract", "B4.controller.manorg.Contract"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/contractView", "B4.controller.manorg.Contract", "view"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/documentation", "B4.controller.manorg.Documentation"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/workmode", "B4.controller.manorg.WorkMode"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/membership", "B4.controller.manorg.Membership"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/activity", "B4.controller.manorg.Activity"));
            map.AddRoute(new ClientRoute("managingorganizationedit/{id}/reformamanorg", "B4.controller.manorg.ReformaManOrg"));
            
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/techpassport/{id}", "B4.controller.TechPassport", requiredPermission: "Gkh.RealityObject.Register.TechPassport.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/objtypeworkcr", "B4.controller.RealtyObjTypeWorkCr", requiredPermission: "Gkh.RealityObject.Register.TechPassport.View"));
            map.AddRoute(new ClientRoute("tpelevatorsimport", "B4.controller.import.TpElevatorsImport", requiredPermission: "Import.TpElevatorsImport"));

            map.AddRoute(new ClientRoute("governmenservice", "B4.controller.licensing.FormGovernmentService", requiredPermission: "GkhGji.Licensing.FormGovService.View"));
            map.AddRoute(new ClientRoute("risdataexport", "B4.controller.administration.RisDataExport"));
            map.AddRoute(new ClientRoute("governmenservicedetail/{id}", "B4.controller.licensing.GovernmenServiceDetail", requiredPermission: "GkhGji.Licensing.FormGovService.View"));

            map.AddRoute(new ClientRoute("technicalcustomers", "B4.controller.TechnicalCustomer", requiredPermission: "Gkh.Orgs.TechnicalCustomer.View"));
            map.AddRoute(new ClientRoute("licenseregistrationreason", "B4.controller.dict.LicenseRegistrationReason", requiredPermission: "Gkh.Dictionaries.LicenseRegistrationReason.View"));
            map.AddRoute(new ClientRoute("licenserejectreason", "B4.controller.dict.LicenseRejectReason", requiredPermission: "Gkh.Dictionaries.LicenseRejectReason.View"));

            map.AddRoute(new ClientRoute("housinginspection", "B4.controller.HousingInspection"));
            map.AddRoute(new ClientRoute("housinginspectionedit/{id}", "B4.controller.housinginspection.Navigation"));
            map.AddRoute(new ClientRoute("housinginspectionedit/{id}/edit", "B4.controller.housinginspection.Edit"));
            map.AddRoute(new ClientRoute("housinginspectionedit/{id}/municipality", "B4.controller.housinginspection.Municipality"));

            // квалификационный экзамен
            map.AddRoute(new ClientRoute("qtestquestionsdict", "B4.controller.dict.QualifyTestQuestions", requiredPermission: "Gkh.Dictionaries.QualifyTestQuestions"));
            map.AddRoute(new ClientRoute("qtestsettingsdict", "B4.controller.dict.QualifyTestSettings", requiredPermission: "Gkh.Dictionaries.QualifyTestQuestions"));

            map.AddRoute(new ClientRoute("qualifysertifiateregistry", "B4.controller.person.QualifySertificate", requiredPermission: "Gkh.RequestToExamRegister.View"));


            map.AddRoute(new ClientRoute("realityobjectedit/{id}/lift", "B4.controller.realityobj.Lift"/*, requiredPermission: "Gkh.RealityObject.Register.Lift"*/));
            map.AddRoute(new ClientRoute("liftregister", "B4.controller.realityobj.LiftRegister"/*, requiredPermission: "Gkh.RealityObject.Register.Lift"*/));
            map.AddRoute(new ClientRoute("realityobjectoutdooredit/{id}", "B4.controller.realityobj.realityobjectoutdoor.Navi", requiredPermission: "Gkh.RealityObjectOutdoor.View"));
            map.AddRoute(new ClientRoute("realityobjectoutdooredit/{id}/edit", "B4.controller.realityobj.realityobjectoutdoor.Edit", requiredPermission: "Gkh.RealityObjectOutdoor.View"));

            map.AddRoute(new ClientRoute("emailmessage", "B4.controller.administration.EmailMessage", requiredPermission: "B4.Audit.EmailMessage.View"));
            map.AddRoute(new ClientRoute("notify", "B4.controller.administration.Notify", requiredPermission: "Administration.Notify.View"));
            map.AddRoute(new ClientRoute("basehouseemergency", "B4.controller.dict.BaseHouseEmergency", requiredPermission: "Gkh.Dictionaries.BaseHouseEmergency.View"));
            map.AddRoute(new ClientRoute("typesheatsource", "B4.controller.dict.TypesHeatSource", requiredPermission: "Gkh.Dictionaries.TypesHeatSource.View"));
            map.AddRoute(new ClientRoute("typeinterhouseheatingsystem", "B4.controller.dict.TypeInterHouseHeatingSystem", requiredPermission: "Gkh.Dictionaries.TypeInterHouseHeatingSystem.View"));
            map.AddRoute(new ClientRoute("typesheatedappliances", "B4.controller.dict.TypesHeatedAppliances", requiredPermission: "Gkh.Dictionaries.TypesHeatedAppliances.View"));
            map.AddRoute(new ClientRoute("networkandrisermaterials", "B4.controller.dict.NetworkAndRiserMaterials", requiredPermission: "Gkh.Dictionaries.NetworkAndRiserMaterials.View"));
            map.AddRoute(new ClientRoute("networkinsulationmaterials", "B4.controller.dict.NetworkInsulationMaterials", requiredPermission: "Gkh.Dictionaries.NetworkInsulationMaterials.View"));
            map.AddRoute(new ClientRoute("typeswaterdisposalmaterial", "B4.controller.dict.TypesWaterDisposalMaterial", requiredPermission: "Gkh.Dictionaries.TypesWaterDisposalMaterial.View"));
            map.AddRoute(new ClientRoute("foundationmaterials", "B4.controller.dict.FoundationMaterials", requiredPermission: "Gkh.Dictionaries.FoundationMaterials.View"));
            map.AddRoute(new ClientRoute("typeswindowmaterials", "B4.controller.dict.TypesWindowMaterials", requiredPermission: "Gkh.Dictionaries.TypesWindowMaterials.View"));
            map.AddRoute(new ClientRoute("typesbearingpartroof", "B4.controller.dict.TypesBearingPartRoof", requiredPermission: "Gkh.Dictionaries.TypesBearingPartRoof.View"));
            map.AddRoute(new ClientRoute("warminglayersattics", "B4.controller.dict.WarmingLayersAttics", requiredPermission: "Gkh.Dictionaries.WarmingLayersAttics.View"));
            map.AddRoute(new ClientRoute("materialroof", "B4.controller.dict.MaterialRoof", requiredPermission: "Gkh.Dictionaries.MaterialRoof.View"));
            map.AddRoute(new ClientRoute("facadedecorationmaterials", "B4.controller.dict.FacadeDecorationMaterials", requiredPermission: "Gkh.Dictionaries.FacadeDecorationMaterials.View"));
            map.AddRoute(new ClientRoute("typesexternalfacadeinsulation", "B4.controller.dict.TypesExternalFacadeInsulation", requiredPermission: "Gkh.Dictionaries.TypesExternalFacadeInsulation.View"));
            map.AddRoute(new ClientRoute("typesexteriorwalls", "B4.controller.dict.TypesExteriorWalls", requiredPermission: "Gkh.Dictionaries.TypesExteriorWalls.View"));
            map.AddRoute(new ClientRoute("waterdispensers", "B4.controller.dict.WaterDispensers", requiredPermission: "Gkh.Dictionaries.WaterDispensers.View"));
            map.AddRoute(new ClientRoute("categoryconsumersequalpopulation", "B4.controller.dict.CategoryConsumersEqualPopulation", requiredPermission: "Gkh.Dictionaries.CategoryConsumersEqualPopulation.View"));
            map.AddRoute(new ClientRoute("typeinformationnpa", "B4.controller.dict.TypeInformationNpa", requiredPermission: "Gkh.Dictionaries.TypeInformationNpa.View"));
            map.AddRoute(new ClientRoute("energyefficiencyclasses", "B4.controller.dict.EnergyEfficiencyClasses", requiredPermission: "Gkh.Dictionaries.EnergyEfficiencyClasses.View"));
            map.AddRoute(new ClientRoute("typenpa", "B4.controller.dict.TypeNpa", requiredPermission: "Gkh.Dictionaries.TypeNpa.View"));
            map.AddRoute(new ClientRoute("typenormativeact", "B4.controller.dict.TypeNormativeAct", requiredPermission: "Gkh.Dictionaries.TypeNormativeAct.View"));
            map.AddRoute(new ClientRoute("contragentrole", "B4.controller.dict.ContragentRole", requiredPermission: "Gkh.Dictionaries.ContragentRole.View"));
            map.AddRoute(new ClientRoute("riskcategory", "B4.controller.dict.RiskCategory", requiredPermission: "Gkh.Dictionaries.RiskCategory.View"));
            map.AddRoute(new ClientRoute("typefloor", "B4.controller.dict.TypeFloor", requiredPermission: "Gkh.Dictionaries.TypeFloor.View"));
            map.AddRoute(new ClientRoute("identitydocumenttype", "B4.controller.dict.IdentityDocumentType", requiredPermission: "Gkh.Dictionaries.IdentityDocumentType.View"));

#if DEBUG
            var debugRoute = new ClientRoute("debugcontroller/{viewName}", "B4.controller.administration.DebugController")
            {
                Conditions = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("viewName", @"B4\.view\.\S+"),
                }
            };
            map.AddRoute(debugRoute);
#endif
        }
    }
}
