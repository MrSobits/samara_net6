namespace Bars.GkhDi.Report.Fucking731
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.GkhDi.DomainService;

    using Entities;
    using System.Collections.Generic;
    using Enums;
    using Gkh.Utils;

    public partial class DisclosureInfo731
    {
        #region Cache

        protected Dictionary<long, decimal?> DictTariffForCitizens;

        protected Dictionary<long, TariffForRsoProxy> DictTariffForRso;

        protected Dictionary<long, ObjectHousingServiceItem[]> DictCostItem;

        protected Dictionary<long, ObjectManagService[]> DictManagService;

        protected Dictionary<long, ObjectRepairServicePpr[]> DictServicePprProxy;

        protected Dictionary<long, ObjectRepairProvider[]> DictRepairProvider;

        protected Dictionary<long, ObjectHousingProvider[]> DictHousingProvider;

        protected Dictionary<long, ObjectAdditionalProvider[]> DictAdditionalProvider;

        protected Dictionary<long, ObjectCommunalService[]> DictCommunalService;

        protected Dictionary<long, ObjectCommonFacility[]> DictCommonFacilities;

        protected Dictionary<long, ObjectCommunalResource[]> DictCommunalResource;

        protected Dictionary<long, ObjectReductionWork[]> DictReductionWork;

        protected Dictionary<long, ObjectReductionPayment[]> DictReductionPayment;

        #endregion Cache

        protected void WarmCache(IQueryable<Robject> roFilterQuery)
        {
            DictTariffForCitizens = Container.ResolveDomain<TariffForConsumers>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.BaseService.DisclosureInfoRealityObj.Id))
                //.Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == dinfo.PeriodDi.Id)
                .OrderByDescending(x => x.DateStart)
                .Select(x => new
                {
                    x.BaseService.Id,
                    x.Cost
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.Select(x => x.Cost).First());

            DictTariffForRso = Container.ResolveDomain<TariffForRso>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.BaseService.DisclosureInfoRealityObj.Id))
                .OrderByDescending(x => x.DateStart)
                .Select(x => new TariffForRsoProxy
                {
                    BaseServiceId = x.BaseService.Id,
                    Cost = x.Cost,
                    NumberAct = x.NumberNormativeLegalAct,
                    DateAct = x.DateNormativeLegalAct,
                    Org = x.OrganizationSetTariff
                })
                .AsEnumerable()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            DictCostItem = Container.ResolveDomain<CostItem>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    BaseServiceId = x.BaseService.Id,
                    x.Sum,
                    x.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(x => x.Key, z => z
                    .Select(x => new ObjectHousingServiceItem
                    {
                        Name = x.Name,
                        Sum = x.Sum ?? 0
                    })
                    .ToArray());

            DictManagService = Container.ResolveDomain<ControlService>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    x.Id,
                    x.TemplateService.Name,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .ToList()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new ObjectManagService
                    {
                        Name = x.Name,
                        Tariff = DictTariffForCitizens.Get(x.Id) ?? 0,
                        UnitMeasure = x.UnitMeasure
                    })
                    .SetNumber()
                    .ToArray());

            // Реализовал получение детализаций по ППР через сервис, т.к. в Татарстане есть доп. поля, а регионах пока нет
            // Отнаследовать и подменить реализацию отчета не позволяет контейнер, т.к. регистрация именована (Named)
            var dictServicePprDetail = Container.Resolve<IWorkRepairDetailService>().GetRepairDetailsDict(roFilterQuery.Select(x => x.DiRoId)); 

            DictServicePprProxy = Container.ResolveDomain<PlanWorkServiceRepairWorks>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.PlanWorkServiceRepair.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    BaseServiceId = x.PlanWorkServiceRepair.BaseService.Id,
                    Sheduled = x.PlanWorkServiceRepair.BaseService.ScheduledPreventiveMaintanance,
                    Work = x.WorkRepairList.GroupWorkPpr.Name,
                    GroupWorkPprId = (long?)x.WorkRepairList.GroupWorkPpr.Id,
                    x.Cost,
                    x.FactCost,
                    x.DateStart,
                    x.DateEnd,
                    x.ReasonRejection,
                    Periodicity = x.PeriodicityTemplateService.Name
                })
                .ToList()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x =>
                    {
                        var objectRepairServiceDetail = new ObjectRepairServiceDetail[0];

                        var dict = dictServicePprDetail.Get(x.BaseServiceId);
                        if (x.GroupWorkPprId.HasValue && dict != null && dict.ContainsKey(x.GroupWorkPprId.Value))
                        {
                            objectRepairServiceDetail = dict[x.GroupWorkPprId.Value].SetNumber().ToArray();
                        }
                        
                        return new ObjectRepairServicePpr
                            {
                                Cost = x.Cost ?? 0,
                                FactCost = x.FactCost ?? 0,
                                DateEnd = x.DateEnd.ToDateString(),
                                DateStart = x.DateStart.ToDateString(),
                                ReasonRejection = x.ReasonRejection,
                                Work = x.Work,
                                ObjectRepairServiceDetail = objectRepairServiceDetail
                            };
                    })
                    .SetNumber()
                    .ToArray());

            var dictRepairWorkTo = Container.ResolveDomain<WorkRepairTechServ>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    BaseServiceId = x.BaseService.Id,
                    x.WorkTo.Name,
                })
                .AsEnumerable()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(x => x.Key, z => z
                    .Select(x => new ObjectRepairServiceTo
                    {
                        Name = x.Name
                    })
                    .SetNumber()
                    .ToArray());

            DictRepairProvider = Container.ResolveDomain<RepairService>().GetAll()
                .Where(x => roFilterQuery.Any(y => y.DiRoId == x.DisclosureInfoRealityObj.Id))
                //.Where(x => x.Id != null)
                .Where(x => x.Provider != null)
                .Select(x => new
                {
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    ProviderId = x.Provider.Id,
                    ProviderName = x.Provider.Name,
                    x.Id,
                    x.TemplateService.Name,
                    x.TypeOfProvisionService,
                    UnitMeasure = x.UnitMeasure.Name,
                    x.ScheduledPreventiveMaintanance
                })
                .ToList()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(x => new ProviderProxy { Id = x.ProviderId, Name = x.ProviderName })
                    .Select(z => new ObjectRepairProvider
                    {
                        Provider = z.Key.Name,
                        ObjectRepairServices = z
                            .Select(x =>
                            {
                                var rec = new ObjectRepairService
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Type = x.TypeOfProvisionService.GetEnumMeta().Display,
                                    ScheduledPreventiveMaintanance = x.ScheduledPreventiveMaintanance.GetEnumMeta().Display,
                                    ObjectRepairServicePpr = DictServicePprProxy.Get(x.Id) ?? new ObjectRepairServicePpr[0],
                                    ObjectRepairServiceTo = dictRepairWorkTo.Get(x.Id) ?? new ObjectRepairServiceTo[0]
                                };

                                if (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                                {
                                    rec.Tariff = "-";
                                    rec.UnitMeasure = "-";
                                }
                                else
                                {
                                    rec.Tariff = FormatDecimal(DictTariffForCitizens.Get(x.Id));
                                    rec.UnitMeasure = x.UnitMeasure;
                                }

                                return rec;
                            })
                            .SetNumber()
                            .ToArray()
                    })
                    .ToArray());

            DictHousingProvider = Container.ResolveDomain<HousingService>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Where(x => x.Provider != null)
                .Select(x => new
                {
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    x.Id,
                    ProviderId = x.Provider.Id,
                    ProviderName = x.Provider.Name,
                    x.TemplateService.Name,
                    UnitMeasure = x.UnitMeasure.Name,
                    Periodicity = x.Periodicity.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(x => new ProviderProxy { Id = x.ProviderId, Name = x.ProviderName })
                    .Select(z => new ObjectHousingProvider
                    {
                        Provider = z.Key.Name,
                        ObjectHousingService = z.Select(x => new ObjectHousingService
                        {
                            Name = x.Name,
                            Periodicity = x.Periodicity,
                            UnitMeasure = x.UnitMeasure,
                            Tariff = DictTariffForCitizens.Get(x.Id) ?? 0,
                            ObjectHousingServiceItem = DictCostItem.Get(x.Id).SetNumber().ToArray()
                        })
                        .SetNumber()
                        .ToArray()
                    })
                    .ToArray());

            DictAdditionalProvider = Container.ResolveDomain<AdditionalService>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Where(x => x.Provider != null)
                .Select(x => new
                {
                    x.Id,
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    ProviderId = x.Provider.Id,
                    ProviderName = x.Provider.Name,
                    x.TemplateService.Name,
                    Periodicity = x.Periodicity.Name,
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(x => new ProviderProxy { Id = x.ProviderId, Name = x.ProviderName })
                    .Select(z => new ObjectAdditionalProvider
                    {
                        Provider = z.Key.Name,
                        ObjectAdditionalService = y
                            .Select(x => new ObjectAdditionalService
                            {
                                Name = x.Name,
                                Periodicity = x.Periodicity,
                                Tariff = DictTariffForCitizens.Get(x.Id) ?? 0
                            })
                            .SetNumber()
                            .ToArray()
                    })
                    .ToArray());

            DictCommunalService = Container.ResolveDomain<CommunalService>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Where(x => x.Provider != null)
                .Select(x => new
                {
                    x.Id,
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    Provider = x.Provider.Name,
                    x.TemplateService.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new ObjectCommunalService
                    {
                        Name = x.Name,
                        Provider = x.Provider,
                        Tariff = DictTariffForCitizens.Get(x.Id) ?? 0
                    })
                    .SetNumber()
                    .ToArray());

            DictCommonFacilities = Container.ResolveDomain<InfoAboutUseCommonFacilities>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    x.KindCommomFacilities,
                    x.Lessee,
                    x.DateStart,
                    x.DateEnd,
                    x.CostContract,
                    x.Number,
                    x.From,
                    x.TypeContract
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new ObjectCommonFacility
                    {
                        KindCommonFacilities = x.KindCommomFacilities,
                        Lessee = x.Lessee,
                        ContractPeriod =
                            string.Format("{0} - {1}", x.DateStart.ToDateString(), x.DateEnd.ToDateString()),
                        CostContract = x.CostContract ?? 0,
                        ProtocolDate = x.From.ToDateString(),
                        ProtocolNumber = x.Number,
                        TypeContract = x.TypeContract.GetEnumMeta().Display
                    })
                    .SetNumber()
                    .ToArray());

            DictCommunalResource = Container.ResolveDomain<CommunalService>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
#warning не убирать проверку на null
                .Where(x => x.Id != null)
                .Select(x => new
                {
                    x.Id,
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    x.TemplateService.Name,
                    Provider = x.Provider.Name,
                    Volume = x.VolumePurchasedResources
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x =>
                    {
                        var rec = new ObjectCommunalResource
                        {
                            Name = x.Name,
                            Provider = x.Provider,
                            Volume = x.Volume ?? 0,
                            TariffCitizens = DictTariffForCitizens.Get(x.Id) ?? 0
                        };

                        var tariffRso = DictTariffForRso.Get(x.Id);

                        if (tariffRso != null)
                        {
                            rec.TariffRso = tariffRso.Cost ?? 0;

                            if (tariffRso.Org != null && "государственый комитет республики татарстан по тарифам".Equals(tariffRso.Org.ToLower()))
                            {
                                rec.TariffRt = tariffRso.Cost ?? 0;
                                rec.Decision =
                                    string.Format("№{0} от {1}",
                                        tariffRso.NumberAct,
                                        tariffRso.DateAct.ToDateString());
                            }
                        }

                        return rec;
                    })
                    .SetNumber()
                    .ToArray());

            WarmReductionWork(roFilterQuery);

            WarmReductionPayment(roFilterQuery);
        }

        protected void WarmReductionWork(IQueryable<Robject> roFilterQuery)
        {
            DictReductionWork = Container.ResolveDomain<PlanReductionExpenseWorks>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.PlanReductionExpense.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    DiRoId = x.PlanReductionExpense.DisclosureInfoRealityObj.Id,
                    x.PlanReductionExpense.BaseService.TemplateService.Name,
                    PlanName = x.Name,
                    x.DateComplete,
                    FactedReductionExpense = x.FactedReductionExpense ?? 0,
                    PlannedReductionExpense = x.PlannedReductionExpense ?? 0,
                    x.ReasonRejection
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new ObjectReductionWork
                    {
                        Name = x.Name,
                        PlanName = x.PlanName,
                        DateComplete = x.DateComplete.ToDateString(),
                        FactedReductionExpense = x.FactedReductionExpense,
                        PlannedReductionExpense = x.PlannedReductionExpense,
                        ReasonRejection = x.ReasonRejection
                    })
                    .SetNumber()
                    .ToArray());
        }

        protected void WarmReductionPayment(IQueryable<Robject> roFilterQuery)
        {
            DictReductionPayment = Container.ResolveDomain<InfoAboutReductionPayment>().GetAll()
                .Where(y => roFilterQuery.Any(x => x.DiRoId == y.DisclosureInfoRealityObj.Id))
                .Select(x => new
                {
                    DiRoId = x.DisclosureInfoRealityObj.Id,
                    x.BaseService.TemplateService.Name,
                    x.OrderDate,
                    x.RecalculationSum,
                    UnitMeasure = x.BaseService.UnitMeasure.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.DiRoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new ObjectReductionPayment
                    {
                        Name = x.Name,
                        OrderDate = x.OrderDate.ToDateString(),
                        RecalculationSum = x.RecalculationSum ?? 0,
                        UnitMeasure = x.UnitMeasure
                    })
                    .SetNumber()
                    .ToArray());
        }
    }
}