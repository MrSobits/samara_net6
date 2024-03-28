namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.Dicts;
    using Microsoft.CSharp.RuntimeBinder;
    using NCalc;
    using NHibernate;
    using Overhaul.Entities;
    using PriorityParams;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.MicroKernel;

    using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

    public class LongProgramService : ILongProgramService
    {
        /// <summary>
        /// Поле, для хранения "вычислителей" балла очередности
        /// </summary>
        private IProgrammPriorityParam[] evaluators;
        
        public IDomainService<RealityObjectStructuralElementInProgrammStage2> stage2Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> stage1Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> stage3Service { get; set; }

        public IDomainService<DpkrCorrectionStage2> dpkrCorrectedDomain { get; set; }

        public IDomainService<SubsidyMunicipality> SubsidyMuService { get; set; }

        public IDomainService<DpkrGroupedYear> DpkrPublishDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }

        public IDomainService<PublishedProgram> PublishedProgramDomain { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public ISessionProvider SessionProvider { get; set; }
        
        public IWindsorContainer Container { get; set; }

        public IRealityObjectStructElementService roStrElService { get; set; }

        private Dictionary<string, string> _dpkrParams;


        public IDataResult MakeLongProgram(BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;
            var useLimitCost = config.HouseAddInProgramConfig.UseLimitCost;

            // Проверяем параметры на валидность
            if (startYear == 0 || endYear == 0 || startYear >= endYear)
            {
                return new BaseDataResult(false, "Не задан параметр \"Период долгосрочной программы\"");
            }

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            List<RealityObjectStructuralElementInProgrammStage2> listStage2ToSave = null;
            List<RealityObjectStructuralElementInProgrammStage3> listStage3ToSave = null;

            try
            {
                var listStage1ToSave = GetStage1(startYear);

                GetStage2And3(listStage1ToSave, out listStage2ToSave, out listStage3ToSave);

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //удаляем существующие записи
                        session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg").ExecuteUpdate();
                        session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg_2").ExecuteUpdate();
                        session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg_3").ExecuteUpdate();
                        session.CreateSQLQuery("DELETE FROM OVRHL_MISS_DPKR_REC").ExecuteUpdate();

                        /* если учитываем предельную стоимость работ, то
                        получаем года в которые, сумма превысила предельную стоимость.
                        Не сохраняем записи, которые превысили, а так же другие записи по данному дому после превышения*/
                        if (useLimitCost == TypeUsage.Used)
                        {
                            var missYearByRoDict = GetMinMissingYearsByLimitCost(listStage3ToSave, session);

                            listStage3ToSave.ForEach(x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Year)
                                    session.Insert(x);
                            });

                            listStage2ToSave.ForEach(x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.RealityObject.Id) || missYearByRoDict[x.RealityObject.Id] > x.Stage3.Year)
                                    session.Insert(x);
                            });
                            listStage1ToSave.ForEach(x =>
                            {
                                if (!missYearByRoDict.ContainsKey(x.Stage2.RealityObject.Id) || missYearByRoDict[x.Stage2.RealityObject.Id] > x.Stage2.Stage3.Year)
                                    session.Insert(x);
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
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }

        public IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(int startYear, IQueryable<RealityObjectStructuralElement> roStructElQuery = null)
        {
            var formulaService = Container.Resolve<IFormulaService>();
            var structElementWorkService = Container.Resolve<IDomainService<StructuralElementWork>>();
            var workPriceService = Container.Resolve<IDomainService<NsoWorkPrice>>();
            var valuesService = Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
            var realEstService = Container.Resolve<IRealEstateTypeService>();
            var roDomain = Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var periodStart = config.ProgrammPeriodStart;
                var endYear = config.ProgrammPeriodEnd;
                var servicePercent = config.ServiceCost;
                var isWorkPriceFirstYear = config.WorkPriceCalcYear == WorkPriceCalcYear.First;
                var workPriceDetermineType = config.WorkPriceDetermineType;


                var listStage1ToSave = new List<RealityObjectStructuralElementInProgramm>();
                var dictNotExistPrices = new Dictionary<long, Dictionary<long, string>>();
                var dictFormulaParams = new Dictionary<string, List<string>>();

                var dictStructElWork = structElementWorkService.GetAll()
                        .Select(x => new
                        {
                            SeId = x.StructuralElement.Id,
                            JobId = x.Job.Id,
                            JobName = x.Job.Name
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.SeId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.JobId).ToDictionary(x => x.Key, z => z.Select(x => x.JobName).FirstOrDefault()));

                var dictPrices = workPriceService.GetAll()
                    .Where(x => x.Municipality != null)
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Year).ToDictionary(x => x.Key, z => z.AsEnumerable()));

                var dictValues = valuesService.GetAll()
                    .Select(x => new
                    {
                        ObjectId = x.Object.Id,
                        AttributeId = x.Attribute.Id,
                        x.Attribute.AttributeType,
                        x.Value
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectId)
                    .ToDictionary(x => x.Key,
                        y => y
                            .GroupBy(x => x.AttributeId)
                            .ToDictionary(x => x.Key, z => z.Select(x => new { x.AttributeType, x.Value }).FirstOrDefault()));

                var roSeQuery = roStructElQuery ?? roStrElService.GetElementsForLongProgram(RoStructElDomain);

                // Получаем Query по домам
                var roQuery = roDomain.GetAll().Where(x => roSeQuery.Any(y => y.RealityObject.Id == x.Id)).AsQueryable();

                // получаем типы домов
                var dictRealEstTypes = realEstService.GetRealEstateTypes(roQuery);

                var data = roSeQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.StructuralElement.Group.Formula,
                        x.StructuralElement.Group.FormulaParams,
                        SeId = x.StructuralElement.Id,
                        RealityObjectId = x.RealityObject.Id,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        x.Volume,
                        x.Wearout,
                        x.LastOverhaulYear,
                        Element = x,
                        x.RealityObject.BuildYear,
                        x.RealityObject.PrivatizationDateFirstApartment,
                        x.RealityObject.PhysicalWear,
                        x.RealityObject.AreaLiving,
                        x.RealityObject.AreaMkd,
                        x.RealityObject.AreaLivingNotLivingMkd,
                        x.StructuralElement,
                        CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                        StructuralElementLifeTime = x.StructuralElement.LifeTime,
                        StructuralElementLifeTimeAfterRepair = x.StructuralElement.LifeTimeAfterRepair,
                        x.StructuralElement.CalculateBy,
                        CapitalGroupId = x.RealityObject.CapitalGroup != null ? x.RealityObject.CapitalGroup.Id : 0
                    })
                    .AsEnumerable();

                foreach (var objectElement in data)
                {
                    // Достаем формулу
                    var formula = objectElement.Formula;

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
                        var listParams = ((ListDataResult)checkFormula).Data as List<string> ?? new List<string>();

                        dictFormulaParams.Add(formula, listParams);
                    }

                    var atrValue = new Dictionary<string, object>();

                    foreach (var formulaParam in objectElement.FormulaParams)
                    {
                        var param = formulaParam;
                        if (param.Attribute != null && dictValues.ContainsKey(objectElement.Id) && dictValues[objectElement.Id].ContainsKey(param.Attribute.Id))
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
                            var resolver =
                                Container.Resolve<IFormulaParameter>(formulaParam.ValueResolverCode,
                                    new Arguments
                                    {
                                        {"Config", config},
                                        {"RealityObjectStructuralElement", objectElement.Element},
                                        {"BuildYear", objectElement.BuildYear},
                                        {"PrivatizationDateFirstApartment", objectElement.PrivatizationDateFirstApartment},
                                        {"PhysicalWear", objectElement.PhysicalWear},
                                        {"StructuralElement", objectElement.StructuralElement},
                                    });

                            if (!atrValue.ContainsKey(param.Name))
                            {
                                atrValue.Add(param.Name, resolver.GetValue());
                            }

                        }
                    }

                    if (dictFormulaParams[formula].Any(x => !atrValue.ContainsKey(x)))
                    {
                        continue;
                    }

                    // Считаем год по формуле
                    var expr = new Expression(formula) { Parameters = atrValue };
                    var planYear = (int)expr.Evaluate().ToDecimal();
                    planYear = planYear > 0 ? planYear : endYear - 1;

                    var jobs = dictStructElWork.ContainsKey(objectElement.SeId)
                        ? dictStructElWork[objectElement.SeId]
                        : new Dictionary<long, string>();

                    var volume = 0M;

                    switch (objectElement.CalculateBy)
                    {
                        case PriceCalculateBy.Volume:
                            volume = objectElement.Volume; break;
                        case PriceCalculateBy.LivingArea:
                            volume = objectElement.AreaLiving.HasValue ? objectElement.AreaLiving.Value : 0m; break;
                        case PriceCalculateBy.TotalArea:
                            volume = objectElement.AreaMkd.HasValue ? objectElement.AreaMkd.Value : 0m; break;
                        case PriceCalculateBy.AreaLivingNotLivingMkd:
                            volume = objectElement.AreaLivingNotLivingMkd.HasValue ? objectElement.AreaLivingNotLivingMkd.Value : 0m; break;
                    }

                    var sum = 0M;
                    var serviceCost = 0M;

                    var workPricesByYear = dictPrices.ContainsKey(objectElement.MunicipalityId)
                                               ? dictPrices[objectElement.MunicipalityId]
                                               : new Dictionary<int, IEnumerable<NsoWorkPrice>>();

                    if (planYear <= endYear)
                    {
                        // Если год по формуле не попадает в программу, то делаем его начальным годом программы
                        planYear = Math.Max(startYear, planYear);

                        // для расченок ерм начальный год имеено начало программы потому что....
                        var year = isWorkPriceFirstYear ? periodStart : planYear;

                        var realEstTypes = dictRealEstTypes.ContainsKey(objectElement.RealityObjectId)
                                                   ? dictRealEstTypes[objectElement.RealityObjectId]
                                                   : null;

                        var workPrices = GetWorkPrices(
                            workPricesByYear,
                            year,
                            jobs.Keys.AsEnumerable(),
                            workPriceDetermineType,
                            objectElement.CapitalGroupId,
                            realEstTypes);

                        var costSum = GetDpkrCostByYear(workPrices, objectElement.CalculateBy);

                        // если не найдена расценка, добавляем ее в dictionary
                        if (!costSum.HasValue)
                        {
                            AddNotExistPrice(ref dictNotExistPrices, jobs, objectElement.MunicipalityId);
                        }

                        sum = (costSum.ToDecimal() * volume).RoundDecimal(2);
                        serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                        var evaluatedElement =
                            new RealityObjectStructuralElementInProgramm
                            {
                                StructuralElement = objectElement.Element,
                                Year = planYear,
                                Sum = sum,
                                ServiceCost = serviceCost,
                                Volume = objectElement.Volume,
                                LastOverhaulYear = objectElement.LastOverhaulYear,
                                Wearout = objectElement.Wearout
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

                        while (year <= endYear)
                        {
                            if (!isWorkPriceFirstYear)
                            {

                                var realEstTypes = dictRealEstTypes.ContainsKey(objectElement.RealityObjectId)
                                                   ? dictRealEstTypes[objectElement.RealityObjectId]
                                                   : null;
                                var workPrices = GetWorkPrices(workPricesByYear,
                                    year,
                                    jobs.Keys.AsEnumerable(),
                                    workPriceDetermineType,
                                    objectElement.CapitalGroupId,
                                    realEstTypes);

                                var costSum = GetDpkrCostByYear(workPrices, objectElement.CalculateBy);

                                // если не найдена расценка, добавляем ее в dictionary
                                if (!costSum.HasValue)
                                {
                                    AddNotExistPrice(ref dictNotExistPrices, jobs, objectElement.MunicipalityId);
                                }

                                sum = (costSum.ToDecimal() * volume).RoundDecimal(2);
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

                return DeleteByWear(roSeQuery, listStage1ToSave);
            }
            finally
            {
                Container.Release(formulaService);
                Container.Release(structElementWorkService);
                Container.Release(workPriceService);
                Container.Release(valuesService);
                Container.Release(realEstService);
                Container.Release(roDomain);
            }
        }

        public void GetStage2And3(IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records, out List<RealityObjectStructuralElementInProgrammStage2> listSt2, out List<RealityObjectStructuralElementInProgrammStage3> listSt3)
        {
            var ceoService = Container.Resolve<IDomainService<CommonEstateObject>>();
            var roService = Container.Resolve<IDomainService<RealityObject>>();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;
            var stage2Period = config.GroupByCeoPeriod;

            // получаем минимальную дату из уже полученных записей 1 этапа
            minYear = stage1Records.Any() ? stage1Records.Min(x => x.Year) : minYear;

            var startYear = minYear;
            var endYear = minYear + stage2Period;

            listSt2 = new List<RealityObjectStructuralElementInProgrammStage2>();
            listSt3 = new List<RealityObjectStructuralElementInProgrammStage3>();

            var ceoDict = ceoService.GetAll().ToDictionary(x => x.Id);

            var roIds = stage1Records.Select(x => x.StructuralElement.RealityObject.Id).Distinct().ToList();

            var dictRoInfo = roService.GetAll()
                                .Where(x => roIds.Contains(x.Id))
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Address
                                })
                                .AsEnumerable()
                                .Distinct(x => x.Id)
                                .ToDictionary(x => x.Id);

            while (startYear <= maxYear)
            {
                var period = startYear;
                var period1 = endYear;

                var elements =
                    stage1Records
                        .Where(x => x.Year >= period && x.Year < period1)
                        .Select(x => new
                        {
                            x.Id,
                            x.Year,
                            RoId = x.StructuralElement.RealityObject.Id,
                            CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                            x.Sum,
                            x.ServiceCost,
                            RoStrElId = x.StructuralElement.Id,
                            Stage1 = x
                        })
                        .GroupBy(x => new {x.RoId, x.CeoId})
                        .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var item in elements)
                {
                    var roInfo = dictRoInfo[item.Key.RoId];

                    var ro = new RealityObject
                    {
                        Id = item.Key.RoId,
                        Address = roInfo.Address
                    };

                    var ceo = ceoDict[item.Key.CeoId];

                    var stage2 = new RealityObjectStructuralElementInProgrammStage2
                    {
                        StructuralElements = string.Empty,
                        CommonEstateObject = ceo,
                        RealityObject = ro,
                        Sum = 0M
                    };

                    var listYears = new List<int>();

                    foreach (var val in item.Value)
                    {
                        var stage1 = val.Stage1;
                        stage1.Stage2 = stage2;

                        listYears.Add(val.Year);
                        stage2.Sum += val.ServiceCost + val.Sum;
                    }

                    stage2.Year = Math.Round(listYears.Average(), MidpointRounding.ToEven).ToInt();

                    listSt2.Add(stage2);

                    var stage3 = new RealityObjectStructuralElementInProgrammStage3
                    {
                        RealityObject = ro,
                        CommonEstateObjects = ceo.Name,
                        Year = stage2.Year,
                        Sum = stage2.Sum
                    };

                    listSt3.Add(stage3);

                    stage2.Stage3 = stage3;
                }

                startYear = endYear;
                endYear = endYear + stage2Period;
            }
        }

        private IEnumerable<NsoWorkPrice> GetWorkPrices(Dictionary<int, IEnumerable<NsoWorkPrice>> workPricesByYear,
                                                        int year,
                                                        IEnumerable<long> jobs,
                                                        WorkPriceDetermineType type,
                                                        long cpitalGroupId,
                                                        IEnumerable<long> RealEstTypes
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
                            type == WorkPriceDetermineType.WithRealEstType && RealEstTypes != null,
                            x => x.RealEstateType != null && RealEstTypes.Contains(x.RealEstateType.Id))
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
            return new List<NsoWorkPrice>();
        }

        /// <summary>
        /// Метод, возвращает сумму стоимостей работ, возвращает null если нет расценок по работам
        /// </summary>
        private decimal? GetDpkrCostByYear(IEnumerable<NsoWorkPrice> jobPrices, PriceCalculateBy calculateBy)
        {
            
            // null потому что нужно выводить список не заполненных расценок
            decimal? costSum = null;

            if (jobPrices.Any())
            {
                switch (calculateBy)
                {
                    case PriceCalculateBy.Volume:
                        costSum = jobPrices.Sum(x => x.NormativeCost);
                        break;
                    case PriceCalculateBy.LivingArea:
                    case PriceCalculateBy.TotalArea:
                    case PriceCalculateBy.AreaLivingNotLivingMkd:
                        costSum = jobPrices.Sum(x => x.SquareMeterCost).ToDecimal();
                        break;
                }
            }

            return costSum;
        }

        private void AddNotExistPrice(ref Dictionary<long, Dictionary<long, string>> dictNotExistPrices, Dictionary<long, string> jobs, long muId)
        {
            if (!dictNotExistPrices.ContainsKey(muId))
            {
                dictNotExistPrices.Add(muId, new Dictionary<long, string>());
            }

            foreach (var job in jobs)
            {
                if (!dictNotExistPrices[muId].ContainsKey(job.Key))
                    dictNotExistPrices[muId].Add(job.Key, job.Value);
            }
        }

        private List<RealityObjectStructuralElementInProgramm> DeleteByWear(IQueryable<RealityObjectStructuralElement> roSeQuery, List<RealityObjectStructuralElementInProgramm> listStage1ToSave)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var typeUseWearMainCeo = config.HouseAddInProgramConfig.TypeUseWearMainCeo;
            var wearMainCeo = config.HouseAddInProgramConfig.WearMainCeo;

            if (typeUseWearMainCeo == TypeUseWearMainCeo.NotUsed)
            {
                return listStage1ToSave;
            }

            var roStructElDict = roSeQuery
                        .Where(x => x.StructuralElement.Group.CommonEstateObject.IsMain)
                        .Select(x => new
                        {
                            RealObjId = x.RealityObject.Id,
                            x.Wearout
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RealObjId)
                        .ToDictionary(x => x.Key, y => new
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
                    } break;

                case TypeUseWearMainCeo.OnlyOne:
                    {
                        roIdToDelete = roStructElDict
                                          .Where(x => x.Value.OverWearoutCnt > 0)
                                          .Select(x => x.Key)
                                          .ToList();
                    } break;
            }

            return listStage1ToSave.Where(x => !roIdToDelete.Contains(x.StructuralElement.RealityObject.Id))
                                            .ToList();
        }

        // возвращает Dictionary, ключ - id Жилого дома, значение - минимальный Год превышающий предельную стоимость
        private Dictionary<long, int> GetMinMissingYearsByLimitCost(List<RealityObjectStructuralElementInProgrammStage3> listStage3, IStatelessSession session)
        {

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var rateCalcArea = config.RateCalcTypeArea;
            var missingDpkrRecs = new List<MissingByMargCostDpkrRec>();
            var result = new Dictionary<long, int>();

            var roQuery = Container.Resolve<IRepository<RealityObject>>().GetAll()
                                    .Where(x => x.TypeHouse != TypeHouse.Individual && x.TypeHouse != TypeHouse.NotSet)
                                    .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated)
                                    .Where(x => !EmergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                                        || EmergencyDomain.GetAll()
                                            .Where(e => e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)
                                            .Any(e => e.RealityObject.Id == x.Id))
                                    .Where(x => !x.IsNotInvolvedCr);

            var roRealEstTypesDict = Container.Resolve<IRealEstateTypeService>().GetRealEstateTypes(roQuery);

            var realEstatesArray =
                Container.Resolve<IDomainService<RealEstateType>>()
                         .GetAll()
                         .Where(x => x.MarginalRepairCost.HasValue)
                         .ToArray();

            var margSumByRoId = roQuery
                         .Select(x => new
                         {
                             x.Id,
                             x.AreaMkd,
                             x.AreaLiving,
                             x.AreaLivingNotLivingMkd
                         })
                         .AsEnumerable()
                         .Where(x => roRealEstTypesDict.ContainsKey(x.Id) && realEstatesArray.Any(y => roRealEstTypesDict[x.Id].Contains(y.Id)))
                         .ToDictionary(x => x.Id, y =>
                         {
                             var realEstateType =
                                 realEstatesArray
                                     .Where(x => roRealEstTypesDict[y.Id].Contains(x.Id))
                                     .OrderBy(x => x.MarginalRepairCost)
                                     .First();

                             var area = rateCalcArea == RateCalcTypeArea.AreaLiving ? y.AreaLiving :
                                               rateCalcArea == RateCalcTypeArea.AreaMkd ? y.AreaMkd :
                                               rateCalcArea == RateCalcTypeArea.AreaLivingNotLiving ? y.AreaLivingNotLivingMkd : 0M;

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
                                 .GroupBy(x => new { x.Year, x.RealityObject.Id })
                         .ToDictionary(x => x.Key, y => new
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
                        missingDpkrRecs.Add(new MissingByMargCostDpkrRec
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
                    missingDpkrRecs.Add(new MissingByMargCostDpkrRec
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
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ValidatePublishedProgram(BaseParams baseParams)
        {
            if (PublishedProgramDomain.GetAll().Any(x => x.ProgramVersion.IsMain && x.State.FinalState))
            {
                return new BaseDataResult(false, "Уже существует утвержденная опубликованная программа");
            }

            return new BaseDataResult(true, null);
        }

        public IDataResult CreateDpkrForPublish(BaseParams baseParams)
        {
            try
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var minYear = config.ProgrammPeriodStart;
                var period = config.PublishProgramConfig.PublicationPeriod;
                var maxYear = config.ProgrammPeriodEnd;
                var shortTerm = config.ShortTermProgPeriod;
                var useShortTerm = config.PublishProgramConfig.UseShortProgramPeriod;

                if (period == 0)
                {
                    return new BaseDataResult(false, "Не найден параметр \" Период для публикации \"");
                }

                if (useShortTerm == TypeUseShortProgramPeriod.WithOut)
                {
                    // Поскольку в настройках указано, что не используем период краткосрочки
                    shortTerm = 0;
                }

                if (shortTerm > 0)
                {
                    minYear = minYear + shortTerm;
                }

                // поулчаем основную версию\

                var version = ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }
            
                var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Удаляем записи опубликованной программы именно этой основной версии
                        session.CreateSQLQuery(string.Format(   "  delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID in ( "+
                                                                "       select id from OVRHL_PUBLISH_PRG where version_id = {0} ) ",
                                                                version.Id)
                                              ).ExecuteUpdate();
                    
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                // Проверяем Существует ли нужный статус и если нет то создаем новый
                var firstState = StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);

                if (firstState == null)
                {
                    firstState = new State
                    {
                        Name = "Черновик",
                        Code = "Черновик",
                        StartState = true,
                        TypeId = "ovrhl_published_program"
                    };

                    StateDomain.Save(firstState);
                }

                PublishedProgram publish = PublishedProgramDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.IsMain);

                // Грохаем существующую запись опубликованной программы поскольку
                // у нее могли существоват ьуже ненужные подписи ЭЦП
                if (publish != null)
                {
                    PublishedProgramDomain.Delete(publish.Id);
                }

                publish = new PublishedProgram
                                  {
                                      State = firstState,
                                      ProgramVersion = version
                                  };

                var listRecordsToSave = new List<PublishedProgramRecord>();

                // Получаем записи корректировки и поним создаем опубликованную программу
                var dataCorrection =
                    dpkrCorrectedDomain.GetAll()
                                       .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                                       .Select(x => new
                                                      {
                                                          x.Id,
                                                          st2Id = x.Stage2.Id,
                                                          Year = x.PlanYear,
                                                          x.Stage2.Stage3Version.IndexNumber,
                                                          Locality = x.RealityObject.FiasAddress.PlaceName,
                                                          Street = x.RealityObject.FiasAddress.StreetName,
                                                          x.RealityObject.FiasAddress.House,
                                                          x.RealityObject.FiasAddress.Housing,
                                                          Address = x.RealityObject.FiasAddress.AddressName,
                                                          CommonEstateobject = x.Stage2.CommonEstateObject.Name,
                                                          CommissioningYear = x.RealityObject.BuildYear.HasValue ? x.RealityObject.BuildYear.Value : 0,
                                                          x.Stage2.Sum
                                                      })
                                       .ToList();

                foreach (var rec in dataCorrection)
                {
                    var publicationYear = rec.Year;

                    if (rec.Year >= minYear)
                    {
                        publicationYear = rec.Year + period - 1 - (rec.Year - minYear) % period;
                        publicationYear = publicationYear > maxYear ? maxYear : publicationYear;
                    }

                    var newRec = new PublishedProgramRecord
                                     {
                                         PublishedProgram = publish,
                                         Stage2 = new VersionRecordStage2 { Id = rec.st2Id },
                                         PublishedYear = publicationYear,
                                         IndexNumber = rec.IndexNumber,
                                         Locality = rec.Locality,
                                         Street = rec.Street,
                                         House = rec.House,
                                         Housing = rec.Housing,
                                         Address = rec.Address,
                                         CommonEstateobject = rec.CommonEstateobject,
                                         CommissioningYear = rec.CommissioningYear,
                                         Sum = rec.Sum
                                     };

                    listRecordsToSave.Add(newRec);
                }

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        if (publish.Id <= 0)
                        {
                            session.Insert(publish);
                        }

                        listRecordsToSave.ForEach(x => session.Insert(x));

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, string.Format("Ошибка очередности:{0}", exc.Message));
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            return new BaseDataResult();
        }

        /// <summary>
        /// Получение параметров очередности ДПКР 
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetParams(BaseParams baseParams)
        {
            // В этом методе получаем Тип метода расчета
            // Расчет по критериям
            // Расчет по баллам
            // Расчет по критериям и баллам
            //
            // И если расчет по критериям и баллам, только в этом случае включаем параметр баллы
            // Иначе только критерии без баллов
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var method = config.MethodOfCalculation;

            var parameters =
                Container.ResolveAll<IProgrammPriorityParam>()
                .Where(x => method == TypePriority.Criteria ? x.Code != PointsParam.Code : x.Code != null)
                    .Select(x => new { Id = x.Code, x.Name, x.Code });

            return new ListDataResult(parameters, parameters.Count());
        }

        /// <summary>
        /// Установка очередности ДПКР по критериям очередности
        /// </summary>
        public IDataResult SetPriority(BaseParams baseParams)
        {
            try
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var method = config.MethodOfCalculation;
                
                var userCriterias = new Dictionary<string, int>();

                var pointsParams = new Dictionary<long, List<StoredPointParam>>();

                switch (method)
                {
                    case TypePriority.Criteria:
                    {
                        var recs = baseParams.Params.GetAs<object[]>("records");
                        userCriterias = GetUserCriterias(recs);
                        break;
                    }
                    case TypePriority.Points:
                    {
                        pointsParams = GetPoints();
                        userCriterias.Add("PointsParam", 1);
                        break;
                    }
                    case TypePriority.CriteriaAndPoins:
                    {
                        var recs = baseParams.Params.GetAs<object[]>("records");
                        userCriterias = GetUserCriterias(recs);
                        pointsParams = this.GetPoints();
                        break;
                    }
                }

                var points = pointsParams.ToDictionary(x => x.Key, y => y.Value.Sum(x => x.Value));

                var currPriorityParamsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

                InTransaction(() =>
                {
                    foreach (var value in currPriorityParamsService.GetAll())
                    {
                        currPriorityParamsService.Delete(value.Id);
                    }

                    foreach (var value in userCriterias)
                    {
                        currPriorityParamsService.Save(
                            new CurrentPrioirityParams {Code = value.Key, Order = value.Value});
                    }
                });

                // Получаем параметры из конфига
                var startYear = config.ProgrammPeriodStart;

                // Такой словарик нужен для того чтобы быстро получать элементы при сохранении уже из готовых данных по Id
                var dictStage3 = stage3Service.GetAll().ToDictionary(x => x.Id);

                // Внимательно - дергаем из сервиса 2 этапа, потому что 3 этап по сути является дубляжом 2 этапа!
                // RoCeoKey тут получаю поскольку в дальнейшем много раз используется этот ключ и чтобы каждый раз его не формирвоать 
                // пытаюсь выиграть в милисекундах
                var muStage3 =
                    stage2Service.GetAll()
                        .Where(x => x.Stage3.RealityObject != null && x.CommonEstateObject != null)
                        .Select(x => new Stage3Order
                        {
                            Id = x.Stage3.Id,
                            Year = x.Stage3.Year,
                            Stage3 = x.Stage3,
                            RoId = x.RealityObject.Id,
                            PrivatizDate = x.RealityObject.PrivatizationDateFirstApartment,
                            BuildYear = x.RealityObject.BuildYear,
                            DateTechInspection = x.RealityObject.DateTechInspection,
                            PhysicalWear = x.RealityObject.PhysicalWear,
                            DateCommissioning = x.RealityObject.DateCommissioning
                        })
                        .ToList();

                var dictDensity = stage3Service.GetAll()
                    .Where(x => x.RealityObject != null)
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObject.AreaLiving,
                        x.RealityObject.NumberLiving
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id,
                        x => x.AreaLiving.HasValue && x.AreaLiving.Value > 0 && x.NumberLiving.HasValue
                            ? x.NumberLiving.Value / x.AreaLiving.Value
                            : 0);

                //получение годом капитального ремонта
                var firstPrivYears = GetFirstPrivYears(startYear);

                var lastOvrhlYears = GetLastOvrhlYears();

                var weights = GetWeights();

                var yearsWithLifetimes = GetSeYearsWithLifetimes();

                foreach (var item in muStage3)
                {
                    var injections = new
                    {
                        item.PrivatizDate,
                        DictPoints = points,
                        item.Year,
                        item.BuildYear,
                        Density = dictDensity.ContainsKey(item.Id) ? dictDensity[item.Id] : 0m,
                        Weights = weights.ContainsKey(item.Id) ? weights[item.Id] : new[] {0},
                        FirstPrivYears = firstPrivYears.ContainsKey(item.Id) ? firstPrivYears[item.Id] : new List<int>(),
                        OverhaulYears = lastOvrhlYears.ContainsKey(item.Id) ? lastOvrhlYears[item.Id] : new List<int>(),
                        OverhaulYearsWithLifetimes = yearsWithLifetimes.ContainsKey(item.Id) ? yearsWithLifetimes[item.Id] : new List<int>()
                    };

                    CalculateOrder(item, userCriterias.Keys, injections);
                }

                /* По-умолчанию сначала сортируем по плановому году */
                var proxyList2 = muStage3.OrderBy(x => x.Year);
                foreach (var item in userCriterias.OrderBy(x => x.Value))
                {
                    var key = item.Key;

                    var eval = ResolveEvaluator(key, new object());
                    if (eval != null)
                    {
                        proxyList2 = eval.Asc
                            ? proxyList2.ThenBy(x => x.OrderDict[key])
                            : proxyList2.ThenByDescending(x => x.OrderDict[key]);
                    }
                }

                var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var index = 1;
                        foreach (var item in proxyList2)
                        {
                            if (!dictStage3.ContainsKey(item.Id)) continue;

                            var st3 = dictStage3[item.Id];

                            this.FillStage3Criteria(st3, item.OrderDict);

                            st3.StoredPointParams = pointsParams.ContainsKey(st3.Id) ? pointsParams[st3.Id] : new List<StoredPointParam>();

                            st3.IndexNumber = index++;

                            session.Update(st3);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, string.Format("Ошибка очередности:{0}", exc.Message));
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
        }

        private Dictionary<string, int> GetUserCriterias(object[] records)
        {
            if (records == null)
            {
                throw new ValidationException("Не указаны параметры");
            }

            var criterias = new Dictionary<string, int>();

            for (int i = 0; i < records.Length; i++)
            {
                var dd = records[i] as DynamicDictionary;
                if (dd == null)
                {
                    continue;
                }

                var code = dd.Get("Code", string.Empty);

                if (criterias.ContainsKey(code))
                {
                    throw new ValidationException("Параметры должны быть уникальными");
                }

                criterias.Add(code, dd.GetAs<int>("Order"));
            }

            return criterias;
        }

        private Dictionary<long, List<StoredPointParam>> GetPoints()
        {
            var result = new Dictionary<long, List<StoredPointParam>>();

            var roseDomain = Container.ResolveDomain<RealityObjectStructuralElement>();

            var prParamAdditionDomain = Container.Resolve<IDomainService<PriorityParamAddition>>();
            var st2Domain = Container.ResolveDomain<RealityObjectStructuralElementInProgrammStage2>();
            var st3Domain = Container.ResolveDomain<RealityObjectStructuralElementInProgrammStage3>();
            var qualDomain = Container.ResolveDomain<QualityPriorityParam>();
            var quantDomain = Container.ResolveDomain<QuantPriorityParam>();
            var multiDomain = Container.ResolveDomain<MultiPriorityParam>();

            var priorityParamAdditions = prParamAdditionDomain.GetAll().ToDictionary(x => x.Code, y => y);
            PriorityParamAddition addtionInfo = null;
            using (Container.Using(roseDomain, st2Domain, st3Domain, qualDomain, quantDomain, multiDomain))
            {
                var dictRoWearout = roseDomain.GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Wearout
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y =>
                {
                    var count = y.Count();

                    decimal wear = 0;

                    if (count > 0)
                    {
                        wear = y.Sum(x => x.Wearout) / count;
                    }
                    return wear;
                });

                var roceoDict =
                    st2Domain.GetAll()
                        .Select(x => new
                        {
                            x.Stage3.Id,
                            CeoId = x.CommonEstateObject.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.CeoId).ToHashSet());

                var wearout = GetWearout();

                var countWorksByRo = GetCountsWorksByRo();

                var countWorksInYear = GetCountWorksInYear();

                var usages = GetStrElUsagesDict();
                                        
                var priorParams = Container.ResolveAll<IPriorityParams>(new Arguments
                {
                    
                { "DictRoWearout", dictRoWearout },
                { "Stage3CeoDict", roceoDict },
                { "CountWorksByRo", countWorksByRo },
                { "CountWorksInYear", countWorksInYear },
                { "StructElementUsageValues", usages },
                { "Wearout", wearout }
                })
                .ToDictionary(x => x.Id);

                var stage3Records =
                    st3Domain.GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            x.Year,
                            RoId = x.RealityObject.Id,
                            MoSettlement = x.RealityObject.MoSettlement,
                            CapGroupId = (long?)x.RealityObject.CapitalGroup.Id,
                            CapGroupCode = x.RealityObject.CapitalGroup.Code,
                            x.RealityObject.EnergyPassport,
                            x.RealityObject.ConfirmWorkDocs,
                            x.RealityObject.ProjectDocs,
                            x.RealityObject.DateCommissioning,
                            x.RealityObject.BuildYear,
                            x.RealityObject.NumberLiving,
                            x.RealityObject.AreaLiving,
                            x.RealityObject.PhysicalWear,
                            x.RealityObject.PercentDebt,
                            x.RealityObject.FiasAddress.PlaceName
                        })
                        .AsEnumerable();

                var qualityParams = qualDomain.GetAll().ToList();
                var quantParams = quantDomain.GetAll().ToList();
                var multiParams = multiDomain.GetAll().ToList();

                foreach (var record in stage3Records)
                {
                    var id = record.Id;

                    result.Add(id, new List<StoredPointParam>());

                    //создаем копию записи, в которую забиваем все нужные поля
                    var stage3Rec = new RealityObjectStructuralElementInProgrammStage3
                    {
                        Id = id,
                        Year = record.Year,
                        RealityObject = new RealityObject
                        {
                            Id = record.RoId,
                            MoSettlement = record.MoSettlement,
                            EnergyPassport = record.EnergyPassport,
                            ConfirmWorkDocs = record.ConfirmWorkDocs,
                            ProjectDocs = record.ProjectDocs,
                            DateCommissioning = record.DateCommissioning,
                            BuildYear = record.BuildYear,
                            AreaLiving = record.AreaLiving,
                            NumberLiving = record.NumberLiving,
                            PhysicalWear = record.PhysicalWear,
                            PercentDebt = record.PercentDebt,
                            CapitalGroup = new CapitalGroup
                            {
                                Id = record.CapGroupId.GetValueOrDefault(),
                                Code = record.CapGroupCode
                            },
                            FiasAddress = new FiasAddress
                            {
                                PlaceName = record.PlaceName
                            }
                        }
                    };

                    foreach (var param in qualityParams)
                    {
                        if (!priorParams.ContainsKey(param.Code))
                        {
                            continue;
                        }

                        var value = (int)priorParams[param.Code].GetValue(stage3Rec);

                        if (value == param.Value)
                        {
                            addtionInfo = priorityParamAdditions.ContainsKey(param.Code) ? priorityParamAdditions[param.Code] : null;
                            result[id].Add(new StoredPointParam
                            {
                                Code = param.Code,
                                Value = param.Point * 
                                (addtionInfo != null && addtionInfo.AdditionFactor == PriorityParamAdditionFactor.Using ? addtionInfo.FactorValue : 1)
                            });
                        }
                    }

                    foreach (var param in quantParams)
                    {
                        if (!priorParams.ContainsKey(param.Code))
                        {
                            continue;
                        }

                        var value = priorParams[param.Code].GetValue(stage3Rec).ToDecimal();

                        decimal? minValue = null;
                        decimal? maxValue = null;

                        if (!string.IsNullOrWhiteSpace(param.MinValue))
                        {
                            minValue = param.MinValue.ToDecimal();
                        }

                        if (!string.IsNullOrWhiteSpace(param.MaxValue))
                        {
                            maxValue = param.MaxValue.ToDecimal();
                        }

                        if ((!minValue.HasValue || minValue <= value) && (!maxValue.HasValue || maxValue.Value >= value))
                        {
                            addtionInfo = priorityParamAdditions.ContainsKey(param.Code) ? priorityParamAdditions[param.Code] : null;
                            var additPoint = (addtionInfo != null && addtionInfo.AdditionFactor == PriorityParamAdditionFactor.Using ? addtionInfo.FactorValue : 1);
                            result[id].Add(new StoredPointParam
                            {
                                Code = param.Code,
                                Value = (addtionInfo != null && addtionInfo.FinalValue == PriorityParamFinalValue.ParameterValue ? value : param.Point) * additPoint
                            });
                        }
                    }

                    foreach (var param in multiParams)
                    {
                        if (!priorParams.ContainsKey(param.Code))
                        {
                            continue;
                        }

                        if (((IMultiPriorityParam)priorParams[param.Code]).CheckContains(stage3Rec, param.StoredValues))
                        {
                            var first = result[id].FirstOrDefault(x => x.Code == param.Code);
                            if (first != null)
                            {
                                first.Value += param.Point;
                            }
                            else
                            {
                                addtionInfo = priorityParamAdditions.ContainsKey(param.Code) ? priorityParamAdditions[param.Code] : null;
                                result[id].Add(new StoredPointParam
                                {
                                    Code = param.Code,
                                    Value = param.Point *
                                    (addtionInfo != null && addtionInfo.AdditionFactor == PriorityParamAdditionFactor.Using ? addtionInfo.FactorValue : 1)
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Заполнение критериев сортировки
        /// </summary>
        /// <param name="st3Item">Объект ДПКР</param>
        /// <param name="orderDict">Словарь данных приоритезации</param>
        private void FillStage3Criteria(RealityObjectStructuralElementInProgrammStage3 st3Item, Dictionary<string, object> orderDict)
        {
            st3Item.StoredCriteria = st3Item.StoredCriteria ?? new List<StoredPriorityParam>();
            st3Item.StoredCriteria.Clear();

            foreach (var param in orderDict)
            {
                st3Item.StoredCriteria.Add(new StoredPriorityParam
                {
                    Criterion = param.Key,
                    Value = param.Value.ToStr()
                });
            }
        }

        /// <summary>
        /// Вычисление параметра приоритезации
        /// </summary>
        /// <param name="stage3">Объект ДПКР</param>
        /// <param name="keys">Названия параметров</param>
        /// <param name="injections">Свойства, необходимые для расчетов параметров</param>
        private void CalculateOrder(Stage3Order st3Oreder, IEnumerable<string> keys, object injections)
        {
            foreach (var key in keys)
            {
                var eval = ResolveEvaluator(key, injections);

                if (eval != null)
                {
                    if (!st3Oreder.OrderDict.ContainsKey(key))
                    {
                        st3Oreder.OrderDict.Add(key, eval.GetValue(st3Oreder.Stage3));
                    }
                }
            }
        }

        public IDataResult ListDetails(BaseParams baseParams)
        {
            var stage3Id = baseParams.Params.GetAs<long>("st3Id");

            var data = stage1Service.GetAll()
                .Where(x => x.Stage2.Stage3.Id == stage3Id)
                .Select(x => new
                {
                    Stage2Year = x.Stage2.Year,
                    Stage2Sum = x.Stage2.Sum,
                    Stage2CeoName = x.Stage2.CommonEstateObject.Name,
                    x.Sum,
                    x.ServiceCost,
                    StructElementName = x.StructuralElement.StructuralElement.Name,
                    UnitMeasureName = x.StructuralElement.StructuralElement.UnitMeasure.Name,
                    x.StructuralElement.Volume,
                    x.Year
                })
                .ToArray();

            var result = data.GroupBy(x => new { Year = x.Stage2Year, Sum = x.Stage2Sum, Name = x.Stage2CeoName })
                .Select(x =>
                    new
                    {
                        x.Key.Year,
                        ServiceAndWorkSum = x.Sum(y => y.Sum) + x.Sum(y => y.ServiceCost),
                        x.Key.Name,
                        leaf = false,
                        WorkSum = x.Sum(y => y.Sum),
                        ServiceSum = x.Sum(y => y.ServiceCost),
                        Volume = x.Sum(y => y.Volume),
                        Children = x.Select(y =>
                            new
                            {
                                Name = y.StructElementName,
                                WorkSum = y.Sum,
                                ServiceSum = y.ServiceCost,
                                Measure = y.UnitMeasureName,
                                ServiceAndWorkSum = y.ServiceCost + y.Sum,
                                y.Volume,
                                y.Year,
                                leaf = true
                            })
                            .ToList()
                    })
                .ToList();

            return new BaseDataResult(new { Children = result });
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("st3Id");

            var stage3 = stage3Service.Get(id);

            if (stage3 == null)
            {
                throw new ArgumentNullException("Не найдена информация по 3 этапу ДПКР");
            }

            var stage1Data = stage1Service.GetAll()
                .Where(x => x.Stage2.Stage3.Id == id)
                .Select(x => new
                {
                    x.ServiceCost,
                    x.Sum
                });

            decimal workSum = 0;
            decimal serviceSum = 0;

            if (stage1Data.Any())
            {
                workSum = stage1Data.Sum(x => x.Sum);
                serviceSum = stage1Data.Sum(x => x.ServiceCost);
            }

            var proxy = new
            {
                stage3.RealityObject.Address,
                stage3.IndexNumber,
                stage3.Point,
                stage3.Year,
                WorkSum = workSum,
                ServiceSum = serviceSum,
                ServiceAndWorkSum = workSum + serviceSum
            };

            return new BaseDataResult(proxy);
        }

        public IDataResult ListWorkTypes(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("st3Id");

            var stage3 = stage3Service.Get(id);

            if (stage3 == null)
            {
                throw new ArgumentNullException("Не найдена информация по 3 этапу ДПКР");
            }

            var stage1StructElementsInfo = stage1Service.GetAll()
                .Where(x => stage2Service.GetAll()
                    .Any(y => y.Id == x.Stage2.Id && y.Stage3.Id == id))
                    .Select(x => new
                    {
                        SeId = x.StructuralElement.StructuralElement.Id,
                        x.Sum,
                        x.ServiceCost
                    });

            var strElWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();
            var roStrelWorkDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            
            var roStrEls = roStrelWorkDomain.GetAll()
                .Where(x =>
                           stage1StructElementsInfo.Any(y => y.SeId == x.StructuralElement.Id)
                           && x.RealityObject.Id == stage3.RealityObject.Id)
                .ToList();

            /* Получили связки КЭ - работа для КЭ из первого этапа */
            var strElWorks = strElWorkDomain.GetAll()
                .Where(x => stage1StructElementsInfo.Any(y => y.SeId == x.StructuralElement.Id))
                .GroupBy(x => x.StructuralElement)
                .ToDictionary(x => x.Key, y => y.ToList());

            var result = new List<WorkTypeProxy>();
            foreach (var strElWork in strElWorks.Keys)
            {
                var roStrEl = roStrEls.FirstOrDefault(x => x.StructuralElement.Id == strElWork.Id);
                var stage1StructElement = stage1StructElementsInfo.FirstOrDefault(x => x.SeId == strElWork.Id);
                
                foreach (var elWork in strElWorks[strElWork])
                {
                    result.Add(new WorkTypeProxy
                    {
                        StructElement = strElWork.Name,
                        WorkKind = elWork.Job.Work.Name,
                        WorkType = elWork.Job.Work.TypeWork.ToStr(),
                        Volume = roStrEl.Return(x => x.Volume),
                        Sum = stage1StructElement.Return(x => x.Sum) + stage1StructElement.Return(x => x.ServiceCost)
                    });
                }
            }

            return new ListDataResult(result, result.Count());
        }

        public IDataResult MakeNewVersion(BaseParams baseParams)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var date = baseParams.Params.GetAs<DateTime>("Date");
            var name = baseParams.Params.GetAs<string>("Name");
            var isMain = baseParams.Params.GetAs<bool>("IsMain");

            var versionService = Container.Resolve<IDomainService<ProgramVersion>>();
            var paramsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            var stage3Records =
                stage3Service.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        x.Year,
                        x.CommonEstateObjects,
                        x.Sum,
                        x.IndexNumber,
                        x.Point,
                        x.StoredCriteria,
                        x.StoredPointParams
                    })
                    .ToList();

            var stage2Records = stage2Service.GetAll()
                .Select(x => new
                {
                    x.Id,
                    CommonEstateObjectId = x.CommonEstateObject.Id,
                    x.CommonEstateObject.Weight,
                    x.Sum,
                    Stage3Id = x.Stage3.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.First());

            var stage1Records = stage1Service.GetAll()
                .Where(x => x.Stage2 != null)
                .Select(x => new
                {
                    x.Id,
                    Stage2Id = x.Stage2.Id,
                    RealityObjectId = x.StructuralElement.RealityObject.Id,
                    x.Year,
                    StructuralElementId = x.StructuralElement.Id,
                    x.Sum,
                    x.ServiceCost,
                    x.Wearout,
                    x.Volume,
                    x.LastOverhaulYear
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage2Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            var stage3Params = paramsService.GetAll()
                .Select(x => new {x.Code, x.Order})
                .ToList();

            // создаем версию 
            var version = new ProgramVersion { Name = name, VersionDate = date, IsMain = isMain };

            // Массивы на сохранение
            var ver3L = new List<VersionRecord>();
            var ver2L = new List<VersionRecordStage2>();
            var ver1L = new List<VersionRecordStage1>();
            var verParams = new List<VersionParam>();

            foreach (var stage3 in stage3Params)
            {
                verParams.Add(
                    new VersionParam
                    {
                        ProgramVersion = version,
                        Code = stage3.Code,
                        Weight = stage3.Order
                    });
            }

            // Parallel тратит ресурсы на синхронизацию объектов
            // Ктому же ненужные условия Where когда можно просто сделать Dictionary
            // Итого 10 секунд на подготовку данных и 2 мин. на сохранение транзакции из 110 тыщ записей 1 этапа, 110 тыщ 2 этапа, 110 тыщ 3 этапа 

            // Это ник чему
            //Parallel.ForEach(stage3Records,
            // stage3 => 
            foreach (var stage3 in stage3Records)
            {
                var ver3 = new VersionRecord
                {
                    ProgramVersion = version,
                    RealityObject = new RealityObject { Id = stage3.RealityObjectId },
                    Year = stage3.Year,
                    CommonEstateObjects = stage3.CommonEstateObjects,
                    Sum = stage3.Sum,
                    IndexNumber = stage3.IndexNumber,
                    Point = stage3.Point,
                    StoredCriteria = stage3.StoredCriteria,
                    StoredPointParams = stage3.StoredPointParams
                };

                ver3L.Add(ver3);

                if (stage2Records.ContainsKey(stage3.Id))
                {
                    var st2Rec = stage2Records[stage3.Id];

                    var st2Version = new VersionRecordStage2
                    {
                        CommonEstateObject = new CommonEstateObject { Id = st2Rec.CommonEstateObjectId },
                        Stage3Version = ver3,
                        CommonEstateObjectWeight = st2Rec.Weight,
                        Sum = st2Rec.Sum,
                    };

                    ver2L.Add(st2Version);

                    /*
                    Parallel.ForEach(stage1Records.Where(x => x.Stage2Id == st2Rec.Id),
                        st1 => //foreach (var st1 in stage1Records.Where(x => x.Stage2Id == st2Rec.Id))
                            ver1L.Add(new VersionRecordStage1
                            {
                                RealityObject = new RealityObject
                                {
                                    Id = st1.RealityObjectId
                                },
                                Stage2Version = st2Version,
                                Year = st1.Year,
                                StructuralElement = new RealityObjectStructuralElement
                                {
                                    Id = st1.StructuralElementId
                                }
                            }));
                    */

                    if (stage1Records.ContainsKey(st2Rec.Id))
                    {
                        var st1data = stage1Records[st2Rec.Id];
                        foreach (var st1 in st1data)
                        {
                            ver1L.Add(new VersionRecordStage1
                            {
                                RealityObject = new RealityObject { Id = st1.RealityObjectId },
                                Stage2Version = st2Version,
                                Year = st1.Year,
                                StructuralElement = new RealityObjectStructuralElement { Id = st1.StructuralElementId },
                                Sum = st1.Sum,
                                SumService = st1.ServiceCost,
                                LastOverhaulYear = st1.LastOverhaulYear,
                                Volume = st1.Volume,
                                Wearout = st1.Wearout
                            });
                        }
                    }
                }
            }

            var versionUpdates = new List<ProgramVersion>();
            if (isMain)
            {
                versionUpdates.AddRange(versionService.GetAll()
                    .Where(x => x.IsMain).ToList()); 
            }

            // простые объекты + 3 этап
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // Обновляем версии которые были IsMain == true но стали IsMain = false
                    versionUpdates.ForEach(x =>
                    {
                        x.IsMain = false;
                        session.Update(x);
                    });

                    // Сохраняем новую версию
                    session.Insert(version);

                    // сохраняем параметры версии
                    verParams.ForEach(x => session.Insert(x));

                    // сохраняем 3й этап
                    ver3L.ForEach(x => session.Insert(x));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            // 2 этап
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    ver2L.ForEach(x => session.Insert(x));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            // 1 этап
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    ver1L.ForEach(x => session.Insert(x));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            Container.Resolve<ISessionProvider>().CloseCurrentSession();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return new BaseDataResult(true);
        }

        /// <summary>
        /// Метод, для выполнения действий в транзации 
        /// </summary>
        /// <param name="action">Действие</param>
        protected virtual void InTransaction(Action action)
        {
            using (var transaction = BeginTransaction())
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
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
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
            return Container.Resolve<IDataTransaction>();
        }

        /// <summary>
        /// Получить вычислитель свойства программы
        /// </summary>
        /// <param name="paramCode">Код вычислителя</param>
        /// <param name="stage3">Анонимный объект для Property injection</param>
        /// <returns>Вычислитель</returns>
        private IProgrammPriorityParam ResolveEvaluator(string paramCode, object stage3)
        {
            return Container.Resolve<IProgrammPriorityParam>(paramCode,  new Arguments
            {
                {"stage3", stage3}
            });
        }

        public IDataResult DeleteDpkr(BaseParams baseParams)
        {
            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg_2").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_ro_struct_el_in_prg_3").ExecuteUpdate();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(true, "Программа ДПКР успешно удалена");
        }

        public IDataResult ValidationDeleteDpkr(BaseParams baseParams)
        {
            if (this.Container.Resolve<IRepository<RealityObjectStructuralElementInProgramm>>().GetAll().Any())
            {
                return new BaseDataResult { Success = true };
            }

            return new BaseDataResult { Success = false };
        }

        private Dictionary<long, List<int>> GetLastOvrhlYears()
        {
            var service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(x => x.Stage2.Stage3 != null)
                .Select(x => new
                {
                    RoSeId = x.StructuralElement.Id,
                    Stage3Id = x.Stage2.Stage3.Id,
                    x.Year,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.RealityObject.BuildYear
                })
                .AsEnumerable();

            var yearsSe =
                stage3Years
                    .GroupBy(x => x.RoSeId)
                    .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Year).Select(x => x.Year).ToList());

            var result = new Dictionary<long, List<int>>();

            //получаем года кап.ремонта конструктивных элементов
            foreach (var stage3 in stage3Years.OrderBy(x => x.Year))
            {
                if (!result.ContainsKey(stage3.Stage3Id))
                {
                    result[stage3.Stage3Id] = new List<int>();
                }

                var years = yearsSe.ContainsKey(stage3.RoSeId) ? yearsSe[stage3.RoSeId] : new List<int>();

                if (years.Any())
                {
                    result[stage3.Stage3Id].AddRange(years);
                }

                int year = stage3.LastOverhaulYear > 0
                        ? stage3.LastOverhaulYear
                        : stage3.BuildYear.ToInt();

                result[stage3.Stage3Id].Add(year);
            }

            return result;
        }

        private Dictionary<long, List<int>> GetSeYearsWithLifetimes()
        {
            var service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(x => x.Stage2.Stage3 != null)
                .Select(x => new
                {
                    RoSeId = x.StructuralElement.Id,
                    Stage3Id = x.Stage2.Stage3.Id,
                    x.Year,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.RealityObject.BuildYear,
                    x.StructuralElement.StructuralElement.LifeTime,
                    x.StructuralElement.StructuralElement.LifeTimeAfterRepair
                })
                .OrderBy(x => x.Year)
                .AsEnumerable();

            var result = new Dictionary<long, List<int>>();

            var addedRoSe = new HashSet<long>();

            //получаем года кап.ремонта конструктивных элементов
            foreach (var stage3 in stage3Years)
            {
                if (!result.ContainsKey(stage3.Stage3Id))
                {
                    result.Add(stage3.Stage3Id, new List<int>());
                }

                if (!addedRoSe.Contains(stage3.RoSeId))
                {
                    addedRoSe.Add(stage3.RoSeId);

                    int year;

                    if (stage3.LastOverhaulYear > 0 && stage3.LastOverhaulYear > stage3.BuildYear)
                    {
                        year = stage3.LastOverhaulYear + stage3.LifeTimeAfterRepair;
                    }
                    else
                    {
                        year = stage3.BuildYear.ToInt() + stage3.LifeTime;
                    }

                    result[stage3.Stage3Id].Add(year);
                }

                result[stage3.Stage3Id].Add(stage3.Year);
            }

            return result;
        }

        private Dictionary<long, List<int>> GetFirstPrivYears(int startYear)
        {
            var service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(x => x.Stage2.Stage3 != null)
                .Select(x => new
                {
                    RoSeId = x.StructuralElement.Id,
                    Stage3Id = x.Stage2.Stage3.Id,
                    x.Year,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.RealityObject.BuildYear,
                    x.StructuralElement.StructuralElement.LifeTime
                })
                .AsEnumerable();

            var result = new Dictionary<long, List<int>>();

            var addedRoSe = new HashSet<long>();

            //получаем года кап.ремонта конструктивных элементов
            foreach (var stage3 in stage3Years.OrderBy(x => x.Year))
            {
                if (!result.ContainsKey(stage3.Stage3Id))
                {
                    result.Add(stage3.Stage3Id, new List<int>());
                }

                //если это первое добавление этого конструктивного элемента
                //то добавим теоритический год кап.ремонта + срок службы, если он не попадает в период программы
                if (!addedRoSe.Contains(stage3.RoSeId))
                {
                    addedRoSe.Add(stage3.RoSeId);

                    int year = stage3.LastOverhaulYear > 0
                        ? stage3.LastOverhaulYear
                        : stage3.BuildYear.ToInt();

                    if (year > 0)
                    {
                        year += stage3.LifeTime;

                        if (startYear > year)
                        {
                            result[stage3.Stage3Id].Add(year);
                        }
                    }
                    else
                    {
                        result[stage3.Stage3Id].Add(startYear);
                    }
                }

                result[stage3.Stage3Id].Add(stage3.Year);
            }

            return result;
        }

        private Dictionary<long, IEnumerable<int>> GetWeights()
        {
            return Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>().GetAll()
                .Where(x => x.CommonEstateObject != null)
                .Select(x => new
                {
                    x.Stage3.Id,
                    x.CommonEstateObject.Weight
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Weight));
        }

        private Dictionary<long, decimal> GetWearout()
        {
            return Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>().GetAll()
                .Select(
                        x => new
                        {
                            x.StructuralElement.Wearout,
                            Stage3Id = x.Stage2.Stage3.Id
                        })
                    .ToList()
                    .GroupBy(x => x.Stage3Id)
                    .Select(x => new { x.Key, Wearout = x.Sum(y => y.Wearout / 70) })
                    .ToDictionary(x => x.Key, x => x.Wearout);
        }

        // Количество КЭ в доме
        private Dictionary<long, int> GetCountsWorksByRo()
        {
            return Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => x.State.StartState)
                    .Select(x => x.RealityObject.Id)
                    .GroupBy(x => x)
                    .Select(x => new { RoId = x.Key, ElCount = x.Count() })
                    .ToList()
                    .ToDictionary(x => x.RoId, x => x.ElCount);
        }

        // Кол-во работ по дому в конкретном году
        private Dictionary<Tuple<int, long>, int> GetCountWorksInYear()
        {
            return Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
                .Select(x => new Tuple<int, long>(x.Year, x.RealityObject.Id))
                    .GroupBy(x => x)
                    .Select(
                        x => new
                        {
                            x.Key,
                            Count = x.Count()
                        })
                    .ToList()
                    .ToDictionary(x => x.Key, x => x.Count);
        }

        // StructElementUsageParams - для этого параметра нужен словарь 
        private Dictionary<long, decimal> GetStrElUsagesDict()
        {

            var result = new Dictionary<long, decimal>();

            var stage1Items =  Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>().GetAll()
                 .Select(
                        x =>
                            new Stage1Node()
                            {
                                Stage3Id = x.Stage2.Stage3.Id,
                                PlanYear = x.Stage2.Stage3.Year,
                                RoId = x.Stage2.RealityObject.Id,
                                RoStructElId = x.StructuralElement.Id,
                                Lifetime = x.StructuralElement.StructuralElement.LifeTime,
                                LifetimeAfterRepair = x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                OverhaulYear = x.StructuralElement.LastOverhaulYear,
                                BuildYear = x.Stage2.RealityObject.BuildYear
                            })
                            .ToList()
                            .GroupBy(x => x.RoStructElId)
                            .ToDictionary(x => x.Key, x => x.OrderBy(y => y.PlanYear));

            // Плановый год - год последнего ремонта
            // Для первого элемента из первого этапа данные могут браться из дома
            // Для остальных из предыдущей записи
            // Сортировка по году
            foreach (var stage1Item in stage1Items)
            {
                Stage1Node prev = null;
                foreach (var item in stage1Item.Value)
                {
                    int lastFixYear = 0;
                    int lifetime = 0;

                    if (prev == null)
                    {
                        lastFixYear = item.OverhaulYear != 0 ? item.OverhaulYear : item.BuildYear.GetValueOrDefault();
                        lifetime = item.Lifetime;
                    }
                    else
                    {
                        lastFixYear = prev.PlanYear;
                        lifetime = prev.LifetimeAfterRepair;
                    }

                    prev = item;

                    var formula = lifetime > 0 ? (item.PlanYear - lastFixYear) / (decimal)lifetime : 0;
                    result[item.Stage3Id] = (formula + result.Get(item.Stage3Id));
                }
            }

            return result;

        }
    }

    public class WorkTypeProxy
    {
        public string StructElement;

        public string WorkType;

        public string WorkKind;

        public decimal Volume;

        public decimal Sum;
    }

    internal static class AccessorCache
    {
        private static readonly Hashtable accessors = new Hashtable();

        private static readonly Hashtable callSites = new Hashtable();

        /// <summary>
        /// Создание "точки вызова" метода получения свойства динамического объекта
        /// </summary>
        /// <param name="name">Наименование свойства</param>
        private static CallSite<Func<CallSite, object, object>> GetCallSiteLocked(string name)
        {
            var callSite = (CallSite<Func<CallSite, object, object>>)callSites[name];
            if (callSite == null)
            {
                callSites[name] = callSite = CallSite<Func<CallSite, object, object>>.Create(
                            Binder.GetMember(CSharpBinderFlags.None, name, typeof(AccessorCache),
                            new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
            }
            return callSite;
        }

        /// <summary>
        /// Получение делегата обращения к свойству динамического объекта
        /// </summary>
        /// <param name="name">Наименование свойства</param>
        internal static Func<dynamic, object> GetAccessor(string name)
        {
            Func<dynamic, object> accessor = (Func<dynamic, object>)accessors[name];
            if (accessor == null)
            {
                lock (accessors)
                {
                    accessor = (Func<dynamic, object>)accessors[name];
                    if (accessor == null)
                    {
                        if (name.IndexOf('.') >= 0)
                        {
                            string[] props = name.Split('.');
                            CallSite<Func<CallSite, object, object>>[] arr = Array.ConvertAll(props, GetCallSiteLocked);
                            accessor = target =>
                            {
                                object val = (object)target;
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    var cs = arr[i];
                                    val = cs.Target(cs, val);
                                }
                                return val;
                            };
                        }
                        else
                        {
                            var callSite = GetCallSiteLocked(name);
                            accessor = target =>
                            {
                                return callSite.Target(callSite, (object)target);
                            };
                        }
                        accessors[name] = accessor;
                    }
                }
            }
            return accessor;
        }
    }
}