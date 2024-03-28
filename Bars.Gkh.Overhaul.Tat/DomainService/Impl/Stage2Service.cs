namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using System;
    using B4.DataAccess;
    using B4.Utils;

    using Gkh.Entities.CommonEstateObject;
    using PriorityParams.Impl;

    using Castle.Windsor;
    using Entities;
    using Enums;
    using PriorityParams;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;

    using Expression = NCalc.Expression;

    public class Stage2Service : IStage2Service
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObject> RoDomainService { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStrElDomainService { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1DomainService { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2DomainService { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Stage3DomainService { get; set; }

        public IDomainService<VersionRecord> VersionRecDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(
            int startYear, int endYear, long municipalityId,
            IQueryable<RealityObjectStructuralElement> roStructElQuery = null,
            IDictionary<long, int> structElementPlanYearsDict = null)
        {
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var servicePercent = config.ServiceCost;

            // Проверяем параметры на валидность
            if (startYear == 0 || endYear == 0 || startYear >= endYear)
            {
                throw new Exception("Не задан параметр \"Период долгосрочной программы\"");
            }

            var valuesService = Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
            var formulaService = Container.Resolve<IFormulaService>();
            var structElementWorkService = Container.Resolve<IDomainService<StructuralElementWork>>();
            var workPriceService = Container.Resolve<IDomainService<WorkPrice>>();
            var roStructElService = Container.Resolve<IRealObjStructElementService>();

            // нужно получить для того чтобы получит ьраценки за начальный год программы
            var periodStart = config.ProgrammPeriodStart;

            var listStage1 = new List<RealityObjectStructuralElementInProgramm>();

            // Подготавливаем словарик в котором будут хранится по ID структурного элемента его параметры
            var dictValues = valuesService.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Object.Id)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Attribute.Id).ToDictionary(x => x.Key, x => x.First()));

            var dictStructElWork = structElementWorkService.GetAll()
                .Select(x => new
                {
                    StrElId = x.StructuralElement.Id,
                    JobId = x.Job.Id,
                    JobName = x.Job.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.StrElId)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new { x.JobId, x.JobName })
                        .GroupBy(x => x.JobId)
                        .ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First()));

            var dictPrices = workPriceService.GetAll()
                .Where(x => x.Municipality != null && x.Municipality.Id == municipalityId)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Year).ToDictionary(x => x.Key, z => z.ToList()));

            roStructElQuery = roStructElQuery ?? roStructElService.GetUsedInLongProgram(); // получаем те записи КЭ которые должны участвовать в ДПКР

            // Идем по каждой записи КЭ дома
            var data = roStructElQuery
                    .Where(x => x.RealityObject.Municipality.Id == municipalityId)
                    .Select(x => new
                    {
                        x.Id,
                        x.StructuralElement.Group.Formula,
                        x.StructuralElement.Group.FormulaParams,
                        StructuralElementId = x.StructuralElement.Id,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        x.Volume,
                        x.RealityObject.AreaLiving,
                        Element = x,
                        x.LastOverhaulYear,
                        x.RealityObject,
                        x.Wearout,
                        x.RealityObject.BuildYear,
                        PrivatizDate = x.RealityObject.PrivatizationDateFirstApartment,
                        x.RealityObject.PhysicalWear,
                        x.StructuralElement.LifeTime,
                        x.StructuralElement,
                        StructuralElementLifeTime = x.StructuralElement.LifeTime,
                        StructuralElementLifeTimeAfterRepair = x.StructuralElement.LifeTimeAfterRepair
                    })
                    .AsEnumerable();

            var allPricesExist = true;

            var dict = new Dictionary<long, Dictionary<long, string>>();
            var dictFormulaParams = new Dictionary<string, List<string>>();
            var resolvers = new List<object>();

            try
            {
                foreach (var objectElement in data)
                {
                    var jobs = dictStructElWork.ContainsKey(objectElement.StructuralElementId)
                        ? dictStructElWork[objectElement.StructuralElementId]
                        : new Dictionary<long, string>();

                    var sum = 0M;
                    var serviceCost = 0M;

                    if (jobs.Any())
                    {
                        // - берем расценки только на начальный год программы
                        var jobPrices = dictPrices.TryGetValue(municipalityId, out var workPriceDict)
                            && workPriceDict.TryGetValue(periodStart, out var workPriceList)
                                ? workPriceList
                                    .Where(x => jobs.ContainsKey(x.Job.Id))
                                    .ToList()
                                : new List<WorkPrice>();

                        if (jobPrices.Any())
                        {
                            switch (objectElement.StructuralElement.CalculateBy)
                            {
                                case PriceCalculateBy.Volume:
                                    sum = jobPrices.Sum(x => objectElement.Volume * x.NormativeCost);
                                    break;
                                case PriceCalculateBy.AreaLivingNotLivingMkd:
                                case PriceCalculateBy.TotalArea:
                                case PriceCalculateBy.LivingArea:
                                    sum = objectElement.AreaLiving.HasValue
                                        ? jobPrices.Sum(x => objectElement.AreaLiving.Value * x.SquareMeterCost)
                                            .ToDecimal()
                                        : 0m;
                                    break;
                            }
                        }
                        else
                        {
                            //Отсутствуют необходимые расценки
                            allPricesExist = false;
                            //state.Stop();

                            if (!dict.ContainsKey(municipalityId))
                            {
                                dict.Add(municipalityId, new Dictionary<long, string>());
                            }

                            foreach (var job in jobs)
                            {
                                if (!dict[municipalityId].ContainsKey(job.Key))
                                {
                                    dict[municipalityId].Add(job.Key, job.Value);
                                }
                            }
                        }

                        serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);
                    }
                    
                    var planYear = 0;

                    // Вычисление планового года по формуле в случае, если
                    // его не удалось определить по словарю с плановыми годами для КЭ домов
                    if (structElementPlanYearsDict == null || !structElementPlanYearsDict.TryGetValue(objectElement.Id, out planYear))
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
                            // Если по данной формуле еще небыло получение параметров то получаем параметры
                            // Параметры получаются только при первом получении
                            var checkFormula =
                                formulaService.GetParamsList(formula);

                            if (!checkFormula.Success)
                            {
                                continue;
                            }

                            // Получаем наименования параметров для нормального вычисления формулы
                            var listParams = ((ListDataResult)checkFormula).Data as List<string> ?? new List<string>();

                            dictFormulaParams.Add(formula, listParams);
                        }

                        var baseParamNames = dictFormulaParams[formula];

                        var formulaParams = objectElement.FormulaParams;
                        var atrValue = new Dictionary<string, object>();

                        foreach (var formulaParam in formulaParams)
                        {
                            var param = formulaParam;
                            if (param.Attribute != null && dictValues.ContainsKey(objectElement.Id)
                                && dictValues[objectElement.Id].ContainsKey(param.Attribute.Id))
                            {
                                var value = dictValues[objectElement.Id][param.Attribute.Id];

                                if (value != null)
                                {
                                    switch (value.Attribute.AttributeType)
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
                                var injections =
                                    new
                                    {
                                        objectElement.Wearout,
                                        objectElement.PhysicalWear,
                                        objectElement.PrivatizDate,
                                        objectElement.LastOverhaulYear,
                                        objectElement.BuildYear,
                                        objectElement.LifeTime,
                                        objectElement.Volume
                                    };

                                var resolver = Container.Resolve<IFormulaParameter>(formulaParam.ValueResolverCode,
                                    new Arguments
                                    {
                                        {"injections", injections}
                                    });

                                atrValue.Add(param.Name, resolver.GetValue());
                                resolvers.Add(resolver);
                            }
                        }

                        if (baseParamNames.Any(x => !atrValue.ContainsKey(x)))
                        {
                            continue;
                        }

                        // Считаем год по формуле
                        var expr = new Expression(formula) { Parameters = atrValue };
                        planYear = (int)expr.Evaluate().ToDecimal();
                        planYear = planYear != 0 ? planYear : endYear - 1;

                        if (planYear <= endYear)
                        {
                            // Если год по формуле не попадает в программу, то делаем его начальным годом программы
                            planYear = Math.Max(startYear, planYear);

                            var evaluatedElement = new RealityObjectStructuralElementInProgramm
                            {
                                StructuralElement = objectElement.Element,
                                Year = planYear,
                                Sum = sum,
                                ServiceCost = serviceCost
                            };

                            // Добавляем в список на сохранен
                            listStage1.Add(evaluatedElement);
                        }
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
                            var elem = new RealityObjectStructuralElementInProgramm
                            {
                                StructuralElement = objectElement.Element,
                                Year = year,
                                Sum = sum,
                                ServiceCost = serviceCost
                            };

                            // Добавляем в список на сохранен
                            listStage1.Add(elem);

                            year += addYear;
                        }
                    }
                }

                if (!allPricesExist)
                {
                    var text = "Отсутсвуют расценки по работам:" + Environment.NewLine;
                    var muService = Container.Resolve<IDomainService<Municipality>>();

                    var dictMu = muService.GetAll().ToDictionary(x => x.Id, y => y.Name);

                    foreach (var muId in dict.Keys)
                    {
                        var muName = dictMu[muId];
                        text += "МО: " + muName + Environment.NewLine;

                        text +=
                            dict[muId].Select(x => x.Value)
                                      .Aggregate(
                                          (result, current) =>
                                          (string.IsNullOrWhiteSpace(result) ? current : ", " + current));

                        text += Environment.NewLine;
                    }

                    throw new Exception(text);
                }
            }
            finally
            {
                this.Container.Release(valuesService);
                this.Container.Release(formulaService);
                this.Container.Release(structElementWorkService);
                this.Container.Release(workPriceService);
                this.Container.Release(roStructElService);
                resolvers.ForEach(obj => this.Container.Release(obj));
            }

            return listStage1;
        }

        public void GetStage2And3(  IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records,
                                    out IList<RealityObjectStructuralElementInProgrammStage2> stage2Records,
                                    out IList<RealityObjectStructuralElementInProgrammStage3> stage3Records)
        {
            var config = Container.GetGkhConfig<OverhaulTatConfig>();

            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;
            var stage2Period = config.GroupByCeoPeriod;

            // получаем минимальную дату из уже полученных записей 1 этапа
            minYear = stage1Records.Any() ? stage1Records.Min(x => x.Year) : minYear;

            var startYear = minYear;
            var endYear = minYear + stage2Period;

            stage2Records = new List<RealityObjectStructuralElementInProgrammStage2>();
            stage3Records = new List<RealityObjectStructuralElementInProgrammStage3>();

            var dictCeo =
                Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                    .ToDictionary(x => x.Id);

            var roIds = stage1Records.Select(x => x.StructuralElement.RealityObject.Id).Distinct().ToList();

            var dictLifetimes =
                Container.Resolve<IDomainService<StructuralElement>>().GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.LifeTime,
                        x.LifeTimeAfterRepair
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

            var dictRoWearout = RoStrElDomainService.GetAll().Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.Wearout,
                    Key = x.RealityObject.Id + "_" + x.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                    Wearout = x.Sum(y => y.Wearout)
                })
                .ToDictionary(x => x.Key, y => y.Count > 0 ? y.Wearout / y.Count : 0);

            var dictRoInfo = RoDomainService.GetAll()
                                .Where(x => roIds.Contains(x.Id))
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Address,
                                    x.EnergyPassport,
                                    x.ConfirmWorkDocs,
                                    x.ProjectDocs,
                                    x.DateCommissioning,
                                    x.BuildYear,
                                    x.PrivatizationDateFirstApartment
                                })
                                .AsEnumerable()
                                .Distinct(x => x.Id)
                                .ToDictionary(x => x.Id);

            var priorityParam = Container.ResolveAll<IPriorityParams>().ToDictionary(x => x.Id);

            var quantParams = Container.Resolve<IDomainService<QuantPriorityParam>>().GetAll().ToList();
            var qualityParams = Container.Resolve<IDomainService<QualityPriorityParam>>().GetAll().ToList();

            var dictYears = new Dictionary<string, int>();

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
                            StrElId = x.StructuralElement.StructuralElement.Id,
                            RoId = x.StructuralElement.RealityObject.Id,
                            CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                            x.StructuralElement.LastOverhaulYear,
                            x.Sum,
                            x.ServiceCost,
                            Stage1 = x
                        })
                        .GroupBy(x => new { x.RoId, x.CeoId })
                        .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var kvp in elements)
                {
                    var roInfo = dictRoInfo[kvp.Key.RoId];

                    var ro = new RealityObject
                    {
                        Id = kvp.Key.RoId,
                        Address = roInfo.Address,
                        EnergyPassport = roInfo.EnergyPassport,
                        ProjectDocs = roInfo.ProjectDocs,
                        ConfirmWorkDocs = roInfo.ConfirmWorkDocs,
                        DateCommissioning = roInfo.DateCommissioning,
                        BuildYear = roInfo.BuildYear,
                        PrivatizationDateFirstApartment = roInfo.PrivatizationDateFirstApartment
                    };

                    var ceo = dictCeo.ContainsKey(kvp.Key.CeoId) ? dictCeo[kvp.Key.CeoId] : null;
                    
                    var stage2 =
                        new RealityObjectStructuralElementInProgrammStage2
                        {
                            StructuralElements = string.Empty,
                            CommonEstateObject = ceo,
                            RealityObject = ro,
                            Sum = 0m
                        };

                    var listYears = new List<int>();

                    var maxStrElYear = 0;
                    var maxStrElLastOverhaulYear = 0;
                    var maxStrElExploitYear = 0;
                    var maxStrElExploitYearAfterRepair = 0;

                    foreach (var val in kvp.Value)
                    {
                        var stage1 = val.Stage1;
                        stage1.Stage2 = stage2;

                        listYears.Add(val.Year);
                        stage2.Sum += val.ServiceCost + val.Sum;

                        maxStrElYear = Math.Max(val.Year, maxStrElYear);
                        maxStrElLastOverhaulYear = Math.Max(val.LastOverhaulYear, maxStrElLastOverhaulYear);
                        maxStrElExploitYear = Math.Max(dictLifetimes[val.StrElId].LifeTime, maxStrElExploitYear);
                        maxStrElExploitYearAfterRepair = Math.Max(dictLifetimes[val.StrElId].LifeTimeAfterRepair, maxStrElExploitYearAfterRepair);
                    }

                    stage2.Year = Math.Round(listYears.Average(), MidpointRounding.ToEven).ToInt();

                    stage2Records.Add(stage2);

                    var stage3 = new RealityObjectStructuralElementInProgrammStage3
                    {
                        RealityObject = ro,
                        CommonEstateObjects = ceo.Name,
                        Year = stage2.Year,
                        Sum = stage2.Sum
                    };

                    var point = 0;

                    var key = string.Format("{0}_{1}", kvp.Key.RoId, kvp.Key.CeoId);

                    foreach (var quant in quantParams)
                    {
                        var param = priorityParam[quant.Code];

                        if (quant.Code == "Wearout")
                        {
                            (param as WearoutPriorityParam).Wearout = dictRoWearout.ContainsKey(key)
                                ? dictRoWearout[key]
                                : 0;
                        }

                        var value = param.GetValue(stage3).ToDecimal();

                        decimal? minValue = null;
                        decimal? maxValue = null;

                        if (!string.IsNullOrEmpty(quant.MinValue))
                        {
                            minValue = quant.MinValue.ToDecimal();
                        }

                        if (!string.IsNullOrEmpty(quant.MaxValue))
                        {
                            maxValue = quant.MaxValue.ToDecimal();
                        }

                        point += (!minValue.HasValue || value >= minValue.Value) && (!maxValue.HasValue || value <= maxValue.Value)
                            ? quant.Point
                            : 0;
                    }

                    foreach (var quality in qualityParams)
                    {
                        var param = priorityParam[quality.Code];

                        if (quality.Value == (int)param.GetValue(stage3))
                        {
                            point += quality.Point;
                        }
                    }

                    stage3.Point = point;

                    if (!dictYears.ContainsKey(key))
                    {
                        dictYears.Add(key, maxStrElLastOverhaulYear);
                    }
                    else
                    {
                        dictYears[key] = maxStrElYear;
                    }

                    stage3.NeedOverhaul = GetNeedOverhaul(stage3,
                        dictYears[key] != maxStrElLastOverhaulYear ? maxStrElYear : maxStrElLastOverhaulYear,
                        dictYears[key] != maxStrElLastOverhaulYear ? maxStrElExploitYearAfterRepair : maxStrElExploitYear);

                    stage3Records.Add(stage3);

                    stage2.Stage3 = stage3;
                }

                startYear = endYear;
                endYear += stage2Period;
            }
        }

#warning не забыть перенести расчет на GetStage2And3
        public IDataResult MakeStage2(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            if (muId < 1)
            {
                return new BaseDataResult(false, "Не выбрано муниципальное образование");
            }
                    
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var stage2Period = config.GroupByCeoPeriod;
            var maxYear = config.ProgrammPeriodEnd;

            var ceoService = Container.Resolve<IDomainService<CommonEstateObject>>();

            // формируем словарь записей 1 этапа
            var dictStage1 =
                Stage1DomainService.GetAll()
                    .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId)
                    .ToDictionary(x => x.Id);
            // получаем минимальную дату из уже полученных записей 1 этапа
            var minYear = dictStage1.Values.Min(x => x.Year);

            // получаем ООИ в словарь
            var dictCeo = ceoService.GetAll().ToDictionary(x => x.Id);

            var list1 = new List<RealityObjectStructuralElementInProgramm>();
            var list2 = new List<RealityObjectStructuralElementInProgrammStage2>();
            var list3 = new List<RealityObjectStructuralElementInProgrammStage3>();

            // Высчитываем параметры для разбивки по периодам
            var startYear = minYear;
            var endYear = minYear + stage2Period;

            var priorityParam = Container.ResolveAll<IPriorityParams>().ToDictionary(x => x.Id);

            var quantParams = Container.Resolve<IDomainService<QuantPriorityParam>>().GetAll().ToList();
            var qualityParams = Container.Resolve<IDomainService<QualityPriorityParam>>().GetAll().ToList();

            var dictLifetimes =
                Container.Resolve<IDomainService<StructuralElement>>().GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.LifeTime,
                        x.LifeTimeAfterRepair
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

            var dictRoWearout = Stage1DomainService.GetAll()
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId)
                .Select(x => new
                {
                    x.StructuralElement.Wearout,
                    Key = x.StructuralElement.RealityObject.Id + "_" + x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                    Wearout = x.Sum(y => y.Wearout)
                })
                .ToDictionary(x => x.Key, y => y.Count > 0 ? y.Wearout / y.Count : 0);

            var dictRoInfo = Stage1DomainService.GetAll()
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId)
                .Select(x => new
                {
                    x.StructuralElement.RealityObject.Id,
                    x.StructuralElement.RealityObject.EnergyPassport,
                    x.StructuralElement.RealityObject.ConfirmWorkDocs,
                    x.StructuralElement.RealityObject.ProjectDocs,
                    x.StructuralElement.RealityObject.DateCommissioning,
                    x.StructuralElement.RealityObject.BuildYear,
                    x.StructuralElement.RealityObject.PrivatizationDateFirstApartment
                })
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToDictionary(x => x.Id);

            var dictYears = new Dictionary<string, int>();

            while (startYear <= maxYear)
            {
                var elements =
                    Stage1DomainService.GetAll()
                        .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId)
                        .Where(x => x.Year >= startYear && x.Year < endYear)
                        .Select(x => new
                        {
                            x.Id,
                            x.Year,
                            StrElId = x.StructuralElement.StructuralElement.Id,
                            RoId = x.StructuralElement.RealityObject.Id,
                            CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                            x.StructuralElement.LastOverhaulYear,
                            x.Sum,
                            x.ServiceCost
                        })
                        .AsEnumerable()
                        .GroupBy(x => new { x.RoId, x.CeoId } );

                foreach (var item in elements)
                {
                    var roInfo = dictRoInfo[item.Key.RoId];

                    var ro = new RealityObject
                    {
                        Id = item.Key.RoId,
                        EnergyPassport = roInfo.EnergyPassport,
                        ProjectDocs = roInfo.ProjectDocs,
                        ConfirmWorkDocs = roInfo.ConfirmWorkDocs,
                        DateCommissioning = roInfo.DateCommissioning,
                        BuildYear = roInfo.BuildYear,
                        PrivatizationDateFirstApartment = roInfo.PrivatizationDateFirstApartment
                    };

                    var ceo = dictCeo[item.Key.CeoId];

                    var stage2 =
                        new RealityObjectStructuralElementInProgrammStage2
                        {
                            StructuralElements = string.Empty,
                            CommonEstateObject = ceo,
                            RealityObject = ro,
                            Sum = 0m
                        };

                    var listYears = new List<int>();

                    var maxStrElYear = 0;
                    var maxStrElLastOverhaulYear = 0;
                    var maxStrElExploitYear = 0;
                    var maxStrElExploitYearAfterRepair = 0;

                    foreach (var val in item)
                    {
                        var stage1 = dictStage1[val.Id];
                        stage1.Stage2 = stage2;

                        list1.Add(stage1);

                        listYears.Add(val.Year);
                        stage2.Sum += val.ServiceCost + val.Sum;

                        maxStrElYear = Math.Max(val.Year, maxStrElYear);
                        maxStrElLastOverhaulYear = Math.Max(val.LastOverhaulYear, maxStrElLastOverhaulYear);
                        maxStrElExploitYear = Math.Max(dictLifetimes[val.StrElId].LifeTime, maxStrElExploitYear);
                        maxStrElExploitYearAfterRepair = Math.Max(dictLifetimes[val.StrElId].LifeTimeAfterRepair, maxStrElExploitYearAfterRepair);
                    }

                    stage2.Year = Math.Round(listYears.Average(), MidpointRounding.ToEven).ToInt();

                    list2.Add(stage2);

                    var stage3 = new RealityObjectStructuralElementInProgrammStage3
                    {
                        RealityObject = ro,
                        CommonEstateObjects = ceo.Name,
                        Year = stage2.Year,
                        Sum = stage2.Sum
                    };

                    var point = 0;

                    var key = string.Format("{0}_{1}", item.Key.RoId, item.Key.CeoId);

                    foreach (var quant in quantParams)
                    {
                        var param = priorityParam[quant.Code];

                        if (quant.Code == "Wearout")
                        {
                            (param as WearoutPriorityParam).Wearout = dictRoWearout.ContainsKey(key)
                                ? dictRoWearout[key]
                                : 0;
                        }

                        var value = param.GetValue(stage3).ToDecimal();

                        decimal? minValue = null;
                        decimal? maxValue = null;

                        if (!string.IsNullOrEmpty(quant.MinValue))
                        {
                            minValue = quant.MinValue.ToDecimal();
                        }

                        if (!string.IsNullOrEmpty(quant.MaxValue))
                        {
                            maxValue = quant.MaxValue.ToDecimal();
                        }

                        point += (!minValue.HasValue || value >= minValue.Value) && (!maxValue.HasValue || value <= maxValue.Value)
                            ? quant.Point
                            : 0;
                    }

                    foreach (var quality in qualityParams)
                    {
                        var param = priorityParam[quality.Code];

                        if (quality.Value == (int)param.GetValue(stage3))
                        {
                            point += quality.Point;
                        }
                    }

                    stage3.Point = point;

                    if (!dictYears.ContainsKey(key))
                    {
                        dictYears.Add(key, maxStrElLastOverhaulYear);
                    }
                    else
                    {
                        dictYears[key] = maxStrElYear;
                    }

                    stage3.NeedOverhaul = GetNeedOverhaul(stage3,
                        dictYears[key] != maxStrElLastOverhaulYear ? maxStrElYear : maxStrElLastOverhaulYear,
                        dictYears[key] != maxStrElLastOverhaulYear ? maxStrElExploitYearAfterRepair : maxStrElExploitYear);

                    list3.Add(stage3);

                    stage2.Stage3 = stage3;
                }

                startYear = endYear;
                endYear = endYear + stage2Period;
            }

            var index = 1;

            foreach (var item in list3.OrderBy(x => x.Year).ThenByDescending(x => x.NeedOverhaul).ThenByDescending(x => x.Point))
            {
                item.IndexNumber = index++;
            }

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.CreateSQLQuery(@"update OVRHL_RO_STRUCT_EL_IN_PRG set stage2_id=null 
                                                where id in (SELECT s1.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG s1
                                                    JOIN OVRHL_RO_STRUCT_EL ro_se ON s1.RO_SE_ID = ro_se.ID
                                                    JOIN GKH_REALITY_OBJECT ro ON ro_se.RO_ID = ro.ID
                                                WHERE ro.municipality_id = :municipalityId)")
                            .SetParameter("municipalityId", muId)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(@"DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 
                                                WHERE ID IN (SELECT s2.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 s2
                                                LEFT JOIN GKH_REALITY_OBJECT ro ON s2.RO_ID = ro.ID
                                                WHERE ro.municipality_id = :municipalityId)")
                            .SetParameter("municipalityId", muId)
                            .ExecuteUpdate();

                        session.CreateSQLQuery(@"DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 
                                                WHERE ID IN (SELECT s3.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 s3
                                                LEFT JOIN GKH_REALITY_OBJECT ro ON s3.RO_ID = ro.ID
                                                WHERE ro.municipality_id = :municipalityId)")
                            .SetParameter("municipalityId", muId)
                            .ExecuteUpdate();

                        list3.ForEach(x => session.Insert(x));

                        list2.ForEach(x => session.Insert(x));

                        list1.ForEach(x => session.Insert(x));

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
        }

        private int GetNeedOverhaul(RealityObjectStructuralElementInProgrammStage3 stage3, int year, int exploitationYear)
        {
            if (!stage3.RealityObject.PrivatizationDateFirstApartment.HasValue)
            {
                return 0;
            }

            var privDate = stage3.RealityObject.PrivatizationDateFirstApartment.Value.Year;

            return year + exploitationYear > privDate ? 0 : 1;

            /*var id = stage3.Id;
            int val;

            if (years.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).ContainsKey(id))
            {
                var idsArr = years.Keys.ToArray();
                var index = Array.IndexOf(idsArr, id);

                if (index > 0)
                {
                    var valArr = years.Values.ToArray();
                    val = valArr[index - 1];
                }
                else
                {
                    yearWithLifetimes.TryGetValue(id, out val);
                }
            }
            else
            {
                yearWithLifetimes.TryGetValue(id, out val);
            }*/

        }

        /// <summary>
        /// Копировать суммы из сохраненной версии
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Объект с результатом выполнения операции</returns>
        public IDataResult CopyFromVersion(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("Version");

            if (versionId == 0)
            {
                return new BaseDataResult(false, "Параметр версии задан некорректно");
            }

            if (!VersionRecDomain.GetAll().Any(x => x.ProgramVersion.Id == versionId))
            {
                return new BaseDataResult(false, "В выбранной версии нет данных");
            }

            var version = Container.Resolve<IDomainService<ProgramVersion>>().Get(versionId);

            //для уменьшения затрат на десериализацию и проход по объектам
            //берем только те записи, которые изменились
            var versionRecords = VersionRecDomain.GetAll()
                .Where(x => x.ProgramVersion.Id == versionId)
                .Where(y => !Stage3DomainService.GetAll()
                    .Any(x => x.RealityObject.Municipality.Id == version.Municipality.Id
                              && x.CommonEstateObjects == y.CommonEstateObjects
                              && x.RealityObject.Id == y.RealityObject.Id
                              && x.Year == y.Year
                              && x.Sum == y.Sum))
                .Select(x => new
                {
                    x.Sum,
                    Key = x.Year + "#" + x.CommonEstateObjects + "#" + x.RealityObject.Id
                })
                .AsEnumerable();

            var versionStage2Records = VersionStage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                .Where(y => !Stage2DomainService.GetAll()
                    .Any(x => x.RealityObject.Municipality.Id == version.Municipality.Id
                              && x.CommonEstateObject.Id == y.CommonEstateObject.Id
                              && x.Year == y.Stage3Version.Year
                              && x.Sum == y.Sum
                              && x.RealityObject.Id == y.Stage3Version.RealityObject.Id))
                .Select(x => new
                {
                    x.Sum,
                    Key = x.Stage3Version.Year + "#" + x.CommonEstateObject.Id + "#" + x.Stage3Version.RealityObject.Id
                })
                .AsEnumerable();

            var versionStage1Records = VersionStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                .Where(y => !Stage1DomainService.GetAll()
                    .Any(x => x.StructuralElement.Id == y.StructuralElement.Id
                              && x.Year == y.Year
                              && x.Sum == y.Sum))
                .Select(x => new
                {
                    x.Sum,
                    x.SumService,
                    Key = x.Year + "#" + x.StructuralElement.Id
                })
                .AsEnumerable();

            var stage3Records = Stage3DomainService.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == version.Municipality.Id)
                .Where(y => !VersionRecDomain.GetAll()
                    .Any(x => x.ProgramVersion.Id == versionId 
                              && x.CommonEstateObjects == y.CommonEstateObjects
                              && x.RealityObject.Id == y.RealityObject.Id
                              && x.Year == y.Year
                              && x.Sum == y.Sum))
                .Select(x => x)
                .AsEnumerable()
                .ToDictionary(x => string.Format("{0}#{1}#{2}", x.Year, x.CommonEstateObjects, x.RealityObject.Id));

            var stage2Records = Stage2DomainService.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == version.Municipality.Id)
                .Where(y => !VersionStage2Domain.GetAll()
                    .Any(x => x.Stage3Version.ProgramVersion.Id == versionId
                              && x.Stage3Version.RealityObject.Id == y.RealityObject.Id
                              && x.CommonEstateObject.Id == y.CommonEstateObject.Id
                              && x.Stage3Version.Year == y.Year
                              && x.Sum == y.Sum))
                .Select(x => x)
                .AsEnumerable()
                .ToDictionary(x => string.Format("{0}#{1}#{2}", x.Year, x.CommonEstateObject.Id, x.RealityObject.Id));

            var stage1Records = Stage1DomainService.GetAll()
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == version.Municipality.Id)
                .Where(y => !VersionStage1Domain.GetAll()
                    .Any(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                              && x.StructuralElement.Id == y.StructuralElement.Id
                              && x.Year == y.Year
                              && x.Sum == y.Sum))
                .Select(x => x)
                .AsEnumerable()
                .ToDictionary(x => string.Format("{0}#{1}", x.Year, x.StructuralElement.Id));

            var stage1ToSave = new List<RealityObjectStructuralElementInProgramm>();
            var stage2ToSave = new List<RealityObjectStructuralElementInProgrammStage2>();
            var stage3ToSave = new List<RealityObjectStructuralElementInProgrammStage3>();

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            foreach (var versionRecord in versionRecords)
            {
                if (stage3Records.ContainsKey(versionRecord.Key))
                {
                    var stage3Record = stage3Records[versionRecord.Key];
                    stage3Record.Sum = versionRecord.Sum;
                    stage3ToSave.Add(stage3Record);
                }
            }

            foreach (var versionStage2Record in versionStage2Records)
            {
                if (stage2Records.ContainsKey(versionStage2Record.Key))
                {
                    var stage2Record = stage2Records[versionStage2Record.Key];
                    stage2Record.Sum = versionStage2Record.Sum;
                    stage2ToSave.Add(stage2Record);
                }
            }

            foreach (var versionStage1Record in versionStage1Records)
            {
                if (stage1Records.ContainsKey(versionStage1Record.Key))
                {
                    var stage1Record = stage1Records[versionStage1Record.Key];
                    stage1Record.Sum = versionStage1Record.Sum;
                    stage1Record.ServiceCost = versionStage1Record.SumService;
                    stage1ToSave.Add(stage1Record);
                }
            }

            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    stage1ToSave.ForEach(x =>
                    {
                        if (x.Id > 0)
                            session.Update(x);
                        else
                            session.Insert(x);
                    }); 
                    stage2ToSave.ForEach(x =>
                    {
                        if (x.Id > 0)
                            session.Update(x);
                        else
                            session.Insert(x);
                    }); 
                    stage3ToSave.ForEach(x =>
                    {
                        if (x.Id > 0)
                            session.Update(x);
                        else
                            session.Insert(x);
                    });

                    tx.Commit();
                }
                catch (ValidationException e)
                {
                    tx.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}
