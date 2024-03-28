namespace Bars.GkhDi.Regions.Tatarstan.Services.Domain
{
    using Bars.GkhDi.Services.Domain;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Services.DataContracts;
    using Entities;
    using Enums;
    using Gkh.Utils;
    using GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;
    using GkhDi.Services.DataContracts.GetPeriods;
    using GkhDi.Entities;
    using Bars.Gkh.DomainService;
    using Bars.GkhDi.Entities.Service;

    using InformationOnContracts = Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.InformationOnContracts;
    using Gkh.DomainService.RegionalFormingOfCr;
    using Gkh.Serialization;
    using Gkh.PassportProvider;
    using Newtonsoft.Json;
    using GkhDi.DomainService;

    using FileInfo = Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.FileInfo;

    public class ServiceDi : IServiceDi
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TehPassportValue> TehPassportValueServis { get; set; }

        public IPassportProvider PassportProviderServis { get; set; }

        public IRealtyObjectLiftService RealtyObjectLiftService { get; set; }

        public IDisclosureInfoRealityObjService DisclosureInfoRealityObjService { get; set; }

        public IDomainService<ConsumptionNormsNpa> ConsumptionNormsNpaDomain { get; set; }

        public IRealityObjectDecisionProtocolProxyService RealityObjectDecisionProtocolProxyService { get; set; }

        private static readonly Tuple<string, string> TehPassportDocumentBasedArea = new Tuple<string, string>("Form_2", "1:3");
        private static readonly Tuple<string, string> TehPassportParkingArea = new Tuple<string, string>("Form_2", "16:3");
        private static readonly Tuple<string, string> EnergyEfficiency = new Tuple<string, string>("Form_6_1", "1:1");
        private static readonly Tuple<string, string> TehPassportChildrenArea = new Tuple<string, string>("Form_2", "9:3");
        private static readonly Tuple<string, string> TehPassportSportArea = new Tuple<string, string>("Form_2", "10:3");
        private static readonly Tuple<string, string> BasementAreaForm = new Tuple<string, string>("Form_5_4", "9:4");
        private static readonly Tuple<string, string> TypeFloorsForm = new Tuple<string, string>("Form_5_3", "1:3");
        private static readonly Tuple<string, string> TypeWallsForm = new Tuple<string, string>("Form_5_2", "1:3");
        private static readonly Tuple<string, string> ConstrChuteForm = new Tuple<string, string>("Form_3_7_2", "1:3");
        private static readonly Tuple<string, string> ChutesNumberForm = new Tuple<string, string>("Form_3_7_3", "5:1");
        
        const string BasementTypeFormId = "Form_5_1";
        const string FacadesForm = "Form_5_8";

        private readonly Dictionary<string, string> energyEfficiencyClassDict = new Dictionary<string, string>
        {
            {"1", "A" },
            {"2", "B++" },
            {"3", "B+" },
            {"4", "B" },
            {"5", "C" },
            {"6", "D" },
            {"7", "E" },
            {"8", "A++" },
            {"9", "A+" },
            {"10", "F" }
        };

        public GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId, string periodId = null)
        {
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContractOwnersDomain = this.Container.Resolve<IDomainService<ManOrgContractOwners>>();
            var manOrgContractJskTsjDomain = this.Container.Resolve<IDomainService<ManOrgJskTsjContract>>();
            var disclosureInfoRealityObjDomain = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var informationOnContractsDomain = this.Container.Resolve<IDomainService<GkhDi.Entities.InformationOnContracts>>();
            var infoAboutReductionPaymentDomain = this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>();
            var planReductionExpenseWorksDomain = this.Container.Resolve<IDomainService<PlanReductionExpenseWorks>>();
            var planReduceMeasureNameDomain = this.Container.Resolve<IDomainService<PlanReduceMeasureName>>();
            var workRepairDetailTatDomain = this.Container.Resolve<IDomainService<WorkRepairDetailTat>>();
            var planWorkServiceRepairWorksDomain = this.Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();
            var repairServiceDomain = this.Container.Resolve<IDomainService<RepairService>>();
            var workRepairTechServDomain = this.Container.Resolve<IDomainService<WorkRepairTechServ>>();
            var documentsRealityObjDomain = this.Container.Resolve<IDomainService<DocumentsRealityObj>>();
            var documentsRealityObjProtocolDomain = this.Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>();
            var otherServiceDomain = this.Container.Resolve<IDomainService<OtherService>>();
            var infoAboutPaymentHousingDomain = this.Container.Resolve<IDomainService<InfoAboutPaymentHousing>>();
            var infoAboutPaymentCommunalDomain = this.Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>();
            var infoAboutUseCommonFacilitiesDomain = this.Container.Resolve<IDomainService<InfoAboutUseCommonFacilities>>();
            var nonResidentialPlacementDomain = this.Container.Resolve<IDomainService<NonResidentialPlacement>>();
            var tariffForRsoDomain = this.Container.Resolve<IDomainService<TariffForRso>>();
            var tariffForConsumersDomain = this.Container.Resolve<IDomainService<TariffForConsumers>>();
            var providerServiceDomain = this.Container.Resolve<IDomainService<ProviderService>>();
            var communalServiceDomain = this.Container.Resolve<IDomainService<CommunalService>>();
            var housingServiceDomain = this.Container.Resolve<IDomainService<HousingService>>();
            var capRepairServiceDomain = this.Container.Resolve<IDomainService<CapRepairService>>();
            var additionalServiceDomain = this.Container.Resolve<IDomainService<AdditionalService>>();
            var controlServiceDomain = this.Container.Resolve<IDomainService<ControlService>>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var nonResidentialPlacementMeteringDeviceDomain = this.Container.Resolve<IDomainService<NonResidentialPlacementMeteringDevice>>();
            var dictPeriodDomain = this.Container.Resolve<IDomainService<PeriodDi>>();
            var manOrgDomain = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var tariffForConsumersOtherServiceDomain = this.Container.Resolve<IDomainService<TariffForConsumersOtherService>>();

            try
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                NumberFormatInfo numberformat = null;
                if (ci != null)
                {
                    ci.NumberFormat.NumberDecimalSeparator = ".";
                    numberformat = ci.NumberFormat;
                }

                var idHouse = houseId.ToLong();
                var periodDi = periodId.ToLong();
                
                if (idHouse == 0)
                {
                    return new GetManOrgRealtyObjectInfoResponse
                    {
                        Result = Result.DataNotFound
                    };
                }
                
                var dictPeriod = dictPeriodDomain.GetAll()
                    .WhereIf(periodDi > 0, x => x.Id == periodDi)
                    .Select(x => new
                    {
                        x.Id,
                        DateStart = x.DateStart ?? DateTime.MinValue,
                        x.DateEnd,
                        x.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());
                
                var disclosureInfoRealityObjects = disclosureInfoRealityObjDomain.GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && dictPeriod.Keys.Contains(x.PeriodDi.Id))
                    .Select(x => new
                    {
                        x.Id,
                        PeriodId = x.PeriodDi.Id,
                        x.ManagingOrganization,
                        x.NonResidentialPlacement,
                        x.PlaceGeneralUse,
                        x.ReductionPayment,
                        x.AdvancePayments,
                        x.CarryOverFunds,
                        x.ChargeForMaintenanceAndRepairsAll,
                        x.ChargeForMaintenanceAndRepairsMaintanance,
                        x.Debt,
                        x.ChargeForMaintenanceAndRepairsRepairs,
                        x.ChargeForMaintenanceAndRepairsManagement,
                        x.ReceivedCashAll,
                        x.ReceivedCashFromOwners,
                        x.ReceivedCashFromOwnersTargeted,
                        x.ReceivedCashAsGrant,
                        x.ReceivedCashFromUsingCommonProperty,
                        x.ReceivedCashFromOtherTypeOfPayments,
                        x.CashBalanceAll,
                        x.CashBalanceAdvancePayments,
                        x.CashBalanceCarryOverFunds,
                        x.CashBalanceDebt,
                        x.ReceivedPretensionCount,
                        x.ApprovedPretensionCount,
                        x.NoApprovedPretensionCount,
                        x.PretensionRecalcSum,
                        x.SentPretensionCount,
                        x.SentPetitionCount,
                        x.ReceiveSumByClaimWork,
                        x.ComServReceivedPretensionCount,
                        x.ComServApprovedPretensionCount,
                        x.ComServNoApprovedPretensionCount,
                        x.ComServPretensionRecalcSum,
                        x.ComServStartAdvancePay,
                        x.ComServStartCarryOverFunds,
                        x.ComServStartDebt,
                        x.ComServEndAdvancePay,
                        x.ComServEndCarryOverFunds,
                        x.ComServEndDebt,
                        x.RealityObject
                    })
                    .ToList();

                var managingOrgItems = new Dictionary<long, List<ManagingOrgItem>>();

                foreach (var disclosureInfoRealityObj in disclosureInfoRealityObjects)
                {
                    // Сведения о договорах
                    var informationOnContracts = informationOnContractsDomain
                        .GetAll()
                        .Where(x => x.RealityObject.Id == idHouse &&
                            x.DisclosureInfo.ManagingOrganization.Id == disclosureInfoRealityObj.ManagingOrganization.Id)
                        .Where(x => x.DisclosureInfo.PeriodDi.Id == disclosureInfoRealityObj.PeriodId)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfo.PeriodDi.Id,
                                x.Name,
                                x.Cost,
                                x.Number,
                                x.PartiesContract,
                                x.Comments,
                                x.From,
                                x.DateStart,
                                x.DateEnd
                            })
                        .AsEnumerable();

                    // Сведения о случаях снижения платы
                    var infoAboutReductionPayment = infoAboutReductionPaymentDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.BaseService.TemplateService.Name,
                                x.BaseService.TemplateService.TypeGroupServiceDi,
                                x.ReasonReduction,
                                x.RecalculationSum,
                                x.OrderDate,
                                x.OrderNum,
                                x.Description
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new InfoAboutReductPaymentItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                        ReasonReduction = z.ReasonReduction.ToStr(),
                                        RecalculationSum =
                                            z.RecalculationSum.HasValue ? z.RecalculationSum.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        OrderDate =
                                            z.OrderDate.HasValue && z.OrderDate.Value.Date != DateTime.MinValue.Date
                                                ? z.OrderDate.Value.ToShortDateString()
                                                : string.Empty,
                                        OrderNumber = z.OrderNum.ToStr(),
                                        Description = z.Description.ToStr()
                                    })
                                .ToArray());

                    var planReductionExpenseWorksQuery = planReductionExpenseWorksDomain
                        .GetAll()
                        .Where(x => x.PlanReductionExpense.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id);

                    var measureNames = planReduceMeasureNameDomain
                        .GetAll()
                        .Where(x => planReductionExpenseWorksQuery.Select(y => y.Id).Contains(x.PlanReductionExpenseWorks.Id))
                        .Select(x => new { x.PlanReductionExpenseWorks.Id, x.MeasuresReduceCosts.MeasureName })
                        .ToDictionary(x => x.Id, x => x.MeasureName);

                    // План работ и мер по снижению расходов
                    var planReductionExpenseWorks = planReductionExpenseWorksDomain
                        .GetAll()
                        .Where(x => x.PlanReductionExpense.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.PlanReductionExpense.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.Name,
                                x.DateComplete,
                                x.PlannedReductionExpense,
                                x.FactedReductionExpense,
                                x.ReasonRejection
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new PlanReductionExpItem
                                    {
                                        Id = z.Id,
                                        Name = measureNames.ContainsKey(z.Id) ? measureNames[z.Id] : z.Name.ToStr(),
                                        DateComplete =
                                            z.DateComplete.HasValue && z.DateComplete.Value.Date != DateTime.MinValue.Date
                                                ? z.DateComplete.Value.ToShortDateString()
                                                : string.Empty,
                                        PlannedReductionExpense =
                                            z.PlannedReductionExpense.HasValue
                                                ? z.PlannedReductionExpense.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        FactedReductionExpense =
                                            z.FactedReductionExpense.HasValue
                                                ? z.FactedReductionExpense.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        ReasonRejection = z.ReasonRejection
                                    })
                                .ToArray());

                    // Работы по содержанию и ремонту МКД
                    var workPprRepairDetailService = workRepairDetailTatDomain;
                    var planWorkServiceRepWorksItem = planWorkServiceRepairWorksDomain
                        .GetAll()
                        .Where(x => x.PlanWorkServiceRepair.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.PlanWorkServiceRepair.DisclosureInfoRealityObj.PeriodDi.Id,
                                GroupWorkPprId = x.WorkRepairList.GroupWorkPpr.Id,
                                x.WorkRepairList.GroupWorkPpr.Name,
                                Periodicity = x.PeriodicityTemplateService.Name,
                                x.DateStart,
                                x.DateEnd,
                                x.DateComplete,
                                x.Cost,
                                x.FactCost,
                                x.ReasonRejection
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => new PlanWorkServiceRepWorksItem
                            {
                                WorksPpr = y.AsEnumerable()
                                    .Select(
                                        z => new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.WorkPpr
                                        {
                                            NamePpr = z.Name.ToStr(),
                                            PeriodicityTemplateService = z.Periodicity.ToStr(),
                                            DateEnd =
                                                z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                    ? z.DateEnd.Value.ToShortDateString()
                                                    : string.Empty,
                                            DateComplete = z.DateComplete.HasValue && z.DateComplete.Value.Date != DateTime.MinValue.Date
                                                ? z.DateStart.Value.ToShortDateString() + " - " + z.DateComplete.Value.ToShortDateString()
                                                : string.Empty,
                                            Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2) : 0,
                                            FactCost = z.FactCost.HasValue ? z.FactCost.Value.RoundDecimal(2) : 0,
                                            ReasonRejection = z.ReasonRejection.ToStr(),

                                            Details = workPprRepairDetailService.GetAll()
                                                .Where(x => x.WorkPpr.GroupWorkPpr.Id == z.GroupWorkPprId)
                                                .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                                                .ToList()
                                                .Select(
                                                    x => new DetailPpr
                                                    {
                                                        Name = x.Return(a => a.WorkPpr).Return(a => a.Name),
                                                        FactSize = x.FactVolume.HasValue ? Math.Round(x.FactVolume.Value, 2) : 0M,
                                                        PlanSize = x.PlannedVolume.HasValue ? Math.Round(x.PlannedVolume.Value, 2) : 0M,
                                                        Izm = x.Return(a => a.WorkPpr).Return(a => a.Measure).Return(a => a.ShortName)
                                                    })
                                                .ToArray()
                                        })
                                    .ToArray()
                            });

                    var worksTo = repairServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(x => x)
                        .ToDictionary(x => x.Id, y => y);

                    var workRepairTechServService = workRepairTechServDomain;
                    var detailsTo = workRepairTechServService.GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                x.BaseService.Id,
                                x.WorkTo.Name
                            });

                    var workRepairTechServ = workRepairTechServService.GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                x.BaseService.Id,
                                PeriodId = x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id,
                                NameGroup = x.WorkTo.GroupWorkTo.Name,
                            })
                        .AsEnumerable()
                        .Distinct()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => new PlanWorkServiceRepWorksItem
                            {
                                WorksTo = y.AsEnumerable()
                                    .Select(
                                        z => new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.WorkTo
                                        {
                                            NameGroup = z.NameGroup,
                                            DateComplete =
                                                (worksTo.ContainsKey(z.Id) && worksTo[z.Id].DateStart.HasValue && worksTo[z.Id].DateEnd.HasValue)
                                                    ? worksTo[z.Id].DateStart.Value.ToShortDateString() + " - " +
                                                    worksTo[z.Id].DateEnd.Value.ToShortDateString()
                                                    : "",
                                            DateEnd =
                                                (worksTo.ContainsKey(z.Id) && worksTo[z.Id].DateEnd.HasValue)
                                                    ? worksTo[z.Id].DateEnd.Value.ToShortDateString()
                                                    : "",
                                            ReasonRejection = worksTo.ContainsKey(z.Id) ? worksTo[z.Id].RejectCause : "",
                                            DetailsTo = detailsTo.Where(d => d.Id == z.Id).Select(d => new DetailTo { Name = d.Name }).ToArray()
                                        })
                                    .ToArray()
                            });

                    foreach (var period in workRepairTechServ)
                    {
                        if (planWorkServiceRepWorksItem.ContainsKey(period.Key))
                        {
                            planWorkServiceRepWorksItem[period.Key].WorksTo = period.Value.WorksTo;
                        }
                        else
                        {
                            planWorkServiceRepWorksItem.Add(period.Key, period.Value);
                        }
                    }

                    // Документы
                    var documents = documentsRealityObjDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.FileActState,
                                x.FileCatalogRepair,
                                x.FileReportPlanRepair
                            })
                        .AsEnumerable()
                        .FirstOrDefault();

                    // Протоколы
                    var currentYear = DateTime.Now.Year;
                    var protocols = documentsRealityObjProtocolDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.Year,
                                x.File
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.Year)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());
                    
                    //тарифы прочих услуг
                    var otherServiceTariffsHash = tariffForConsumersOtherServiceDomain
                        .GetAll()
                        .Where(x => x.OtherService.TemplateOtherService != null 
                            && x.OtherService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(x => new
                        {
                            x.Id,
                            OtherServiceId = x.OtherService.Id,
                            x.DateStart,
                            x.Cost,
                            x.DateEnd,
                            x.TariffIsSetFor,
                            x.TypeOrganSetTariffDi
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.OtherServiceId)
                        .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateStart).ThenByDescending(y => y.Id));

                    // Прочие услуги
                    var otherService = otherServiceDomain
                        .GetAll()
                        .Where(x => x.TemplateOtherService != null && x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.Name,
                                UnitMeasureName = x.UnitMeasure.Name,
                                x.UnitMeasureStr,
                                x.Code,
                                TemplateOtherServiceName = x.TemplateOtherService.Name
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new OtherServiceItem
                                    {
                                        Id = z.Id,
                                        Name = (z.TemplateOtherServiceName ?? z.Name).ToStr(),
                                        UnitMeasure = (z.UnitMeasureName ?? z.UnitMeasureStr).ToStr(),
                                        Tariffs = otherServiceTariffsHash.ContainsKey(z.Id)
                                            ? otherServiceTariffsHash[z.Id].Select(x => new TariffForConsumersItem
                                            {
                                                DateStart = x.DateStart.HasValue ? x.DateStart.Value.ToString("dd.MM.yyyy") : string.Empty,
                                                DateEnd = x.DateEnd.HasValue ? x.DateEnd.Value.ToString("dd.MM.yyyy") : string.Empty,
                                                TariffIsSetFor = x.TariffIsSetFor.GetEnumMeta().Display,
                                                TypeOrganSetTariffDi = x.TypeOrganSetTariffDi.GetEnumMeta().Display,
                                                Cost = x.Cost.HasValue ? x.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty
                                            }).ToArray()
                                            : new TariffForConsumersItem[0],
                                        Code = z.Code.ToStr()
                                    })
                                .ToList());

                    // Сведения об оплатах
                    var infoAboutPaymentHousing = infoAboutPaymentHousingDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.BaseService.TemplateService.Name,
                                x.BaseService.TemplateService.TypeGroupServiceDi,
                                ProviderName = x.BaseService.Provider.Name,
                                x.CounterValuePeriodStart,
                                x.CounterValuePeriodEnd,
                                x.GeneralAccrual,
                                x.Collection
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new InfoAboutPayItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                        Provider = z.ProviderName.ToStr(),
                                        CounterValuePeriodStart =
                                            z.CounterValuePeriodStart.HasValue
                                                ? z.CounterValuePeriodStart.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        CounterValuePeriodEnd =
                                            z.CounterValuePeriodEnd.HasValue
                                                ? z.CounterValuePeriodEnd.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        GeneralAccrual =
                                            z.GeneralAccrual.HasValue ? z.GeneralAccrual.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        Collection = z.Collection.HasValue ? z.Collection.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        Accrual = null,
                                        Debt = null,
                                        Payed = null
                                    })
                                .ToList());

                    var infoAboutPaymentCommunal = infoAboutPaymentCommunalDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.BaseService.TemplateService.Name,
                                x.BaseService.TemplateService.TypeGroupServiceDi,
                                ProviderName = x.BaseService.Provider.Name,
                                x.CounterValuePeriodStart,
                                x.CounterValuePeriodEnd,
                                x.Accrual,
                                x.Payed,
                                x.Debt
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new InfoAboutPayItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                        Provider = z.ProviderName.ToStr(),
                                        CounterValuePeriodStart =
                                            z.CounterValuePeriodStart.HasValue
                                                ? z.CounterValuePeriodStart.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        CounterValuePeriodEnd =
                                            z.CounterValuePeriodEnd.HasValue
                                                ? z.CounterValuePeriodEnd.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        Accrual = z.Accrual.HasValue ? z.Accrual.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        Debt = z.Debt.HasValue ? z.Debt.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        Payed = z.Payed.HasValue ? z.Payed.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        GeneralAccrual = null,
                                        Collection = null
                                    })
                                .ToList());

                    // Сведения об использовании мест общего пользования
                    var infoAboutCommonFacil = infoAboutUseCommonFacilitiesDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.KindCommomFacilities,
                                x.Lessee,
                                x.DateStart,
                                x.DateEnd,
                                x.CostContract,
                                x.CostByContractInMonth,
                                x.Number,
                                x.From,
                                x.TypeContract,
                                x.AreaOfCommonFacilities,
                                x.AppointmentCommonFacilities,
                                Hcp = x.DisclosureInfoRealityObj.PlaceGeneralUse.GetEnumMeta().Display
                            })
                        .AsEnumerable();

                    // Сведения об исп нежилых помещений
                    var nonResidentialPlacement = nonResidentialPlacementDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.ContragentName,
                                x.TypeContragentDi,
                                x.Area,
                                x.DocumentDateApartment,
                                x.DocumentDateCommunal,
                                x.DocumentNumApartment,
                                x.DocumentNumCommunal,
                                x.DateStart,
                                x.DateEnd
                            })
                        .AsEnumerable();

                    // Приборы учета нежилых помещений
                    var nonResidentialPlaceMeteringDevice = nonResidentialPlacementMeteringDeviceDomain
                        .GetAll()
                        .Where(x => x.NonResidentialPlacement.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                NonResidentialPlacementId = x.NonResidentialPlacement.Id,
                                x.MeteringDevice.Name,
                                x.MeteringDevice.AccuracyClass,
                                x.MeteringDevice.TypeAccounting
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.NonResidentialPlacementId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new NonResidentialPlaceMetering
                                    {
                                        AccuracyClass = z.AccuracyClass,
                                        Name = z.Name.ToStr(),
                                        TypeAccounting = z.TypeAccounting.GetEnumMeta().Display
                                    })
                                .ToList());

                    // Тарифы РСО
                    var tariffRso = tariffForRsoDomain
                        .GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                BaseServiceId = x.BaseService.Id,
                                x.Cost,
                                x.NumberNormativeLegalAct,
                                x.DateNormativeLegalAct,
                                x.OrganizationSetTariff,
                                x.DateStart,
                                x.DateEnd
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.BaseServiceId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.OrderByDescending(z => z.DateStart)
                                .Select(
                                    z => new TariffForRsoItem
                                    {
                                        Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        DateNormativeLegalAct =
                                            z.DateNormativeLegalAct.HasValue && z.DateNormativeLegalAct.Value.Date != DateTime.MinValue.Date
                                                ? z.DateNormativeLegalAct.Value.ToShortDateString()
                                                : string.Empty,
                                        DateStart =
                                            z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date
                                                ? z.DateStart.Value.ToShortDateString()
                                                : string.Empty,
                                        DateEnd =
                                            z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                ? z.DateEnd.Value.ToShortDateString()
                                                : string.Empty,
                                        NumberNormativeLegalAct = z.NumberNormativeLegalAct,
                                        OrganizationSetTariff = z.OrganizationSetTariff
                                    })
                                .ToArray());

                    // Тарифы потребителей
                    var tariffConsumers = tariffForConsumersDomain
                        .GetAll()
                        .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x => new
                            {
                                BaseServiceId = x.BaseService.Id,
                                x.DateStart,
                                x.DateEnd,
                                x.TariffIsSetFor,
                                x.OrganizationSetTariff,
                                x.TypeOrganSetTariffDi,
                                x.Cost
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.BaseServiceId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.OrderByDescending(z => z.DateStart)
                                .Select(
                                    z => new TariffForConsumersItem
                                    {
                                        Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                        TariffIsSetFor = z.TariffIsSetFor.GetEnumMeta().Display,
                                        DateStart =
                                            z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date
                                                ? z.DateStart.Value.ToShortDateString()
                                                : string.Empty,
                                        DateEnd =
                                            z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                ? z.DateEnd.Value.ToShortDateString()
                                                : string.Empty,
                                        TypeOrganSetTariffDi = z.TypeOrganSetTariffDi.GetEnumMeta().Display,
                                        OrganizationSetTariff = z.OrganizationSetTariff
                                    })
                                .ToArray());

                    // Услуги
                    var communalService = communalServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    ProviderName = providerServiceDomain.GetAll()
                                        .Where(ps => ps.BaseService.Id == x.Id)
                                        .OrderByDescending(ps => ps.DateStartContract)
                                        .Select(ps => ps.Provider.Name)
                                        .FirstOrDefault(),
                                    x.TemplateService.Code,
                                    x.TariffIsSetForDi,
                                    x.TemplateService.KindServiceDi,
                                    Price = x.TariffIsSetForDi,
                                    x.VolumePurchasedResources,
                                    x.TariffForConsumers

                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new ServiceItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                        Provider = z.ProviderName,
                                        Code = z.Code.ToStr(),
                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                        VolumePurchasedResources =
                                            z.VolumePurchasedResources.HasValue
                                                ? z.VolumePurchasedResources.Value.RoundDecimal(2).ToString(numberformat)
                                                : string.Empty,
                                        IsExecutor = string.IsNullOrEmpty(z.ProviderName) ? "Да" : "Нет",
                                        Periodicity = null,
                                        Price = null,
                                        TypeProvision = null,
                                        TypeProvisionUoTsjJsk = null,
                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                    })
                                .ToList());

                    var housingService = housingServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    PeriodicityName = x.Periodicity.Name,
                                    ProviderName = providerServiceDomain.GetAll()
                                        .Where(ps => ps.BaseService.Id == x.Id)
                                        .OrderByDescending(ps => ps.DateStartContract)
                                        .Select(ps => ps.Provider.Name)
                                        .FirstOrDefault(),
                                    x.TemplateService.Code,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi,
                                    x.TypeOfProvisionService
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new ServiceItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                        Periodicity = z.PeriodicityName.ToStr(),
                                        Provider = z.ProviderName.ToStr(),
                                        Code = z.Code.ToStr(),
                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                        TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                        TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                        IsExecutor = null,
                                        Price = null,
                                        VolumePurchasedResources = null,
                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                    })
                                .ToList());

                    var data = repairServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    ProviderName = new List<string>(),
                                    x.TemplateService.Code,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi,
                                    x.TypeOfProvisionService
                                })
                        .ToArray();

                    foreach (var serv in data)
                    {
                        serv.ProviderName.Clear();
                        var list = providerServiceDomain.GetAll()
                            .Where(x => (x.BaseService.Id == serv.Id) && x.IsActive)
                            .Select(x => x.Provider.Name)
                            .ToList();

                        if (list.Count > 0)
                        {
                            serv.ProviderName.AddRange(list);
                        }
                    }

                    var group = data.GroupBy(x => x.Periodid);

                    var repairService = group.ToDictionary(
                        x => x.Key,
                        y => y.AsEnumerable()
                            .Select(
                                z => new ServiceItem
                                {
                                    Id = z.Id,
                                    Name = z.Name.ToStr(),
                                    UnitMeasure = z.UnitMeasure.ToStr(),
                                    Provider = !z.ProviderName.IsEmpty()
                                        ? z.ProviderName.AggregateWithSeparator(", ")
                                        : providerServiceDomain.GetAll()
                                            .Where(ps => ps.BaseService.Id == z.Id)
                                            .OrderByDescending(ps => ps.DateStartContract)
                                            .Select(ps => ps.Provider.Name)
                                            .FirstOrDefault(),
                                    Code = z.Code.ToStr(),
                                    KindService = z.KindServiceDi.GetEnumMeta().Display,
                                    TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                    TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                    IsExecutor = null,
                                    Periodicity = null,
                                    Price = null,
                                    VolumePurchasedResources = null,
                                    TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                    TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                })
                            .ToList());

                    var capRepairService = capRepairServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    ProviderName = providerServiceDomain.GetAll()
                                        .Where(ps => ps.BaseService.Id == x.Id)
                                        .OrderByDescending(ps => ps.DateStartContract)
                                        .Select(ps => ps.Provider.Name)
                                        .FirstOrDefault(),
                                    x.TemplateService.Code,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi,
                                    x.TypeOfProvisionService
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new ServiceItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                        Provider = z.ProviderName.ToStr(),
                                        Code = z.Code.ToStr(),
                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                        TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                        TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                        IsExecutor = null,
                                        Periodicity = null,
                                        Price = null,
                                        VolumePurchasedResources = null,
                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                    })
                                .ToList());

                    var additionalService = additionalServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    PeriodicityName = x.Periodicity.Name,
                                    ProviderName = providerServiceDomain.GetAll()
                                        .Where(ps => ps.BaseService.Id == x.Id)
                                        .OrderByDescending(ps => ps.DateStartContract)
                                        .Select(ps => ps.Provider.Name)
                                        .FirstOrDefault(),
                                    x.TemplateService.Code,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new ServiceItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                        Provider = z.ProviderName.ToStr(),
                                        Code = z.Code.ToStr(),
                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                        IsExecutor = null,
                                        Periodicity = z.PeriodicityName,
                                        Price = null,
                                        TypeProvision = null,
                                        TypeProvisionUoTsjJsk = null,
                                        VolumePurchasedResources = null,
                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                    })
                                .ToList());

                    var controlService = controlServiceDomain
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    ProviderName = providerServiceDomain.GetAll()
                                        .Where(ps => ps.BaseService.Id == x.Id)
                                        .OrderByDescending(ps => ps.DateStartContract)
                                        .Select(ps => ps.Provider.Name)
                                        .FirstOrDefault(),
                                    x.TemplateService.Code,
                                    x.TariffIsSetForDi,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(
                            x => x.Key,
                            y => y.AsEnumerable()
                                .Select(
                                    z => new ServiceItem
                                    {
                                        Id = z.Id,
                                        Name = z.Name.ToStr(),
                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                        Provider = z.ProviderName.ToStr(),
                                        Code = z.Code.ToStr(),
                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                        VolumePurchasedResources = null,
                                        Price = null,
                                        Periodicity = null,
                                        IsExecutor = null,
                                        TypeProvision = null,
                                        TypeProvisionUoTsjJsk = null,
                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                    })
                                .ToList());

                    // Договора дома с ук
                    var contractManOrg = manOrgContractRealityObjectDomain
                        .GetAll()
                        .Where(x => x.RealityObject.Id == idHouse && x.ManOrgContract.ManagingOrganization != null
                            && x.ManOrgContract.ManagingOrganization.Id == disclosureInfoRealityObj.ManagingOrganization.Id)
                        .Select(
                            x => new
                            {
                                ManOrgId = x.ManOrgContract.ManagingOrganization.Id,
                                x.ManOrgContract.StartDate,
                                x.ManOrgContract.EndDate,
                                x.ManOrgContract.TypeContractManOrgRealObj,

                                DocumentNum = x.ManOrgContract.DocumentNumber,

                                TerminationDate = (x.ManOrgContract as ManOrgContractTransfer).TerminationDate ??
                                    (x.ManOrgContract as ManOrgContractOwners).TerminationDate ??
                                    (x.ManOrgContract as ManOrgJskTsjContract).TerminationDate,

                                TerminateReason = (x.ManOrgContract as ManOrgContractTransfer).TerminateReason ??
                                    (x.ManOrgContract as ManOrgContractOwners).TerminateReason ??
                                    (x.ManOrgContract as ManOrgJskTsjContract).TerminateReason,

                                DocumentDate = (x.ManOrgContract as ManOrgContractTransfer).DocumentDate ??
                                    (x.ManOrgContract as ManOrgContractOwners).DocumentDate ??
                                    (x.ManOrgContract as ManOrgJskTsjContract).DocumentDate,

                                x.ManOrgContract.Id,
                                x.ManOrgContract.ManagingOrganization.TypeManagement,
                                x.ManOrgContract.DocumentName,
                                x.ManOrgContract.FileInfo,
                                x.RealityObject.AreaMkd
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.ManOrgId)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());


                    var houseMeteringDevice = new HouseMeteringDevice[] { };
                    var engineeringSystem = new List<EngineeringSystem>();

                    var device =
                        this.DisclosureInfoRealityObjService.GetRealtyObjectDevices(disclosureInfoRealityObj.Id).Data as
                            List<DisclosureInfoRealityObjService.RealtyObjectDevice>;

                    houseMeteringDevice = device.Where(x => x.Number != null)
                        .Select(
                            x => new HouseMeteringDevice
                            {
                                CommunalResourceType = x.TypeCommResourse,
                                Availability = x.ExistMeterDevice,
                                MeterType = x.InterfaceType,
                                UnitOfMeasurement = x.UnutOfMeasure,
                                CommissioningDate = x.InstallDate,
                                CalibrationDate = x.CheckDate
                            })
                        .ToArray();

                    var realtyObjectEngineerSystems =
                        this.DisclosureInfoRealityObjService.GetRealtyObjectEngineerSystems(disclosureInfoRealityObj.Id).Data as
                            DisclosureInfoRealityObjService.RealtyObjectEngineerSystems;
                    engineeringSystem.Add(
                        new EngineeringSystem
                        {
                            HeatingType = realtyObjectEngineerSystems?.TypeHeating,
                            HotWaterType = realtyObjectEngineerSystems?.TypeHotWater,
                            ColdWaterType = realtyObjectEngineerSystems.TypeColdWater,
                            SewerageType = realtyObjectEngineerSystems.TypeSewage,
                            SewerageCesspoolsVolume = realtyObjectEngineerSystems.SewageVolume,
                            GasType = realtyObjectEngineerSystems.TypeGas,
                            VentilationType = realtyObjectEngineerSystems.TypeVentilation,
                            FirefightingType = realtyObjectEngineerSystems.Firefighting,
                            DrainageType = realtyObjectEngineerSystems.TypeDrainage,
                            TypePower = realtyObjectEngineerSystems.TypePower,
                            TypePowerPoints = realtyObjectEngineerSystems.TypePowerPoints
                        });
                    
                    var communalServiceQuery = communalServiceDomain.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                        .Where(x => x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal);

                    var communalServiceDictionary = communalServiceQuery
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.DisclosureInfoRealityObj.PeriodDi,
                                Type = x.TemplateService.Name,
                                FillingFact = x.TypeOfProvisionService,
                                MeasurementUnitsServiсу = x.UnitMeasure.Name,
                                Costs = x.PricePurchasedResources ?? 0,
                                ConsumptionNorm = x.ConsumptionNormLivingHouse ?? 0,
                                ConsumptionNormUnitOfMeasurement = x.UnitMeasureLivingHouse.Name,
                                VolumePurchasedResources = x.VolumePurchasedResources
                            });

                    var providerDictionary = providerServiceDomain.GetAll()
                        .Where(x => communalServiceQuery.Any(y => x.BaseService.Id == y.Id))
                        .Select(
                            x => new
                            {
                                x.BaseService.Id,
                                ProviderName = x.Provider.Name,
                                SupplyContractNumber = x.NumberContract,
                                SupplyContractDate = x.DateStartContract.Value.Date,
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(
                                y => new Provider
                                {
                                    ProviderName = y.ProviderName,
                                    SupplyContractNumber = y.SupplyContractNumber,
                                    SupplyContractDate = y.SupplyContractDate.Date
                                }));

                    var consumptionNormsNpaDictionary = this.ConsumptionNormsNpaDomain.GetAll()
                        .Where(x => communalServiceQuery.Any(y => x.BaseService.Id == y.Id))
                        .Select(
                            x => new
                            {
                                x.BaseService.Id,
                                DocumentDate = x.NpaDate ?? default(DateTime),
                                DocumentNumber = x.NpaNumber,
                                DocumentOrganizationName = x.NpaAcceptor
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(
                                y => new ConsumptionNorms
                                {
                                    DocumentDate = y.DocumentDate.Date,
                                    DocumentNumber = y.DocumentNumber,
                                    DocumentOrganizationName = y.DocumentOrganizationName
                                }));

                    var tariffForConsumersDict = tariffForConsumersDomain.GetAll()
                        .Where(x => communalServiceQuery.Any(y => x.BaseService.Id == y.Id))
                        .Select(
                            x => new
                            {
                                x.BaseService.Id,
                                x.Cost,
                                x.DateStart,
                                x.DateEnd
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateStart).First());

                    var houseCommunalService = communalServiceDictionary
                        .AsEnumerable()
                        .GroupBy(x => x.PeriodDi.Id)
                        .ToDictionary(x => x.Key,
                            y => y.Select(x => new HouseCommunalService
                            {
                                Type = x.Type,
                                FillingFact = x.FillingFact.GetDisplayName(),
                                MeasurementUnitsServiсу = x.MeasurementUnitsServiсу,
                                Costs = x.Costs.RoundDecimal(2),
                                Provider = providerDictionary.Get(x.Id)?.ToArray(),
                                ConsumptionNorm = x.ConsumptionNorm.RoundDecimal(2),
                                ConsumptionNormUnitOfMeasurement = x.ConsumptionNormUnitOfMeasurement,
                                ConsumptionNorms = consumptionNormsNpaDictionary.Get(x.Id)?.ToArray(),
                                DateStart = tariffForConsumersDict.Get(x.Id)?.DateStart,
                                DateEnd = tariffForConsumersDict.Get(x.Id)?.DateEnd,
                                Cost = tariffForConsumersDict.Get(x.Id)?.Cost,
                                VolumePurchasedResources = x.VolumePurchasedResources
                            }));


                    var houseCommonProperty = infoAboutCommonFacil.Select(x => new
                        {
                            x.PeriodId,
                            Hcp = x.Hcp,
                            NameHcp = x.KindCommomFacilities,
                            AreaHcp = x.AreaOfCommonFacilities?.RoundDecimal(2) ?? 0,
                            FunctionHcp = x.AppointmentCommonFacilities,
                        })
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(x => x.Key,
                            x => x.Select(y => new HouseCommonProperty
                            {
                                Hcp = x.First().Hcp,
                                NameHcp = x.First().NameHcp,
                                AreaHcp = x.First().AreaHcp,
                                FunctionHcp = x.First().FunctionHcp,
                            }));

                    var managingOrgs = manOrgDomain.GetAll()
                        .Where(x => x.Id == disclosureInfoRealityObj.ManagingOrganization.Id)
                        .Select(
                            y => new
                            {
                                y.Id,
                                y.Contragent.Name,
                                y.Contragent.JuridicalAddress,
                                y.Contragent.Ogrn,
                                y.Contragent.DateRegistration,
                                y.Contragent.OgrnRegistration,
                                y.Contragent.MailingAddress,
                                y.Contragent.Phone,
                                y.Contragent.Email,
                                y.Contragent.OfficialWebsite,
                                y.TypeManagement
                            }
                        )
                        .AsEnumerable()
                        .Select(y => new ManagingOrgItem
                        {
                            FileInfo = new FileInfo
                            {
                                Name = contractManOrg.Get(y.Id)?.FileInfo?.Name ?? string.Empty,
                                Extention = contractManOrg.Get(y.Id)?.FileInfo?.Extention ?? string.Empty,
                                IdFile = contractManOrg.Get(y.Id)?.FileInfo?.Id.ToStr() ?? string.Empty
                            },
                            Id = y.Id,
                            Name = y.Name.ToStr(),
                            JuridicalAddress = y.JuridicalAddress.ToStr(),
                            Ogrn = y.Ogrn.ToStr(),
                            YearRegistration =
                                y.DateRegistration.HasValue && y.DateRegistration.Value.Date != DateTime.MinValue.Date
                                    ? y.DateRegistration.Value.Year.ToStr()
                                    : string.Empty,
                            OgrnRegistration = y.OgrnRegistration.ToStr(),
                            PostAddress = y.MailingAddress.ToStr(),
                            Phone = y.Phone.ToStr(),
                            Email = y.Email.ToStr(),
                            Suite = y.OfficialWebsite.ToStr(),
                            MkdArea = contractManOrg.Get(y.Id)?.AreaMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            ManagingType = y.TypeManagement.GetEnumMeta().Display,
                            ContractStart =
                                contractManOrg.ContainsKey(y.Id) && contractManOrg[y.Id].StartDate.HasValue
                                && contractManOrg[y.Id].StartDate.Value.Date != DateTime.MinValue.Date
                                    ? contractManOrg[y.Id].StartDate.GetValueOrDefault().ToShortDateString()
                                    : string.Empty,
                            ContractEnd =
                                contractManOrg.ContainsKey(y.Id) && contractManOrg[y.Id].EndDate.HasValue
                                && contractManOrg[y.Id].EndDate.Value.Date != DateTime.MinValue.Date
                                    ? contractManOrg[y.Id].EndDate.GetValueOrDefault().ToShortDateString()
                                    : string.Empty,
                            ContractType =
                                contractManOrg.ContainsKey(y.Id)
                                    ? contractManOrg[y.Id].TypeContractManOrgRealObj.GetEnumMeta().Display
                                    : string.Empty,
                            DocumentDate = contractManOrg.Get(y.Id)?.DocumentDate.GetValueOrDefault().ToShortDateString() ??
                                string.Empty,
                            DocumentNumber = contractManOrg.Get(y.Id)?.DocumentNum.ToString() ?? string.Empty,
                            TerminateReason = contractManOrg.Get(y.Id)?.TerminateReason ?? string.Empty,
                            TerminationDate = contractManOrg.Get(y.Id)?.TerminationDate.ToString() ?? string.Empty,
                            ContractFoundation = contractManOrg.Get(y.Id)?.Return(contract =>
                                {
                                    if (contract == null)
                                    {
                                        return string.Empty;
                                    }


                                    switch (contract.TypeManagement)
                                    {
                                        case TypeManagementManOrg.UK:
                                            var foundationOwners = (ManOrgContractOwnersFoundation?) manOrgContractOwnersDomain
                                                .GetAll()
                                                .Where(x => x.Id == contract.Id)
                                                .Select(x => x.ContractFoundation)
                                                .FirstOrDefault();
                                            return foundationOwners?.GetAttribute<DisplayAttribute>()?.Value ?? string.Empty;
                                        case TypeManagementManOrg.TSJ:
                                        case TypeManagementManOrg.JSK:
                                            var foundationManOrg = (ManOrgJskTsjContractFoundation?) manOrgContractJskTsjDomain
                                                .GetAll()
                                                .Where(x => x.Id == contract.Id)
                                                .Select(x => x.ContractFoundation)
                                                .FirstOrDefault();
                                            return foundationManOrg?.GetAttribute<DisplayAttribute>()?.Value ?? string.Empty;
                                        default:
                                            return string.Empty;
                                    }
                                }) ?? string.Empty,
                            DocumentName = contractManOrg.Get(y.Id)
                                ?.Return(contract =>
                                {
                                    if (contract == null)
                                    {
                                        return string.Empty;
                                    }

                                    if (contract.TypeManagement == TypeManagementManOrg.UK
                                        || contract.TypeManagement == TypeManagementManOrg.TSJ)
                                    {
                                        return contract.FileInfo?.Name;
                                    }

                                    if (contract.TypeManagement == TypeManagementManOrg.JSK)
                                    {
                                        return contract.DocumentName;
                                    }

                                    return string.Empty;
                                }) ?? string.Empty,
                            DataByRealityObject = new DataByRealityObject
                            {
                                Services = this.MergeServices(
                                    disclosureInfoRealityObj.PeriodId,
                                    new List<Dictionary<long, List<ServiceItem>>>
                                    {
                                        communalService,
                                        housingService,
                                        additionalService,
                                        controlService,
                                        repairService,
                                        capRepairService
                                    }),
                                NonResidentialPlace = new NonResidentialPlace
                                {
                                    NonResidentialPlacement =
                                        disclosureInfoRealityObj != null
                                            ? disclosureInfoRealityObj.NonResidentialPlacement == YesNoNotSet.Yes
                                                ? "Нет"
                                                : "Да"
                                            : "Не задано",
                                    NonResidentialPlaceItem = nonResidentialPlacement
                                        .Select(
                                            z => new NonResidentialPlaceItem
                                            {
                                                ContragentName = z.ContragentName,
                                                Area = z.Area.HasValue
                                                    ? z.Area.Value.RoundDecimal(2).ToString(numberformat)
                                                    : string.Empty,
                                                TypeContragentDi = z.TypeContragentDi.GetEnumMeta().Display.ToStr(),
                                                DocumentNumApartment = z.DocumentNumApartment,
                                                DocumentNumCommunal = z.DocumentNumCommunal,
                                                DateStart =
                                                    z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date
                                                        ? z.DateStart.Value.ToShortDateString()
                                                        : string.Empty,
                                                DateEnd =
                                                    z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                        ? z.DateEnd.Value.ToShortDateString()
                                                        : string.Empty,
                                                DocumentDateCommunal =
                                                    z.DocumentDateCommunal.HasValue
                                                    && z.DocumentDateCommunal.Value.Date != DateTime.MinValue.Date
                                                        ? z.DocumentDateCommunal.Value.ToShortDateString()
                                                        : string.Empty,
                                                DocumentDateApartment =
                                                    z.DocumentDateApartment.HasValue
                                                    && z.DocumentDateApartment.Value.Date != DateTime.MinValue.Date
                                                        ? z.DocumentDateApartment.Value.ToShortDateString()
                                                        : string.Empty,
                                                NonResidentialPlaceMetering =
                                                    nonResidentialPlaceMeteringDevice.ContainsKey(z.Id)
                                                        ? nonResidentialPlaceMeteringDevice[z.Id].ToArray()
                                                        : null
                                            })
                                        .ToArray()
                                },
                                UseCommonFacil = new UseCommonFacil
                                {
                                    PlaceGeneralUse =
                                        disclosureInfoRealityObj != null
                                            ? disclosureInfoRealityObj.PlaceGeneralUse == YesNoNotSet.Yes
                                                ? "Нет"
                                                : "Да"
                                            : "Не задано",
                                    InfoAboutUseCommonFacil = infoAboutCommonFacil
                                        .Where(
                                            z =>
                                                ((z.DateEnd.HasValue && z.DateEnd >= dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart) ||
                                                    !z.DateEnd.HasValue)
                                                && ((dictPeriod[disclosureInfoRealityObj.PeriodId].DateEnd.HasValue &&
                                                        dictPeriod[disclosureInfoRealityObj.PeriodId].DateEnd >= z.DateStart)
                                                    || !dictPeriod[disclosureInfoRealityObj.PeriodId].DateEnd.HasValue))
                                        .Select(
                                            z => new InfoAboutUseCommonFacilItem
                                            {
                                                Id = z.Id,
                                                KindCommomFacilities = z.KindCommomFacilities.ToStr(),
                                                Lessee = z.Lessee.ToStr(),
                                                DateStart =
                                                    z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date
                                                        ? z.DateStart.Value.ToShortDateString()
                                                        : string.Empty,
                                                DateEnd =
                                                    z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                        ? z.DateEnd.Value.ToShortDateString()
                                                        : string.Empty,
                                                CostContract =
                                                    z.CostByContractInMonth.HasValue
                                                        ? z.CostByContractInMonth.Value.RoundDecimal(2).ToString(numberformat)
                                                        : string.Empty,
                                                Number = z.Number.ToStr(),
                                                From =
                                                    z.From.HasValue && z.From.Value.Date != DateTime.MinValue.Date
                                                        ? z.From.Value.ToShortDateString()
                                                        : string.Empty,
                                                TypeContract = z.TypeContract.GetEnumMeta().Display.ToStr(),
                                                AreaOfCommonFacilities = z.AreaOfCommonFacilities?.RoundDecimal(2) ?? 0
                                            })
                                        .ToArray()
                                },
                                InfoAboutPay = this.MergeServices(
                                    disclosureInfoRealityObj.PeriodId,
                                    new List<Dictionary<long, List<InfoAboutPayItem>>>
                                    {
                                        infoAboutPaymentCommunal,
                                        infoAboutPaymentHousing
                                    }),
                                OtherService = otherService.ContainsKey(disclosureInfoRealityObj.PeriodId)
                                    ? otherService[disclosureInfoRealityObj.PeriodId].ToArray()
                                    : null,
                                OrderAndConditionService = new OrderAndConditionService
                                {
                                    Act = documents?.FileActState != null
                                        ? new DocumentRealObj
                                        {
                                            Id = documents.Id,
                                            FileId = documents.FileActState.Id,
                                            FileName = documents.FileActState.Name.ToStr()
                                        }
                                        : null,
                                    WorksList = documents?.FileCatalogRepair != null
                                        ? new DocumentRealObj
                                        {
                                            Id = documents.Id,
                                            FileId = documents.FileCatalogRepair.Id,
                                            FileName = documents.FileCatalogRepair.Name.ToStr()
                                        }
                                        : null,
                                    Documents = new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.Documents
                                    {
                                        DocumentReportOfPlan = documents?.FileReportPlanRepair != null
                                            ? new DocumentRealObj
                                            {
                                                Id = documents.Id,
                                                FileId = documents.FileReportPlanRepair.Id,
                                                FileName = documents.FileReportPlanRepair.Name.ToStr()
                                            }
                                            : null,
                                        DocumentCurrentYear = protocols.ContainsKey(currentYear) && protocols[currentYear].File != null
                                            ? new DocumentRealObj
                                            {
                                                Id = protocols[currentYear].Id,
                                                FileId = protocols[currentYear].File.Id,
                                                FileName = protocols[currentYear].File.Name.ToStr()
                                            }
                                            : null,

                                        DocumentPrevYear = protocols.ContainsKey(currentYear - 1) && protocols[currentYear - 1].File != null
                                            ? new DocumentRealObj
                                            {
                                                Id = protocols[currentYear - 1].Id,
                                                FileId = protocols[currentYear - 1].File.Id,
                                                FileName = protocols[currentYear - 1].File.Name.ToStr()
                                            }
                                            : null

                                    },
                                    PlanReductionExp =
                                        planReductionExpenseWorks.ContainsKey(disclosureInfoRealityObj.PeriodId)
                                            ? planReductionExpenseWorks[disclosureInfoRealityObj.PeriodId]
                                            : null,
                                    PlanWorkServiceRepWorks =
                                        planWorkServiceRepWorksItem.ContainsKey(disclosureInfoRealityObj.PeriodId)
                                            ? planWorkServiceRepWorksItem[disclosureInfoRealityObj.PeriodId]
                                            : null,
                                    ReductPayment = new ReductPayment
                                    {
                                        IsReductionPayment =
                                            disclosureInfoRealityObj != null
                                                ? disclosureInfoRealityObj.ReductionPayment.GetEnumMeta().Display
                                                : "Другое",
                                        InfoAboutReductPayment =
                                            infoAboutReductionPayment.ContainsKey(disclosureInfoRealityObj.PeriodId)
                                                ? infoAboutReductionPayment[disclosureInfoRealityObj.PeriodId]
                                                : null
                                    },
                                    InformationOnContracts = new InformationOnContracts
                                    {
                                        Count =
                                            informationOnContracts.Count(
                                                z =>
                                                    ((z.DateStart.HasValue && z.DateStart >= dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart
                                                            || !z.DateStart.HasValue)
                                                        && (z.DateStart.HasValue && dictPeriod[disclosureInfoRealityObj.PeriodId].DateEnd >= z.DateStart))
                                                    || ((z.DateStart.HasValue && dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart >= z.DateStart
                                                            || !z.DateStart.HasValue)
                                                        && (z.DateEnd.HasValue && z.DateEnd >= dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart
                                                            || !z.DateEnd.HasValue || z.DateEnd <= DateTime.MinValue))),
                                        InformationOnCont = informationOnContracts
                                            .Where(
                                                z =>
                                                    ((z.DateStart.HasValue && z.DateStart >= dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart
                                                            || !z.DateStart.HasValue)
                                                        && (z.DateStart.HasValue && dictPeriod[disclosureInfoRealityObj.PeriodId].DateEnd >= z.DateStart))
                                                    || ((z.DateStart.HasValue && dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart >= z.DateStart
                                                            || !z.DateStart.HasValue)
                                                        && (z.DateEnd.HasValue && z.DateEnd >= dictPeriod[disclosureInfoRealityObj.PeriodId].DateStart
                                                            || !z.DateEnd.HasValue || z.DateEnd <= DateTime.MinValue)))
                                            .Select(
                                                z => new InformationOnContItem
                                                {
                                                    Name = z.Name.ToStr(),
                                                    Cost =
                                                        z.Cost.HasValue
                                                            ? z.Cost.Value.RoundDecimal(2).ToString(numberformat)
                                                            : string.Empty,
                                                    Number = z.Number.ToStr(),
                                                    PartiesContract = z.PartiesContract.ToStr(),
                                                    Comments = z.Comments.ToStr(),
                                                    From =
                                                        z.From.HasValue && z.From.Value.Date != DateTime.MinValue.Date
                                                            ? z.From.Value.ToShortDateString()
                                                            : string.Empty,
                                                    DateStart =
                                                        z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date
                                                            ? z.DateStart.Value.ToShortDateString()
                                                            : string.Empty,
                                                    DateEnd =
                                                        z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date
                                                            ? z.DateEnd.Value.ToShortDateString()
                                                            : string.Empty
                                                })
                                            .ToArray()
                                    },
                                },
                                Claims = new[]
                                {
                                    new Claims
                                    {
                                        СlaimsReceivedCount = disclosureInfoRealityObj.ComServReceivedPretensionCount ?? 0,
                                        ClaimsSatisfiedCount = disclosureInfoRealityObj.ComServApprovedPretensionCount ?? 0,
                                        ClaimsDeniedCount = disclosureInfoRealityObj.ComServNoApprovedPretensionCount ?? 0,
                                        ProducedRecalculationAmount = disclosureInfoRealityObj.ComServPretensionRecalcSum ?? 0
                                    }
                                },
                                HouseMeteringDevice = houseMeteringDevice.ToArray(),
                                EngineeringSystem = engineeringSystem.ToArray()
                            },
                            HouseCommunalService = houseCommunalService.Get(disclosureInfoRealityObj.PeriodId)?.ToArray(),
                            HouseCommonProperty = houseCommonProperty.Get(disclosureInfoRealityObj.PeriodId)?.ToArray(),
                            HouseReportCommon = new[]
                            {
                                new HouseReportCommon
                                {
                                    CashBalanceBeginningPeriodConsumersOverpayment = disclosureInfoRealityObj.AdvancePayments?.RoundDecimal(2) ?? 0,
                                    CashBalanceBeginningPeriod = disclosureInfoRealityObj.CarryOverFunds?.RoundDecimal(2) ?? 0,
                                    CashBalanceBeginningPeriodConsumersArrears = disclosureInfoRealityObj.Debt?.RoundDecimal(2) ?? 0,
                                    ChargedForServices = disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsAll?.RoundDecimal(2) ?? 0,
                                    ChargedForMaintenanceOfHouse =
                                        disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsMaintanance?.RoundDecimal(2) ?? 0,
                                    ChargedForMaintenanceWork = disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsRepairs?.RoundDecimal(2) ?? 0,
                                    ChargedForManagementService = disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsManagement?.RoundDecimal(2) ?? 0,

                                    ReceivedCash = disclosureInfoRealityObj.ReceivedCashAll?.RoundDecimal(2) ?? 0,
                                    ReceivedCashFromOwners = disclosureInfoRealityObj.ReceivedCashFromOwners?.RoundDecimal(2) ?? 0,
                                    ReceivedTargetPaymentFromOwners = disclosureInfoRealityObj.ReceivedCashFromOwnersTargeted?.RoundDecimal(2) ?? 0,
                                    ReceivedSubsidies = disclosureInfoRealityObj.ReceivedCashAsGrant?.RoundDecimal(2) ?? 0,
                                    ReceivedFromUseOfCommonProperty = disclosureInfoRealityObj.ReceivedCashFromUsingCommonProperty?.RoundDecimal(2) ?? 0,
                                    ReceivedFromOther = disclosureInfoRealityObj.ReceivedCashFromOtherTypeOfPayments?.RoundDecimal(2) ?? 0,

                                    CashTotal = disclosureInfoRealityObj.CashBalanceAll?.RoundDecimal(2) ?? 0,
                                    CashBalanceEndingPeriodConsumersOverpayment =
                                        disclosureInfoRealityObj.CashBalanceAdvancePayments?.RoundDecimal(2) ?? 0,
                                    CashBalanceEndingPeriod = disclosureInfoRealityObj.CashBalanceCarryOverFunds?.RoundDecimal(2) ?? 0,
                                    CashBalanceEndingPeriodConsumersArrears = disclosureInfoRealityObj.CashBalanceDebt?.RoundDecimal(2) ?? 0,

                                    ClaimsReceivedCount = disclosureInfoRealityObj.ReceivedPretensionCount ?? 0,
                                    ClaimsSatisfiedCount = disclosureInfoRealityObj.ApprovedPretensionCount ?? 0,
                                    ClaimsDeniedCount = disclosureInfoRealityObj.NoApprovedPretensionCount ?? 0,
                                    ProducedRecalculationAmount = disclosureInfoRealityObj.PretensionRecalcSum?.RoundDecimal(2) ?? 0,

                                    SentClaimsCount = disclosureInfoRealityObj.SentPretensionCount ?? 0,
                                    FiledActionsCount = disclosureInfoRealityObj.SentPetitionCount ?? 0,
                                    ReceivedCashAmount = disclosureInfoRealityObj.ReceiveSumByClaimWork?.RoundDecimal(2) ?? 0,
                                    ComServStartAdvancePay = disclosureInfoRealityObj.ComServStartAdvancePay?.RoundDecimal(2) ?? 0,
                                    ComServStartCarryOverFunds = disclosureInfoRealityObj.ComServStartCarryOverFunds?.RoundDecimal(2) ?? 0,
                                    ComServStartDebt = disclosureInfoRealityObj.ComServStartDebt?.RoundDecimal(2) ?? 0,
                                    ComServEndAdvancePay = disclosureInfoRealityObj.ComServEndAdvancePay?.RoundDecimal(2) ?? 0,
                                    ComServEndCarryOverFunds = disclosureInfoRealityObj.ComServEndCarryOverFunds?.RoundDecimal(2) ?? 0,
                                    ComServEndDebt = disclosureInfoRealityObj.ComServEndDebt?.RoundDecimal(2) ?? 0,
                                    ComServReceivedPretensionCount = disclosureInfoRealityObj.ComServApprovedPretensionCount ?? 0,
                                    ComServApprovedPretensionCount = disclosureInfoRealityObj.ComServApprovedPretensionCount ?? 0,
                                    ComServNoApprovedPretensionCount = disclosureInfoRealityObj.ComServNoApprovedPretensionCount ?? 0,
                                    ComServPretensionRecalcSum = disclosureInfoRealityObj.ComServPretensionRecalcSum?.RoundDecimal(2) ?? 0,
                                }
                            }
                        })
                        .FirstOrDefault();

                    var managingOrgsList = managingOrgItems.ContainsKey(disclosureInfoRealityObj.PeriodId)
                        ? managingOrgItems[disclosureInfoRealityObj.PeriodId]
                        : new List<ManagingOrgItem>();
                    managingOrgsList.Add(managingOrgs);

                    managingOrgItems[disclosureInfoRealityObj.PeriodId] = managingOrgsList;
                }

                var realityObject = realityObjectDomain.GetAll().FirstOrDefault(x => x.Id == idHouse);
                var bothProtocolProxy = this.RealityObjectDecisionProtocolProxyService.GetBothProtocolProxy(realityObject);
                var typeOfFormingCr = this.GetTypeOfFormingCr(realityObject);
                var houseOverhaul = new List<HouseOverhaul>();

                if (bothProtocolProxy != null)
                {
                    var paymentAmountForms = this.RealityObjectDecisionProtocolProxyService.GetPaysize(bothProtocolProxy.Id, realityObject.Id, null);

                    houseOverhaul.Add(
                        new HouseOverhaul
                        {
                            CommonMeetingProtocolDate = bothProtocolProxy.ProtocolDate,
                            CommonMeetingProtocolNumber = bothProtocolProxy.ProtocolNumber,
                            PaymentAmountForms = paymentAmountForms ?? 0,
                            ProviderName = typeOfFormingCr.GetAttribute<DisplayAttribute>().Value
                        });
                }

                var documentBasedArea = this.GetDataFromPassport(realityObject, ServiceDi.TehPassportDocumentBasedArea);
                var parkingArea = this.GetDataFromPassport(realityObject, ServiceDi.TehPassportParkingArea);
                var energyEfficiency = this.GetDataFromPassport(realityObject, ServiceDi.EnergyEfficiency);
                var childrenArea = this.GetDataFromPassport(realityObject, ServiceDi.TehPassportChildrenArea);
                var sportArea = this.GetDataFromPassport(realityObject, ServiceDi.TehPassportSportArea);
                var basementArea = this.GetDataFromPassport(realityObject, ServiceDi.BasementAreaForm);

                var passport = this.Container.ResolveAll<IPassportProvider>()
                    .FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");

                var tehPassport = this.TehPassportValueServis
                    .GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == realityObject.Id)
                    .ToArray();

                //BasementType
                var basementTypeCodes =
                    passport.GetComponentBy(ServiceDi.BasementTypeFormId, ServiceDi.BasementTypeFormId) as ComponentTechPassport;

                var basementTypeValue =
                    tehPassport.Where(x => x.FormCode == "Form_5_1" && x.Value == "1")
                        .OrderByDescending(x => x.CellCode)
                        .FirstOrDefault();
                CellTechPassport basementType = null;
                if (basementTypeValue != null && basementTypeCodes != null)
                {
                    basementType =
                        basementTypeCodes.Cells.FirstOrDefault(
                            x => x.Code.Split(':').First() == basementTypeValue.CellCode.Split(':').First());
                }

                var facadeList = new List<Facade>();

                //Оштукатуренный
                var facadesPlastered =
                    tehPassport.FirstOrDefault(x => x.FormCode == ServiceDi.FacadesForm && x.CellCode == "23:1");
                if (facadesPlastered?.Value.ToDecimal() > 0)
                {
                    facadeList.Add(new Facade { FacadeType = "Оштукатуренный", EditDate = facadesPlastered.ObjectEditDate });
                }

                //Облицованный плиткой
                var facadesTiled = tehPassport.FirstOrDefault(
                    x => x.FormCode == ServiceDi.FacadesForm && x.CellCode == "26:1");
                if (facadesTiled?.Value.ToDecimal() > 0)
                {
                    facadeList.Add(new Facade { FacadeType = "Облицованный плиткой", EditDate = facadesTiled.ObjectEditDate });
                }

                //Окрашенный 
                var facadesPrinted = tehPassport.FirstOrDefault(x => x.FormCode == ServiceDi.FacadesForm && x.CellCode == "9:1");
                if (facadesPrinted?.Value.ToDecimal() > 0)
                {
                    facadeList.Add(new Facade { FacadeType = "Окрашенный", EditDate = facadesPrinted.ObjectEditDate });
                }

                //Сайдинг
                var facadesSiding = tehPassport.FirstOrDefault(x => x.FormCode == ServiceDi.FacadesForm && x.CellCode == "27:1");
                if (facadesSiding?.Value.ToDecimal() > 0)
                {
                    facadeList.Add(new Facade { FacadeType = "Сайдинг", EditDate = facadesSiding.ObjectEditDate });
                }

                //Соответствует материалу стен 
                var facadesTotalAreaFacade = tehPassport.FirstOrDefault(x => x.FormCode == ServiceDi.FacadesForm && x.CellCode == "22:1");

                //Не заполнено или Соответствует материалу стен(если указана общая площадь)
                if (!facadeList.Any())
                {
                    facadeList.Add(
                        facadesTotalAreaFacade?.Value.ToDecimal() > 0
                            ? new Facade { FacadeType = "Соответствует материалу стен" }
                            : new Facade { FacadeType = "Не заполнено" });
                }

                var facades = facadeList.AggregateWithSeparator(x => x.FacadeType, ", ");

                var roofTypes = new List<Roof>();
                var roofType = realityObject.TypeRoof.GetEnumMeta().Display;
                var roofingType = realityObject.RoofingMaterial.Return(x => x.Name);
                roofTypes.Add(new Roof { RoofType = roofType, RoofingType = roofingType });

                var roofs = roofTypes.AggregateWithSeparator(x => x.RoofType, ", ");

                var chutesNumber =
                    tehPassport.FirstOrDefault(
                        x => x.FormCode == ServiceDi.ChutesNumberForm.Item1 && x.CellCode == ServiceDi.ChutesNumberForm.Item2);

                // Периоды раскрытия инф-ии
                var periods = dictPeriod
                    .Select(
                        x =>
                            new PeriodItem
                            {
                                Id = x.Key,
                                Name = x.Value.Name.ToStr(),
                                Code = x.Key,
                                DateStart =
                                    x.Value.DateStart != DateTime.MinValue
                                        ? x.Value.DateStart.ToShortDateString()
                                        : string.Empty,
                                DateEnd =
                                    x.Value.DateEnd.HasValue && x.Value.DateEnd.Value.Date != DateTime.MinValue.Date
                                        ? x.Value.DateEnd.Value.ToShortDateString()
                                        : string.Empty,
                                ManagingOrgs = managingOrgItems.Keys.Contains(x.Key) ? managingOrgItems[x.Key].ToArray() : null,
                                HouseLift = this.RealtyObjectLiftService.GetRealtyObjectLift(realityObject.Id),
                                HouseOverhaul = houseOverhaul.ToArray()

                            }).ToArray();

                var realityObjectItem = new RealityObjectItem
                {
                    Periods = periods,
                    MoId = realityObject.Municipality.FiasId,
                    Municipality = realityObject.Municipality.Name.ToStr(),
                    Address = realityObject.FiasAddress.AddressName.ToStr(),
                    AddressKladr = realityObject.GkhCode.ToStr(),
                    ExpluatationYear = realityObject.DateCommissioning.HasValue ? realityObject.DateCommissioning.Value.Year.ToStr() : string.Empty,
                    Floor = realityObject.Floors.GetValueOrDefault(),
                    ApartamentCount = realityObject.NumberApartments.GetValueOrDefault(),
                    LivingPeople = realityObject.NumberLiving.GetValueOrDefault(),
                    Deterioration =
                        realityObject.PhysicalWear.HasValue ? realityObject.PhysicalWear.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                    YearCapRepair = realityObject.DateLastOverhaul.HasValue ? realityObject.DateLastOverhaul.Value.Year.ToStr() : string.Empty,
                    SeriaHouse = realityObject.SeriesHome.ToStr(),
                    Fasad = realityObject.HavingBasement.GetEnumMeta().Display,
                    GatesCount = realityObject.NumberEntrances.GetValueOrDefault(),
                    LivingNotLivingArea =
                        realityObject.AreaLivingNotLivingMkd.HasValue
                            ? realityObject.AreaLivingNotLivingMkd.Value.RoundDecimal(2).ToString(numberformat)
                            : string.Empty,
                    LivingAreaPeople =
                        realityObject.AreaLivingOwned.HasValue
                            ? realityObject.AreaLivingOwned.Value.RoundDecimal(2).ToString(numberformat)
                            : string.Empty,
                    LiftCount = realityObject.NumberLifts.GetValueOrDefault(),
                    FasadArea =
                        realityObject.AreaBasement.HasValue ? realityObject.AreaBasement.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                    RoofType = realityObject.TypeRoof.GetEnumMeta().Display,
                    RoofMaterial = realityObject.RoofingMaterial != null ? realityObject.RoofingMaterial.Name.ToStr() : string.Empty,
                    BuiltYear = realityObject.BuildYear.ToStr() ?? string.Empty,
                    HouseType = realityObject.TypeHouse.GetDisplayName(),
                    FloorCountMax = realityObject.Floors.GetValueOrDefault(),
                    FloorCountMin = realityObject.MaximumFloors.GetValueOrDefault(),
                    FlatsCount = (realityObject.NumberApartments ?? 0) + (realityObject.NumberNonResidentialPremises ?? 0),
                    NotLivingQuartersCount = realityObject.NumberNonResidentialPremises ?? 0,
                    GeneralArea = realityObject.AreaMkd.ToStr() ?? "0",
                    AreaResidential = realityObject.AreaLiving ?? 0,
                    AreaNonResidential = realityObject.AreaNotLivingPremises ?? 0,
                    AreaCommonProperty = realityObject.AreaNotLivingFunctional ?? 0,
                    CadastralNumbers = realityObject.CadastreNumber ?? string.Empty,
                    AreaLand = documentBasedArea.ToDecimal(),
                    ParkingSquare = parkingArea.ToDecimal(),
                    EnergyEfficiency = this.energyEfficiencyClassDict.Get(energyEfficiency ?? "", "не присвоен"),
                    HasPlayground = childrenArea != null && childrenArea != "0",
                    HasSportsground = sportArea != null && sportArea != "0",
                    MethodOfFormingOverhaulFund = typeOfFormingCr.GetAttribute<DisplayAttribute>().Value,
                    FoundationType = basementType != null ? basementType.Value : string.Empty,
                    AreaBasement = basementArea ?? string.Empty,
                    FloorType = this.GetCellValue(tehPassport, ServiceDi.TypeFloorsForm.Item1, ServiceDi.TypeFloorsForm.Item2, ref passport),
                    WallMaterial = this.GetCellValue(tehPassport, ServiceDi.TypeWallsForm.Item1, ServiceDi.TypeWallsForm.Item2, ref passport),
                    Facades = facades,
                    Roofs = roofs,
                    СhuteЕype = this.GetCellValue(tehPassport, ServiceDi.ConstrChuteForm.Item1, ServiceDi.ConstrChuteForm.Item2, ref passport),
                    СhuteСount = chutesNumber != null ? chutesNumber.Value : string.Empty
                };

                if (periods.Length != 0)
                {
                    return new GetManOrgRealtyObjectInfoResponse
                    {
                        Result = Result.NoErrors,
                        RealityObjectItem = realityObjectItem
                    };
                }
                else
                {
                    return new GetManOrgRealtyObjectInfoResponse
                    {
                        Result = Result.DataNotFound
                    };
                }
            }
            catch
            {
                return new GetManOrgRealtyObjectInfoResponse
                {
                    Result = Result.DataNotFound
                };
            }
            finally
            {
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(manOrgContractOwnersDomain);
                this.Container.Release(manOrgContractJskTsjDomain);
                this.Container.Release(disclosureInfoRealityObjDomain);
                this.Container.Release(informationOnContractsDomain);
                this.Container.Release(infoAboutReductionPaymentDomain);
                this.Container.Release(planReduceMeasureNameDomain);
                this.Container.Release(planReductionExpenseWorksDomain);
                this.Container.Release(workRepairDetailTatDomain);
                this.Container.Release(planWorkServiceRepairWorksDomain);
                this.Container.Release(repairServiceDomain);
                this.Container.Release(workRepairTechServDomain);
                this.Container.Release(documentsRealityObjDomain);
                this.Container.Release(documentsRealityObjProtocolDomain);
                this.Container.Release(otherServiceDomain);
                this.Container.Release(infoAboutPaymentHousingDomain);
                this.Container.Release(infoAboutPaymentCommunalDomain);
                this.Container.Release(infoAboutUseCommonFacilitiesDomain);
                this.Container.Release(nonResidentialPlacementDomain);
                this.Container.Release(tariffForRsoDomain);
                this.Container.Release(tariffForConsumersDomain);
                this.Container.Release(providerServiceDomain);
                this.Container.Release(communalServiceDomain);
                this.Container.Release(housingServiceDomain);
                this.Container.Release(capRepairServiceDomain);
                this.Container.Release(additionalServiceDomain);
                this.Container.Release(controlServiceDomain);
                this.Container.Release(disclosureInfoRelationDomain);
                this.Container.Release(realityObjectDomain);
                this.Container.Release(nonResidentialPlacementMeteringDeviceDomain);
                this.Container.Release(dictPeriodDomain);
                this.Container.Release(manOrgDomain);
                this.Container.Release(tariffForConsumersOtherServiceDomain);
            }
        }

        private string GetFiles(B4.Modules.FileStorage.FileInfo fileInfo)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var result = fileInfo != null ? fileManager.GetBase64String(fileInfo) : string.Empty;

                return result;
            }
            catch (FileNotFoundException)
            {
                return string.Empty;
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }

        private T[] MergeServices<T>(long periodId, IEnumerable<Dictionary<long, List<T>>> serviceDict)
        {
            var list = new List<T>();

            foreach (var serviceDictitem in serviceDict)
            {
                if (serviceDictitem.ContainsKey(periodId))
                {
                    list.AddRange(serviceDictitem[periodId]);
                }
            }

            return list.ToArray();
        }

        private string GetDataFromPassport(RealityObject ro, Tuple<string, string> key)
        {
            string result = null;

            var techPassportService = this.Container.Resolve<ITechPassportService>();
            var data = techPassportService.GetValue(ro.Id, key.Item1, key.Item2);
            if (data != null)
            {
                result = data.Value;
            }

            return result;
        }

        private CrFundFormationType GetTypeOfFormingCr(RealityObject ro)
        {
            var typeOfFormingCrProvider = this.Container.Resolve<ITypeOfFormingCrProvider>();
            return typeOfFormingCrProvider.GetTypeOfFormingCr(ro);
        }

        private string GetCellValue(IEnumerable<TehPassportValue> tehPassportRow, string formCode, string cellCode, ref IPassportProvider passport)
        {
            TehPassportValue passportValue = tehPassportRow.FirstOrDefault(x => x.FormCode == formCode && x.CellCode == cellCode);
            if (passportValue != null)
            {
                return passport.GetTextForCellValue(formCode, cellCode, passportValue.Value);
            }
            return string.Empty;
        }

        private HouseMeteringDevice[] GetHouseMeteringDevice(long id)
        {
            var device = this.DisclosureInfoRealityObjService.GetRealtyObjectDevices(id).Data as List<DisclosureInfoRealityObjService.RealtyObjectDevice>;

            var houseMeteringDevice = device.Where(x => x.Number != null)
                .Select(
                    x => new HouseMeteringDevice
                    {
                        CommunalResourceType = x.TypeCommResourse,
                        Availability = x.ExistMeterDevice,
                        MeterType = x.InterfaceType,
                        UnitOfMeasurement = x.UnutOfMeasure,
                        CommissioningDate = x.InstallDate,
                        CalibrationDate = x.CheckDate
                    })
                    .ToArray();

            return houseMeteringDevice;
        }
    }

    public class Facade
    {
        public string FacadeType { get; set; }

        [JsonIgnore]
        public DateTime EditDate { get; set; }
    }

    public class Roof
    {
        public string RoofType { get; set; }

        public string RoofingType { get; set; }
    }
}