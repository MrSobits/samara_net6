namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;
    using Castle.Windsor;

    using NCalc;

    using NHibernate;

    /// <summary>
    /// Сервис для работы с ДПКР
    /// </summary>
    public class LongProgramService : ILongProgramService
    {
        private Dictionary<long, Dictionary<int, IEnumerable<HmaoWorkPrice>>> workPricesByMu;

        private Dictionary<long, Dictionary<long, string>> dictStructElWork;

        private WorkpriceMoLevel? workpriceMoLevel;

        private readonly IQueryable<RealityObjectStructuralElement> roStructElQuery = null;

        private readonly Dictionary<long, Dictionary<long, List<long>>> allRealEstateTypes = new Dictionary<long, Dictionary<long, List<long>>>();

        private OverhaulHmaoConfig overhaulHmaoConfig;

        private WorkPriceDetermineType? workPriceDetermineType;

        private int? periodStartYear;
        private int? endYear;
        private bool? isWorkPriceFirstYear;

        /// <summary>
        /// Домен-сервис <see cref="DpkrCorrectionStage2" />
        /// </summary>
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectedDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ProgramVersion" />
        /// </summary>
        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublishedProgram" />
        /// </summary>
        public IDomainService<PublishedProgram> PublishedProgramDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublishedProgramRecord" />
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectStructuralElement" />
        /// </summary>
        public IDomainService<RealityObjectStructuralElement> RoStructElDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State" />
        /// </summary>
        public IDomainService<State> StateDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality" />
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="EmergencyObject" />
        /// </summary>
        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }

        /// <summary>
        /// Сервис рабоыт с муниципалилтетами
        /// </summary>
        public IMunicipalityService MunicipalityService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="HmaoWorkPrice" />
        /// </summary>
        public IDomainService<HmaoWorkPrice> WorkPriceService { get; set; }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DpkrCorrectedDomain" />
        /// </summary>
        public IDomainService<StructuralElementWork> StructElementWorkService { get; set; }

        /// <summary>
        /// Словарь расценок
        /// </summary>
        public Dictionary<long, Dictionary<int, IEnumerable<HmaoWorkPrice>>> WorkPricesByMu
        {
            get
            {
                return this.workPricesByMu ??
                    (this.workPricesByMu = this.WorkPriceService.GetAll()
                        .GroupBy(x => x.Municipality.Id)
                        .ToDictionary(
                            x => x.Key,
                            y => y.GroupBy(x => x.Year)
                                .ToDictionary(x => x.Key, z => z.AsEnumerable())));
            }
        }

        /// <summary>
        /// Словарь конструктивов
        /// </summary>
        public Dictionary<long, Dictionary<long, string>> DictStructElWork
        {
            get
            {
                return this.dictStructElWork ??
                    (this.dictStructElWork = this.StructElementWorkService.GetAll()
                        .Select(
                            x => new
                            {
                                SeId = x.StructuralElement.Id,
                                JobId = x.Job.Id,
                                JobName = x.Job.Name
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.SeId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.JobId).ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First())));
            }
        }

        /// <summary>
        /// Уровень муниципального образования
        /// </summary>
        public WorkpriceMoLevel? WorkpriceMoLevel
        {
            get
            {
                return
                    this.workpriceMoLevel ?? (this.workpriceMoLevel =
                        (WorkpriceMoLevel) this.GkhParams.GetParams().GetAs<int>("WorkPriceMoLevel")).Value;
            }
        }

        /// <summary>
        /// Сервис для работы с типами дома
        /// </summary>
        public IRealEstateTypeService RealEstService { get; set; }

        /// <summary>
        /// Сервис для работы с конструктивами дома
        /// </summary>
        public IRealityObjectStructElementService RoStrElService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObject" />
        /// </summary>
        public IDomainService<RealityObject> RoDomain { get; set; }

        /// <summary>
        /// Настройки ДПКР
        /// </summary>
        public OverhaulHmaoConfig OverhaulHmaoConfig
        {
            get { return this.overhaulHmaoConfig ?? (this.overhaulHmaoConfig = this.Container.GetGkhConfig<OverhaulHmaoConfig>()); }
        }

        /// <summary>
        /// ЖКХ параметры
        /// </summary>
        public IGkhParams GkhParams { get; set; }

        /// <summary>
        /// Определение расценки по работам при расчете ДПКР (с учетом группы капитальности или без учета)
        /// </summary>
        public WorkPriceDetermineType WorkPriceDetermineType
        {
            get { return this.workPriceDetermineType ?? (this.workPriceDetermineType = this.OverhaulHmaoConfig.WorkPriceDetermineType).Value; }
        }

        /// <summary>
        /// Начало периода
        /// </summary>
        public int PeriodStartYear
        {
            get { return (this.periodStartYear ?? (this.periodStartYear = this.OverhaulHmaoConfig.ProgrammPeriodStart)).Value; }
        }

        /// <summary>
        /// Окончание периода
        /// </summary>
        public int EndYear
        {
            get { return this.endYear ?? (this.endYear = this.OverhaulHmaoConfig.ProgrammPeriodEnd).Value; }
        }

        /// <summary>
        /// Первый год в расценках
        /// </summary>
        public bool IsWorkPriceFirstYear
        {
            get
            {
                return this.isWorkPriceFirstYear ?? (this.isWorkPriceFirstYear = this.OverhaulHmaoConfig.WorkPriceCalcYear == WorkPriceCalcYear.First).Value;
            }
        }

        /// <summary>
        /// Создать ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public IDataResult MakeLongProgram(BaseParams baseParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;
            var stage2Period = config.GroupByCeoPeriod;
            var useLimitCost = config.HouseAddInProgramConfig.UseLimitCost;

            var muId = baseParams.Params.GetAs<long>("muId");

            if (muId < 1)
            {
                return new BaseDataResult(false, "Не выбрано муниципальное образование");
            }

            if (stage2Period < 1)
            {
                return new BaseDataResult(false, "Не задан параметр \"Период группировки КЭ\"");
            }

            // Проверяем параметры на валидность
            if (startYear == 0 || endYear == 0 || startYear >= endYear)
            {
                return new BaseDataResult(false, "Не задан параметр \"Период долгосрочной программы\"");
            }

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                var listStage1ToSave = this.GetStage1(startYear, muId);

                var listStage2ToSave = this.GetStage2(listStage1ToSave);

                var listStage3ToSave = this.GetStage3(listStage2ToSave);

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // удаляем существующие записи
                        session.CreateSQLQuery(
                            "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG " +
                                "where id in (SELECT s1.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG s1 " +
                                "inner JOIN OVRHL_RO_STRUCT_EL ro_se ON s1.RO_SE_ID = ro_se.ID " +
                                "inner JOIN GKH_REALITY_OBJECT ro ON ro_se.RO_ID = ro.ID " +
                                "WHERE (ro.municipality_id = :municipalityId OR ro.stl_municipality_id = :municipalityId))")
                            .SetInt64("municipalityId", muId)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(
                            "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 " +
                                "WHERE ID IN (SELECT s2.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 s2 " +
                                "inner JOIN GKH_REALITY_OBJECT ro ON s2.RO_ID = ro.ID " +
                                "WHERE (ro.municipality_id = :municipalityId OR ro.stl_municipality_id = :municipalityId))")
                            .SetInt64("municipalityId", muId)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(
                            "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 " +
                                "WHERE ID IN (SELECT s3.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 s3 " +
                                "inner JOIN GKH_REALITY_OBJECT ro ON s3.RO_ID = ro.ID " +
                                "WHERE (ro.municipality_id = :municipalityId OR ro.stl_municipality_id = :municipalityId))")
                            .SetInt64("municipalityId", muId)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(
                            "DELETE FROM OVRHL_MISS_DPKR_REC " +
                                "WHERE ID IN (SELECT MDR.ID FROM OVRHL_MISS_DPKR_REC MDR " +
                                "inner JOIN GKH_REALITY_OBJECT ro ON MDR.RO_ID = ro.ID " +
                                "WHERE (ro.municipality_id = :municipalityId OR ro.stl_municipality_id = :municipalityId))")
                            .SetInt64("municipalityId", muId)
                            .ExecuteUpdate();

                        /* если учитываем предельную стоимость работ, то
                         получаем года в которые, сумма превысила предельную стоимость.
                         Не сохраняем записи, которые превысили, а так же другие записи по данному дому после превышения*/
                        if (useLimitCost == TypeUsage.Used)
                        {
                            var missYearByRoDict = this.GetMinMissingYearsByLimitCost(new List<long> {muId}, listStage3ToSave, session);

                            listStage3ToSave.ForEach(
                                x =>
                                {
                                    if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Year)
                                    {
                                        session.Insert(x);
                                    }
                                });

                            listStage2ToSave.ForEach(
                                x =>
                                {
                                    if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Stage3.Year)
                                    {
                                        session.Insert(x);
                                    }
                                });
                            listStage1ToSave.ForEach(
                                x =>
                                {
                                    if (!missYearByRoDict.ContainsKey(x.Stage2.RealityObject.Id)
                                        || missYearByRoDict[x.Stage2.RealityObject.Id] > x.Stage2.Stage3.Year)
                                    {
                                        session.Insert(x);
                                    }
                                });
                        }
                        else
                        {
                            // сохраняем новые записи
                            listStage3ToSave.ForEach(x => session.Insert(x));
                            listStage2ToSave.ForEach(x => session.Insert(x));
                            listStage1ToSave.ForEach(x => session.Insert(x));
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        /// <summary>
        /// Создать все ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые программы</param>
        public IDataResult MakeLongProgramAll(BaseParams baseParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;
            var stage2Period = config.GroupByCeoPeriod;
            var useLimitCost = config.HouseAddInProgramConfig.UseLimitCost;

            if (stage2Period < 1)
            {
                return new BaseDataResult(false, "Не задан параметр \"Период группировки КЭ\"");
            }

            // Проверяем параметры на валидность
            if (startYear == 0 || endYear == 0 || startYear >= endYear)
            {
                return new BaseDataResult(false, "Не задан параметр \"Период долгосрочной программы\"");
            }

            var gkhParams = this.Container.Resolve<IGkhParams>().GetParams();

            var moLevel = (MoLevel) gkhParams.GetAs<int>("MoLevel");

            var muIds = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Level
                    })
                .ToList()
                .Where(x => x.Level.ToMoLevel(this.Container) == moLevel)
                .Select(x => x.Id).ToList();

            var stage1ToSave = new List<RealityObjectStructuralElementInProgramm>();
            var stage2ToSave = new List<RealityObjectStructuralElementInProgrammStage2>();
            var stage3ToSave = new List<RealityObjectStructuralElementInProgrammStage3>();

            var message = string.Empty;

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            foreach (var muId in muIds)
            {
                try
                {
                    var listStage1ToSave = this.GetStage1(startYear, muId);
                    var listStage2ToSave = this.GetStage2(listStage1ToSave);
                    var listStage3ToSave = this.GetStage3(listStage2ToSave);

                    stage1ToSave.AddRange(listStage1ToSave);
                    stage2ToSave.AddRange(listStage2ToSave);
                    stage3ToSave.AddRange(listStage3ToSave);
                }
                catch (ValidationException e)
                {
                    message += e.Message;
                }
            }

            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // удаляем существующие записи
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_MISS_DPKR_REC").ExecuteUpdate();

                    /* если учитываем предельную стоимость работ, то
                         получаем года в которые, сумма превысила предельную стоимость.
                         Не сохраняем записи, которые превысили, а так же другие записи по данному дому после превышения*/
                    if (useLimitCost == TypeUsage.Used)
                    {
                        var missYearByRoDict = this.GetMinMissingYearsByLimitCost(muIds, stage3ToSave, session);

                        stage3ToSave.ForEach(
                            x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Year)
                                {
                                    session.Insert(x);
                                }
                            });

                        stage2ToSave.ForEach(
                            x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Stage3.Year)
                                {
                                    session.Insert(x);
                                }
                            });

                        stage1ToSave.ForEach(
                            x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.Stage2.RealityObject.Id)
                                    || missYearByRoDict[x.Stage2.RealityObject.Id] > x.Stage2.Stage3.Year)
                                {
                                    session.Insert(x);
                                }
                            });
                    }
                    else
                    {
                        // сохраняем новые записи
                        stage3ToSave.ForEach(x => session.Insert(x));
                        stage2ToSave.ForEach(x => session.Insert(x));
                        stage1ToSave.ForEach(x => session.Insert(x));
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(true, string.Empty);
        }

        /// <summary>
        /// Получить стоимость ДПКР
        /// </summary>
        public decimal? GetDpkrCost(
            long muId,
            long? settlementId,
            int workPriceYear,
            long strElemId,
            long roId,
            long capitalGroupId,
            PriceCalculateBy calculateBy,
            decimal objVolume,
            decimal? areaLiving,
            decimal? areaMkd,
            decimal? areaLivingNotLivingMkd)
        {
            workPriceYear = this.IsWorkPriceFirstYear ? this.PeriodStartYear : workPriceYear;

            Dictionary<int, IEnumerable<HmaoWorkPrice>> workPricesByYear;

            // берем расценку по району
            if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.MunicipalUnion)
            {
                workPricesByYear = this.WorkPricesByMu.Get(muId);
            }
            else if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.Settlement)
            {
                // берем расценку по мо
                workPricesByYear = this.WorkPricesByMu.Get(settlementId.ToLong());
            }
            else
            {
                // берем расценку по мо, если  ее нет - по району
                workPricesByYear = this.WorkPricesByMu.Get(settlementId.ToLong()) ?? this.WorkPricesByMu.Get(muId);
            }

            if (workPricesByYear == null)
            {
                return null;
            }

            var jobs = this.DictStructElWork.ContainsKey(strElemId)
                ? this.DictStructElWork[strElemId]
                : new Dictionary<long, string>();

            var dictRealEstTypes = this.GetRealEstateTypes(muId);
            var realEstTypes = dictRealEstTypes.ContainsKey(roId)
                ? dictRealEstTypes[roId]
                : null;

            var workPrices = this.GetWorkPrices(
                workPricesByYear,
                workPriceYear,
                jobs.Keys.AsEnumerable(),
                this.WorkPriceDetermineType,
                capitalGroupId,
                realEstTypes);

            var costSum = this.GetDpkrCostByYear(workPrices, calculateBy);

            var volume = 0M;

            switch (calculateBy)
            {
                case PriceCalculateBy.Volume:
                    volume = objVolume;
                    break;
                case PriceCalculateBy.LivingArea:
                    volume = areaLiving.HasValue ? areaLiving.Value : 0m;
                    break;
                case PriceCalculateBy.TotalArea:
                    volume = areaMkd.HasValue ? areaMkd.Value : 0m;
                    break;
                case PriceCalculateBy.AreaLivingNotLivingMkd:
                    volume = areaLivingNotLivingMkd.HasValue ? areaLivingNotLivingMkd.Value : 0m;
                    break;
            }

            if (costSum.HasValue)
            {
                return (costSum.ToDecimal() * volume).RoundDecimal(2);
            }

            return null;
        }

        /// <summary>
        /// Получить 1 этап
        /// </summary>
        public IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(
            int startYear,
            long municipalityId,
            IQueryable<RealityObjectStructuralElement> roStructElQuery = null,
            int? newEndYear = null,
            bool fromActualize = false)
        {
            if (newEndYear.HasValue)
            {
                this.endYear = newEndYear.Value;
            }

            var valuesService = this.Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
            var formulaService = this.Container.Resolve<IFormulaService>();

            try
            {
                var servicePercent = this.OverhaulHmaoConfig.ServiceCost;

                var listStage1ToSave = new List<RealityObjectStructuralElementInProgramm>();
                var dictNotExistPrices = new Dictionary<long, string>();
                var dictFormulaParams = new Dictionary<string, List<string>>();

                var dictValues = valuesService.GetAll()
                  //      .Where(x => x.Object.RealityObject.Id == 39494)
                    .Select(
                        x => new
                        {
                            ObjectId = x.Object.Id,
                            AttributeId = x.Attribute.Id,
                            x.Attribute.AttributeType,
                            x.Value
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.GroupBy(x => x.AttributeId).ToDictionary(x => x.Key, x => x.Select(z => new {z.AttributeType, z.Value}).First()));

                // Идем по каждой записи КЭ дома
                var data = (roStructElQuery ?? this.GetRoStructElQuery(municipalityId))
           //           .Where(x => x.RealityObject.Id == 39494)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.StructuralElement.Group.Formula,
                            x.StructuralElement.Group.FormulaParams,
                            GroupName = x.StructuralElement.Group.Name,
                            SeId = x.StructuralElement.Id,
                            x.Volume,
                            Element = x,
                            RealityObjectId = x.RealityObject.Id,
                            x.RealityObject.BuildYear,
                            x.RealityObject.PrivatizationDateFirstApartment,
                            x.RealityObject.PhysicalWear,
                            x.RealityObject.AreaLiving,
                            x.RealityObject.AreaMkd,
                            x.RealityObject.AreaLivingNotLivingMkd,
                            x.RealityObject.IsRepairInadvisable,
                            x.StructuralElement,
                            CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                            StructuralElementLifeTime = x.StructuralElement.LifeTime,
                            StructuralElementLifeTimeAfterRepair = x.StructuralElement.LifeTimeAfterRepair,
                            x.StructuralElement.CalculateBy,
                            municipalityId = x.RealityObject.Municipality != null ? x.RealityObject.Municipality.Id : 0,
                            settlementId = x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Id : 0,
                            parentMuId =
                                x.RealityObject.Municipality != null && x.RealityObject.Municipality.ParentMo != null
                                    ? x.RealityObject.Municipality.ParentMo.Id
                                    : 0,
                            CapitalGroupId = x.RealityObject.CapitalGroup != null ? x.RealityObject.CapitalGroup.Id : 0
                        })
                    .AsEnumerable();

                foreach (var objectElement in data)
                {
                    // Достаем формулу
                    var formula = objectElement.Formula?.Trim();

                    // Если формула не задана, то пропускаем этот шаг цикла
                    if (string.IsNullOrEmpty(formula))
                    {
                        continue;
                    }

                    // Обрабатываем параметры формулы
                    if (!dictFormulaParams.ContainsKey(formula))
                    {
                        // Если по данной формуле еще не было получение параметров то получаем параметры
                        // Параметры получаются только при первом получении
                        var checkFormula = formulaService.GetParamsList(formula);

                        if (!checkFormula.Success)
                        {
                            continue;
                        }

                        // Получаем наименования параметров для нормального вычисления формулы
                        var listParams = ((ListDataResult) checkFormula).Data as List<string> ?? new List<string>();

                        dictFormulaParams.Add(formula, listParams);
                    }

                    var atrValue = new Dictionary<string, object>();

                    foreach (var formulaParam in objectElement.FormulaParams)
                    {
                        var param = formulaParam;
                        if (param.Attribute != null && dictValues.ContainsKey(objectElement.Id)
                            && dictValues[objectElement.Id].ContainsKey(param.Attribute.Id))
                        {
                            var value = dictValues[objectElement.Id][param.Attribute.Id];

                            if (!atrValue.ContainsKey(param.Name))
                            {
                                switch (value.AttributeType)
                                {
                                    case AttributeType.Boolean:
                                        atrValue.Add(param.Name, value.Value.ToBool());
                                        break;
                                    case AttributeType.String:
                                        atrValue.Add(param.Name, value.Value.ToStr());
                                        break;
                                    case AttributeType.Int:
                                        atrValue.Add(param.Name, value.Value.ToInt());
                                        break;
                                    case AttributeType.Decimal:
                                        atrValue.Add(param.Name, value.Value.ToDecimal());
                                        break;
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(param.ValueResolverCode))
                        {
                            var resolver = this.Container.Resolve<IFormulaParameter>(
                                formulaParam.ValueResolverCode,
                                new Arguments
                                {
                                    {"OverhaulHmaoConfig", this.OverhaulHmaoConfig},
                                    {"RealityObjectStructuralElement", objectElement.Element},
                                    {"BuildYear", objectElement.BuildYear},
                                    {"PrivatizationDateFirstApartment", objectElement.PrivatizationDateFirstApartment},
                                    {"PhysicalWear", objectElement.PhysicalWear},
                                    {"StructuralElement", objectElement.StructuralElement},
                                });

                            atrValue.Add(param.Name, resolver.GetValue());
                        }
                    }

                    if (dictFormulaParams[formula].Any(x => !atrValue.ContainsKey(x)))
                    {
                        continue;
                    }

                    var planYear = 0;

                    try
                    {
                        // Считаем год по формуле
                        var expr = new Expression(formula) {Parameters = atrValue};
                        planYear = (int) expr.Evaluate().ToDecimal();
                    }
                    catch (Exception exc)
                    {
                        throw new ValidationException(
                            string.Format(
                                "При вычислении формулы ({0}) для группы ООИ ({1}) произошла ошибка : <br> {2}",
                                formula,
                                objectElement.GroupName,
                                exc.Message));
                    }

                    if (objectElement.IsRepairInadvisable)
                    {
                        planYear = this.EndYear;
                    }
                    else
                    {
                        planYear = planYear > 0 ? planYear : this.EndYear - 1;
                    }

                    var jobs = this.DictStructElWork.ContainsKey(objectElement.SeId)
                        ? this.DictStructElWork[objectElement.SeId]
                        : new Dictionary<long, string>();

                    var sum = 0m;
                    var serviceCost = 0m;
                    //Если у КЭ год постройки или последнего ресонта + срок эксплуатации больше года окончания ДПКР, то вставляем ремонт последним годом
                    if (planYear > this.EndYear)
                    {
                        planYear = this.EndYear;
                    }

                    if (planYear <= this.EndYear)
                    {
                        // Если год по формуле не попадает в программу, то делаем его начальным годом программы
                        planYear = Math.Max(startYear, planYear);

                        var workPriceYear = this.IsWorkPriceFirstYear ? this.PeriodStartYear : planYear;

                        var costSum = this.GetDpkrCost(
                            municipalityId,
                            objectElement.settlementId,
                            workPriceYear,
                            objectElement.SeId,
                            objectElement.RealityObjectId,
                            objectElement.CapitalGroupId,
                            objectElement.CalculateBy,
                            objectElement.Volume,
                            objectElement.AreaLiving,
                            objectElement.AreaMkd,
                            objectElement.AreaLivingNotLivingMkd);

                        if (!costSum.HasValue)
                        {
                            // если не найдена расценка, добавляем ее в dictionary
                            foreach (var job in jobs)
                            {
                                if (!dictNotExistPrices.ContainsKey(job.Key))
                                {
                                    dictNotExistPrices.Add(job.Key, job.Value);
                                }
                            }
                            sum = 0;
                        }
                        else
                        {
                            sum = costSum.Value;
                        }
                        serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                        var evaluatedElement =
                            new RealityObjectStructuralElementInProgramm
                            {
                                StructuralElement = objectElement.Element,
                                Year = planYear,
                                Sum = sum,
                                ServiceCost = serviceCost
                            };

                        // Добавляем в список на сохранение
                        listStage1ToSave.Add(evaluatedElement);
                    }

                    // Если КЭ имеет срок эксплуатации - то добавляем в программу "С новым годом! Счастья, здоровья в новом году!"
                    if (objectElement.StructuralElementLifeTime > 0)
                    {
                        // Если у КЭ указано значение "Срок эксплуатации после ремонта", то используем его, иначе "Срок эксплуатации"
                        // Однако в первый проход - используем "Срок эксплуатации"
                        var addYear = objectElement.StructuralElementLifeTimeAfterRepair > 0
                            ? objectElement.StructuralElementLifeTimeAfterRepair
                            : objectElement.StructuralElementLifeTime;

                        var year = planYear + addYear;

                        while (year <= this.EndYear)
                        {
                            if (!this.IsWorkPriceFirstYear)
                            {
                                var costSum = this.GetDpkrCost(
                                    municipalityId,
                                    objectElement.settlementId,
                                    year,
                                    objectElement.SeId,
                                    objectElement.RealityObjectId,
                                    objectElement.CapitalGroupId,
                                    objectElement.CalculateBy,
                                    objectElement.Volume,
                                    objectElement.AreaLiving,
                                    objectElement.AreaMkd,
                                    objectElement.AreaLivingNotLivingMkd);

                                if (!costSum.HasValue)
                                {
                                    foreach (var job in jobs)
                                    {
                                        if (!dictNotExistPrices.ContainsKey(job.Key))
                                        {
                                            dictNotExistPrices.Add(job.Key, job.Value);
                                        }
                                    }
                                }
                                else
                                {
                                    sum = costSum.Value;
                                }

                                serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);
                            }

                            var elem =
                                new RealityObjectStructuralElementInProgramm
                                {
                                    StructuralElement = objectElement.Element,
                                    Year = year,
                                    Sum = sum,
                                    ServiceCost = serviceCost
                                };

                            // Добавляем в список на сохранен
                            listStage1ToSave.Add(elem);

                            year = year + addYear;
                        }
                    }
                }

                if (fromActualize)
                {
                    return listStage1ToSave;
                }

                return this.DeleteByWear(roStructElQuery, listStage1ToSave);
            }
            finally
            {
                this.Container.Release(this.StructElementWorkService);
                this.Container.Release(this.WorkPriceService);
                this.Container.Release(valuesService);
                this.Container.Release(this.RoStrElService);
                this.Container.Release(formulaService);
                this.Container.Release(this.RealEstService);
                this.Container.Release(this.RoDomain);
            }
        }

        /// <summary>
        /// Получить 2 этап
        /// </summary>
        public IEnumerable<RealityObjectStructuralElementInProgrammStage2> GetStage2(IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records)
        {
            var startYear = this.OverhaulHmaoConfig.ProgrammPeriodStart;
            var endYear = this.OverhaulHmaoConfig.ProgrammPeriodEnd;
            var stage2Period = this.OverhaulHmaoConfig.GroupByCeoPeriod;

            if (stage2Period < 1)
            {
                throw new ValidationException($"Настройка \"Период группировки КЭ (год)\" задана неверно: {stage2Period}");
            }

            var startPeriod = startYear;
            var endPeriod = startYear + stage2Period;

            var result = new ConcurrentBag<RealityObjectStructuralElementInProgrammStage2>();

            var dictCeo = this.Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                .ToDictionary(x => x.Id);

            var lockObj = new object();

            while (startPeriod <= endYear)
            {
                var period = startPeriod;
                var period1 = endPeriod;

                var elements =
                    stage1Records
                        .Where(x => x.Year >= period && x.Year < period1)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.Year,
                                RoId = x.StructuralElement.RealityObject.Id,
                                RoAddress = x.StructuralElement.RealityObject.Address,
                                CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                                x.Sum,
                                x.ServiceCost,
                                Stage1 = x
                            })
                        .GroupBy(x => new {x.RoId, x.RoAddress, x.CeoId});

                Parallel.ForEach(
                    elements,
                    item =>
                    {
                        CommonEstateObject ceo;
                        lock (lockObj)
                        {
                            ceo = dictCeo.ContainsKey(item.Key.CeoId) ? dictCeo[item.Key.CeoId] : null;
                        }

                        var stage2 = new RealityObjectStructuralElementInProgrammStage2
                        {
                            StructuralElements = string.Empty,
                            CommonEstateObject = ceo,
                            RealityObject = new RealityObject {Id = item.Key.RoId, Address = item.Key.RoAddress},
                            Sum = 0M
                        };

                        var listYears = new List<int>();

                        foreach (var val in item)
                        {
                            var stage1 = val.Stage1;
                            stage1.Stage2 = stage2;

                            listYears.Add(val.Year);
                            stage2.Sum += val.ServiceCost + val.Sum;
                        }

                        stage2.Year = Math.Round(listYears.Average(x => x), MidpointRounding.ToEven).ToInt();

                        result.Add(stage2);
                    });

                startPeriod = endPeriod;
                endPeriod += stage2Period;
            }

            return result;
        }

        /// <summary>
        /// Получить 3 этап
        /// </summary>
        public IEnumerable<RealityObjectStructuralElementInProgrammStage3> GetStage3(
            IEnumerable<RealityObjectStructuralElementInProgrammStage2> stage2Records)
        {
            var result = new List<RealityObjectStructuralElementInProgrammStage3>();

            var startYear = this.OverhaulHmaoConfig.ProgrammPeriodStart;
            var endYear = this.OverhaulHmaoConfig.ProgrammPeriodEnd;
            var stage3Period = this.OverhaulHmaoConfig.GroupByRoPeriod;
            int endPeriod = startYear + stage3Period;

            if (stage3Period > 0)
            {
                while (startYear <= endYear)
                {
                    var grouped = stage2Records
                        .Where(x => x.Year >= startYear && x.Year < endPeriod)
                        .GroupBy(x => new {x.RealityObject.Id, x.RealityObject.Address});

                    foreach (var stage2 in grouped)
                    {
                        var ceoString = stage2.Select(x => x.CommonEstateObject.Name).AggregateWithSeparator(", ");

                        var stage3 = new RealityObjectStructuralElementInProgrammStage3
                        {
                            RealityObject = new RealityObject {Id = stage2.Key.Id, Address = stage2.Key.Address},
                            Year = Math.Round(stage2.Average(x => x.Year), MidpointRounding.ToEven).ToInt(),
                            Sum = stage2.Sum(x => x.Sum),
                            CommonEstateObjects = ceoString.Length > 4000 ? ceoString.Substring(0, 4000) : ceoString
                        };

                        foreach (var rec in stage2)
                        {
                            rec.Stage3 = stage3;
                        }

                        result.Add(stage3);
                    }

                    startYear = endPeriod;
                    endPeriod += stage3Period;
                }
            }
            else
            {
                foreach (var stage2 in stage2Records)
                {
                    var stage3 = new RealityObjectStructuralElementInProgrammStage3
                    {
                        CommonEstateObjects = stage2.CommonEstateObject.Name,
                        Sum = stage2.Sum,
                        Year = stage2.Year,
                        RealityObject = new RealityObject {Id = stage2.RealityObject.Id, Address = stage2.RealityObject.Address}
                    };

                    stage2.Stage3 = stage3;

                    result.Add(stage3);
                }
            }

            return result;
        }

        /// <summary>
        /// Создать ДПКР для публикации
        /// </summary>
        public IDataResult CreateDpkrForPublish(BaseParams baseParams)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var versionRecordStage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();

            try
            {
                var moId = baseParams.Params.GetAsId("mo_id");

                var minYear = this.OverhaulHmaoConfig.ProgrammPeriodStart;
                var period = this.OverhaulHmaoConfig.PublishProgramConfig.PublicationPeriod;
                var maxYear = this.OverhaulHmaoConfig.ProgrammPeriodEnd;
                var shortTerm = this.OverhaulHmaoConfig.ShortTermProgPeriod;

                var isMass = baseParams.Params.GetAs<bool>("isMass");

                if (moId <= 0 && !isMass)
                {
                    return new BaseDataResult(false, "Не удалось получить муниципальное образование");
                }

                if (period == 0)
                {
                    return new BaseDataResult(false, "Не найден параметр \" Период для публикации\"");
                }

                var useShortTerm = this.OverhaulHmaoConfig.PublishProgramConfig.UseShortProgramPeriod;
                if (useShortTerm == TypeUseShortProgramPeriod.WithOut)
                {
                    // Поскольку в настройках указано, что не используем период краткосрочки
                    shortTerm = 0;
                }

                if (shortTerm > 0)
                {
                    minYear += shortTerm;
                }

                // получаем основную версию
                var versions = this.ProgramVersionDomain.GetAll()
                    .Where(x => x.IsMain)
                    .WhereIf(!isMass, x => x.Municipality.Id == moId);

                if (!versions.Any())
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                // Записи прошлой опубликованной программы в виде:
                // Словарь с группировкой по версии -> Словарь с группировкой по дому -> Словарь с группировкой по Stage2
                var oldPublishedProgramRecDict = this.PublishedProgramRecordDomain.GetAll()
                    .Where(x => versions.Any(y => y == x.PublishedProgram.ProgramVersion))
                    .Where(x => x.RealityObject != null)
                    .Where(x => x.Stage2 != null)
                    .Select(
                       x => new PublishedProgramRecDto
                       {
                           ProgramVersionId = x.PublishedProgram.ProgramVersion.Id,
                           RoId = x.RealityObject.Id,
                           Stage2Id = x.Stage2.Id,
                           Stage3PlanYear = x.Stage2.Stage3Version.Year,
                           PublishedYear = x.PublishedYear,
                           Number = x.IndexNumber,
                           CommonEstateObject = x.Stage2.CommonEstateObject.Name,
                           Sum = x.Sum,
                           WorkCode = x.Stage2.Stage3Version.WorkCode,
                           IsValid = true
                       })
                   .AsEnumerable()
                   .GroupBy(x => x.ProgramVersionId)
                   .ToDictionary(x => x.Key,
                       y => y.GroupBy(x => x.RoId)
                           .ToDictionary(x => x.Key,
                               z => z.GroupBy(x => x.Stage2Id)
                                   .ToDictionary(x => x.Key,
                                       t => t.First())));

                var session = sessionProvider.OpenStatelessSession();
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var programVersion in versions)
                        {
                            // Удаляем записи опубликованной программы именно этой основной версии
                            session.CreateSQLQuery(
                                string.Format(
                                    "  delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID in ( " +
                                        "       select id from OVRHL_PUBLISH_PRG where version_id = {0} ) ",
                                    programVersion.Id)
                                ).ExecuteUpdate();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                // Проверяем Существует ли нужный статус и если нет то создаем новый
                var firstState = this.StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);

                if (firstState == null)
                {
                    firstState = new State
                    {
                        Name = "Черновик",
                        Code = "Черновик",
                        StartState = true,
                        TypeId = "ovrhl_published_program"
                    };

                    this.StateDomain.Save(firstState);
                }

                var publishPrograms = this.PublishedProgramDomain.GetAll()
                    .Where(x => versions.Any(y => y.Id == x.ProgramVersion.Id))
                    .ToList();

                var versionRecordsForUpd = versionRecordDomain.GetAll()
                    .Where(x => versions.Any(y => y.Id == x.ProgramVersion.Id))
                    .Where(x => x.IsDividedRec)
                    .ToList();

                // Грохаем существующую запись опубликованной программы поскольку
                // у нее могли существоват ьуже ненужные подписи ЭЦП
                if (publishPrograms.Any())
                {
                    publishPrograms.ForEach(x => this.PublishedProgramDomain.Delete(x.Id));
                }

                var listPubProgsToSave = new List<PublishedProgram>();
                var listRecordsToSave = new List<PublishedProgramRecord>();
                var listActualizeLogsToSave = new List<VersionActualizeLog>();
                var listActualizeLogRecordsToSave = new List<VersionActualizeLogRecord>();

                var newPublishedProgramRecDict = new Dictionary<PublishedProgram, Dictionary<long, List<PublishedProgramRecDto>>>();

                // Получаем записи корректировки и по ним создаем опубликованную программу
                var dataCorrections = this.DpkrCorrectedDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .Where(x => versions.Any(y => y.Id == x.Stage2.Stage3Version.ProgramVersion.Id))
                    .Select(
                        x => new PublishedProgramRecDto
                        {
                            Id = x.Id,
                            Stage2Id = x.Stage2.Id,
                            PlanYear = x.PlanYear,
                            Stage3PlanYear = x.Stage2.Stage3Version.Year,
                            Number = x.Stage2.Stage3Version.IndexNumber,
                            Locality = x.RealityObject.FiasAddress.PlaceName,
                            Street = x.RealityObject.FiasAddress.StreetName,
                            House = x.RealityObject.FiasAddress.House,
                            Housing = x.RealityObject.FiasAddress.Housing,
                            Address = x.RealityObject.FiasAddress.AddressName,
                            CommonEstateObject = x.Stage2.CommonEstateObject.Name,
                            CommissioningYear = x.RealityObject.BuildYear ?? 0,
                            ProgramVersionId = x.Stage2.Stage3Version.ProgramVersion.Id,
                            Sum = x.Stage2.Sum,
                            WorkCode = x.Stage2.Stage3Version.WorkCode,
                            RoId = x.RealityObject.Id,
                            IsValid = true
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ProgramVersionId)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(x => x.RoId)
                            .ToDictionary(x => x.Key));

                var workVolumesDict = versionRecordStage1Domain.GetAll()
                    .Where(x => versions.Any(y => y == x.Stage2Version.Stage3Version.ProgramVersion))
                    .Select(x => new
                    {
                        Stage2Id = x.Stage2Version.Id,
                        x.Volume
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Stage2Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Volume).Sum());

                foreach (var programVersion in versions)
                {
                    var publishProg = new PublishedProgram
                    {
                        State = firstState,
                        ProgramVersion = programVersion
                    };

                    newPublishedProgramRecDict.Add(publishProg, new Dictionary<long, List<PublishedProgramRecDto>>());

                    listPubProgsToSave.Add(publishProg);

                    foreach (var roIdGrouped in dataCorrections[programVersion.Id])
                    {
                        if (!newPublishedProgramRecDict[publishProg].ContainsKey(roIdGrouped.Key))
                        {
                            newPublishedProgramRecDict[publishProg].Add(roIdGrouped.Key, new List<PublishedProgramRecDto>());
                        }

                        foreach (var rec in roIdGrouped.Value)
                        {
                            var publicationYear = rec.PlanYear;

                            if (rec.PlanYear >= minYear)
                            {
                                publicationYear = rec.PlanYear + period - 1 - (rec.PlanYear - minYear) % period;
                                publicationYear = publicationYear > maxYear ? maxYear : publicationYear;
                            }

                            rec.PlanYear = publicationYear;
                            rec.PublishedYear = publicationYear;

                            var newRec = new PublishedProgramRecord
                            {
                                PublishedProgram = publishProg,
                                Stage2 = new VersionRecordStage2 { Id = rec.Stage2Id },
                                RealityObject = new RealityObject { Id = rec.RoId },
                                PublishedYear = publicationYear,
                                IndexNumber = rec.Number,
                                Locality = rec.Locality,
                                Street = rec.Street,
                                House = rec.House,
                                Housing = rec.Housing,
                                Address = rec.Address,
                                CommonEstateobject = rec.CommonEstateObject,
                                CommissioningYear = rec.CommissioningYear,
                                Sum = rec.Sum
                            };

                            newPublishedProgramRecDict[publishProg][rec.RoId].Add(rec);
                            listRecordsToSave.Add(newRec);
                        }
                    }
                }

                var userName = userManager.GetActiveUser().Name;

                foreach (var newProgram in newPublishedProgramRecDict)
                {
                    newProgram.Key.TotalRoCount = newProgram.Value.Keys.Count;

                    var log = new VersionActualizeLog
                    {
                        ActualizeType = VersionActualizeType.CreateDpkrForPublish,
                        DateAction = DateTime.Now,
                        Municipality = newProgram.Key.ProgramVersion.Municipality,
                        ProgramVersion = newProgram.Key.ProgramVersion,
                        UserName = userName
                    };

                    listActualizeLogsToSave.Add(log);

                    if (oldPublishedProgramRecDict.ContainsKey(newProgram.Key.ProgramVersion.Id))
                    {
                        var recDict = oldPublishedProgramRecDict[newProgram.Key.ProgramVersion.Id];

                        newProgram.Key.ExcludedRoCount = recDict.Keys.Except(newProgram.Value.Keys).Count();
                        newProgram.Key.IncludedRoCount = newProgram.Value.Keys.Except(recDict.Keys).Count();

                        var oldRecs = recDict.Values.SelectMany(x => x.Values).ToArray();
                        var newRecs = newProgram.Value.SelectMany(y => y.Value).ToArray();

                        // Записи, которые былы исключены (не были перенесены из прошлой версии в новую)
                        var excludedRecs = oldRecs
                            .Where(x => !newRecs.Select(y => y.Stage2Id).Contains(x.Stage2Id)).ToList();

                        // Записи, которые были добавлены (появились только в новой версии)
                        var includedRecs = newRecs
                            .Where(x => !oldRecs.Select(y => y.Stage2Id).Contains(x.Stage2Id)).ToList();

                        // Записи, которые были изменены (перенесены из прошлой версии в новую)
                        var editedRecs = newRecs.Except(includedRecs).ToList();

                        // Переносим значения для записей-изменений
                        editedRecs.ForEach(x =>
                        {
                            var oldRec = recDict[x.RoId][x.Stage2Id];

                            x.ChangePublishedYear = x.PublishedYear;
                            x.PublishedYear = oldRec.PublishedYear;
                            x.ChangeSum = x.Sum;
                            x.Sum = oldRec.Sum;
                            x.ChangeNumber = x.Number;
                            x.Number = oldRec.Number;

                            // Если по сравнению с прошлой записью изменений значения не произошло, то помечаем,
                            // чтобы не добавлять в логи записи без смысловой нагрузки для пользователя
                            if (oldRec.PublishedYear == x.ChangePublishedYear &&
                                oldRec.Sum == x.ChangeSum &&
                                oldRec.Number == x.ChangeNumber)
                            {
                                x.IsValid = false;
                            }
                        });

                        var deleteLogsList = this.GetActualizeLogsForCreatedPublishedProgram(
                            excludedRecs,
                            workVolumesDict,
                            log,
                            VersionActualizeAction.Delete);

                        var saveLogsList = this.GetActualizeLogsForCreatedPublishedProgram(
                            includedRecs,
                            workVolumesDict,
                            log,
                            VersionActualizeAction.Save);

                        var editLogsList = this.GetActualizeLogsForCreatedPublishedProgram(
                            editedRecs,
                            workVolumesDict,
                            log,
                            VersionActualizeAction.Update);

                        new [] { deleteLogsList, saveLogsList, editLogsList }.ForEach(x =>
                        {
                            listActualizeLogRecordsToSave.AddRange(x);
                        });
                    }
                    else
                    {
                        var logsList = this.GetActualizeLogsForCreatedPublishedProgram(
                            newProgram.Value.SelectMany(x => x.Value).ToList(),
                            workVolumesDict,
                            log,
                            VersionActualizeAction.Save);

                        listActualizeLogRecordsToSave.AddRange(logsList);
                    }
                }

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listPubProgsToSave.Where(x => x.Id == 0).ForEach(x => session.Insert(x));
                        listRecordsToSave.ForEach(x => session.Insert(x));
                        listActualizeLogsToSave.ForEach(x => session.Insert(x));
                        listActualizeLogRecordsToSave.ForEach(x => session.Insert(x));

                        versionRecordsForUpd.ForEach(
                            x =>
                            {
                                x.IsDividedRec = false;
                                x.PublishYearForDividedRec = 0;

                                session.Update(x);
                            });

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, $"Ошибка очередности:{exc.Message}");
            }
            finally
            {
                this.Container.Release(versionRecordDomain);
                this.Container.Release(versionRecordStage1Domain);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Получение параметров очередности ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetParams(BaseParams baseParams)
        {
            var currParamsDomain = this.Container.ResolveDomain<CurrentPrioirityParams>();

            using (this.Container.Using(currParamsDomain))
            {
                var currParams = currParamsDomain.GetAll().Select(x => x.Code).ToArray();

                var parameters = this.Container.ResolveAll<IProgrammPriorityParam>()
                    .Where(x => !currParams.Contains(x.Code))
                    .Select(x => new {Id = x.Code, x.Name, x.Code});

                return new ListDataResult(parameters, parameters.Count());
            }
        }

        /// <summary>
        /// Получение всех параметров очередности ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetAllParams(BaseParams baseParams)
        {
            var parameters = this.Container.ResolveAll<IProgrammPriorityParam>()
                .Select(x => new {x.Name, x.Code});

            return new ListDataResult(parameters, parameters.Count());
        }

        /// <summary>
        /// Валидация удаления ДПКР
        /// </summary>
        public IDataResult ValidationDeleteDpkr(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            if (
                this.Container.Resolve<IRepository<RealityObjectStructuralElementInProgramm>>()
                    .GetAll()
                    .Any(x => x.StructuralElement.RealityObject.Municipality.Id == muId || x.StructuralElement.RealityObject.MoSettlement.Id == muId))
            {
                return new BaseDataResult {Success = true};
            }

            return new BaseDataResult {Success = false};
        }

        /// <summary>
        /// Удалить ДПКР
        /// </summary>
        public IDataResult DeleteDpkr(BaseParams baseParams)
        {
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();
            var muId = baseParams.Params.GetAs<long>("muId");

            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // удаляем существующие записи
                    session.CreateSQLQuery(
                        "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG " +
                            "where id in (SELECT s1.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG s1 " +
                            "inner JOIN OVRHL_RO_STRUCT_EL ro_se ON s1.RO_SE_ID = ro_se.ID " +
                            "inner JOIN GKH_REALITY_OBJECT ro ON ro_se.RO_ID = ro.ID " +
                            "WHERE (ro.municipality_id = :municipalityId or ro.stl_municipality_id = :municipalityId))")
                        .SetInt64("municipalityId", muId)
                        .ExecuteUpdate();

                    session.CreateSQLQuery(
                        "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 " +
                            "WHERE ID IN (SELECT s2.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 s2 " +
                            "inner JOIN GKH_REALITY_OBJECT ro ON s2.RO_ID = ro.ID " +
                            "WHERE (ro.municipality_id = :municipalityId or ro.stl_municipality_id = :municipalityId))")
                        .SetInt64("municipalityId", muId)
                        .ExecuteUpdate();

                    session.CreateSQLQuery(
                        "DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 " +
                            "WHERE ID IN (SELECT s3.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 s3 " +
                            "inner JOIN GKH_REALITY_OBJECT ro ON s3.RO_ID = ro.ID " +
                            "WHERE (ro.municipality_id = :municipalityId or ro.stl_municipality_id = :municipalityId))")
                        .SetInt64("municipalityId", muId)
                        .ExecuteUpdate();

                    transaction.Commit();
                    return new BaseDataResult(true, "Программа ДПКР успешно удалена");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private Dictionary<long, List<long>> GetRealEstateTypes(long muId)
        {
            if (this.allRealEstateTypes.ContainsKey(muId))
            {
                return this.allRealEstateTypes[muId];
            }

            var realEstateTypes = this.RealEstService.GetRealEstateTypes(this.GetRoQueryStage1(muId));

            this.allRealEstateTypes.Add(muId, realEstateTypes);

            return realEstateTypes;
        }

        private IQueryable<RealityObjectStructuralElement> GetRoStructElQuery(long municipalityId = 0)
        {
            var roStructElQuery = this.roStructElQuery ?? this.RoStrElService.GetElementsForLongProgram(this.RoStructElDomain)
                .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId || x.RealityObject.MoSettlement.Id == municipalityId);

            return roStructElQuery;
        }

        private IQueryable<RealityObject> GetRoQueryStage1(long municipalityId = 0)
        {
            // Получаем Query по домам
            var roQuery = this.RoDomain.GetAll().Where(x => this.GetRoStructElQuery(municipalityId).Any(y => y.RealityObject.Id == x.Id)).AsQueryable();

            return roQuery;
        }

        private IEnumerable<HmaoWorkPrice> GetWorkPrices(
            Dictionary<int, IEnumerable<HmaoWorkPrice>> workPricesByYear,
            int year,
            IEnumerable<long> jobs,
            WorkPriceDetermineType type,
            long cpitalGroupId,
            IEnumerable<long> realEstTypes
            )
        {
            if (workPricesByYear.ContainsKey(year))
            {
                var list = workPricesByYear[year]
                    .Where(x => jobs.Contains(x.Job.Id));

                var prices =
                    list.AsQueryable()
                        .WhereIf(type == WorkPriceDetermineType.WithoutCapitalGroup, x => x.CapitalGroup == null)
                        .WhereIf(
                            type == WorkPriceDetermineType.WithCapitalGroup,
                            x => x.CapitalGroup != null && x.CapitalGroup.Id == cpitalGroupId)
                        .WhereIf(
                            type == WorkPriceDetermineType.WithRealEstType && realEstTypes != null,
                            x => x.RealEstateType != null && realEstTypes.Contains(x.RealEstateType.Id))
                        .GroupBy(x => x.Job.Id)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault())
                        .Values
                        .AsEnumerable();

                if (!prices.Any() && type == WorkPriceDetermineType.WithRealEstType)
                {
                    // Если хотели достать по типу дома но результата неполучили, то тогда 
                    // Пробуем достть среди тех расценок которые без типа
                    prices = list.Where(x => x.RealEstateType == null)
                        .GroupBy(x => x.Job.Id)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault())
                        .Values
                        .AsEnumerable();
                }

                return prices;
            }
            return new List<HmaoWorkPrice>();
        }

        /// <summary>
        /// Метод, возвращает сумму стоимостей работ, возвращает null если нет расценок по работам
        /// </summary>
        private decimal? GetDpkrCostByYear(IEnumerable<HmaoWorkPrice> workPrices, PriceCalculateBy calculateBy)
        {
            // null  потому что нужно выводить список не заполненных расценок
            decimal? costSum = null;

            if (workPrices.Any())
            {
                switch (calculateBy)
                {
                    case PriceCalculateBy.Volume:
                        costSum = workPrices.Sum(x => x.NormativeCost);
                        break;
                    case PriceCalculateBy.LivingArea:
                    case PriceCalculateBy.TotalArea:
                    case PriceCalculateBy.AreaLivingNotLivingMkd:
                        costSum = workPrices.Sum(x => x.SquareMeterCost).ToDecimal();
                        break;
                }
            }

            return costSum;
        }

        private IEnumerable<RealityObjectStructuralElementInProgramm> DeleteByWear(
            IQueryable<RealityObjectStructuralElement> roSeQuery,
            List<RealityObjectStructuralElementInProgramm> listStage1ToSave)
        {
            var typeUseWearMainCeo = this.OverhaulHmaoConfig.HouseAddInProgramConfig.TypeUseWearMainCeo;
            var wearMainCeo = this.OverhaulHmaoConfig.HouseAddInProgramConfig.WearMainCeo;

            if (typeUseWearMainCeo == TypeUseWearMainCeo.NotUsed)
            {
                return listStage1ToSave;
            }

            if (roSeQuery == null)
            {
                roSeQuery = this.GetRoStructElQuery();
            }

            var roStructElDict = roSeQuery
                .Where(x => x.StructuralElement.Group.CommonEstateObject.IsMain)
                .Select(
                    x => new
                    {
                        RealObjId = x.RealityObject.Id,
                        x.Wearout
                    })
                .AsEnumerable()
                .GroupBy(x => x.RealObjId)
                .ToDictionary(
                    x => x.Key,
                    y => new
                    {
                        AllSeCnt = y.Count(),
                        OverWearoutCnt = y.Count(x => wearMainCeo > 0 && x.Wearout > wearMainCeo)
                    });

            var roIdToDelete = new List<long>();

            switch (typeUseWearMainCeo)
            {
                case TypeUseWearMainCeo.AllCeo:
                {
                    roIdToDelete = roStructElDict
                        .Where(x => x.Value.AllSeCnt == x.Value.OverWearoutCnt)
                        .Select(x => x.Key)
                        .ToList();
                    break;
                }
                case TypeUseWearMainCeo.OnlyOne:
                {
                    roIdToDelete = roStructElDict
                        .Where(x => x.Value.OverWearoutCnt > 0)
                        .Select(x => x.Key)
                        .ToList();
                    break;
                }
            }

            return listStage1ToSave
                .Where(x => !roIdToDelete.Contains(x.StructuralElement.RealityObject.Id))
                .ToList();
        }

        // возвращает Dictionary, ключ - id Жилого дома, значение - минимальный Год превышающий предельную стоимость
        private Dictionary<long, int> GetMinMissingYearsByLimitCost(
            IEnumerable<long> muIds,
            IEnumerable<RealityObjectStructuralElementInProgrammStage3> listStage3,
            IStatelessSession session)
        {
            var rateCalcArea = this.OverhaulHmaoConfig.RateCalcTypeArea;
            var missingDpkrRecs = new List<MissingByMargCostDpkrRec>();
            var result = new Dictionary<long, int>();

            var roQuery = this.Container.Resolve<IRepository<RealityObject>>().GetAll()
                .WhereIf(muIds.Any(), x => muIds.Contains(x.Municipality.Id) || muIds.Contains(x.MoSettlement.Id))
                .Where(x => x.TypeHouse != TypeHouse.Individual && x.TypeHouse != TypeHouse.NotSet)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(
                    x => !this.EmergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                        || this.EmergencyDomain.GetAll()
                            .Where(e => e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)
                            .Any(e => e.RealityObject.Id == x.Id))
                .Where(x => !x.IsNotInvolvedCr);

            var roRealEstTypesDict = this.RealEstService.GetRealEstateTypes(roQuery);

            var realEstatesArray = this.Container.Resolve<IDomainService<RealEstateType>>()
                .GetAll()
                .Where(x => x.MarginalRepairCost.HasValue)
                .ToArray();

            var margSumByRoId = roQuery
                .Select(
                    x => new
                    {
                        x.Id,
                        x.AreaMkd,
                        x.AreaLiving,
                        x.AreaLivingNotLivingMkd
                    })
                .AsEnumerable()
                .Where(x => roRealEstTypesDict.ContainsKey(x.Id) && realEstatesArray.Any(y => roRealEstTypesDict[x.Id].Contains(y.Id)))
                .ToDictionary(
                    x => x.Id,
                    y =>
                    {
                        var realEstateType = realEstatesArray.Where(x => roRealEstTypesDict[y.Id].Contains(x.Id))
                            .OrderBy(x => x.MarginalRepairCost)
                            .First();

                        var area = rateCalcArea == RateCalcTypeArea.AreaLiving
                            ? y.AreaLiving
                            : rateCalcArea == RateCalcTypeArea.AreaMkd
                                ? y.AreaMkd
                                : rateCalcArea == RateCalcTypeArea.AreaLivingNotLiving ? y.AreaLivingNotLivingMkd : 0M;

                        return new
                        {
                            Area = area,
                            Sum = area * realEstateType.MarginalRepairCost,
                            MargSum = realEstateType.MarginalRepairCost.ToDecimal(),
                            RealEstateTypeName = realEstateType.Name
                        };
                    });

            // Стоимость проверяем в разрезе года, поэтому при группировке ООИ = 0, необходимо сгруппировать записи по году 

            var groupedStage3 = listStage3
                .GroupBy(x => new {x.Year, x.RealityObject.Id})
                .ToDictionary(
                    x => x.Key,
                    y => new
                    {
                        y.First().RealityObject,
                        y.Key.Year,
                        Sum = y.Sum(x => x.Sum),
                        CommonEstateObjects = y.Select(x => x.CommonEstateObjects).AggregateWithSeparator(", "),
                        Stage3List = y.ToList()
                    }).Select(x => x.Value).ToList();

            foreach (var groupedSt3Rec in groupedStage3.OrderBy(x => x.Year))
            {
                if (result.ContainsKey(groupedSt3Rec.RealityObject.Id))
                {
                    foreach (var st3Rec in groupedSt3Rec.Stage3List)
                    {
                        missingDpkrRecs.Add(
                            new MissingByMargCostDpkrRec
                            {
                                RealityObject = st3Rec.RealityObject,
                                CommonEstateObjects = st3Rec.CommonEstateObjects,
                                Year = st3Rec.Year,
                                Sum = st3Rec.Sum,
                                Area = margSumByRoId[st3Rec.RealityObject.Id].Area,
                                MargSum = margSumByRoId[st3Rec.RealityObject.Id].MargSum,
                                RealEstateTypeName = margSumByRoId[st3Rec.RealityObject.Id].RealEstateTypeName
                            });
                    }
                }

                if (!result.ContainsKey(groupedSt3Rec.RealityObject.Id) && margSumByRoId.ContainsKey(groupedSt3Rec.RealityObject.Id)
                    && margSumByRoId[groupedSt3Rec.RealityObject.Id].Sum < groupedSt3Rec.Sum)
                {
                    result.Add(groupedSt3Rec.RealityObject.Id, groupedSt3Rec.Year);

                    foreach (var st3Rec in groupedSt3Rec.Stage3List)
                    {
                        missingDpkrRecs.Add(
                            new MissingByMargCostDpkrRec
                            {
                                RealityObject = st3Rec.RealityObject,
                                CommonEstateObjects = st3Rec.CommonEstateObjects,
                                Year = st3Rec.Year,
                                Sum = st3Rec.Sum,
                                Area = margSumByRoId[st3Rec.RealityObject.Id].Area,
                                MargSum = margSumByRoId[st3Rec.RealityObject.Id].MargSum,
                                RealEstateTypeName = margSumByRoId[st3Rec.RealityObject.Id].RealEstateTypeName
                            });
                    }
                }
            }

            missingDpkrRecs.ForEach(x => session.Insert(x));

            return result;
        }

        /// <summary>
        /// Метод проверки можно ли опубликовывать программу
        /// Программу нельзя опубликовывать Если уже существует опубликованная программа в конечном Статусе
        /// </summary>
        public IDataResult ValidatePublishedProgram(BaseParams baseParams)
        {
            if (this.PublishedProgramDomain.GetAll().Any(x => x.ProgramVersion.IsMain && x.State.FinalState))
            {
                return new BaseDataResult(false, "Уже существует утвержденная опубликованная программа");
            }

            return new BaseDataResult(true, null);
        }

        /// <summary>
        /// Метод, для выполнения действий в транзации
        /// </summary>
        /// <param name="action">Действие</param>
        protected virtual void InTransaction(Action action)
        {
            using (var transaction = this.BeginTransaction())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            $"Произошла не известная ошибка при откате транзакции: \r\nMessage: {e.Message}; \r\nStackTrace:{e.StackTrace};",
                            exc);
                    }

                    throw;
                }
            }
        }

        /// <summary>Открыть транзакцию</summary>
        /// <returns>Экземпляр IDataTransaction</returns>
        protected virtual IDataTransaction BeginTransaction()
        {
            return this.Container.Resolve<IDataTransaction>();
        }

        /// <summary>
        /// Получить логи актуализации по записям опубликованной программы
        /// </summary>
        private List<VersionActualizeLogRecord> GetActualizeLogsForCreatedPublishedProgram(
            List<PublishedProgramRecDto> publishedProgramRecs,
            Dictionary<long, decimal> workVolumesDict,
            VersionActualizeLog log,
            VersionActualizeAction action)
        {
            var result = new List<VersionActualizeLogRecord>();

            if (!publishedProgramRecs.Any(x => x.IsValid))
            {
                return result;
            }

            foreach (var publishedProgramRec in publishedProgramRecs.Where(x => x.IsValid))
            {
                var logRecord = new VersionActualizeLogRecord
                {
                    ActualizeLog = log,
                    Action = action.GetDisplayName(),
                    RealityObject = new RealityObject { Id = publishedProgramRec.RoId },
                    WorkCode = publishedProgramRec.WorkCode,
                    Ceo = publishedProgramRec.CommonEstateObject,
                    PlanYear = publishedProgramRec.Stage3PlanYear,
                    PublishYear = publishedProgramRec.PublishedYear,
                    Volume = workVolumesDict.Get(publishedProgramRec.Stage2Id),
                    Sum = publishedProgramRec.Sum,
                    Number = publishedProgramRec.Number
                };

                switch (action)
                {
                    case VersionActualizeAction.Update:
                        logRecord.Sum = publishedProgramRec.Sum;
                        logRecord.ChangeSum = publishedProgramRec.Sum != publishedProgramRec.ChangeSum ? publishedProgramRec.ChangeSum : default;
                        logRecord.ChangePublishYear = publishedProgramRec.ChangePublishedYear;
                        logRecord.ChangeNumber = publishedProgramRec.ChangeNumber;
                        break;
                }

                result.Add(logRecord);
            }

            return result;
        }

        /// <summary>
        /// Dto-шка с информацией для создания
        /// записи опубликованной программы и лога актуализации
        /// </summary>
        private class PublishedProgramRecDto
        {
            /// <summary>
            /// Идентификатор записи
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Идентификатор версии программы
            /// </summary>
            public long ProgramVersionId { get; set; }

            /// <summary>
            /// Идентификатор stage2
            /// </summary>
            public long Stage2Id { get; set; }

            /// <summary>
            /// Населенный пункт из ФИАС
            /// </summary>
            public string Locality { get; set; }

            /// <summary>
            /// Улица из ФИАС
            /// </summary>
            public string Street { get; set; }

            /// <summary>
            /// Дом из ФИАС
            /// </summary>
            public string House { get; set; }

            /// <summary>
            /// Корпус из ФИАС
            /// </summary>
            public string Housing { get; set; }

            /// <summary>
            /// Адрес из ФИАС
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// ООИ
            /// </summary>
            public string CommonEstateObject { get; set; }

            /// <summary>
            /// Год постройки дома
            /// </summary>
            public int CommissioningYear { get; set; }

            /// <summary>
            /// Плановый год из записи опубликованной программы
            /// </summary>
            public int PlanYear { get; set; }

            /// <summary>
            /// Злановый год по stage3
            /// </summary>
            public int Stage3PlanYear { get; set; }

            /// <summary>
            /// Год публикации
            /// </summary>
            public int PublishedYear { get; set; }

            /// <summary>
            /// Изменение года публикации
            /// </summary>
            public int ChangePublishedYear { get; set; }

            /// <summary>
            /// Сумма
            /// </summary>
            public decimal Sum { get; set; }

            /// <summary>
            /// Изменение суммы
            /// </summary>
            public decimal ChangeSum { get; set; }

            /// <summary>
            /// Номер
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// Изменение номера
            /// </summary>
            public int ChangeNumber { get; set; }

            /// <summary>
            /// Идентификатор дома
            /// </summary>
            public long RoId { get; set; }

            /// <summary>
            /// Код работы
            /// </summary>
            public string WorkCode { get; set; }

            /// <summary>
            /// Валидность записи (признак создания записи лога)
            /// </summary>
            public bool IsValid { get; set; }
        }
    }
}