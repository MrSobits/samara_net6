namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    using Interface;

    /// <summary>
    /// Сборщик данных по жилома дому
    /// </summary>
    public partial class RobjectDataCollector : DataCollectorBase, IRobjectDataCollector, IRobject988DataCollector
    {
        private readonly ISyncService syncService;
        private readonly IClassMerger merger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncService">Сервис синхронизации</param>
        /// <param name="merger">Склеиватель инстансов</param>
        public RobjectDataCollector(IWindsorContainer container, ISyncService syncService, IClassMerger merger)
            : base(container)
        {
            this.syncService = syncService;
            this.merger = merger;
        }

        private void FillManagementContract(HouseProfileData result, RealityObject robject)
        {
            var service = this.Container.Resolve<IRobjectService>();
            var refService = this.Container.ResolveDomain<RefRealityObject>();
            try
            {
                var currentInn = refService.GetAll().Where(x => x.RealityObject.Id == robject.Id).Select(x => x.RefManagingOrganization.Inn).FirstOrDefault();
                if (string.IsNullOrEmpty(currentInn))
                {
                    return;
                }

                var management = service.GetManagingHistory(robject.Id).LastOrDefault(x => x.ManOrgInn == currentInn);
                if (management == null)
                {
                    return;
                }

                result.management_contract = new ManagementContract
                                                 {
                                                     date_start = management.DateStart ?? (management.DateEnd ?? DateTime.Now).AddDays(-2),
                                                     plan_date_stop = management.PlannedDateEnd,
                                                     notice = management.Note
                                                 };
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(refService);
            }
        }

        private void FillDisclosureDependent(HouseProfileData result, RealityObject robject, PeriodDi period)
        {
            var diRobjectService = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            try
            {
                var diRobject = diRobjectService.GetAll().FirstOrDefault(x => x.RealityObject.Id == robject.Id && x.PeriodDi.Id == period.Id);
                if (diRobject == null)
                {
                    return;
                }

                this.FillProviders(result, diRobject);
                this.FillFinances(result, diRobject);
            }
            finally
            {
                this.Container.Release(diRobjectService);
            }
        }

        private void FillProviders(HouseProfileData result, DisclosureInfoRealityObj diRobject)
        {
            var serviceDomain = this.Container.ResolveDomain<CommunalService>();
            try
            {
                var services = serviceDomain.GetAll().Where(x => x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal && x.DisclosureInfoRealityObj.Id == diRobject.Id).ToArray();
                foreach (var service in services)
                {
                    if (service.TemplateService.Name.Equals("Отопление", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.heating_provider = new Provider();
                        this.FillProvider(result.heating_provider, service);
                    }
                    else if (service.TemplateService.Name.Equals("Электроснабжение", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.electricity_provider = new Provider();
                        this.FillProvider(result.electricity_provider, service);
                    }
                    else if (service.TemplateService.Name.Equals("Газоснабжение", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.gas_provider = new Provider();
                        this.FillProvider(result.gas_provider, service);
                    }
                    else if (service.TemplateService.Name.Equals("Горячее водоснабжение", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.hot_water_provider = new Provider();
                        this.FillProvider(result.hot_water_provider, service);
                    }
                    else if (service.TemplateService.Name.Equals("Холодное водоснабжение", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.cold_water_provider = new Provider();
                        this.FillProvider(result.cold_water_provider, service);
                    }
                    else if (service.TemplateService.Name.Equals("Водоотведение", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.drainage_provider = new Provider();
                        this.FillProvider(result.drainage_provider, service);
                    }
                }
            }
            finally
            {
                this.Container.Release(serviceDomain);
            }
        }

        private void FillProvider(Provider provider, CommunalService service)
        {
            if (service.Provider != null)
            {
                provider.inn = service.Provider.Inn;
                provider.alias = service.Provider.Name;
            }

            provider.supplied_via_management_organization = service.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo;
            provider.is_supported = service.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable;
        }

        private void FillFinances(HouseProfileData result, DisclosureInfoRealityObj diRobject)
        {
            var finance = new Finance();
            result.finance = finance;

            finance.payment_claims_compensation = diRobject.ClaimCompensationDamage;
            finance.payment_claims_refusal = diRobject.ClaimReductionPaymentNonService;
            finance.payment_claims_not_delivered = diRobject.ClaimReductionPaymentNonDelivery;
            finance.repair_work = diRobject.WorkRepair;
            finance.beautification_work = diRobject.WorkLandscaping;
            finance.raised_funds_subsidies = diRobject.Subsidies;
            finance.raised_funds_credits = diRobject.Credit;
            finance.raised_funds_leasing = diRobject.FinanceLeasingContract;
            finance.raised_funds_energy_service = diRobject.FinanceEnergServContract;
            finance.raised_funds_contributions = diRobject.OccupantContribution;
            finance.raised_funds_others = diRobject.OtherSource;

            this.FillFinanceByServices(finance, diRobject);
        }

        private void FillFinanceByServices(Finance finance, DisclosureInfoRealityObj diRobject)
        {
            var service = this.Container.ResolveDomain<FinActivityRealityObjCommunalService>();
            try
            {
                var communalServices = service.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == diRobject.Id).ToArray();
                foreach (var communalService in communalServices)
                {
                    switch (communalService.TypeServiceDi)
                    {
                        case TypeServiceDi.Heating:
                            finance.debt_owners_heating = communalService.DebtOwner;
                            finance.paid_services_heating = communalService.PaidByIndicator;
                            finance.paid_resources_heating = communalService.PaidByAccount;
                            break;
                        case TypeServiceDi.Electrical:
                            finance.debt_owners_electricity = communalService.DebtOwner;
                            finance.paid_services_electricity = communalService.PaidByIndicator;
                            finance.paid_resources_electricity = communalService.PaidByAccount;
                            break;
                        case TypeServiceDi.Gas:
                            finance.debt_owners_gaz = communalService.DebtOwner;
                            finance.paid_services_gaz = communalService.PaidByIndicator;
                            finance.paid_resources_gaz = communalService.PaidByAccount;
                            break;
                        case TypeServiceDi.HotWater:
                            finance.debt_owners_hot_water = communalService.DebtOwner;
                            finance.paid_services_hot_water = communalService.PaidByIndicator;
                            finance.paid_resources_hot_water = communalService.PaidByAccount;
                            break;
                        case TypeServiceDi.ColdWater:
                            finance.debt_owners_cold_water = communalService.DebtOwner;
                            finance.paid_services_cold_water = communalService.PaidByIndicator;
                            finance.paid_resources_cold_water = communalService.PaidByAccount;
                            break;
                        case TypeServiceDi.Wastewater:
                            finance.debt_owners_wastewater = communalService.DebtOwner;
                            break;
                    }
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}