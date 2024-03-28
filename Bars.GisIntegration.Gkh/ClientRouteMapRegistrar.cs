namespace Bars.GisIntegration.Gkh
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.RegisterRoute("gisintegration", "B4.controller.integrations.ExternalRis", requiredPermission: "Administration.OutsideSystemIntegrations.Gis");
            map.RegisterRoute("gisintegrationsettings", "B4.controller.integrations.ExternalRis", requiredPermission: "Administration.OutsideSystemIntegrations.Gis");
            map.RegisterRoute("delegacy", "B4.controller.integrations.ExternalRis", requiredPermission: "Administration.OutsideSystemIntegrations.Delegacy");
            map.RegisterRoute("gisrole", "B4.controller.integrations.ExternalRis", requiredPermission: "Administration.OutsideSystemIntegrations.GisRole");
            map.RegisterRoute("riscontragentrole", "B4.controller.integrations.ExternalRis", requiredPermission: "Administration.OutsideSystemIntegrations.RisContragentRole");

            map.RegisterRoute("importdatakernel", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Administration.Import.ImportData");
            map.RegisterRoute("datasupplier", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Administration.Import.DataSupplier");
            map.RegisterRoute("servicematching", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Administration.Import.ServiceMatching");
            map.RegisterRoute("addressmatching", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Administration.Import.ImportData");
            map.RegisterRoute("processingdata", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Administration.Import.ProcessingData");

            map.RegisterRoute("housingokilist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Housing.Oki.OkiList");
            map.RegisterRoute("housingskilist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Housing.Ski.SkiList");

            map.RegisterRoute("energysavingprogramlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Housing.EnergySaving.ProgramList");
            map.RegisterRoute("energysavinghouseprogramlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Housing.EnergySaving.HouseProgramList");
            map.RegisterRoute("energysavingescontractlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Housing.EnergySaving.EsContractList");

            map.RegisterRoute("personlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Contragent.SocialSupport.PersonList");

            map.RegisterRoute("dictokeilist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Common.Okei");
            map.RegisterRoute("dictnpaactthemelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Common.NpaActTheme");

            map.RegisterRoute("dictelectrostationtypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.ElectroStationType");
            map.RegisterRoute("dictgaspressurelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.GasPressure");
            map.RegisterRoute("dictcommunalservicelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.CommunalService");
            map.RegisterRoute("dictwaterintaketypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.WaterIntakeType");
            map.RegisterRoute("dictgasnettypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.GasNetType");
            map.RegisterRoute("dictelectrosubstantiontypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "GisRis.Dict.Oki.ElectroSubstantionType");
            map.RegisterRoute("dictrunbaselist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.RunBase");
            map.RegisterRoute("dictfueltypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.FuelType");
            map.RegisterRoute("dictheattypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.HeatType");
            map.RegisterRoute("dictvoltagelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.Voltage");
            map.RegisterRoute("dictskitypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.SkiType");
            map.RegisterRoute("dictokitypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.OkiType");
            map.RegisterRoute("dictcommunalsourcelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.Oki.CommunalSource");

            map.RegisterRoute("dictcontractconclusionlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.ContractConclusion");
            map.RegisterRoute("dicthouseservicelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.HouseService");
            map.RegisterRoute("dictbenefitcategorylist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.BenefitCategory");
            map.RegisterRoute("dictworktypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.WorkType");
            map.RegisterRoute("dictpremisecategorylist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.PremiseCategory");
            map.RegisterRoute("dicthousetypelist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Dict.House.HouseType");

            map.RegisterRoute("documentsnpaact", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.Documents.NpaAct");
            map.RegisterRoute("benefitlist", "B4.controller.integrations.ExternalRis", requiredPermission: "Ris.SocialSupport.SocialSupport.BenefitList");

            map.AddRoute(new ClientRoute("contragentlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("managementorganizationlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("stategovlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("localgovlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("rsocompanylist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("rekcompanylist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("sroorglist", "B4.controller.sro.SroOrgList"));

            map.AddRoute(new ClientRoute("housingokilist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("housingskilist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("personalaccountlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("energysavingprogramlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("energysavinghouseprogramlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("energysavingescontractlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("ownrightlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("informationmessagelist", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("dictnsipostlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictnsimoterritorylist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictokeilist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictnpaactthemelist", "B4.controller.integrations.ExternalRis"));

            //Жилищный фонд
            map.AddRoute(new ClientRoute("houselist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictcontractconclusionlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dicthouseservicelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictbenefitcategorylist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictworktypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictcommunalsourcelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictburdentypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictnonlivepremiselocationlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictpremisecategorylist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictpremisepurposelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictlivepremisekindlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dicthousetypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("form22gkh", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("dictgaspressurelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictelectrostationtypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictcommunalservicelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictwaterintaketypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictgasnettypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictelectrosubstantiontypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictrunbaselist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictfueltypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictheattypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictvoltagelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictskitypelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("dictokitypelist", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("importdatakernel", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("addressmatching", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("servicematching", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("datasupplier", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("exportdatakernel", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("disqualifiedpersons", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("qualificationcertificate", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("informationmessagelist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("skimodernizationlist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("housingtargerslist", "B4.controller.integrations.ExternalRis"));
            map.AddRoute(new ClientRoute("appeallist", "B4.controller.integrations.ExternalRis"));
        }
    }
}