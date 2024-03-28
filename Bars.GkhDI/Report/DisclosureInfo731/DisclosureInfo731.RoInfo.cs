namespace Bars.GkhDi.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.Modules.Reports;

    using DomainService;
    using Entities;
    using Enums;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.Enums;

    public partial class DisclosureInfo731
    {
        private Dictionary<long, decimal?> _dictTariffForCitizens;

        private Dictionary<long, TariffForRsoProxy> _dictTariffForRso;

        private Dictionary<long, Dictionary<long, List<RepairWorkDetailProxy>>> _dictPprDetail;

        private Dictionary<long, Dictionary<long, IEnumerable<CostItem>>> _dictCostItem;

        private void FillRobjects(ReportParams reportParams, DisclosureInfo dinfo)
        {
            var sectionRobject = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRobject");

            var manorgRoService = Container.Resolve<IManagingOrgRealityObjectService>();

            _dictTariffForCitizens = Container.ResolveDomain<TariffForConsumers>().GetAll()
                .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == dinfo.PeriodDi.Id)
                .OrderByDescending(x => x.DateStart)
                .Select(x => new
                {
                    x.BaseService.Id,
                    x.Cost
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.Select(x => x.Cost).First());

            _dictTariffForRso = Container.ResolveDomain<TariffForRso>().GetAll()
                .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == dinfo.PeriodDi.Id)
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

            _dictCostItem = Container.ResolveDomain<CostItem>().GetAll()
                .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == dinfo.PeriodDi.Id)
                .Select(x => new
                {
                    x.BaseService.DisclosureInfoRealityObj.Id,
                    BaseServiceId = x.BaseService.Id,
                    x.Sum,
                    x.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y
                        .GroupBy(x => x.BaseServiceId)
                        .ToDictionary(x => x.Key, z => z.Select(x => new CostItem
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Sum = x.Sum
                        })));

            try
            {
                var filterRo =
                    manorgRoService
                        .GetAllActive(dinfo.PeriodDi.DateStart.Value, dinfo.PeriodDi.DateEnd)
                        .Where(x => x.ManOrgContract.ManagingOrganization.Id == dinfo.ManagingOrganization.Id)
                        .Select(x => x.RealityObject);

                var diRobjects = Container.ResolveDomain<DisclosureInfoRealityObj>().GetAll()
                    .Where(x => x.PeriodDi.Id == dinfo.PeriodDi.Id)
                    .Where(y => filterRo.Any(x => x.Id == y.RealityObject.Id));

                this._dictPprDetail = Container.ResolveDomain<WorkRepairDetail>().GetAll()
                    .Where(x => diRobjects.Any(y => y.RealityObject.Id == x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .Select(x => new RepairWorkDetailProxy
                    {
                        RealityObjectId = x.BaseService.DisclosureInfoRealityObj.RealityObject.Id,
                        BaseServiceId = x.BaseService.Id,
                        Name = x.WorkPpr.Name,
                        UnitMeasure = x.WorkPpr.Measure.Name,
                        GroupWorkPprId = (long?)x.WorkPpr.GroupWorkPpr.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.GroupBy(x => x.BaseServiceId).ToDictionary(z => z.Key, x => x.ToList()));

                foreach (var obj in diRobjects.OrderBy(x => x.RealityObject.Address).Select(x => new DisclosureInfoRealityObj { Id = x.Id, RealityObject = new RealityObject { Address = x.RealityObject.Address, Id = x.RealityObject.Id } }))
                {
                    sectionRobject.ДобавитьСтроку();

                    sectionRobject["Address"] = obj.RealityObject.Address;

                    FillObjectManagService(sectionRobject, obj);

                    FillObjectRepairService(sectionRobject, obj);

                    FillObjectHousingService(sectionRobject, obj);

                    FillObjectAdditionalService(sectionRobject, obj);

                    FillObjectCommunalService(sectionRobject, obj);

                    FillObjectCommonFacilities(sectionRobject, obj);

                    FillObjectCommunalResources(sectionRobject, obj);

                    FillObjectReductionWorks(sectionRobject, obj);

                    FillObjectReductionPayment(sectionRobject, obj);
                }
            }
            finally
            {
                Container.Release(manorgRoService);
            }
        }

        private void FillObjectManagService(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<ControlService>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new
                    {
						x.Id,
                        x.TemplateService.Name,
                        UnitMeasure = x.UnitMeasure.Name
                    });

                var section = parentSection.ДобавитьСекцию("sectionObjectManagService");

                var i = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Name"] = item.Name;
                    section["Tariff"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));
                    section["UnitMeasure"] = item.UnitMeasure;
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillObjectRepairService(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<RepairService>();
            var domainPpr = Container.ResolveDomain<PlanWorkServiceRepairWorks>();
            var domainWorkRepairTechServ = Container.ResolveDomain<WorkRepairTechServ>(); 
            
            try
            {
                var dataPpr = domainPpr.GetAll()
                    .Where(x => x.PlanWorkServiceRepair.BaseService.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new ServicePprProxy
                    {
                        BaseServiceId = x.PlanWorkServiceRepair.BaseService.Id,
                        Sheduled = x.PlanWorkServiceRepair.BaseService.ScheduledPreventiveMaintanance,
                        Work = x.WorkRepairList.GroupWorkPpr.Name,
                        GroupWorkPprId = (long?)x.WorkRepairList.GroupWorkPpr.Id,
                        PlanCost = x.Cost,
                        FactCost = x.FactCost,
                        DateStart = x.DateStart,
                        DateEnd = x.DateEnd,
                        ReasonRejection = x.ReasonRejection,
                        Periodicity = x.PeriodicityTemplateService.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                var dataTo = domainWorkRepairTechServ.GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new RepairWorkTo
                    {
                        BaseServiceId = x.BaseService.Id,
                        Name = x.WorkTo.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                var dataDetail = this._dictPprDetail.ContainsKey(diRo.RealityObject.Id)
                    ? this._dictPprDetail[diRo.RealityObject.Id]
                    : new Dictionary<long, List<RepairWorkDetailProxy>>();

                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Where(x => x.Provider != null)
                    .Select(x => new 
                    {
                        Provider = new
                        {
                            x.Provider.Id,
                            x.Provider.Name
                        },
                        x.Id,
                        x.TemplateService.Name,
                        x.TypeOfProvisionService,
                        UnitMeasure = x.UnitMeasure.Name,
                        x.ScheduledPreventiveMaintanance
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Provider);
                    
                var section = parentSection.ДобавитьСекцию("sectionObjectRepairProvider");

                foreach (var provider in data)
                {
                    section.ДобавитьСтроку();
                    section["Provider"] = provider.Key.Name;

                    var sectionRepair = section.ДобавитьСекцию("sectionObjectRepairService");
                    var sectionRepairPpr = sectionRepair.ДобавитьСекцию("sectionObjectRepairServicePpr");
                    var sectionRepairDetail = sectionRepairPpr.ДобавитьСекцию("sectionObjectRepairServiceDetail");
                    var sectionRepairTo = sectionRepair.ДобавитьСекцию("sectionObjectRepairServiceTo");

					var i = 0;

                    foreach (var item in provider)
                    {
                        sectionRepair.ДобавитьСтроку();

                        sectionRepair["Number"] = ++i;
                        sectionRepair["Name"] = item.Name;
                        sectionRepair["Type"] = item.TypeOfProvisionService.GetEnumMeta().Display;

                        if (item.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable)
                        {
                            sectionRepair["UnitMeasure"] = "-";
                            sectionRepair["Tariff"] = "-";
                        }
                        else
                        {
                            sectionRepair["UnitMeasure"] = item.UnitMeasure;
                            sectionRepair["Tariff"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));
                        }

                        if (item.ScheduledPreventiveMaintanance == YesNoNotSet.Yes)
                        {
                            FillObjectRepairPpr(sectionRepairPpr, sectionRepairDetail, dataPpr.Get(item.Id), dataDetail.Get(item.Id));
                        }
                        else 
                        {
                            sectionRepair["ScheduledPreventiveMaintanance"] = ": " + item.ScheduledPreventiveMaintanance.GetEnumMeta().Display;
                        }
                        
                        FillObjectRepairTo(sectionRepairTo, dataTo.Get(item.Id));
                    }
                }
            }
            finally
            {
                Container.Release(domain);
                Container.Release(domainWorkRepairTechServ);
            }
        }

        private void FillObjectRepairPpr(Section section, Section sectionDetail, IEnumerable<ServicePprProxy> data, IEnumerable<RepairWorkDetailProxy> dataDetail)
        {
            if (data != null)
            {
                var i = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Work"] = item.Work; 
                    section["Periodicity"] = item.Periodicity;
                    section["Cost"] = FormatDecimal(item.PlanCost);
                    section["FactCost"] = FormatDecimal(item.FactCost);
                    section["ReasonRejection"] = item.ReasonRejection;
                    section["DateStart"] = FormatDate(item.DateStart);
                    section["DateEnd"] = FormatDate(item.DateEnd);

                    FillObjectRepairDetail(sectionDetail, dataDetail, item.GroupWorkPprId);
                }
            }
        }

        private void FillObjectRepairDetail(Section section, IEnumerable<RepairWorkDetailProxy> data, long? groupWorkPprId)
        {
            if (data != null)
            {
                var i = 0;

                var details = data.Where(x => x.GroupWorkPprId == groupWorkPprId).ToList();

                foreach (var item in details)
                {
                    section.ДобавитьСтроку();
                    
                    section["Number"] = ++i;
                    section["Name"] = item.Name; 
                    section["Volume"] = FormatDecimal(item.PlanVolume);
                    section["FactVolume"] = FormatDecimal(item.FactVolume);
                    section["UnitMeasure"] = item.UnitMeasure;
                }
            }
        }

        private void FillObjectRepairTo(Section section, IEnumerable<RepairWorkTo> data)
        {
            if (data != null)
            {
                var i = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Name"] = item.Name;
                    section["Cost"] = FormatDecimal(item.PlanCost);
                    section["FactCost"] = FormatDecimal(item.FactCost);
                    section["DateStart"] = FormatDate(item.DateStart);
                    section["DateEnd"] = FormatDate(item.DateEnd);
                }
            }
        }

        private void FillObjectHousingService(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<HousingService>();
            var domainItem = Container.ResolveDomain<CostItem>();

            var dataItem = _dictCostItem.ContainsKey(diRo.Id)
                ? _dictCostItem[diRo.Id]
                : new Dictionary<long, IEnumerable<CostItem>>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Where(x => x.Provider != null)
                    .Select(x => new
                    {
                        x.Id,
                        Provider = new {x.Provider.Id, x.Provider.Name},
                        x.TemplateService.Name,
                        UnitMeasure = x.UnitMeasure.Name,
                        Periodicity = x.Periodicity.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Provider);

                var sectionProvider = parentSection.ДобавитьСекцию("sectionObjectHousingProvider");

                foreach (var provider in data)
                {
                    sectionProvider.ДобавитьСтроку();
                    sectionProvider["Provider"] = provider.Key.Name;

                    var sectionService = sectionProvider.ДобавитьСекцию("sectionObjectHousingService");
                    var sectionServiceItem = sectionService.ДобавитьСекцию("sectionObjectHousingServiceItem");

                    var i = 0;

                    foreach (var item in provider)
                    {
                        sectionService.ДобавитьСтроку();

                        sectionService["Number"] = ++i;
                        sectionService["Name"] = item.Name;
                        sectionService["UnitMeasure"] = item.UnitMeasure;
                        sectionService["Periodicity"] = item.Periodicity;
                        sectionService["Tariff"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));

                        FillObjectHousingItems(sectionServiceItem, dataItem.Get(item.Id));
                    }
                }
            }
            finally
            {
                Container.Release(domain);
                Container.Release(domainItem);
            }
        }

        private void FillObjectHousingItems(Section section, IEnumerable<CostItem> data)
        {
            if (data != null)
            {
                var iterator = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++iterator;
                    section["Name"] = item.Name;
                    section["Sum"] = item.Sum;
                }
            }
        }

        private void FillObjectAdditionalService(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<AdditionalService>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Where(x => x.Provider != null)
                    .Select(x => new
                    {
                        x.Id,
                        Provider = new {x.Provider.Id, x.Provider.Name},
                        x.TemplateService.Name,
                        Periodicity = x.Periodicity.Name,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Provider);

                var sectionProvider = parentSection.ДобавитьСекцию("sectionObjectAdditionalProvider");

                foreach (var provider in data)
                {
                    sectionProvider.ДобавитьСтроку();

                    sectionProvider["Name"] = provider.Key.Name;

                    var sectionService = sectionProvider.ДобавитьСекцию("sectionObjectAdditionalService");

                    var i = 0;

                    foreach (var item in provider)
                    {
                        sectionService.ДобавитьСтроку();

                        sectionService["Number"] = ++i;
                        sectionService["Name"] = item.Name;
                        sectionService["Periodicity"] = item.Periodicity;
                        sectionService["Tariff"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));
                    }
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillObjectCommunalService(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<CommunalService>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Where(x => x.Provider != null)
                    .Select(x => new
                    {
                        x.Id,
                        Provider = x.Provider.Name,
                        x.TemplateService.Name
                    });

                var i = 0;

                var section = parentSection.ДобавитьСекцию("sectionCommunalObjectService");
                
                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Name"] = item.Name;
                    section["Provider"] = item.Provider;
                    section["Tariff"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillObjectCommonFacilities(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<InfoAboutUseCommonFacilities>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new
                    {
                        x.KindCommomFacilities,
                        x.Lessee,
                        x.DateStart,
                        x.DateEnd,
                        x.CostContract,
                        x.Number,
                        x.From,
                        x.TypeContract
                    });

                var i = 0;
                var section = parentSection.ДобавитьСекцию("sectionObjectCommonFacilities");

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;

                    section["KindCommomFacilities"] = item.KindCommomFacilities;
                    section["Lessee"] = item.Lessee;
                    section["ContractPeriod"] =
                        string.Format("{0} - {1}", FormatDate(item.DateStart), FormatDate(item.DateEnd));

                    section["CostContract"] = item.CostContract;
                    section["ProtocolNumber"] = item.Number;
                    section["ProtocolDate"] = FormatDate(item.From);
                    section["TypeContract"] = item.TypeContract.GetEnumMeta().Display;
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillObjectCommunalResources(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var data = Container.ResolveDomain<CommunalService>().GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
#warning не убирать проверку на null
                .Where(x => x.Id != null)
                .Select(x => new
                {
                    x.Id,
                    x.TemplateService.Name,
                    Provider = x.Provider.Name,
                    Volume = x.VolumePurchasedResources
                });

            var section = parentSection.ДобавитьСекцию("sectionObjectCommunalResources");

            var i = 0;

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["Number"] = ++i;
                section["Name"] = item.Name;
                section["Provider"] = item.Provider;
                section["Volume"] = FormatDecimal(item.Volume);
                section["TariffCitizens"] = FormatDecimal(_dictTariffForCitizens.Get(item.Id));

                var tariffRso = _dictTariffForRso.Get(item.Id);

                if (tariffRso != null)
                {
                    section["TariffRso"] = FormatDecimal(tariffRso.Cost);

                    if (tariffRso.Org != null && "государственый комитет республики татарстан по тарифам".Equals(tariffRso.Org.ToLower()))
                    {
                        section["TariffRt"] = FormatDecimal(tariffRso.Cost);
                        section["Decision"] =
                            string.Format("№{0} от {1}",
                                tariffRso.NumberAct,
                                FormatDate(tariffRso.DateAct));
                    }
                }
            }
        }

        private void FillObjectReductionWorks(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<PlanReductionExpenseWorks>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.PlanReductionExpense.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new
                    {
                        ServiceName = x.PlanReductionExpense.BaseService.TemplateService.Name,
                        x.Name,
                        x.DateComplete,
                        x.PlannedReductionExpense,
                        x.FactedReductionExpense,
                        x.ReasonRejection
                    });

                var section = parentSection.ДобавитьСекцию("sectionObjectReductionWorks");

                var i = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["PlanName"] = item.Name;
                    section["Name"] = item.ServiceName;
                    section["DateComplete"] = FormatDate(item.DateComplete);
                    section["PlannedReductionExpense"] = FormatDecimal(item.PlannedReductionExpense);
                    section["FactedReductionExpense"] = FormatDecimal(item.FactedReductionExpense);
                    section["ReasonRejection"] = item.ReasonRejection;
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillObjectReductionPayment(Section parentSection, DisclosureInfoRealityObj diRo)
        {
            var domain = Container.ResolveDomain<InfoAboutReductionPayment>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRo.Id)
                    .Select(x => new
                    {
                        x.BaseService.TemplateService.Name,
                        x.OrderDate,
                        x.RecalculationSum,
                        UniMeasure = x.BaseService.UnitMeasure.Name
                    });

                var i = 0;

                decimal sum = 0m;
                int count = 0;

                var section = parentSection.ДобавитьСекцию("sectionObjectReductionPayment");

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;

                    section["Name"] = item.Name;
                    section["OrderDate"] = FormatDate(item.OrderDate);
                    section["RecaclulationSum"] = FormatDecimal(item.RecalculationSum);
                    section["UnitMeasure"] = item.UniMeasure;

                    count++;
                    if (item.RecalculationSum.HasValue)
                    {
                        sum += item.RecalculationSum.Value;
                    }
                }

                parentSection["ObjectReductionPaymentCount"] = count;
                parentSection["ObjectReductionPaymentSum"] = sum.ToString("0.00");
            }
            finally
            {
                Container.Release(domain);
            }
        }
    }
}