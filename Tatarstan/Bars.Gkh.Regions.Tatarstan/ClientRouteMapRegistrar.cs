namespace Bars.Gkh.Regions.Tatarstan
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("roletypehousepermission", "B4.controller.RoleTypeHousePermission", requiredPermission: "Gkh.RealityObject.RoleTypeHousePermission.View"));
            map.AddRoute(new ClientRoute("constructionobject", "B4.controller.ConstructionObject", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/", "B4.controller.constructionobject.Navi"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/edit/", "B4.controller.constructionobject.Edit", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/photos/", "B4.controller.constructionobject.Photo", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/typework/", "B4.controller.constructionobject.TypeWork", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/documents/", "B4.controller.constructionobject.Document", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/contracts/", "B4.controller.constructionobject.Contract", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/participants/", "B4.controller.constructionobject.Participant", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.View"));
            map.AddRoute(new ClientRoute("periodnormconsumption", "B4.controller.dict.PeriodNormConsumption", requiredPermission: "Gkh.Dictionaries.PeriodNormConsumption.View"));

            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/smr/", "B4.controller.constructionobject.Smr"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/smr/schedule/", "B4.controller.constructionobject.smr.Schedule", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/smr/progress/", "B4.controller.constructionobject.smr.Progress", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.View"));
            map.AddRoute(new ClientRoute("constructionobjectedit/{id}/smr/workers/", "B4.controller.constructionobject.smr.Workers", requiredPermission: "Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.View"));
            map.AddRoute(new ClientRoute("constructionobjectmasschangestate", "B4.controller.ConstructionObjectMassChangeState"));

            map.AddRoute(new ClientRoute("planpaymentspercentage", "B4.controller.PlanPaymentsPercentage", requiredPermission: "Gkh.Dictionaries.PlanPaymentsPercentage.View"));

            map.AddRoute(new ClientRoute("utilitydebtorclaimwork", "B4.controller.utilityclaimwork.UtilityDebtor", requiredPermission: "Clw.ClaimWork.UtilityDebtor.View"));
           
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/utilitydebtoredit", "B4.controller.utilityclaimwork.EditUtilityDebtor", requiredPermission: "Clw.ClaimWork.UtilityDebtor.Debt.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/execprocess", "B4.controller.utilityclaimwork.ExecutoryProcess", requiredPermission: "Clw.ClaimWork.UtilityDebtor.ExecutoryProcess.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/propseizure", "B4.controller.utilityclaimwork.SeizureOfProperty", requiredPermission: "Clw.ClaimWork.UtilityDebtor.SeizureOfProperty.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/departrestrict", "B4.controller.utilityclaimwork.DepartureRestriction", requiredPermission: "Clw.ClaimWork.UtilityDebtor.DepartureRestriction.View"));
            
            map.AddRoute(new ClientRoute("normconsumption", "B4.controller.NormConsumption", requiredPermission: "Gkh.NormConsumption.View"));
            map.AddRoute(new ClientRoute("normconscoldwater/{id}/", "B4.controller.normconsumption.NormConsumptionColdWater"));
            map.AddRoute(new ClientRoute("normconshotwater/{id}/", "B4.controller.normconsumption.NormConsumptionHotWater"));
            map.AddRoute(new ClientRoute("normconsfiring/{id}/", "B4.controller.normconsumption.NormConsumptionFiring"));
            map.AddRoute(new ClientRoute("normconsheating/{id}/", "B4.controller.normconsumption.NormConsumptionHeating"));

            map.AddRoute(new ClientRoute("chargessplitting", "B4.controller.chargessplitting.ChargesSplitting", requiredPermission: "Gkh.ChargesSplitting.View"));
            map.AddRoute(new ClientRoute("contractperiodsumm_detail/{id}/", "B4.controller.chargessplitting.contrpersumm.ContractPeriodSummDetail"));
            map.AddRoute(new ClientRoute("contractperiods", "B4.controller.chargessplitting.contrpersumm.ContractPeriod", requiredPermission: "Gkh.ContractPeriodSumm.Period_View"));
            map.AddRoute(new ClientRoute("fuelenergyresource_detail/{id}/", "B4.controller.chargessplitting.fuelenergyresrc.FuelEnergyResourceDetail"));

            map.AddRoute(new ClientRoute("gasequipmentorg", "B4.controller.GasEquipmentOrg"));
            map.AddRoute(new ClientRoute("gasequipmentorgedit/{id}", "B4.controller.gasequipmentorg.Navigation"));
            map.AddRoute(new ClientRoute("gasequipmentorgedit/{id}/edit", "B4.controller.gasequipmentorg.Edit"));
            map.AddRoute(new ClientRoute("gasequipmentorgedit/{id}/contract", "B4.controller.gasequipmentorg.Contract"));

            map.AddRoute(new ClientRoute("efratingperiod", "B4.controller.dict.EfficiencyRatingPeriod", requiredPermission: "Gkh.Dictionaries.EfficiencyRating.Period.View"));
            map.AddRoute(new ClientRoute(
                "efficiencyratingconstructor",
                "B4.controller.efficiencyrating.Constructor",
                requiredPermission: "Gkh.Orgs.EfficiencyRating.Constructor.View"));

            map.AddRoute(new ClientRoute(
                "efmanagingorganization",
                "B4.controller.efficiencyrating.ManagingOrganization",
                requiredPermission: "Gkh.Orgs.EfficiencyRating.ManagingOrganization.View"));

            map.AddRoute(new ClientRoute(
                "efmanorg_rating/{id}",
                "B4.controller.efficiencyrating.Rating",
                requiredPermission: "Gkh.Orgs.EfficiencyRating.ManagingOrganization.View"));

            map.AddRoute(new ClientRoute("efanalitics", "B4.controller.efficiencyrating.Analitics", requiredPermission: "Gkh.Orgs.EfficiencyRating.Analitics.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/infoOverview",
                "B4.controller.realityobj.housingcommunalservice.InfoOverview",
                requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.InfoOverview.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/HouseAccount",
                "B4.controller.realityobj.housingcommunalservice.Account",
                requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.Account.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/meteringdevicevalue",
                "B4.controller.realityobj.housingcommunalservice.MeteringDeviceValue",
                requiredPermission: "Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/apartinfo",
                "B4.controller.realityobj.ApartInfo",
                requiredPermission: "Gkh.RealityObject.Register.ApartInfo.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/intercom", "B4.controller.realityobj.Intercom", requiredPermission: "Gkh.RealityObject.Register.Intercom.View"));

            map.AddRoute(new ClientRoute("egsointegration",
                "B4.controller.egsointegration.EgsoIntegration",
                requiredPermission: "Administration.OutsideSystemIntegrations.EgsoIntegration.View"));

            map.AddRoute(new ClientRoute("realityobjectoutdooredit/{id}/image", "B4.controller.outdoor.Image", requiredPermission: "Gkh.RealityObjectOutdoor.Register.Image.View"));
            map.AddRoute(new ClientRoute("realityobjectoutdooredit/{id}/element", "B4.controller.outdoor.Element", requiredPermission: "Gkh.RealityObjectOutdoor.Register.Element.View"));
            
            map.AddRoute(new ClientRoute("courtordergku", "B4.controller.fssp.CourtOrderGkuPanel", requiredPermission: "Clw.Fssp.CourtOrderGku.View"));
            map.AddRoute(new ClientRoute("paymentgku", "B4.controller.fssp.PaymentGku", requiredPermission: "Clw.Fssp.PaymentGku.View"));

            map.AddRoute(new ClientRoute("tariffdataintegrationlog", "B4.controller.administration.TariffDataIntegrationLog", requiredPermission: "Administration.TariffDataIntegrationLog.View"));
            map.AddRoute(new ClientRoute("tasks", "B4.controller.tasks.TaskEntry"));
        }
    }
}