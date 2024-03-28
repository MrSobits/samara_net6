namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Calculating;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using NHibernate.Linq;

    /// <summary>
    /// Методы для подсчёта раскрытия информации по дому
    /// </summary>
    public partial class PercentCalculation988
    {
        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }
        public IDomainService<HousingService> HousingServiceDomain { get; set; }
        public IDomainService<CommunalService> CommunalServiceDomain { get; set; }
        public IDomainService<CapRepairService> CapRepairServiceDomain { get; set; }
        public IDomainService<ConsumptionNormsNpa> ConsumptionNormsNpaDomain { get; set; }
        public IDomainService<CostItem> CostItemDomain { get; set; }
        public IDomainService<WorkCapRepair> WorkCapRepairDomain { get; set; }
        public IDomainService<DocumentsRealityObjProtocol> DocumentsRealityObjProtocolDomain { get; set; }
        public IDomainService<ManOrgContractRelation> ManOrgContractRelationDomain { get; set; }
        public IDomainService<TemplateService> TemplateServiceDomain { get; set; }
        public IRealityObjectDecisionProtocolProxyService RoDecisionProtocolProxyService { get; set; }
        public ITypeOfFormingCrProvider TypeOfFormingCrProvider { get; set; }
        public IServService ServService { get; set; }

        protected HashSet<long> ChildrenAreaTp;
        protected HashSet<long> SportAreaTp;
        protected HashSet<long> HasRoofTypeRoIds;

        protected Dictionary<long, StructElementsTechPassportProxy> StructElementsTp;
        protected Dictionary<long, EngineerSystemsProxy> EngineerSystemsDict;

        protected Dictionary<long, List<LiftDiProxy>> LiftsDict; 
        protected Dictionary<long, int> LiftsCountDict;

        protected Dictionary<long, MeteringDeviceDiProxy[]> MeteringDeviceDict;

        protected readonly string[] FormCodesStructElements = { "Form_3_7_2", "Form_3_7_3", "Form_5_1", "Form_5_2", "Form_5_3", "Form_5_4", "Form_5_8" };
        protected readonly string[] FacadesTypeCellCodes = {"23:1", "26:1", "9:1", "27:1", "22:1"};

        protected virtual int[] CalculableCommunalServices { get; set; } = { };
        protected virtual int[] RequiredCommunalServices { get; set; } = { };
        protected virtual int[] RequiredHousingService { get; set; } = { };
        protected virtual int[] CalculableHousingServices { get; set; } = { };

        protected IDictionary<long, string[]> unfilledMandatoryServsNameList;

        protected readonly Dictionary<string, string> EngineerSystems = new Dictionary<string, string>
        {
            // Счетчик тепловой энергии
            { "Form_3_1", "1:3" },
            // Счетчик ГВС
            { "Form_3_2", "1:3" },
            // Счетчик газоснабжения
            { "Form_3_4", "1:3" },
            // Вентиляция
            { "Form_3_5", "1:3" },
            // Пожаротушение
            { "Form_3_8", "1:3" },
            // Водостоки
            { "Form_3_6", "1:3" },
            // Счетчик электроэнергии
            { "Form_3_3", "1:3" },
            // Электроснабжение
            { "Form_3_3_3", "15:1" },
            // Счетчик ХВС
            { "Form_3_2_CW", "1:3" },
            // Водоотведение (канализация)
            { "Form_3_3_Water", "1:3" }
        };

        protected Dictionary<string, DisclosureInfoRealityObjEmptyFields> FinActivityHouseRequiredProperties { get; }
            = new Dictionary<string, DisclosureInfoRealityObjEmptyFields>
            {
                {
                    "AdvancePayments",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Авансовые платежи потребителей (на начало периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "CarryOverFunds",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Переходящие остатки денежных средств (на начало периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "CashBalanceAdvancePayments",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Авансовые платежи потребителей (на конец периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "CashBalanceAll",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Всего денежных средств с учетом остатков",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "CashBalanceCarryOverFunds",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Переходящие остатки денежных средств (на конец периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "CashBalanceDebt",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Задолженность потребителей (на конец периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ChargeForMaintenanceAndRepairsAll",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Начислено за услуги (работы) по содержанию и текущему ремонту",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ChargeForMaintenanceAndRepairsMaintanance",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Начислено за содержание дома",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ChargeForMaintenanceAndRepairsManagement",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Начислено за услуги управления",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ChargeForMaintenanceAndRepairsRepairs",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Начислено за текущий ремонт",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ComServApprovedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество удовлетворенных претензий",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServEndAdvancePay",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Авансовые платежи потребителей (на конец периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServEndCarryOverFunds",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Переходящие остатки денежных средств (на конец периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServEndDebt",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Задолженность потребителей (на конец периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServNoApprovedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество претензий, в удовлетворении которых отказано",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServPretensionRecalcSum",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Сумма произведенного перерасчета",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServReceivedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество поступивших претензий",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServStartAdvancePay",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Авансовые платежи потребителей (на начало периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServStartCarryOverFunds",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Переходящие остатки денежных средств (на начало периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "ComServStartDebt",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Задолженность потребителей (на начало периода)",
                        PathId = DiFieldPathType.Path4
                    }
                },
                {
                    "Debt",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Задолженность потребителей (на начало периода)",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashAll",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено денежных средств всего",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashAsGrant",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено субсидий",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashFromOtherTypeOfPayments",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Прочие поступления",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashFromOwners",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено денежных средств от собственников/нанимателей помещений",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashFromOwnersTargeted",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено целевых взносов от собственников/нанимателей помещений",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ReceivedCashFromUsingCommonProperty",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено денежных средств от использования общего имущества",
                        PathId = DiFieldPathType.Path5
                    }
                },
                {
                    "ApprovedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество удовлетворенных претензий",
                        PathId = DiFieldPathType.Path6
                    }
                },
                {
                    "NoApprovedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество претензий, в удовлетворении которых отказано",
                        PathId = DiFieldPathType.Path6
                    }
                },
                {
                    "PretensionRecalcSum",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Сумма произведенного перерасчета",
                        PathId = DiFieldPathType.Path6
                    }
                },
                {
                    "ReceivedPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество поступивших претензий",
                        PathId = DiFieldPathType.Path6
                    }
                },
                {
                    "ReceiveSumByClaimWork",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Получено денежных средств по результатам претензионно-исковой работы",
                        PathId = DiFieldPathType.Path7
                    }
                },
                {
                    "SentPetitionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Направлено исковых заявлений",
                        PathId = DiFieldPathType.Path7
                    }
                },
                {
                    "SentPretensionCount",
                    new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Направлено претензий потребителям-должникам",
                        PathId = DiFieldPathType.Path7
                    }
                }
            };

        protected Dictionary<long, ServiceProxy988[]> ServicesDict;
        protected Dictionary<long, ProviderService> ProviderServices; 
        protected Dictionary<long, CommunalServiceDiProxy> CommunalServices;
        protected Dictionary<long, IEnumerable<TariffForRsoDiProxy>> TariffForRsoDict;
        protected Dictionary<long, List<ConsumptionNormsNpa>> ConsumptionNormsNpaDict;

        protected Dictionary<long, TypeOfProvisionServiceDi> HousingServiceProvision;
        protected Dictionary<long, decimal?> RepairWorkPlanSum; 
        protected Dictionary<long, IEnumerable<decimal?>> HousingSum; 
        protected Dictionary<long, IEnumerable<decimal?>> CapRepairSum; 
        protected Dictionary<long, IEnumerable<decimal?>> ManagingSum; 
        protected Dictionary<long, InfoAboutUseCommonFacilitiesProxy[]> InfoAboutUseCommonFacilitiesDict;
        protected Dictionary<long, DocumentsRealityObjDiProxy> DocumentsRealityObjDiProxyDict; 
        protected Dictionary<long, List<DocumentsRealityObjProtocol>> DocumentsRealityObjProtocolDict;

        protected Dictionary<long, ManOrgBaseContract> ManorgContracts;
        protected Dictionary<long, ManOrgBaseContract> ManorgChildContracts;
        protected Dictionary<long, RealityObjectDecisionProtocolProxy> RealityObjectDecisionProtocolDict;

        protected virtual void PrepareRealityObjectCache(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            var periodDi = this.period;

            this.CalculableCommunalServices = this.TemplateServiceDomain.GetAll()
                .Where(x => x.TypeGroupServiceDi == TypeGroupServiceDi.Communal && x.IsConsiderInCalc)
                .Where(x => (x.ActualYearStart.HasValue && periodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                    && (x.ActualYearEnd.HasValue && periodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                .Select(x => x.Code.ToInt()).ToArray();

            this.RequiredCommunalServices = this.TemplateServiceDomain.GetAll()
                .Where(x => x.KindServiceDi == KindServiceDi.Communal && x.IsMandatory &&
                    (x.ActualYearStart.HasValue && periodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                    && (x.ActualYearEnd.HasValue && periodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                .Select(x => x.Code.ToInt()).ToArray();

            this.CalculableHousingServices = this.TemplateServiceDomain.GetAll()
                .Where(x => x.TypeGroupServiceDi == TypeGroupServiceDi.Housing && x.IsConsiderInCalc)
                .Where(x => (x.ActualYearStart.HasValue && periodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                    && (x.ActualYearEnd.HasValue && periodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                .Select(x => x.Code.ToInt()).ToArray();

            this.RequiredHousingService = this.TemplateServiceDomain.GetAll()
                .Where(x => x.KindServiceDi != KindServiceDi.Communal && x.KindServiceDi != KindServiceDi.Additional)
                .Where(x => x.IsMandatory &&
                    (x.ActualYearStart.HasValue && periodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                    && (x.ActualYearEnd.HasValue && periodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                .Select(x => x.Code.ToInt())
                .ToArray();

            this.ChildrenAreaTp = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_2")
                .Where(x => x.CellCode == "9:3")
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .Select(x => x.TehPassport.RealityObject.Id)
                .ToHashSet();

            this.SportAreaTp = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_2")
                .Where(x => x.CellCode == "10:3")
               .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
               .Select(x => x.TehPassport.RealityObject.Id)
               .ToHashSet();

            this.StructElementsTp = this.TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .Where(x => this.FormCodesStructElements.Contains(x.FormCode))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => new StructElementsTechPassportProxy
                    {
                        HasBasementType = y.Any(x => x.FormCode == "Form_5_1" && x.Value.IsNotEmpty()),
                        HasBasementArea = y.Any(x => x.FormCode == "Form_5_4" && x.CellCode == "9:4" && x.Value != null),
                        HasTypeWalls = y.Any(x => x.FormCode == "Form_5_2" && x.CellCode == "1:3" && x.Value != null),
                        HasConstructionChute = y.Any(x => x.FormCode == "Form_3_7_2" && x.CellCode == "1:3" && x.Value != null),
                        ChutesNumber = y.FirstOrDefault(x => x.FormCode == "Form_3_7_3" && x.CellCode == "5:1")?.Value.ToIntNullable(),
                        HasFacade = y.Any(x => x.FormCode == "Form_5_8" && this.FacadesTypeCellCodes.Contains(x.CellCode) && x.Value != null),
                        HasTypeFloor = y.Any(x => x.FormCode == "Form_5_3" && x.CellCode == "1:3" && x.Value != null),
                    });

            this.EngineerSystemsDict = this.TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .Where(x => this.EngineerSystems.Keys.Contains(x.FormCode))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Where(x => this.EngineerSystems.Any(e => e.Key == x.FormCode && e.Value == x.CellCode))
                        .Aggregate(new EngineerSystemsProxy(),
                            (proxy, el) =>
                                EngineerSystemsProxy.InitEngineerSystemsProxy(proxy, el.FormCode, el.CellCode, el.Value)
                            ));

            this.LiftsCountDict = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_4_2")
                .Where(x => x.CellCode == "1:1")
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .AsEnumerable()
                 .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.First().Value.ToInt());

            this.LiftsDict = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_4_2_1")
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => int.Parse(x.CellCode.Split(':').First())).ToDictionary(x => x.Key, x => x.ToList())
                    .Select(
                        x => new LiftDiProxy
                        {
                            EntranceNumber = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":1")?.Value,
                            CommissioningYear = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":9")?.Value,
                            Type = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":19")?.Value
                        }).ToList());

            this.MeteringDeviceDict = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_6_6_2")
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => int.Parse(x.CellCode.Split(':').First()))
                        .ToDictionary(x => x.Key, x => x.ToList())
                        .Select(
                            x => new MeteringDeviceDiProxy
                            {
                                ExistMeterDevice = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":2")?.Value.ToInt(),
                                InterfaceType = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":3")?.Value.ToInt(),
                                UnutOfMeasure = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":4")?.Value.ToInt(),
                                InstallDate = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":5")?.Value.ToDateTimeNullable(),
                                CheckDate = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":6")?.Value.ToDateTimeNullable(),
                            }).ToArray());

            this.ServicesDict = this.BaseServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x =>
                    (x.TemplateService.ActualYearStart.HasValue &&
                        x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year >= x.TemplateService.ActualYearStart.Value ||
                        !x.TemplateService.ActualYearStart.HasValue)
                    && (x.TemplateService.ActualYearEnd.HasValue &&
                        x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year <= x.TemplateService.ActualYearEnd.Value ||
                        !x.TemplateService.ActualYearEnd.HasValue))
                .Select(
                    x => new
                    {
                        DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                        x.Id,
                        x.TemplateService.Code,
                        x.TemplateService.Name,
                        x.TemplateService.KindServiceDi,
                        RealObjId = x.DisclosureInfoRealityObj.RealityObject.Id,
                        AnyProvider = this.ProviderServiceDomain.GetAll().Any(y => y.BaseService.Id == x.Id),
                        x.Provider,
                        x.TariffForConsumers,
                        x.ScheduledPreventiveMaintanance
                    })
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObjId)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x => new ServiceProxy988
                        {
                            Id = x.Id,
                            Code = x.Code.ToInt(),
                            Name = x.Name,
                            KindServiceDi = x.KindServiceDi,
                            DiRealObjId = x.DisclosureInfoRealityObjId,
                            RealObjId = x.RealObjId,
                            HasProvider = x.AnyProvider,
                            TariffForConsumers = x.TariffForConsumers,
                            ScheduledPreventiveMaintanance = x.ScheduledPreventiveMaintanance
                        })
                        .Where(x => this.CalculableCommunalServices.Contains(x.Code) || this.CalculableHousingServices.Contains(x.Code))
                        .ToArray());

            this.CommunalServices = this.CommunalServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id,
                    x => new CommunalServiceDiProxy
                    {
                        ProvisionService = x.TypeOfProvisionService,
                        UnitMeasure = x.UnitMeasure?.Name,
                        UnitMeasureLivingHouseName = x.UnitMeasureLivingHouse?.Name,
                        ConsumptionNormLivingHouse = x.ConsumptionNormLivingHouse,
                        PricePurchasedResources = x.PricePurchasedResources
                    });

            this.ProviderServices = this.ProviderServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateStartContract).FirstOrDefault());

            this.TariffForRsoDict = this.TariffForRsoDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x => new TariffForRsoDiProxy
                        {
                            DateNormativeLegalAct = x.DateNormativeLegalAct,
                            NumberNormativeLegalAct = x.NumberNormativeLegalAct,
                            OrganizationSetTariff = x.OrganizationSetTariff,
                            DateStart = x.DateStart
                        }));

            this.ConsumptionNormsNpaDict = this.ConsumptionNormsNpaDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            this.HousingServiceProvision = new Dictionary<long, TypeOfProvisionServiceDi>();

            this.HousingServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().TypeOfProvisionService)
                .ForEach(x => this.HousingServiceProvision.Add(x.Key, x.Value));

            this.CapRepairServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().TypeOfProvisionService)
                .ForEach(x => this.HousingServiceProvision.Add(x.Key, x.Value));

            this.RepairServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().TypeOfProvisionService)
                .ForEach(x => this.HousingServiceProvision.Add(x.Key, x.Value));

            this.RepairWorkPlanSum = this.RepairServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .ToDictionary(x => x.Id, x => x.SumWorkTo);

            this.HousingSum = this.CostItemDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Sum));

            this.CapRepairSum = this.WorkCapRepairDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.PlannedCost));

            this.ManagingSum = this.TariffForConsumersDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Cost));

            this.InfoAboutUseCommonFacilitiesDict = this.InfoAboutUseCommonFacilitiesDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => periodDi != null
                               && (((periodDi.DateStart.HasValue && x.DateStart >= periodDi.DateStart.Value
                                     || !periodDi.DateStart.HasValue)
                                    && (periodDi.DateEnd.HasValue && periodDi.DateEnd.Value >= x.DateStart
                                        || !periodDi.DateEnd.HasValue))
                                   || ((periodDi.DateStart.HasValue && periodDi.DateStart.Value >= x.DateStart)
                                       && (periodDi.DateStart.HasValue && x.DateEnd >= periodDi.DateStart.Value
                                           || x.DateEnd <= DateTime.MinValue))))
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x => new InfoAboutUseCommonFacilitiesProxy
                        {
                            AppointmentCommonFacilities = x.AppointmentCommonFacilities,
                            AreaOfCommonFacilities = x.AreaOfCommonFacilities,
                            KindCommomFacilities = x.KindCommomFacilities,
                            Inn = x.Inn,
                            Ogrn = x.Ogrn,
                            ContractDate = x.ContractDate,
                            ContractNumber = x.ContractNumber,
                            CostByContractInMonth = x.CostByContractInMonth,
                            LesseeTypeDi = x.LesseeType
                        })
                        .ToArray());

            this.DocumentsRealityObjDiProxyDict = this.DocumentsRealityObjDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .ToDictionary(
                    x => x.DisclosureInfoRealityObj.Id,
                    x => new DocumentsRealityObjDiProxy
                    {
                        DescriptionActState = x.DescriptionActState,
                        HasFileActState = x.FileActState != null,
                        HasFileCatalogRepair = x.FileCatalogRepair != null,
                        HasFileReportPlanRepair = x.FileReportPlanRepair != null,
                        HasGeneralMeetingOfOwners = x.HasGeneralMeetingOfOwners
                    });

            this.DocumentsRealityObjProtocolDict = this.DocumentsRealityObjProtocolDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            this.ManorgContracts = this.moRoQuery
                .Fetch(x => x.ManOrgContract)
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        Contract = x.ManOrgContract
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Contract).OrderByDescending(x => x.DocumentDate).FirstOrDefault());

            var today = DateTime.UtcNow;
            var periodStart = this.period.DateStart ?? new DateTime(today.Year, 1, 1);
            var periodEnd = this.period.DateStart ?? new DateTime(today.Year, 12, 31);

            this.ManorgChildContracts = this.ManOrgContractRelationDomain.GetAll()
                .Where(x => this.moRoQuery.Any(y => y.ManOrgContract.Id == x.Parent.Id))
                .Where(x =>
                    x.Children.StartDate == null && x.Children.EndDate != null && x.Children.EndDate.Value.Date >= periodStart ||
                    x.Children.StartDate != null && x.Children.EndDate == null && x.Children.StartDate.Value.Date <= periodEnd ||
                    x.Children.StartDate != null && x.Children.EndDate != null && x.Children.StartDate.Value.Date <= periodEnd && x.Children.EndDate.Value.Date >= periodStart)
                .GroupBy(x => x.Parent.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Children).OrderByDescending(x => x.DocumentDate).First());

            if (this.RoDecisionProtocolProxyService.IsNotNull())
            {
                this.RealityObjectDecisionProtocolDict = this.RoDecisionProtocolProxyService.GetBothProtocolProxy(diRoQuery.Select(x => x.RealityObject));
            }

            this.unfilledMandatoryServsNameList = this.ServService.GetUnfilledMandatoryServsNameList(diRoQuery);
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям о доме
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateRealityObjectGeneralDataPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 18,
                completePositionsCount = 0;

            if (realObj.BuildYear.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Год постройки",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.DateCommissioning.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Дата ввода в эксплуатацию",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.FiasAddress.IsNotNull() && realObj.FiasAddress.AddressName.IsNotEmpty())
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Адрес МКД",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.TypeProject.IsNotEmpty())
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Серия, тип постройки здания",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.TypeHouse != TypeHouse.NotSet)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип дома",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.Floors.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во этажей наименьшее",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.MaximumFloors.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во этажей наибольшее",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberEntrances.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во подъездов",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberLifts.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во лифтов",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AllNumberOfPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во помещений всего",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberApartments.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во жилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberNonResidentialPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во нежилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaMkd.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь дома",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaLiving.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь жилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaNotLivingPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь нежилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaOfAllNotLivingPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь помещений, входящих в состав общего имущества",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (this.ChildrenAreaTp.Contains(realObj.RoId))
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Детская площадка",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (this.SportAreaTp.Contains(realObj.RoId))
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Спортивная площадка",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            // Сведения о фонде КР, если есть модуль регфонд (иначе словарь будет null)
            if (this.RealityObjectDecisionProtocolDict.IsNotNull())
            {
                var type = this.GetAccountFormationType(realObj);
                if (type != CrFundFormationType.Unknown && type != CrFundFormationType.NotSelected)
                {
                    positionsCount += 3;
                    var bothProtocolProxy = this.RealityObjectDecisionProtocolDict.Get(realObj.RoId);
                    if (bothProtocolProxy.IsNotNull())
                    {
                        if (bothProtocolProxy.ProtocolDate.IsValid())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата протокола общего собрания собственников помещения",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (bothProtocolProxy.ProtocolNumber.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Номер протокола общего собрания собственников помещения",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (bothProtocolProxy.CrPaySize != null)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Размер взноса на капитальный ремонт",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        // Если спец. счет
                        if (type != CrFundFormationType.RegOpAccount)
                        {
                            positionsCount += 2;

                            // ИНН владельца специального счета
                            if (bothProtocolProxy.RegOpContragentInn.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "ИНН владельца",
                                    PathId = DiFieldPathType.Path1,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            // Наименование владельца специального счета
                            var name = string.Empty;
                            if (type == CrFundFormationType.SpecialRegOpAccount)
                            {
                                name = bothProtocolProxy.RegOpContragentName;
                            }
                            else if (type == CrFundFormationType.SpecialAccount)
                            {
                                name = this.ManorgContracts.Get(realObj.RoId)?.ManagingOrganization.Contragent.Name;
                            }

                            if (name.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Владелец специального счета",
                                    PathId = DiFieldPathType.Path1,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }
                        }
                    }
                } else if (type == CrFundFormationType.NotSelected && realObj.ConditionHouse == ConditionHouse.Emergency)
                {
                    positionsCount += 3;
                    completePositionsCount += 3;
                }
            }

            var percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "RealityObjectGeneralDataPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по конструктивным элементам дома
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateStructElementsPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int tempPositionsCount = 9,
                tempCompletePositionsCount = 1;

            var structElementsTp = this.StructElementsTp.Get(realObj.RoId);
            if (structElementsTp.IsNotNull())
            {
                if (structElementsTp.HasBasementType)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Тип фундамента",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (structElementsTp.HasBasementArea)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Площадь подвала по полу",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (structElementsTp.ChutesNumber.HasValue)
                {
                    tempCompletePositionsCount++;

                    // если количество мусоропроводов = 0, то не учитываем тип мусоропровода
                    if (structElementsTp.ChutesNumber > 0)
                    {
                        if (structElementsTp.HasConstructionChute)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Тип мусоропровода",
                                PathId = DiFieldPathType.Path9,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    }
                    else
                    {
                        tempCompletePositionsCount++;
                    }
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Количество мусоропроводов",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (structElementsTp.HasFacade)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Тип фасада",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (structElementsTp.HasTypeWalls)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Материал несущих стен",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (structElementsTp.HasTypeFloor)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Тип перекрытий",
                        PathId = DiFieldPathType.Path9,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }
            }

            if (realObj.RoofingMaterial.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип кровли",
                    PathId = DiFieldPathType.Path9,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            return new DiRealObjPercent
            {
                Code = "StructElementsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по инжинерным системам дома
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateEngineerSystemsPercent(DisclosureInfoRealityObjProxy realObj)
        {
            const int tempPositionsCount = 10;
            var tempCompletePositionsCount = 0;
            var engineerSystems = this.EngineerSystemsDict.Get(realObj.RoId);

            if (engineerSystems?.HasHeatingForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы теплоснабжения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasHotWaterForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы горячего водоснабжения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasColdWaterForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы холодного водоснабжения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasGasForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы газоснабжения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasVentilationForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы вентиляции",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasFirefightingForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы пожаротушения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasDrainageForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы водостоков",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasPowerForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы электроснабжения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasPowerPointsForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во вводов в дом",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (engineerSystems?.HasSewageForm ?? false)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип системы водоотведения",
                    PathId = DiFieldPathType.Path8,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            return new DiRealObjPercent
            {
                Code = "EngineerSystemsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по лифтам
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateLiftsPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int tempPositionsCount = 0,
                tempCompletePositionsCount = 0;

            decimal? percent;

            var liftsCount = this.LiftsCountDict.Get(realObj.RoId);
            if (liftsCount > 0)
            {
                var addedLifts = this.LiftsDict.Get(realObj.RoId, Enumerable.Empty<LiftDiProxy>());
                // на каждый лифт 3 обязательных поля (либо смотрим на поле в техпаспорте, если лифтов добавлено больше, то на них)
                tempPositionsCount = Math.Max(liftsCount, addedLifts.Count()) * 3;

                foreach (var lift in addedLifts)
                {
                    if (lift.EntranceNumber.IsNotEmpty())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Номер подъезда",
                            PathId = DiFieldPathType.Path10,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }

                    if (lift.CommissioningYear.IsNotEmpty())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Год ввода в эксплуатацию",
                            PathId = DiFieldPathType.Path10,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }

                    if (lift.Type.IsNotEmpty())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Тип лифта",
                            PathId = DiFieldPathType.Path10,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
            }
            else
            {
                percent = 100;
            }

            return new DiRealObjPercent
            {
                Code = "LiftsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по приборам учёта
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateRealtyObjectDevicesPercent(DisclosureInfoRealityObjProxy realObj)
        {
            // 6 - количество приборов учёта и 5 - количество обязательных полей
            int tempPositionsCount = 6 * 5,
                tempCompletePositionsCount = 0;

            var meteringDeviceDiProxies = this.MeteringDeviceDict.Get(realObj.RoId);
            if (meteringDeviceDiProxies.IsNotEmpty())
            {
                var completedDevices = (meteringDeviceDiProxies?.Where(meteringDeviceDiProxy => meteringDeviceDiProxy.ExistMeterDevice > 0))
                    .AllOrEmpty()
                    .ToList();

                tempCompletePositionsCount = completedDevices.Count;

                foreach (var meteringDeviceDiProxy in completedDevices)
                {
                    if (meteringDeviceDiProxy.ExistMeterDevice == 3)
                    {
                        if (meteringDeviceDiProxy.InterfaceType > 0)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Тип интерфейса",
                                PathId = DiFieldPathType.Path14,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (meteringDeviceDiProxy.UnutOfMeasure > 0)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Единица измерения",
                                PathId = DiFieldPathType.Path14,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (meteringDeviceDiProxy.InstallDate.HasValue)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата ввода в эксплуатацию",
                                PathId = DiFieldPathType.Path14,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (meteringDeviceDiProxy.CheckDate.HasValue)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата поверки/замены прибора учета",
                                PathId = DiFieldPathType.Path14,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    } else if (meteringDeviceDiProxy.ExistMeterDevice == null || meteringDeviceDiProxy.ExistMeterDevice == 0)
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Наличие прибора учета",
                            PathId = DiFieldPathType.Path14,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }
                    else
                    {
                        tempCompletePositionsCount += 4;
                    }
                }
            }

            var percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "RealtyObjectDevicesPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по жилищным и коммунальным услугам
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateServicesPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = this.RequiredCommunalServices.Length + this.RequiredHousingService.Length,
                completePositionsCount = 0;

            if (this.ServicesDict.ContainsKey(realObj.Id))
            {
                var services = this.ServicesDict.Get(realObj.Id).Where(this.IsServiceCalculable).ToList();
                completePositionsCount += services.DistinctBy(x => x.Code)
                    .Count(x => this.RequiredCommunalServices.Contains(x.Code) || this.RequiredHousingService.Contains(x.Code));

                // у нас могут быть несчитаемые обязательные услуги(лифт - 2 и мусоропровод - 1),
                // здесь мы проверяем, не добавились ли они в процессе проверки this.IsServiceCalculable
                positionsCount -= this.ServicePercents.Count(x => this.ServicesDict.Get(realObj.Id).DistinctBy(y => y.Code).Any(y => y.Id == x.Service.Id));
                positionsCount -= this.CountNotRequiredHousingServices(this.ServicesDict.Get(realObj.Id).ToList(), realObj);

                foreach (var service in services)
                {
                    ServicePercent diPercent = null;
                    if (service.KindServiceDi == KindServiceDi.Communal)
                    {
                        diPercent = this.ValidateCommunalService(service);
                    }
                    else if (service.KindServiceDi == KindServiceDi.Housing ||
                        service.KindServiceDi == KindServiceDi.CapitalRepair ||
                        service.KindServiceDi == KindServiceDi.Repair ||
                        service.KindServiceDi == KindServiceDi.Managing)
                    {
                        diPercent = this.ValidateHousingService(service);
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = $"Тип предоставления услуги({service.Name})",
                            PathId = DiFieldPathType.Path18,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }

                    if (diPercent.IsNotNull())
                    {
                        completePositionsCount += diPercent.CompletePositionsCount;
                        positionsCount += diPercent.PositionsCount;
                        this.ServicePercents.Add(diPercent);
                    }
                }

                if (positionsCount > completePositionsCount)
                {
                    var emptyServiceNames = this.unfilledMandatoryServsNameList.Get(realObj.Id);

                    foreach (var name in emptyServiceNames)
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = $"Тип предоставления услуги({name})",
                            PathId = DiFieldPathType.Path18,
                            RealityObj = this.GetRealityObjectProxy(realObj.Id)
                        });
                    }
                }
            }

            return new DiRealObjPercent
            {
                Code = "ServicesPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.Service,
                Percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по планам работ по содержанию и ремонту
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculatePlanWorkServiceRepairPercent(DisclosureInfoRealityObjProxy realObj)
        {
            return new DiRealObjPercent
            {
                Code = "PlanWorkServiceRepairPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по планам работ по сведениям об использовании мест общего пользования
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculatePlaceGeneralUsePercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            if (realObj.PlaceGeneralUse == YesNoNotSet.No)
            {
                completePositionsCount++;
            }
            else if (realObj.PlaceGeneralUse == YesNoNotSet.Yes)
            {
                var infoAboutUseCommonFacilities = this.InfoAboutUseCommonFacilitiesDict.Get(realObj.Id);
                if (infoAboutUseCommonFacilities.IsNotEmpty())
                {
                    completePositionsCount++;

                    foreach (var info in infoAboutUseCommonFacilities)
                    {
                        positionsCount += 3;

                        if (info.AppointmentCommonFacilities.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Назначение общего имущества",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (info.AreaOfCommonFacilities.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Площадь общего имущества",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (info.KindCommomFacilities.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Наименование общего имущества",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    }
                }
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Договоры на использование мест общего пользования имеются",
                    PathId = DiFieldPathType.Path16,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            return new DiRealObjPercent
            {
                Code = "PlaceGeneralUsePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по документам дома
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateDocumentsDiRealObjPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 4,
                completePositionsCount = 0;

            decimal percent = 0;

            var docs = this.DocumentsRealityObjDiProxyDict.Get(realObj.Id);
            if (docs.IsNotNull())
            {
                if (docs.HasGeneralMeetingOfOwners == YesNoNotSet.No)
                {
                    percent = 100;
                    completePositionsCount = 4;
                }
                else if (docs.HasGeneralMeetingOfOwners == YesNoNotSet.Yes)
                {
                    var docProtocols = this.DocumentsRealityObjProtocolDict.Get(realObj.Id);
                    var year = this.period.DateStart?.Year ?? DateTime.UtcNow.Year;

                    if (docProtocols.IsNotEmpty())
                    {
                        completePositionsCount = 1;

                        var maxPositions = 0;

                        foreach (var docProtocol in docProtocols.Where(x => x.Year == year).DistinctBy(x => x.Year))
                        {
                            var curPositions = 0;

                            if (docProtocol.DocDate.HasValue)
                            {
                                curPositions++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Дата протокола общего собрания собственников помещений",
                                    PathId = DiFieldPathType.Path3,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (docProtocol.DocNum.IsNotEmpty())
                            {
                                curPositions++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Номер протокола общего собрания собственников помещений",
                                    PathId = DiFieldPathType.Path3,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (docProtocol.File.IsNotNull())
                            {
                                curPositions++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Файл",
                                    PathId = DiFieldPathType.Path3,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (curPositions > maxPositions)
                            {
                                maxPositions = curPositions;
                            }
                        }

                        completePositionsCount += maxPositions;
                    }

                    percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Проводились ли общие собрания собственников помещений в многоквартирном доме с участием управляющей организации после 01.12.2014г?",
                        PathId = DiFieldPathType.Path3,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }
            }

            return new DiRealObjPercent
            {
                Code = "DocumentsDiRealObjPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по управлению домом
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateHouseManagingPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 0,
                completePositionsCount = 0;

            decimal? percent = null;

            var manOrgContract = this.ManorgContracts.Get(realObj.RoId);
            if (manOrgContract.IsNotNull() && 
                manOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
            {
                positionsCount = 7;
                completePositionsCount = 3;

                if (manOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK ||
                    manOrgContract.DocumentName.IsNotEmpty() ||
                    manOrgContract.FileInfo?.Name != null)
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Документ, подтверждающий выбранный способ управления",
                        PathId = DiFieldPathType.Path37,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (manOrgContract.StartDate.HasValue)
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Дата начала управления",
                        PathId = DiFieldPathType.Path37,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (manOrgContract.DocumentDate.HasValue)
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Дата документа",
                        PathId = DiFieldPathType.Path37,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (manOrgContract.DocumentNumber.IsNotEmpty())
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Номер документа",
                        PathId = DiFieldPathType.Path37,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (manOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                {
                    var actualContract = this.ManorgChildContracts.Get(manOrgContract.Id) as ManOrgContractTransfer;
                    if (actualContract.IsNotNull())
                    {
                        positionsCount += 3;
                        if (actualContract.DocumentDate.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата документа",
                                PathId = DiFieldPathType.Path37,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (actualContract.StartDate.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата начала управления",
                                PathId = DiFieldPathType.Path37,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (actualContract.ProtocolFileInfo.IsNotNull())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Документ, подтверждающий выбранный способ управления",
                                PathId = DiFieldPathType.Path37,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    }
                }

                percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
            }

            return new DiRealObjPercent
            {
                Code = "HouseManagingPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по снижению расходов
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculatePlanReductionExpensePercent(DisclosureInfoRealityObjProxy realObj)
        {
            return new DiRealObjPercent
            {
                Code = "PlanReductionExpensePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по сведениям о случаях снижения платы
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateInfoAboutReductionPaymentPercent(DisclosureInfoRealityObjProxy realObj)
        {
            return new DiRealObjPercent
            {
                Code = "InfoAboutReductionPaymentPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по сведениям об использовании нежилых помещений
        /// </summary>
        /// <param name="realityObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateNonResidentialPlacementPercent(DisclosureInfoRealityObjProxy realityObj)
        {
            return new DiRealObjPercent
            {
                Code = "NonResidentialPlacementPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realityObj)
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по финансовой активности по дому
        /// </summary>
        /// <param name="realityObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateFinActivityHousePercent(DisclosureInfoRealityObjProxy realityObj)
        {
            var typeOfProxy = typeof(DisclosureInfoRealityObjProxy);

            int positionsCount = this.FinActivityHouseRequiredProperties.Count;
            int completePositionsCount = 0;

            foreach (var property in this.FinActivityHouseRequiredProperties)
            {
                if (typeOfProxy.GetProperty(property.Key).GetValue(realityObj) != null)
                {
                    completePositionsCount++;
                }
                else
                {
                    var diRealityObjEmptyField = property.Value;
                    diRealityObjEmptyField.RealityObj = this.GetRealityObjectProxy(realityObj.Id);
                    this.EmptyFieldsLogger.Add(diRealityObjEmptyField);
                }
            }

            return new DiRealObjPercent
            {
                Code = "FinActivityHousePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realityObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по сведениям об оплатах коммунальных услуг
        /// </summary>
        /// <param name="realityObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DiRealObjPercent CalculateCommunalServicesPaymentPercent(DisclosureInfoRealityObjProxy realityObj)
        {
            return null;
        }

        /// <summary>
        /// Проверяет услугу на необходимость в проведении расчётов.
        /// <remarks>Если услуга не нуждается в расчётах, то метод добавляет для неё null-проценты</remarks>
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Необходимость в расчётах</returns>
        protected virtual bool IsServiceCalculable(ServiceProxy service)
        {
            var serviceCalculable = false;
            if (service.KindServiceDi == KindServiceDi.Communal)
            {
                serviceCalculable = this.IsCommunalServiceCalculable(service);
            }
            else if (service.KindServiceDi == KindServiceDi.Housing || 
                service.KindServiceDi == KindServiceDi.CapitalRepair || 
                service.KindServiceDi == KindServiceDi.Repair ||
                service.KindServiceDi == KindServiceDi.Managing)
            {
                serviceCalculable = this.IsHousingServiceCalculable(service);
            }

            if (!serviceCalculable && this.ServicePercents.All(x => x.Service.Id != service.Id))
            {
                this.AddServiceNullPercent(service.Id);
            }

            return serviceCalculable;
        }

        /// <summary>
        /// Возвращает количество обязательных услуг, которые не нужно считать по каким-либо условиям и они не были добавлены
        /// </summary>
        /// <param name="services">Услуги дома</param>
        /// /// <param name="realityObj">Дом</param>
        /// <returns>Количество</returns>
        protected virtual int CountNotRequiredHousingServices(List<ServiceProxy988> services, DisclosureInfoRealityObjProxy realityObj)
        {
            return 0;
        }

        /// <summary>
        /// Проверяет коммунальную услугу на необходимость в проведении расчётов
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Необходимость в расчётах</returns>
        protected virtual bool IsCommunalServiceCalculable(ServiceProxy service)
        {
            return this.CalculableCommunalServices.Contains(service.Code);
        }

        /// <summary>
        /// Проверяет жилищную услугу на необходимость в проведении расчётов
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Необходимость в расчётах</returns>
        protected virtual bool IsHousingServiceCalculable(ServiceProxy service)
        {
            return this.CalculableHousingServices.Contains(service.Code);
        }

        /// <summary>
        /// Проверка раскрытия процентов по коммунальной услуге
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Блок процента</returns>
        protected virtual ServicePercent ValidateCommunalService(ServiceProxy service)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            decimal percent;

            var communalService = this.CommunalServices.Get(service.Id);
            var serviceProvisionType = communalService?.ProvisionService;
            if (serviceProvisionType == TypeOfProvisionServiceDi.ServiceNotAvailable)
            {
                completePositionsCount++;
            }
            else
            {
                // два заполненых поля: Тип услуги и тип предоставления
                positionsCount = 2;
                completePositionsCount = 2;

                positionsCount += 3;

                var serviceProvider = this.ProviderServices.Get(service.Id);
                if (serviceProvider.IsNotNull())
                {
                    completePositionsCount++;

                    if (serviceProvider.DateStartContract.HasValue)
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Дата заключения договора",
                            PathId = DiFieldPathType.Path19,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }

                    if (serviceProvider.NumberContract.IsNotEmpty())
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Номер договора",
                            PathId = DiFieldPathType.Path19,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }

                if (serviceProvisionType == TypeOfProvisionServiceDi.ServiceProvidedMo)
                {
                    positionsCount += 2;

                    if (communalService.IsNotNull())
                    {
                        if (communalService.UnitMeasure.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Единица измерения",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (communalService.PricePurchasedResources.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Цена закупаемых ресурсов (тариф)",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }
                    }
                }

                positionsCount += 2;
                if (communalService.IsNotNull())
                {
                    if (communalService.ConsumptionNormLivingHouse >= 0)
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "(Нормативы потребления) Норматив потребления коммунальной услуги в жилых помещениях",
                            PathId = DiFieldPathType.Path19,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }

                    if (communalService.UnitMeasureLivingHouseName.IsNotEmpty())
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "(Нормативы потребления) Единица измерения норматива потребления коммунальной услуги",
                            PathId = DiFieldPathType.Path19,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }

                positionsCount += 4;
                var tariffRso = this.TariffForRsoDict.Get(service.Id);
                if (tariffRso.IsNotEmpty())
                {
                    var maxPositionCount = 0;
                    int curPositionCount;

                    foreach (var tariffForRsoDiProxy in tariffRso)
                    {
                        curPositionCount = 0;
                        if (tariffForRsoDiProxy.NumberNormativeLegalAct.IsNotEmpty())
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(Тарифы для РСО) Номер нормативного правового акта",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (tariffForRsoDiProxy.DateNormativeLegalAct.HasValue)
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(Тарифы для РСО) Дата нормативного правового акта",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (tariffForRsoDiProxy.OrganizationSetTariff.IsNotEmpty())
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(Тарифы для РСО) Организация установившая тариф",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (tariffForRsoDiProxy.DateStart.HasValue)
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(Тарифы для РСО) Дата начала действия тарифа",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (curPositionCount > maxPositionCount)
                        {
                            maxPositionCount = curPositionCount;
                        }
                    }

                    completePositionsCount += maxPositionCount;
                }

                positionsCount += 3;
                var consumptionNormsNpa = this.ConsumptionNormsNpaDict.Get(service.Id);
                if (consumptionNormsNpa.IsNotEmpty())
                {
                    var maxPositionCount = 0;
                    int curPositionCount;

                    foreach (var normsNpa in consumptionNormsNpa)
                    {
                        curPositionCount = 0;
                        if (normsNpa.NpaAcceptor.IsNotEmpty())
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(НПА нормативов потребления) Наименование принявшего акт органа",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (normsNpa.NpaDate.HasValue)
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(НПА нормативов потребления) Дата",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (normsNpa.NpaNumber.IsNotEmpty())
                        {
                            curPositionCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "(НПА нормативов потребления) Номер нормативно правового акта",
                                PathId = DiFieldPathType.Path19,
                                RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                            });
                        }

                        if (curPositionCount > maxPositionCount)
                        {
                            maxPositionCount = curPositionCount;
                        }
                    }

                    completePositionsCount += maxPositionCount;
                }
            }

            percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
            return new ServicePercent
            {
                Code = "ServicePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.Service,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                Service = new BaseService {Id = service.Id}
            };
        }

        /// <summary>
        /// Проверка раскрытия процентов по жилищной услуге
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Блок процента</returns>
        protected virtual ServicePercent ValidateHousingService(ServiceProxy988 service)
        {
            int positionsCount = 1,
                completePositionsCount;


            var typeProvision = this.HousingServiceProvision.Get(service.Id);

            // учитывать 100%, если по жилищным услугам тип предоставления=сосбтвенники отказались от предоставления услуги, 
            // а для всех остальных услуг тип предоставления = услуга не предоставляется
            if (service.KindServiceDi == KindServiceDi.Housing && typeProvision == TypeOfProvisionServiceDi.OwnersRefused ||
                service.KindServiceDi != KindServiceDi.Housing && typeProvision == TypeOfProvisionServiceDi.ServiceNotAvailable)
            {
                completePositionsCount = 1;
            }
            else
            {
                // Тип предоставления услуги = 1 из 1
                positionsCount = 2;
                completePositionsCount = 1;

                if (service.KindServiceDi == KindServiceDi.Repair)
                {
                    if(this.RepairWorkPlanSum.Get(service.Id).HasValue)
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                        {
                            FieldName = "Тарифы",
                            PathId = DiFieldPathType.Path42,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }
                else if (service.KindServiceDi == KindServiceDi.Housing)
                {
                    if (this.HousingSum.Get(service.Id).ReturnSafe(x => x.Any(y => y.HasValue)))
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                        {
                            FieldName = "Статьи затрат",
                            PathId = DiFieldPathType.Path43,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }
                else if (service.KindServiceDi == KindServiceDi.CapitalRepair)
                {
                    if (this.CapRepairSum.Get(service.Id).ReturnSafe(x => x.Any(y => y.HasValue)))
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                        {
                            FieldName = "Работы",
                            PathId = DiFieldPathType.Path41,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }
                else if (service.KindServiceDi == KindServiceDi.Managing)
                {
                    positionsCount++;

                    if (service.TariffForConsumers.HasValue)
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                        {
                            FieldName = "Тарифы",
                            PathId = DiFieldPathType.Path44,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }

                    if (this.ManagingSum.Get(service.Id).ReturnSafe(x => x.Any(y => y.HasValue)))
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                        {
                            FieldName = "Тарифы",
                            PathId = DiFieldPathType.Path44,
                            RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                        });
                    }
                }
            }

            var percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
            return new ServicePercent
            {
                Code = "ServicePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.Service,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                Service = new BaseService { Id = service.Id }
            };
        }

        /// <summary>
        /// Акцессор для получения способа формирования фонда
        /// </summary>
        protected virtual CrFundFormationType GetAccountFormationType(DisclosureInfoRealityObjProxy diRo)
        {
            return diRo.AccountFormationVariant ?? CrFundFormationType.Unknown;
        }

        protected DisclosureInfoRealityObj GetDisclosureInfoRealityObj(DisclosureInfoRealityObjProxy proxy)
        {
            return new DisclosureInfoRealityObj
            {
                Id = proxy.Id,
                RealityObject = new RealityObject { Id = proxy.RoId }
            };
        }
    }
}