namespace Bars.Gkh.Gis
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.RegisterRoute("indicator", "B4.controller.gisrealestate.Indicator", requiredPermission: "Gis.Indicators.View");

            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}", "B4.controller.PersonalAccountInfo");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/params", "B4.controller.PersonalAccountParam");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/counters", "B4.controller.PersonalAccountCounter");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/services", "B4.controller.PersonalAccountService");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/accruals", "B4.controller.PersonalAccountAccruals");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/saldo", "B4.controller.PersonalAccountSaldo");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/delta", "B4.controller.delta.DeltaOfChargesOverride"/* "B4.controller.delta.DeltaOfCharges"*/);
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/tenantsubsidy", "B4.controller.personalaccount.TenantSubsidy");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/servicesubsidy", "B4.controller.personalaccount.ServiceSubsidy");
            map.RegisterRoute("personalaccountinfo/{realityObjectId}/{id}/publiccontrol", "B4.controller.personalaccount.PublicControl", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Account.PublicControl.View");

            map.RegisterRoute("realityobjectedit/{id}/params", "B4.controller.HouseParam", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Params.View");
            map.RegisterRoute("realityobjectedit/{id}/service", "B4.controller.HouseService", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Service.View");
            map.RegisterRoute("realityobjectedit/{id}/accruals", "B4.controller.HouseAccruals", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Accruals.View");
            map.RegisterRoute("realityobjectedit/{id}/counters", "B4.controller.HouseCounter", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Counters.View");
            map.RegisterRoute("realityobjectedit/{id}/account", "B4.controller.PersonalAccount", requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Account.View");
            map.RegisterRoute("realityobjectedit/{id}/claims", "B4.controller.HouseClaims");

            map.RegisterRoute("gisrealestatetype", "B4.controller.gisrealestate.RealEstateType", requiredPermission: "Gis.RealEstateType.View");
            map.RegisterRoute("gisrealestatetype/{id}", "B4.controller.gisrealestate.RealEstateType", "edit", requiredPermission: "Gis.RealEstateType.View");

            map.RegisterRoute("analysisofindicators", "B4.controller.gisrealestate.AnalysisOfIndicators");
            map.RegisterRoute("regressionanalysis", "B4.controller.regressionanalysis.RegressionAnalysis");

            map.RegisterRoute("volumediscrepancy", "B4.controller.volumediscrepancy.VolumeDiscrepancy");
            map.RegisterRoute("multipleanalysis", "B4.controller.multipleAnalysis.MultipleAnalysis");
            map.RegisterRoute("olapCube", "B4.controller.olap.Cube");
            map.RegisterRoute("gisaddressmatching", "B4.controller.gisaddressMatching.AddressMatching");
            map.RegisterRoute("billingaddressmatching", "B4.controller.billingaddressMatching.AddressMatching", requiredPermission: "Administration.AddressDirectory.AddressMatching.View");
            map.RegisterRoute("skap", "B4.controller.skap.Skap", requiredPermission: "Gis.Skap.View");

            map.RegisterRoute("incrementalimport", "B4.controller.importexport.ImportData", requiredPermission: "Administration.ImportExport.IncrementalImport");
            map.RegisterRoute("gisdatabank", "B4.controller.GisDataBank");
            
            map.RegisterRoute("importdataot", "B4.controller.analytics.importdata.ImportDataOt", requiredPermission: "Gis.ImportExportData.ImportDataOt");
            map.RegisterRoute("unloadcountervalues", "B4.controller.importexport.UnloadCounterValues", requiredPermission: "Gis.ImportExportData.UnloadCounterValues");

            map.RegisterRoute("wastecollection", "B4.controller.wastecollection.WasteCollection", requiredPermission: "Gkh.WasteCollectionPlaces.View");
            map.RegisterRoute("wastecollectionplaceedit/{id}", "B4.controller.wastecollection.Navi", requiredPermission: "Gkh.WasteCollectionPlaces.View");
            map.RegisterRoute("wastecollectionplaceedit/{id}/edit", "B4.controller.wastecollection.Edit", requiredPermission: "Gkh.WasteCollectionPlaces.View");

            // Справочники
            map.RegisterRoute("gisservicedict", "B4.controller.dict.Service", requiredPermission: "Gis.Dict.Service.View");
            map.RegisterRoute("gisnormativdict", "B4.controller.dict.Normativ", requiredPermission: "Gis.Dict.Normativ.View");
            map.RegisterRoute("gistariffdict", "B4.controller.dict.Tariff", requiredPermission: "Gis.Dict.Tariff.View");
        }
    }
}