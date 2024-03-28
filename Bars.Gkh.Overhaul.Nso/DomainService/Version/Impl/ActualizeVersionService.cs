
using Bars.Gkh.Config;
using Bars.Gkh.Domain;
using Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams;

namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Bars.Gkh.Utils;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.MicroKernel;
    using Castle.Windsor;
    using Enums;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities.CommonEstateObject;
    using NHibernate.Linq;
    using Overhaul.DomainService;

    /// <summary>
    /// Сервис для актуализации версии
    /// </summary>
    public class ActualizeVersionService : IActualizeVersionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<VersionActualizeLog> LogDomain { get; set; }

        public IActualizeVersionLogService LogService { get; set; }

        public IUserIdentity User { get; set; }

        public IGkhParams GkhParams { get; set; }

        public IDataResult ActualizeOrder(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var method = config.MethodOfCalculation;

            var versionParamsDomain = Container.ResolveDomain<VersionParam>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();
            var versionRecordSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var versionRecordSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var priorityService = Container.Resolve<IPriorityService>();
            var correctDomain = Container.ResolveDomain<DpkrCorrectionStage2>();
            var actualizeLogDomain = Container.ResolveDomain<VersionActualizeLog>();

            try
            {
                var excludeIds = correctDomain.GetAll()
                    .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == versionId && y.PlanYear < actualizeStart)
                    .Select(y => y.Stage2.Stage3Version.Id)
                    .ToList();

                var ver3recsQuery = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Where(x => !excludeIds.Contains(x.Id)); // исключаем те записи котоыре находятся по корреткировке меньше начала актуализации для того чтобы нетрогать эту нумерацию
                
                var maxIndex = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Where(x => excludeIds.Contains(x.Id))
                    .SafeMax(x => x.IndexNumber);

                var ver3recs =
                    ver3recsQuery.Select(
                        x =>

                        new
                        {
                            x.Id,
                            verSt3 = x,
                            x.RealityObject.Address,
                            x.Sum,
                            x.CommonEstateObjects,
                            x.Year,
                            roId = x.RealityObject.Id,
                            x.Point,
                            x.IndexNumber,
                            x.RealityObject.EnergyPassport,
                            x.RealityObject.ConfirmWorkDocs,
                            x.RealityObject.ProjectDocs,
                            x.RealityObject.DateCommissioning,
                            x.RealityObject.BuildYear,
                            x.RealityObject.PrivatizationDateFirstApartment
                        })
                        .OrderBy(x => x.IndexNumber)
                        .ToList();

                var volumeDict = versionRecordSt1Domain.GetAll()
                                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                    .Select(
                                        x => new { st3Id = x.Stage2Version.Stage3Version.Id, x.Volume })
                                    .AsEnumerable()
                                    .GroupBy(x => x.st3Id)
                                    .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                var priorityParamsQuery = versionParamsDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId);

                var priorityParams = priorityParamsQuery
                    .ToDictionary(x => x.Code, y => y.Weight);

                var version = programVersionDomain.Get(versionId);
                
                var pointsParams = new Dictionary<long, List<StoredPointParam>>();

                var versionSt2Query = versionRecordSt2Domain.GetAll()
                       .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                        .Select(x => new Stage2Proxy
                        {
                            Stage3Id = x.Stage3Version.Id,
                            CeoId = x.CommonEstateObject.Id,
                            RoId = x.Stage3Version.RealityObject.Id
                        })
                        .AsEnumerable();

                var versionSt1Query = versionRecordSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new Stage1Proxy
                    {
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
                        Wearout = x.StructuralElement.Wearout
                    })
                    .AsEnumerable();

                var sbLogDescription = new StringBuilder("Расчет очередности");
                switch (method)
                {
                    case TypePriority.Criteria:
                        {
                            sbLogDescription.Append(" по критериям");
                            if (priorityParams.Count == 0)
                            {
                                throw new ValidationException("Не указаны параметры");
                            }
                            break;
                        }

                    case TypePriority.Points:
                        {
                            sbLogDescription.Append(" по баллам");
                            pointsParams = priorityService.GetPoints(ver3recsQuery, versionSt2Query, versionSt1Query, versionId);
                            priorityParams["PointsParam"] = 1;
                            break;
                        }
                    case TypePriority.CriteriaAndPoins:
                        {
                            sbLogDescription.Append(" по критериям и баллам");
                            pointsParams = priorityService.GetPoints(ver3recsQuery, versionSt2Query, versionSt1Query, versionId);
                            break;
                        }
                }

                if (method != TypePriority.Points)
                {
                    this.Container.UsingForResolvedAll<IProgrammPriorityParam>((container, programPriorityParams) =>
                    {
                        var priorityParamsDict = programPriorityParams.ToDictionary(x => x.Code, x => x.Name);
                        var priorityParamNames = priorityParamsQuery
                            .Where(x => x.Code != "PointsParam")
                            .AsEnumerable()
                            .Select(x => priorityParamsDict.Get(x.Code))
                            .ToArray();
                        if (priorityParamNames.Length > 0)
                        {
                            sbLogDescription.Append(": ");
                            sbLogDescription.Append(string.Join(", ", priorityParamNames));
                        }
                    });
                }
                var logDescription = sbLogDescription.ToString();

                var points = pointsParams.ToDictionary(x => x.Key, y => y.Value.Sum(x => x.Value));

                // Такой словарик нужен для того чтобы быстро получать элементы при сохранении уже из готовых данных по Id
                var dictStage3 = ver3recsQuery.ToDictionary(x => x.Id);

                var muStage3 = ver3recsQuery
                        .Select(x => new Stage3Order
                        {
                            Id = x.Id,
                            Year = x.Year,
                            Stage3 = x,
                            RoId = x.RealityObject.Id,
                            PrivatizDate = x.RealityObject.PrivatizationDateFirstApartment,
                            BuildYear = x.RealityObject.BuildYear,
                            DateTechInspection = x.RealityObject.DateTechInspection,
                            PhysicalWear = x.RealityObject.PhysicalWear,
                            DateCommissioning = x.RealityObject.DateCommissioning
                        })
                        .ToList();

                var dictDensity = ver3recsQuery
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
                var firstPrivYears = GetFirstPrivYears(version, startYear);

                var lastOvrhlYears = GetLastOvrhlYears(version);

                var weights = GetWeights(version);

                var yearsWithLifetimes = GetSeYearsWithLifetimes(version);
                
                foreach (var item in muStage3)
                {
                    
                    var injections = new
                    {
                        item.PrivatizDate,
                        DictPoints = points,
                        item.Year,
                        item.BuildYear,
                        Density = dictDensity.ContainsKey(item.Id) ? dictDensity[item.Id] : 0m,
                        Weights = weights.ContainsKey(item.Id) ? weights[item.Id] : new[] { 0 },
                        FirstPrivYears = firstPrivYears.ContainsKey(item.Id) ? firstPrivYears[item.Id] : new List<int>(),
                        OverhaulYears = lastOvrhlYears.ContainsKey(item.Id) ? lastOvrhlYears[item.Id] : new List<int>(),
                        OverhaulYearsWithLifetimes = yearsWithLifetimes.ContainsKey(item.Id) ? yearsWithLifetimes[item.Id] : new List<int>()
                    };

                    priorityService.CalculateOrder(item, priorityParams.Keys, injections);
                }

                /*
                * Сначала сортируем по плановому году 
                */
                var proxyList2 = muStage3.OrderBy(x => x.Year);
                foreach (var item in priorityParams.OrderBy(x => x.Value))
                {
                    var key = item.Key;

                    var eval = Container.Resolve<IProgrammPriorityParam>(key, new Arguments
                    {
                        {"object",  new object()},
                    });
                    if (eval != null)
                    {
                        proxyList2 = eval.Asc
                            ? proxyList2.ThenBy(x => x.OrderDict[key])
                            : proxyList2.ThenByDescending(x => x.OrderDict[key]);
                    }
                }

                var index = maxIndex + 1; // начинаем с последнего номера среди тех записей котоыре были до периода актазации
                foreach (var item in proxyList2.ThenBy(x => x.RoId).ThenBy(x => x.Stage3.Id))
                {
                    if (!dictStage3.ContainsKey(item.Id)) continue;

                    var st3 = dictStage3[item.Id];

                    priorityService.FillStage3Criteria(st3, item.OrderDict);

                    st3.StoredPointParams = pointsParams.ContainsKey(st3.Id) ? pointsParams[st3.Id] : new List<StoredPointParam>();

                    st3.IndexNumber = index++;
                }

                // формируем логи актуализации
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeOrder;
                log.DateAction = DateTime.Now;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                // Формируемстроки для логов
                foreach (var rec in ver3recs)
                {
                    if (!dictStage3.ContainsKey(rec.Id))
                    {
                        continue;
                    }

                    var newRec = dictStage3[rec.Id];

                    if (rec.IndexNumber == newRec.IndexNumber && rec.Point == newRec.Point)
                        continue;

                    if (rec.IndexNumber != newRec.IndexNumber)
                    {
                        var logRecord = new ActualizeVersionLogRecord
                        {
                            TypeAction = VersionActualizeType.ActualizeOrder,
                            Action = "Изменение",
                            Description = logDescription,
                            Address = rec.Address,
                            Ceo = rec.CommonEstateObjects,
                            PlanYear = rec.Year,
                            Volume = volumeDict.ContainsKey(rec.Id) ? volumeDict[rec.Id] : 0m,
                            Sum = rec.Sum,
                            Number = rec.IndexNumber, // старый номер
                            ChangeNumber = newRec.IndexNumber // новый номер
                        };

                        logRecords.Add(logRecord);
                    }

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

                TransactionHelper.InsertInManyTransactions(Container, dictStage3.Values, 10000, true, true);

                actualizeLogDomain.Save(log);

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(versionParamsDomain);
                Container.Release(versionRecordDomain);
                Container.Release(versionRecordSt2Domain);
                Container.Release(versionRecordSt1Domain);
                Container.Release(programVersionDomain);
                Container.Release(priorityService);
                Container.Release(correctDomain);
                Container.Release(actualizeLogDomain);
            }
        }

        private Dictionary<long, List<int>> GetFirstPrivYears(ProgramVersion version, int startYear)
        {
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();

            try
            {
                var stage3Years = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                    .Select(x => new
                    {
                        RoSeId = x.StructuralElement.Id,
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
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
            finally
            {
                Container.Release(versSt1Domain);
            }
        }

        private Dictionary<long, List<int>> GetLastOvrhlYears(ProgramVersion version)
        {
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();

            try
            {
                var stage3Years = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                    .Select(x => new
                    {
                        RoSeId = x.StructuralElement.Id,
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
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
                        result.Add(stage3.Stage3Id, new List<int>());
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
            finally
            {
                Container.Release(versSt1Domain);
            }
        }

        private Dictionary<long, int> GetCountWorks(ProgramVersion version)
        {
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var structuralElWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();

            try
            {
                var result = new Dictionary<long, int>();

                var stage1Recs = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                    .Select(x => new
                    {
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
                        x.StructuralElement.StructuralElement.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Stage3Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id));

                var strElWorks = structuralElWorkDomain.GetAll()
                    .Select(x => x.StructuralElement.Id)
                    .AsEnumerable()
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, y => y.Count());

                foreach (var stage1 in stage1Recs)
                {
                    result.Add(stage1.Key, 0);

                    foreach (var seId in stage1.Value)
                    {
                        result[stage1.Key] += strElWorks.ContainsKey(seId) ? strElWorks[seId] : 0;
                    }
                }

                return result;
            }
            finally
            {
                Container.Release(versSt1Domain);
                Container.Release(structuralElWorkDomain);
            }
        }

        private Dictionary<long, IEnumerable<int>> GetWeights(ProgramVersion version)
        {
            var versSt2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            try
            {
                return versSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                    .Where(x => x.CommonEstateObject != null)
                    .Select(x => new
                    {
                        x.Stage3Version.Id,
                        x.CommonEstateObject.Weight
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Weight));            }
            finally
            {
                Container.Release(versSt2Domain);
            }
        }

        private Dictionary<long, List<int>> GetSeYearsWithLifetimes(ProgramVersion version)
        {
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();

            try
            {
                var stage3Years = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                    .Select(x => new
                    {
                        RoSeId = x.StructuralElement.Id,
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
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
            finally
            {
                Container.Release(versSt1Domain);
            }
        }

        private IDataResult GetNewRecords(BaseParams baseParams, out List<VersionRecord> ver3S, out List<VersionRecordStage2> ver2S, out List<VersionRecordStage1> ver1S, Dictionary<long, RoStrElAfterRepair> afterRepaired = null, List<long> RoStrElList = null, List<string> existRoStrInYear = null)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            ver3S = new List<VersionRecord>();
            ver2S = new List<VersionRecordStage2>();
            ver1S = new List<VersionRecordStage1>();

            var verParams = new List<VersionParam>();

            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = Container.Resolve<ILongProgramService>();
            var correctDomain = Container.ResolveDomain<DpkrCorrectionStage2>();
            var versSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = Container.ResolveDomain<VersionRecord>();
            var roStructElService = Container.Resolve<IRealityObjectStructElementService>();
            var paramsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            using (
                Container.Using(
                    programVersionDomain,
                    roStructElDomain,
                    longProgramService,
                    versSt1Domain,
                    versSt2Domain,
                    versSt3Domain,
                    correctDomain,
                    roStructElService,
                    paramsService))
            {
                var version = programVersionDomain.Get(versionId);

                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var periodEnd = config.ProgrammPeriodEnd;

                //получаем те конструктивные элементы, которые считаем потенциально существующиеми и которые нельзя дублирвоат ьесли они есть уже
                //сравниваем с годом актуализации для того чтобы недобавить такой КЭ
                var st1ExistStrElIds = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .WhereIf(afterRepaired != null && afterRepaired.Any(),
                        x =>
                            (correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart))
                            || !correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id))
                    .Select(x => x.StructuralElement.Id)
                    .ToList();  

                var newStructQuery =
                    roStructElService.GetElementsForLongProgram()
                                     .WhereIf(RoStrElList != null, x => RoStrElList.Contains(x.Id) || !st1ExistStrElIds.Contains(x.Id))
                                     .WhereIf(RoStrElList == null, x => !st1ExistStrElIds.Contains(x.Id));
                                     
                if (!newStructQuery.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var stage1ToSave = longProgramService.GetStage1(actualizeStart, newStructQuery);
                
                if (afterRepaired != null && afterRepaired.Any())
                {
                    var stage1ByRoStrEl = stage1ToSave.GroupBy(x => x.StructuralElement.Id)
                                                      .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Year).ToList());

                    // необходимо понять  какие записи уже существуют в нетронутых годах и относительно сроков эксплуатации сдвигать последующие записи
                    foreach (var kvp in afterRepaired)
                    {
                        if (!stage1ByRoStrEl.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        // отталкиваемся от того что данный кэ уже учтен в какихто годах а значит берем этот год как год ремонта и прибавляем срок эксплуатации
                        int nextYear = kvp.Value.LastYearRepair + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);
                        
                        // всем записям которые неполностью удалились из дпкрприсваиваем год относителньо года последней записи находящейся в версии
                        foreach (var roStrEl in stage1ByRoStrEl[kvp.Key])
                        {
                            roStrEl.Year = nextYear;

                            nextYear = roStrEl.Year + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);
                        }
                    }

                    stage1ToSave = stage1ToSave.Where(x => x.Year <= periodEnd).ToList();
                }

                if (existRoStrInYear != null && existRoStrInYear.Any())
                {
                    // поскольку мы заново расчитали относительно года актуализации новый срез данных, значит
                    // исключаем те комбинации которые уже существуют в системе, тоесть чтобы небыло дублей
                    stage1ToSave =
                        stage1ToSave.Where(
                            x => !existRoStrInYear.Contains("{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year)))
                                    .ToList();
                }
                
                if (!stage1ToSave.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var roIds = stage1ToSave.Select(x => x.StructuralElement.RealityObject.Id).Distinct().ToList();

                List<RealityObjectStructuralElementInProgrammStage2> stage2ToSave;
                List<RealityObjectStructuralElementInProgrammStage3> stage3ToSave;

                var stage2VerRecs =
                    versSt2Domain.GetAll()
                                 .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                                 .Where(x => roIds.Contains(x.Stage3Version.RealityObject.Id))
                                 .Select(
                                     x =>
                                     new
                                         {
                                             Record = x,
                                             RoId = x.Stage3Version.RealityObject.Id,
                                             CeoId = x.CommonEstateObject.Id,
                                             x.Stage3Version.Year
                                         })
                                 .AsEnumerable()
                                 .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.RoId, x.CeoId, x.Year))
                                 .ToDictionary(x => x.Key, y => y.Select(x => x.Record).FirstOrDefault());
                
                longProgramService.GetStage2And3(stage1ToSave, out stage2ToSave, out stage3ToSave);

                var stage2Dict = stage2ToSave.Where(x => x.Stage3 != null).GroupBy(x => x.Stage3).ToDictionary(x => x.Key, y => y.ToArray());

                var stage1Dict = stage1ToSave.Where(x => x.Stage2 != null).GroupBy(x => x.Stage2).ToDictionary(x => x.Key, y => y.ToArray());

                var stage3Params = paramsService.GetAll().Select(x => new { x.Code, x.Order }).ToList();

                foreach (var stage3 in stage3Params)
                {
                    verParams.Add(
                        new VersionParam { ProgramVersion = version, Code = stage3.Code, Weight = stage3.Order });
                }

                foreach (var stage3 in stage3ToSave)
                {
                    var ver3 = new VersionRecord
                                  {
                                      ProgramVersion = version,
                                      RealityObject = stage3.RealityObject,
                                      Year = stage3.Year,
                                      CommonEstateObjects = stage3.CommonEstateObjects,
                                      IndexNumber = 0,
                                      Point = stage3.Point,
                                      StoredCriteria = stage3.StoredCriteria,
                                      StoredPointParams = stage3.StoredPointParams,
                                      ObjectCreateDate = DateTime.Now,
                                      Changes = string.Format("Добавлено: {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm"))
                                  };

                    ver3.Sum += stage3.Sum;
                    ver3.ObjectEditDate = DateTime.Now;

                    if (stage2Dict.ContainsKey(stage3))
                    {
                        var st2RecList = stage2Dict[stage3];

                        foreach (var stage2 in st2RecList)
                        {
                            var st2Version =
                                stage2VerRecs.Get(
                                    "{0}_{1}_{2}".FormatUsing(
                                        stage2.RealityObject.Id, stage2.CommonEstateObject.Id, stage2.Year));

                            if (st2Version == null)
                            {
                                if (ver3.Id > 0)
                                {
                                    ver3.CommonEstateObjects = string.Format(
                                        "{0}, {1}", ver3.CommonEstateObjects, stage2.CommonEstateObject.Name);
                                }

                                st2Version = new VersionRecordStage2
                                                 {
                                                     CommonEstateObject =
                                                         new CommonEstateObject { Id = stage2.CommonEstateObject.Id},
                                                     Stage3Version = ver3,
                                                     CommonEstateObjectWeight =
                                                         stage2.CommonEstateObject.Weight,
                                                     ObjectCreateDate = DateTime.Now
                                                 };
                            }
                            else
                            {
                                ver3 = st2Version.Stage3Version;
                                ver3.Sum += stage3.Sum;
                                ver3.ObjectEditDate = DateTime.Now;
                            }

                            st2Version.Sum += stage2.Sum;
                            st2Version.ObjectEditDate = DateTime.Now;
                            ver2S.Add(st2Version);

                            if (stage1Dict.ContainsKey(stage2))
                            {
                                var st1RecList = stage1Dict[stage2];

                                foreach (var stage1 in st1RecList)
                                {
                                    ver1S.Add(
                                        new VersionRecordStage1
                                            {
                                                RealityObject = new RealityObject { Id = stage1.StructuralElement.RealityObject.Id },
                                                Stage2Version = st2Version,
                                                Year = stage1.Year,
                                                StructuralElement =
                                                    new RealityObjectStructuralElement
                                                        {
                                                            Id = stage1.StructuralElement.Id
                                                        },
                                                Sum = stage1.Sum,
                                                SumService = stage1.ServiceCost,
                                                Volume = stage1.Volume,
                                                LastOverhaulYear = stage1.LastOverhaulYear,
                                                Wearout = stage1.Wearout,
                                                ObjectCreateDate = DateTime.Now,
                                                ObjectEditDate = DateTime.Now
                                            });
                                }
                            }
                        }
                    }

                    ver3S.Add(ver3);
                }

            }

            return new BaseDataResult();
        }   
        
        public IDataResult ActualizeNewRecords(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");

            var sessionProvider = Container.Resolve<ISessionProvider>();
            var unProxy = Container.Resolve<IUnProxy>();

            try
            {

                List<VersionRecord> ver3S = null;
                List<VersionRecordStage2> ver2S = null;
                List<VersionRecordStage1> ver1S = null;

                var dataResult = GetNewRecords(baseParams, out ver3S, out ver2S, out ver1S);

                if (!ver3S.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

// формируем логи актуализации

                var volumeDict =
                    ver1S.Select(x => new { x.Stage2Version.Stage3Version, x.Volume })
                         .GroupBy(x => x.Stage3Version)
                         .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume));

                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeNewRecords;
                log.DateAction = DateTime.Now;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                // Формируем строки для логов
                foreach (var rec in ver3S)
                {
                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeNewRecords,
                        Action = "Добавление",
                        Description = "Удовлетворяет условиям ДПКР",
                        Address = rec.RealityObject.Address,
                        Ceo = rec.CommonEstateObjects,
                        PlanYear = rec.Year,
                        Number = rec.IndexNumber,
                        Volume = volumeDict.ContainsKey(rec) ? volumeDict[rec] : 0m,
                        Sum = rec.Sum
                    };

                    logRecords.Add(logRecord);

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            ver3S.ForEach(x =>
                            {
                                if (x.Id > 0) session.Update(unProxy.GetUnProxyObject(x));
                                else session.Insert(x);
                            });

                            ver2S.ForEach(x =>
                            {
                                if (x.Stage3Version != null)
                                {
                                    if (x.Id > 0) session.Update(x);
                                    else session.Insert(x);
                                }
                            });

                            ver1S.ForEach(x =>
                            {
                                if (x.Stage2Version != null)
                                {
                                    session.Insert(x);
                                }
                            });

                            session.Insert(log);

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return dataResult;
            }
            finally 
            {
                Container.Release(unProxy);
            }

            return new BaseDataResult();
        }

        public IDataResult ActualizeSum(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);
            
            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = Container.ResolveDomain<VersionRecord>();
            var workPriceDomain = Container.ResolveDomain<NsoWorkPrice>();
            var realEstService = Container.Resolve<IRealEstateTypeService>();
            var structElementWorkDomain = Container.ResolveDomain<StructuralElementWork>();
            var correctDomain = Container.ResolveDomain<DpkrCorrectionStage2>();
            var roDomain = Container.Resolve<IDomainService<RealityObject>>();

            using (Container.Using(programVersionDomain, roDomain, correctDomain, realEstService, versSt1Domain, versSt2Domain, versSt3Domain, workPriceDomain, structElementWorkDomain))
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var periodStart = config.ProgrammPeriodStart;
                var servicePercent = config.ServiceCost;
                var isWorkPriceFirstYear = config.WorkPriceCalcYear == WorkPriceCalcYear.First;
                var workPriceDetermineType = config.WorkPriceDetermineType;

                // получаем только те записи которые либо по Году корректировки либо плановому году лежат в периоде актуализации
                var verSt1Query = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x =>
                            (correctDomain.GetAll()
                                .Any(
                                    y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart && y.PlanYear <= actualizeEnd))
                            || (!correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id) && x.Stage2Version.Stage3Version.Year >= actualizeStart && x.Stage2Version.Stage3Version.Year <= actualizeEnd));

                // фиксируем старые значения для того чтобы в логи записать информацию об изменении
                var OldDataSt1 = verSt1Query.Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume, Sum = x.Sum + x.SumService })
                               .AsEnumerable();
                
                var contractionYears = correctDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new
                    {
                        x.Id,
                        st2Id = x.Stage2.Id,
                        x.PlanYear
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.PlanYear).FirstOrDefault());

                // В РТ могли работы забивать прямо в ДПКР а потом они переходили в версию
                // у некоторых записей Связи КЭ и Дома может не быть но объем пересчитать нужно 
                // поэтому если связь с домом по КЭ ест ьто берем объем от туда иначе берм объем сохраненный в версии

                // получаем дома попадающие под вборку
                var roQuery = roDomain.GetAll().Where(x => verSt1Query.Any(y => y.RealityObject.Id == x.Id)).AsQueryable();

                // получаем типы домов
                var dictRealEstTypes = realEstService.GetRealEstateTypes(roQuery);

                var st2Dict = versSt2Domain.GetAll().Where(x => verSt1Query.Any(y => y.Stage2Version.Id == x.Id))
                    .ToDictionary(x => x.Id);

                var st3Dict = versSt3Domain.GetAll().Where(x => verSt1Query.Any(y => y.Stage2Version.Stage3Version.Id == x.Id))
                    .ToDictionary(x => x.Id);

                var versSt1Recs = verSt1Query
                    .Select(x => new
                                    {
                                        x.Id,
                                        St2Id = x.Stage2Version.Id,
                                        St3Id = x.Stage2Version.Stage3Version.Id,
                                        MuId = x.RealityObject.Municipality.Id,
                                        MuName = x.RealityObject.Municipality.Name,
                                        // Переносил с РТ в НСО над окорреткирвоанный го дполучать подругому
                                        //x.Stage2Version.Stage3Version.CorrectYear,
                                        x.Stage2Version.Stage3Version.Year,
                                        RoId = x.RealityObject.Id,
                                        x.Sum,
                                        x.SumService,
                                        x.StructuralElement.StructuralElement.CalculateBy,
                                        StElId = x.StructuralElement.StructuralElement.Id,
                                        Volume = x.StructuralElement != null ? x.StructuralElement.Volume : x.Volume,
                                        x.RealityObject.AreaLiving,
                                        x.RealityObject.AreaMkd,
                                        x.RealityObject.AreaLivingNotLivingMkd,
                                        CapitalGroupId = x.RealityObject.CapitalGroup != null ? x.RealityObject.CapitalGroup.Id : 0
                                    })
                    .ToList()
                    .Select(x => new
                                    {
                                        x.Id,
                                        x.St2Id,
                                        x.St3Id,
                                        x.Year,
                                        x.RoId,
                                        x.MuId,
                                        x.MuName,
                                        x.Sum,
                                        x.SumService,
                                        x.CalculateBy,
                                        x.StElId,
                                        x.CapitalGroupId,
                                        x.Volume,
                                        x.AreaLiving,
                                        x.AreaMkd,
                                        x.AreaLivingNotLivingMkd,
                                        CorrectYear = contractionYears.ContainsKey(x.St2Id) ? contractionYears[x.St2Id] : x.Year
                                    })
                    .ToList();

                // словарь для быстрого получения нужных записей 
                var versSt1Dect = verSt1Query.ToDictionary(x => x.Id);

                var st1RecToUpdate = new List<VersionRecordStage1>();
                var st2RecToUpdate = new Dictionary<long, VersionRecordStage2>();
                var st3RecToUpdate = new Dictionary<long, VersionRecord>();

                var dictPrices = workPriceDomain.GetAll()
                    .Where(x => x.Municipality != null)
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Year).ToDictionary(x => x.Key, z => z.AsEnumerable()));
                
                var dictStructElWork = structElementWorkDomain.GetAll()
                            .Select(x => new
                            {
                                SeId = x.StructuralElement.Id,
                                JobId = x.Job.Id,
                                JobName = x.Job.Name
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.SeId)
                            .ToDictionary(x => x.Key, y => y.GroupBy(x => x.JobId).ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First()));

                var dictNotExistPrices = new Dictionary<long, Dictionary<long, string>>();

                var date = DateTime.Now;

                foreach (var st1Rec in versSt1Recs)
                {
                    
                    var jobs = dictStructElWork.ContainsKey(st1Rec.StElId)
                                    ? dictStructElWork[st1Rec.StElId]
                                    : new Dictionary<long, string>();

                   var volume = 0m;
                    
                    switch (st1Rec.CalculateBy)
                    {
                        case PriceCalculateBy.Volume:
                            volume = st1Rec.Volume; break;
                        case PriceCalculateBy.LivingArea:
                            volume = st1Rec.AreaLiving.HasValue ? st1Rec.AreaLiving.Value : 0m; break;
                        case PriceCalculateBy.TotalArea:
                            volume = st1Rec.AreaMkd.HasValue ? st1Rec.AreaMkd.Value : 0m; break;
                        case PriceCalculateBy.AreaLivingNotLivingMkd:
                            volume = st1Rec.AreaLivingNotLivingMkd.HasValue ? st1Rec.AreaLivingNotLivingMkd.Value : 0m; break;
                    }
                    
                    var sum = 0M;
                    var serviceCost = 0M;

                    var workPricesByYear = dictPrices.ContainsKey(st1Rec.MuId)
                                               ? dictPrices[st1Rec.MuId]
                                               : new Dictionary<int, IEnumerable<NsoWorkPrice>>();

                    var year = isWorkPriceFirstYear ? periodStart : st1Rec.CorrectYear;

                    var realEstTypes = dictRealEstTypes.ContainsKey(st1Rec.RoId)
                                                   ? dictRealEstTypes[st1Rec.RoId]
                                                   : null;

                    var workPrices = GetWorkPrices(
                        workPricesByYear,
                        year,
                        jobs.Keys.AsEnumerable(),
                        workPriceDetermineType,
                        st1Rec.CapitalGroupId,
                        realEstTypes);

                    var costSum = GetDpkrCostByYear(workPrices, st1Rec.CalculateBy);

                    // если не найдена расценка, добавляем ее в dictionary
                    if (!costSum.HasValue)
                    {
                        AddNotExistPrice(ref dictNotExistPrices, jobs, st1Rec.MuId);
                    }

                    sum = (costSum.ToDecimal() * volume).RoundDecimal(2);
                    serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                    // Теперь получив все значения над овыяснить относительно уже имеющихся данны
                    var rec1 = versSt1Dect[st1Rec.Id];

                    if (rec1.Sum != sum || rec1.SumService != serviceCost || rec1.Volume != st1Rec.Volume)
                    {
                        rec1.Sum = sum;
                        rec1.SumService = serviceCost;
                        rec1.ObjectEditDate = DateTime.Now;
                        rec1.Volume = st1Rec.Volume;
                        st1RecToUpdate.Add(rec1);

                        if (!st2RecToUpdate.ContainsKey(st1Rec.St2Id) && st2Dict.ContainsKey(st1Rec.St2Id))
                        {
                            var st2 = st2Dict[st1Rec.St2Id];
                            st2.Sum = 0; // обнуляем сумму потому что она будет потмо пересчитана
                            st2.ObjectEditDate = date;
                            st2RecToUpdate.Add(st1Rec.St2Id, st2); 
                        }

                        if (!st3RecToUpdate.ContainsKey(st1Rec.St3Id) && st3Dict.ContainsKey(st1Rec.St3Id))
                        {
                            var st3 = st3Dict[st1Rec.St3Id];
                            st3.Sum = 0; // обнуляем сумму потому что она будет потмо пересчитана
                            st3.Changes = string.Format("Изменено: {0}", date.ToString("dd.MM.yyyy HH:mm"));
                            st3.ObjectEditDate = date;
                            st3RecToUpdate.Add(st1Rec.St3Id, st3);
                        }

                    }
                    
                }

                // Сумма по 2му этапа 
                var st2NewSumDict =  versSt1Dect.Values.GroupBy(x => x.Stage2Version.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum + z.SumService));

                foreach (var kvp in st2RecToUpdate)
                {
                    if (st2NewSumDict.ContainsKey(kvp.Key))
                    {
                        kvp.Value.Sum = st2NewSumDict[kvp.Key];
                    }
                }

                // Теперь получаем новые суммы по 3 этапу
                var st3NewSumDict = st2Dict.Values.GroupBy(x => x.Stage3Version.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

                foreach (var kvp in st3RecToUpdate)
                {
                    if (st3NewSumDict.ContainsKey(kvp.Key))
                    {
                        kvp.Value.Sum = st3NewSumDict[kvp.Key];
                    }
                }


                // теперь поскольку пересчитан 1 этап то пробуем пере



                /*
                 var st3Rec = st3RecToUpdate.Get(st1Rec.St3Id) ?? st1Rec.St3;
                    st3Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                    st3Rec.Changes = string.Format("Изменено: {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        
                    st3Rec.ObjectEditDate = DateTime.Now;

                    if (!st3RecToUpdate.ContainsKey(st3Rec.Id))
                    {
                        st3RecToUpdate.Add(st3Rec.Id, st3Rec);
                    }

                    var st2Rec = st2RecToUpdate.Get(st1Rec.St2Id) ?? st1Rec.St2;
                    st2Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                    st2Rec.ObjectEditDate = DateTime.Now;

                    if (!st2RecToUpdate.ContainsKey(st2Rec.Id))
                    {
                        st2RecToUpdate.Add(st2Rec.Id, st2Rec);
                    }
                 */

                if (!st1RecToUpdate.Any() && !st2RecToUpdate.Any() && !st3RecToUpdate.Any())
                {
                    return new BaseDataResult(false, "Нет записей для изменения стоимости");
                }

                // Старое значение объемов
                var volumeDict = OldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // Старое значение суммы
                var sumDict = OldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

                // новое значение объемов
                var newVolumeDict =
                    st1RecToUpdate.Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume})
                                  .GroupBy(x => x.St3Id)
                                  .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // новое значение 
                var roAddress =
                    verSt1Query.Select(x => new { roId = x.RealityObject.Id, x.RealityObject.Address })
                               .AsEnumerable()
                               .GroupBy(x => x.roId)
                               .ToDictionary(x => x.Key, y => y.Select(z => z.Address).FirstOrDefault());
                
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeSum;
                log.DateAction = DateTime.Now;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                // Формируем строки для логов
                var recs = st3RecToUpdate.Values;

                foreach (var rec in recs)
                {
                    var oldVolume = volumeDict.ContainsKey(rec.Id) ? volumeDict[rec.Id] : 0m;
                    var newVolume = newVolumeDict.ContainsKey(rec.Id) ? newVolumeDict[rec.Id] : 0m;
                    var oldSum = sumDict.ContainsKey(rec.Id) ? sumDict[rec.Id] : 0m;

                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeSum,
                        Action = "Изменение",
                        Description = "Актуализация стоимости",
                        Address = roAddress.ContainsKey(rec.RealityObject.Id) ? roAddress[rec.RealityObject.Id] : rec.RealityObject.Address,
                        Ceo = rec.CommonEstateObjects,
                        PlanYear = rec.Year,
                        Volume = oldVolume,
                        Number = rec.IndexNumber,
                        Sum = oldSum,
                        ChangeVolume = oldVolume != newVolume ? newVolume : 0m, // Проставляем только в случае изменения
                        ChangeSum = oldSum != rec.Sum ? rec.Sum : 0m // проставляем только в случае изменения
                    };

                    logRecords.Add(logRecord);

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {

                        st3RecToUpdate.Values.ForEach(versSt3Domain.Update);
                        st2RecToUpdate.Values.ForEach(versSt2Domain.Update);
                        st1RecToUpdate.ForEach(versSt1Domain.Update);

                        LogDomain.Save(log);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            
            return new BaseDataResult();
            
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

        public IDataResult ActualizeYear(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = Container.Resolve<ILongProgramService>();
            var versSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var publishProgRecDomain = Container.ResolveDomain<PublishedProgramRecord>();
            var dpkrCorrectionDomain = Container.ResolveDomain<DpkrCorrectionStage2>();
            var twSt1Domain = Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var versSt3Domain = Container.ResolveDomain<VersionRecord>();
            var shortRecDomain = Container.ResolveDomain<ShortProgramRecord>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var unProxy = Container.Resolve<IUnProxy>();
            
            try
            {
                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
                var periodEnd = config.ProgrammPeriodEnd;

                List<VersionRecord> ver3S = null;
                List<VersionRecordStage2> ver2S = null;
                List<VersionRecordStage1> ver1S = null;

                // подготавливаем записи 2 этапа 
                var stage2Dict = versSt2Domain.GetAll()
                                .Where(
                                     x =>
                                     x.Stage3Version.ProgramVersion.Id == versionId)
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.First());

                // подготавливаем записи 3 этапа 
                var stage3Dict = versSt3Domain.GetAll()
                                 .Where(
                                     x =>
                                     x.ProgramVersion.Id == versionId)
                                 .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.First());

                var correctionYear = dpkrCorrectionDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new {st2Id = x.Stage2.Id, x.PlanYear})
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.PlanYear).FirstOrDefault());
                
                var st1List =
                    versSt1Domain.GetAll()
                                 .Where(x => x.Stage2Version != null 
                                            && x.Stage2Version.Stage3Version != null 
                                            && x.Stage2Version.Stage3Version.ProgramVersion != null
                                            && x.Stage2Version.Stage3Version.RealityObject != null
                                            && x.StructuralElement != null)
                                 .Where(
                                     x =>
                                     x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             RoSeId = x.StructuralElement.Id,
                                             x.StructuralElement.StructuralElement.LifeTime,
                                             x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                             //x.Stage2Version.Stage3Version.FixedYear, Переносил с РТ если надо раскоментируйте
                                             //x.Stage2Version.Stage3Version.CorrectYear,
                                             CorrectYear =  periodEnd,
                                             x.Year,
                                             St1Id = x.Id,
                                             St1Sum = x.Sum,
                                             St1SumService = x.SumService,
                                             St2Id = x.Stage2Version.Id,
                                             St2Sum = x.Stage2Version.Sum,
                                             St3Id = x.Stage2Version.Stage3Version.Id,
                                             St3Sum = x.Stage2Version.Stage3Version.Sum
                                         })
                                 .ToList()
                                 .Select(
                                     x =>
                                     new
                                     {
                                         x.RoSeId,
                                         x.LifeTime,
                                         x.LifeTimeAfterRepair,
                                         CorrectYear =  correctionYear.ContainsKey(x.St2Id) ? correctionYear[x.St2Id] : x.CorrectYear,
                                         x.Year,
                                         x.St1Id,
                                         x.St1Sum,
                                         x.St1SumService,
                                         x.St2Id,
                                         x.St2Sum,
                                         x.St3Id,
                                         x.St3Sum
                                     })
                                 .ToList();

                var stage1VerRecs = st1List
                    .Where(x => x.CorrectYear >= actualizeStart)
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}".FormatUsing(x.RoSeId, x.Year))
                    .ToDictionary(x => x.Key, y => y.Select(z => z.RoSeId).First());

                // поулачем список отремонтированных структурных элементов со своими сроками эксплуатации
                var St1Repaired =
                    st1List.Where(x => x.CorrectYear < actualizeStart /*|| x.FixedYear*/)
                           .AsEnumerable()
                           .GroupBy(x => x.RoSeId)
                           .ToDictionary(
                               x => x.Key,
                               y =>
                               y.Select(
                                   z =>
                                   new RoStrElAfterRepair
                                       {
                                           RoStrElId = z.RoSeId,
                                           LifeTime = z.LifeTime,
                                           LifeTimeAfterRepair = z.LifeTimeAfterRepair,
                                           LastYearRepair = z.CorrectYear
                                       })
                                .OrderByDescending(z => z.LastYearRepair)
                                .FirstOrDefault());


                var allStage1VerRecs = st1List.Where(x => x.CorrectYear >= actualizeStart).ToList();

                // после этого когда мы удалили ненужные записи
                // теперь пытаемся найти записи которые будут новыми типа

                var changedRoStrEls = new List<long>();

                // Переносил с татарстана поскольку там была фиксация годов то здесь это ненужно было 
                //var notDeleteRoStrEls = st1List.Where(x => x.FixedYear).Select(x => "{0}_{1}".FormatUsing(x.RoSeId, x.Year)).ToList();
                var notDeleteRoStrEls = new List<string>();

                if (stage1VerRecs.Any() || changedRoStrEls.Any())
                {

                    var forDeleting = new List<long>();
                    forDeleting.AddRange(changedRoStrEls);

                    // и оставшиеся записи в версии также помечаем как необзходимые на удаление
                    foreach (var roStId in stage1VerRecs.Values.Distinct())
                    {
                        if (!forDeleting.Contains(roStId))
                        {
                            forDeleting.Add(roStId);
                        }
                    }

                    // теперь пытаемся для Якобы удаляемых элементов 
                    var newRes = GetNewRecords(baseParams, out ver3S, out ver2S, out ver1S, St1Repaired, forDeleting);

                    if (ver1S.Any())
                    {
                        var newDictSt1 = ver1S.GroupBy(x => "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year))
                                                .ToDictionary(x => x.Key, y => y.Select(z => z.StructuralElement.Id).First());

                        changedRoStrEls = new List<long>();

                        foreach (var kvp in newDictSt1)
                        {
                            if (stage1VerRecs.ContainsKey(kvp.Key))
                            {
                                if (!notDeleteRoStrEls.Contains(kvp.Key))
                                {
                                    notDeleteRoStrEls.Add(kvp.Key);
                                }

                                stage1VerRecs.Remove(kvp.Key);
                            }
                            else
                            {
                                if (!changedRoStrEls.Contains(kvp.Value))
                                    changedRoStrEls.Add(kvp.Value);
                            }
                        }

                    }

                    // и оставшиеся записи в версии также помечаем как необзходимые на удаление
                    foreach (var roStId in stage1VerRecs.Values.Distinct())
                    {
                        if (!changedRoStrEls.Contains(roStId))
                        {
                            changedRoStrEls.Add(roStId);
                        }
                    }
                }

                var Ids = allStage1VerRecs.Select(x => x.St1Id).Distinct().ToList();

                stage1VerRecs.Select(x => x.Value).ForEach(x =>
                    {
                        if (!changedRoStrEls.Contains(x)) 
                            changedRoStrEls.Add(x); 
                    });

                // значения объемов по 3 этапу
                var oldData = versSt1Domain.GetAll()
                                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             st3Id = x.Stage2Version.Stage3Version.Id,
                                             x.Volume,
                                             address = x.RealityObject.Address,
                                             ceoNames = x.Stage2Version.Stage3Version.CommonEstateObjects,
                                             x.Stage2Version.Stage3Version.Sum,
                                             x.Stage2Version.Stage3Version.Year,
                                             x.Stage2Version.Stage3Version.IndexNumber
                                         })
                                 .AsEnumerable()
                                 .GroupBy(x => x.st3Id)
                                    .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {

                                            
                                            ceoNames = y.Select(z => z.ceoNames).ToList().Any() ? 
                                                          y.Select(z => z.ceoNames)
                                                                .Distinct()
                                                                .ToList()
                                                                .Aggregate(
                                                                    (str, result) => string.IsNullOrEmpty(result) ? str : result + ", " + str) : string.Empty,
                                            volume = y.Sum(z => z.Volume),
                                            sum = y.Select(z => z.Sum).FirstOrDefault(),
                                            number = y.Select(z => z.IndexNumber).FirstOrDefault(),
                                            year = y.Select(z => z.Year).FirstOrDefault(),
                                            address = y.Select(z => z.address).FirstOrDefault()
                                        }
                                   );
                //логируем 
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeYear;
                log.DateAction = DateTime.Now;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                var listChangeSt3 = new List<long>();
                var listDeletedSt3 = new List<long>();

                var dateNow = DateTime.Now;

                if (changedRoStrEls.Any())
                {

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {

                            var existIds = twSt1Domain.GetAll()
                                                        .Where(y => Ids.Contains(y.Stage1Version.Id))
                                                        .Select(y => y.Stage1Version.Id)
                                                        .ToList();

                            allStage1VerRecs.Where(x => !existIds.Contains(x.St1Id))
                                // нужны те записи которых не было в краткосрочке
                                            .Where(x => changedRoStrEls.Contains(x.RoSeId)).ForEach(
                                                val =>
                                                    {

                                                        var key = "{0}_{1}".FormatUsing(val.RoSeId, val.Year);

                                                        if (notDeleteRoStrEls.Contains(key))
                                                        {
                                                            // нельзя удалять такую запись поскольку эта запись при добавлении все равно добавится 
                                                            return;
                                                        }

                                                        versSt1Domain.Delete(val.St1Id);

                                                        var st2 = stage2Dict[val.St2Id];
                                                        st2.Sum = val.St2Sum - val.St1Sum - val.St1SumService;
                                                        versSt2Domain.Update(st2);

                                                        var st3 = stage3Dict[val.St3Id];
                                                        st3.Sum = val.St3Sum - val.St1Sum;
                                                        st3.Changes = string.Format("Изменено: {0}", dateNow.ToString("dd.MM.yyyy HH:mm"));
                                                        versSt3Domain.Update(st3);

                                                        listChangeSt3.Add(st3.Id);
                                                    });

                            var st2ForDelete =
                                versSt2Domain.GetAll()
                                             .Where(
                                                 x =>
                                                 !versSt1Domain.GetAll().Any(y => y.Stage2Version.Id == x.Id)
                                                 && x.Stage3Version.ProgramVersion.Id == versionId);
                            dpkrCorrectionDomain.GetAll()
                                                .Where(
                                                    x =>
                                                    st2ForDelete.Any(y => y.Id == x.Stage2.Id)
                                                    && x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                                .ForEach(x => dpkrCorrectionDomain.Delete(x.Id));
                            publishProgRecDomain.GetAll()
                                                .Where(
                                                    x =>
                                                    st2ForDelete.Any(y => y.Id == x.Stage2.Id)
                                                    && x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                                .ForEach(x => publishProgRecDomain.Delete(x.Id));

                            shortRecDomain.GetAll()
                                                .Where(
                                                    x =>
                                                    st2ForDelete.Any(y => y.Id == x.Stage2.Id)
                                                    && x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                                .ForEach(x => shortRecDomain.Delete(x.Id));

                            st2ForDelete.ForEach(x => versSt2Domain.Delete(x.Id));

                            versSt3Domain.GetAll()
                                         .Where(
                                             x =>
                                             !versSt2Domain.GetAll().Any(y => y.Stage3Version.Id == x.Id)
                                             && x.ProgramVersion.Id == versionId)
                                         .Select(
                                             x =>
                                             new
                                                 {
                                                     x.Id,
                                                     x.RealityObject.Address,
                                                     x.Sum,
                                                     x.Year,
                                                     x.CommonEstateObjects,
                                                     x.IndexNumber
                                                 })
                                         .ForEach(
                                             x =>
                                                 {
                                                     // логируем Удаление Записи
                                                     var oldValues = oldData.ContainsKey(x.Id) ? oldData[x.Id] : null;

                                                     var logRecord = new ActualizeVersionLogRecord
                                                        {
                                                            TypeAction = VersionActualizeType.ActualizeYear,
                                                            Action = "Удаление",
                                                            Description = "Актуализация года",
                                                            Address = x.Address,
                                                            Ceo = oldValues != null ? oldValues.ceoNames : string.Empty,
                                                            PlanYear = oldValues != null ? oldValues.year: 0,
                                                            Volume = oldValues != null ? oldValues.volume: 0,
                                                            Sum = oldValues != null ? oldValues.sum : 0,
                                                            Number = oldValues != null ? oldValues.number: 0
                                                        };

                                                     logRecords.Add(logRecord);

                                                     listDeletedSt3.Add(x.Id);

                                                     versSt3Domain.Delete(x.Id);
                                                 });

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    // получаем итоговый список измененных объектов
                    listChangeSt3 = listChangeSt3.Where(x => !listDeletedSt3.Contains(x)).Distinct().ToList();

                    if (listChangeSt3.Any())
                    {
                        using (var session = sessionProvider.OpenStatelessSession())
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                try
                                {
                                    // Подготавливаем данные записей котоыре изменились чтобы 
                                    var changesData =
                                        versSt1Domain.GetAll()
                                                     .Where(x => listChangeSt3.Contains(x.Stage2Version.Stage3Version.Id))
                                                     .Select(
                                                         x =>
                                                         new
                                                             {
                                                                 st3Id = x.Stage2Version.Stage3Version.Id,
                                                                 ceoNames = x.Stage2Version.CommonEstateObject.Name,
                                                                 x.Volume,
                                                                 x.Stage2Version.Stage3Version.Sum,
                                                                 x.Year,
                                                                 x.Stage2Version.Stage3Version.IndexNumber
                                                             })
                                                     .AsEnumerable()
                                                     .GroupBy(x => x.st3Id)
                                                     .ToDictionary(
                                                         x => x.Key,
                                                         y =>
                                                         new
                                                             {
                                                                 ceoNames = y.Select(z => z.ceoNames).ToList().Any() ? 
                                                                             y.Select(z => z.ceoNames)
                                                                              .Distinct()
                                                                              .ToList()
                                                                              .Aggregate(
                                                                                  (str, result) =>
                                                                                  string.IsNullOrEmpty(result)
                                                                                      ? str
                                                                                      : result + ", " + str) : string.Empty,
                                                                 volume = y.Sum(z => z.Volume),
                                                                 year = y.Select(z => z.Year).FirstOrDefault(),
                                                                 sum = y.Select(z => z.Sum).FirstOrDefault(),
                                                                 number = y.Select(z => z.IndexNumber).FirstOrDefault()
                                                             });

                                    foreach (var kvp in changesData)
                                    {
                                        var st3 = versSt3Domain.Load(kvp.Key);
                                        st3.CommonEstateObjects = kvp.Value.ceoNames;
                                        st3.Changes = string.Format("Изменено: {0}", dateNow.ToString("dd.MM.yyyy HH:mm"));
                                        versSt3Domain.Save(st3);

                                        var oldAddress = string.Empty;
                                        var oldCeoNames = string.Empty;
                                        var oldYear = 0;
                                        var oldVolume = 0m;
                                        var oldSum = 0m;
                                        var oldNumber = 0;

                                        if (oldData.ContainsKey(kvp.Key))
                                        {
                                            var oldValues = oldData[kvp.Key];

                                            oldAddress = oldValues.address;
                                            oldCeoNames = oldValues.ceoNames;
                                            oldYear = oldValues.year;
                                            oldVolume = oldValues.volume;
                                            oldSum = oldValues.sum;
                                            oldNumber = oldValues.number;
                                        }

                                        var newValues = kvp.Value;

                                        // логируем Изменение Записи
                                        var logRecord = new ActualizeVersionLogRecord
                                        {
                                            TypeAction = VersionActualizeType.ActualizeYear,
                                            Action = "Изменение",
                                            Description ="Актуализация года",
                                            Address = oldAddress,
                                            Ceo = oldCeoNames,
                                            PlanYear = oldYear,
                                            Volume = oldVolume,
                                            Sum = oldSum,
                                            Number = oldNumber,
                                            ChangeCeo = newValues.ceoNames != oldCeoNames ? newValues.ceoNames : null,
                                            ChangeNumber = newValues.number != oldNumber ? newValues.number : 0,
                                            ChangePlanYear = newValues.year != oldYear ? newValues.year : 0,
                                            ChangeSum = newValues.sum != oldSum ? newValues.sum : 0m,
                                            ChangeVolume = newValues.volume != oldVolume ? newValues.volume : 0m
                                        };

                                        logRecords.Add(logRecord);

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
                    }

                    var dataResult = GetNewRecords(
                        baseParams, out ver3S, out ver2S, out ver1S, St1Repaired, null, notDeleteRoStrEls);

                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {

                                var newVolumeDict =
                                    ver1S.Select(x => new { x.Stage2Version.Stage3Version, x.Volume })
                                         .AsEnumerable()
                                         .GroupBy(x => x.Stage3Version)
                                         .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                                ver3S.ForEach(
                                    x =>
                                        {
                                            if (x.Id > 0)
                                            {
                                                x.Changes = string.Format("Изменено: {0}", dateNow.ToString("dd.MM.yyyy HH:mm"));
                                                session.Update(unProxy.GetUnProxyObject(x));

                                                var newVolume = 0m;
                                                if (newVolumeDict.ContainsKey(x))
                                                {
                                                    newVolume = newVolumeDict[x];
                                                }

                                                var oldAddress = string.Empty;
                                                var oldCeoNames = string.Empty;
                                                var oldYear = 0;
                                                var oldVolume = 0m;
                                                var oldSum = 0m;
                                                var oldNumber = 0;

                                                if (oldData.ContainsKey(x.Id))
                                                {
                                                    var oldValues = oldData[x.Id];

                                                    oldAddress = oldValues.address;
                                                    oldCeoNames = oldValues.ceoNames;
                                                    oldYear = oldValues.year;
                                                    oldVolume = oldValues.volume;
                                                    oldSum = oldValues.sum;
                                                    oldNumber = oldValues.number;
                                                }

                                                // логируем Изменение Записи
                                                var logRecord = new ActualizeVersionLogRecord
                                                {
                                                    TypeAction = VersionActualizeType.ActualizeYear,
                                                    Action = "Изменение",
                                                    Description = "Актуализация года",
                                                    Address = oldAddress,
                                                    Ceo = oldCeoNames,
                                                    PlanYear = oldYear,
                                                    Volume = oldVolume,
                                                    Sum = oldSum,
                                                    Number = oldNumber,
                                                    ChangeCeo = x.CommonEstateObjects != oldCeoNames ? x.CommonEstateObjects : null,
                                                    ChangeNumber = x.IndexNumber != oldNumber ? x.IndexNumber : 0,
                                                    ChangePlanYear = x.Year != oldYear ? x.Year : 0,
                                                    ChangeSum = x.Sum != oldSum ? x.Sum : 0m,
                                                    ChangeVolume = newVolume != oldVolume ? newVolume : 0m
                                                };

                                                logRecords.Add(logRecord);
                                            }
                                            else
                                            {
                                                session.Insert(x);

                                                var newVolume = 0m;
                                                if (newVolumeDict.ContainsKey(x))
                                                {
                                                    newVolume = newVolumeDict[x];
                                                }

                                                // логируем Добавление Записи
                                                var logRecord = new ActualizeVersionLogRecord
                                                {
                                                    TypeAction = VersionActualizeType.ActualizeYear,
                                                    Action = "Добавление",
                                                    Description = "Актуализация года",
                                                    Address = x.RealityObject.Address,
                                                    Ceo = x.CommonEstateObjects,
                                                    PlanYear = x.Year,
                                                    Volume = newVolume,
                                                    Sum = x.Sum,
                                                    Number = x.IndexNumber
                                                };

                                                logRecords.Add(logRecord);

                                            }
                                        });

                                ver2S.ForEach(
                                    x =>
                                        {
                                            if (x.Id > 0) session.Update(x);
                                            else session.Insert(x);
                                        });

                                ver1S.ForEach(x => session.Insert(x));
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                    if (logRecords.Any())
                    {
                        log.CountActions = logRecords.Count();
                        log.LogFile = LogService.CreateLogFile(
                            logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Логи отсутсвуют");
                    }

                    LogDomain.Save(log);

                    return dataResult;
                }
                else
                {
                    return new BaseDataResult(false, "Нет изменений для Актуализации года");
                }
            }
            finally
            {
                Container.Release(programVersionDomain);
                Container.Release(roStructElDomain);
                Container.Release(longProgramService);
                Container.Release(versSt1Domain);
                Container.Release(versSt2Domain);
                Container.Release(publishProgRecDomain);
                Container.Release(dpkrCorrectionDomain);
                Container.Release(versSt3Domain);
                Container.Release(unProxy);
                Container.Release(twSt1Domain);
                Container.Release(shortRecDomain);
            }
            
        }
        
        private class RoStrElAfterRepair
        {
            public long RoStrElId { get; set; }
            public int LifeTime { get; set; }
            public int LifeTimeAfterRepair { get; set; }
            public int LastYearRepair { get; set; }
        }

        protected virtual LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Bars.B4.DomainService.BaseParams.Converter.ToLoadParam);
        }

        public IDataResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var version = loadParams.Filter.GetAs<long>("versionId");
            var yearStart = loadParams.Filter.GetAs("yearStart", 0);

            var actualizeService = Container.Resolve<IActualizeVersionService>();

            try
            {
                
                // Теперь получаем те записи 3го этапа которые есть в списке полученных из 1го этапа
                var result = actualizeService.GetDeletedEntriesQueryable(version, yearStart)
                    .Select(x => new
                    {
                        x.Stage2Version.Stage3Version.Id,
                        RealityObject = x.Stage2Version.Stage3Version.RealityObject.Address,
                        x.Stage2Version.Stage3Version.CommonEstateObjects,
                        x.Stage2Version.Stage3Version.Year,
                        x.Stage2Version.Stage3Version.IndexNumber,
                        x.Stage2Version.Stage3Version.Sum
                    })
                    .ToList()
                    .Distinct(x => x.Id)
                    .AsQueryable()
                    .Filter(loadParams, Container)
                    .OrderBy(x => x.IndexNumber);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                Container.Release(actualizeService);
            }
            
        }
        public IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(long versionId, int actualizeStartYear)
        {
            
            var roStructElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roStrElService = Container.Resolve<IRealityObjectStructElementService>();
            var versStage1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var twpSt1Domain = Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            var correctDomain = Container.ResolveDomain<DpkrCorrectionStage2>();

            try
            {
                var roStrElQuery = roStrElService.GetElementsForLongProgram();

                // Поулчаем те записи 1го этапа версии для которые уже считаются не актуальными
                return versStage1Domain.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                        .Where(x => (correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStartYear))
                            || !correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id))
                        .Where(x => !twpSt1Domain.GetAll().Any(y => y.Stage1Version.Id == x.Id)) // нужны только записи, которых нет в краткосрочке
                        .Where(x => !roStrElQuery.Any(y => y.Id == x.StructuralElement.Id)); // нужны записи которых нет среди нужных для ДПКР КЭ
            }
            finally
            {
                Container.Release(roStructElDomain);
                Container.Release(roStrElService);
                Container.Release(versStage1Domain);
                Container.Release(twpSt1Domain);
                Container.Release(correctDomain);
            }
        }

        public IDataResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            var roStructElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var stage2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var stage1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var stage3Domain = Container.Resolve<IDomainService<VersionRecord>>();
            var roStrElService = Container.Resolve<IRealityObjectStructElementService>();
            var unProxy = Container.Resolve<IUnProxy>();

            try
            {
                
                var query = GetDeletedEntriesQueryable(versionId, actualizeStart);
                
                if (query.Any())
                {
                   var st1Query = stage1Domain.GetAll()
                        .Fetch(x => x.Stage2Version)
                        .ThenFetch(x => x.Stage3Version)
                        .ThenFetch(x => x.ProgramVersion)
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                        .Where(x => query.Any(y => y.Stage2Version.Stage3Version.Id == x.Stage2Version.Stage3Version.Id));

                    var stage1IdsForDelete = query.Select(x => x.Id).ToHashSet();

                    var volumeDict = st1Query
                                        .AsEnumerable()
                                        .GroupBy(x => x.Stage2Version.Stage3Version.Id)
                                        .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));


                    var deleteVolumeDict = query
                                        .AsEnumerable()
                                        .GroupBy(x => x.Stage2Version.Stage3Version.Id)
                                        .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                    var st2Dict = st1Query
                        .AsEnumerable()
                        .GroupBy(x => x.Stage2Version)
                        .Select(x =>
                        {
                            var existSt1 = x.Where(y => !stage1IdsForDelete.Contains(y.Id)).ToArray();

                            x.Key.Sum = existSt1.SafeSum(y => y.Sum + y.SumService);

                            return new
                            {
                                x.Key,
                                Value = existSt1
                            };
                        })
                        .ToDictionary(x => x.Key, y => y.Value);

                    var stage2IdsForDelete = st2Dict.Where(x => !x.Value.Any()).Select(x => x.Key.Id).ToHashSet();

                    var st3Dict = st2Dict
                        .Select(x => x.Key)
                        .GroupBy(x => x.Stage3Version)
                        .ToDictionary(x => x.Key, y => y.Where(x => !stage2IdsForDelete.Contains(x.Id)).ToArray());

                    var stage3IdsForDelete = st3Dict.Where(x => !x.Value.Any()).Select(x => x.Key.Id).ToHashSet();

                    if (!stage1IdsForDelete.Any())
                    {
                        return new BaseDataResult(false, "Нет записей для удаления");
                    }

                    var log = new VersionActualizeLog();
                    log.ActualizeType = VersionActualizeType.ActualizeDeletedEntries;
                    log.DateAction = DateTime.Now;
                    log.ProgramVersion = new ProgramVersion() { Id = versionId };
                    log.UserName = User.Name;

                    var logRecords = new List<ActualizeVersionLogRecord>();

                    var st3ToUpdate = new List<VersionRecord>();
                    var st2ToUpdate = new List<VersionRecordStage2>();
                    foreach (var st3 in st3Dict)
                    {
                        var oldVolume = volumeDict.ContainsKey(st3.Key.Id) ? volumeDict[st3.Key.Id] : 0m;
                        var deleteVolume = deleteVolumeDict.ContainsKey(st3.Key.Id) ? deleteVolumeDict[st3.Key.Id] : 0m;

                        if (stage3IdsForDelete.Contains(st3.Key.Id))
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Удаление",
                                Description = "Не удовлетворяет условиям ДПКР",
                                Address = st3.Key.RealityObject.Address,
                                Ceo = st3.Key.CommonEstateObjects,
                                PlanYear = st3.Key.Year,
                                Volume = oldVolume,
                                Sum = st3.Key.Sum,
                                Number = st3.Key.IndexNumber
                            };

                            logRecords.Add(logRecord);
                        }
                        else
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Изменение",
                                Description = "Часть КЭ не удовлетворяет условиям ДПКР",
                                Address = st3.Key.RealityObject.Address,
                                Ceo = st3.Key.CommonEstateObjects,
                                PlanYear = st3.Key.Year,
                                Volume = oldVolume,
                                Sum = st3.Key.Sum,
                                Number = st3.Key.IndexNumber
                            };


                            st3.Key.Sum = st3.Value.SafeSum(y => y.Sum);
                            st3.Key.CommonEstateObjects = st3.Value.Select(y => y.CommonEstateObject.Name).Distinct().AggregateWithSeparator(", ");

                            logRecord.ChangeCeo = st3.Key.CommonEstateObjects;
                            logRecord.ChangeSum = st3.Key.Sum;
                            logRecord.ChangeVolume = oldVolume - deleteVolume;

                            logRecords.Add(logRecord);

                            st3ToUpdate.Add(st3.Key);
                            st2ToUpdate.AddRange(st3.Value);
                        }
                    }

                    if (logRecords.Any())
                    {
                        log.CountActions = logRecords.Count();
                        log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Логи отсутсвуют");
                    }

                    var sessionProvider = Container.Resolve<ISessionProvider>();
                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {

                                foreach (var id in stage1IdsForDelete)
                                {
                                    
                                    // удаляем связь с видами работ
                                    session.CreateSQLQuery(
                                        string.Format(@"delete from ovrhl_type_work_cr_st1 where st1_id = {0}", id))
                                            .ExecuteUpdate();

                                    // удаляем запись 1 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE1_VERSION where id = {0}", id)).ExecuteUpdate();
                                }


                                foreach (var id in stage2IdsForDelete)
                                {

                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_rec where stage2_id = {0} ",
                                                            id)).ExecuteUpdate();

                                    session.CreateSQLQuery(string.Format(@"delete from ovhl_dpkr_correct_st2 where st2_version_id = {0} ",
                                                            id)).ExecuteUpdate();

                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg_rec where stage2_id = {0} ",
                                                            id)).ExecuteUpdate();

                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where id = {0} ",
                                                            id)).ExecuteUpdate();
                                }

                                foreach (var id in stage3IdsForDelete)
                                {
                                    // удаляем запись 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_VERSION_REC where id = {0}", id)).ExecuteUpdate();
                                }

                                foreach (var st3 in st3ToUpdate)
                                {
                                    session.Update(unProxy.GetUnProxyObject(st3));
                                }

                                foreach (var st2 in st2ToUpdate)
                                {
                                    session.Update(unProxy.GetUnProxyObject(st2));
                                }

                                session.Insert(log);

                                transaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                transaction.Rollback();
                                throw exc;
                            }
                        }

                    }

                    // Поскольку удалили чтото из версии то над опересчитать заново индекс
                    // Фообщем нельзя после удаления пересчитыват ьочередь потомучто в Лог уже записалиь очередность, пусть они нажимают пересчет очереди и сами там делают что им надо
                    // UpdateIndexNumber(versionId, startIndex);
                }

                return new BaseDataResult();

            }
            finally
            {
                Container.Release(roStructElDomain);
                Container.Release(roStrElService);
                Container.Release(stage2Domain);
                Container.Release(stage1Domain);
                Container.Release(stage3Domain);
            }
            
            return new BaseDataResult();
        }

        private void UpdateIndexNumber(long versionId, int startIndex = -1)
        {
            
            // Тут короче я прохожу по всем записям версии и просто проверяют если ест ькакието пустоты то заполняю их нужными индексами
            // следовательно вся очередь уплотняется к наименьшему значению
            
            var stage3Domain = Container.Resolve<IDomainService<VersionRecord>>();

            try
            {
                var data = stage3Domain.GetAll().Where(x => x.ProgramVersion.Id == versionId)
                                .WhereIf(startIndex != -1, x => x.IndexNumber >= startIndex)
                                .OrderBy(x => x.IndexNumber)
                                .AsEnumerable();

                var listToUpdate = new List<VersionRecord>();

                var idx = 1;

                foreach (var rec in data)
                {
                    if (rec.IndexNumber != idx)
                    {
                        rec.IndexNumber = idx;
                        listToUpdate.Add(rec);
                    }

                    ++idx;
                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToUpdate.ForEach(stage3Domain.Update);
                        tr.Commit();
                    }
                    catch (Exception)
                    {

                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Release(stage3Domain);
            }
            
        }

        public IDataResult GetWarningMessage(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);
            var typeWorkSt1Domain = Container.ResolveDomain<TypeWorkCrVersionStage1>();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var startPeriod = config.ProgrammPeriodStart;
            var endPeriod = config.ProgrammPeriodEnd;
            var useValidShortProgram = config.ActualizeConfig.ActualizeUseValidShortProgram;

            using (Container.Using(typeWorkSt1Domain))
            {
                if (actualizeStart > 0)
                {
                    if (actualizeStart > actualizeEnd && actualizeEnd > 0)
                    {
                        return new BaseDataResult(false, "Начало периода актуазиации не может быть больше окончания периода актуализации");
                    }

                    if (actualizeStart < startPeriod)
                    {
                        return new BaseDataResult(false, string.Format("Начало периода актуазиации не должно быть меньше начала периода долгосрочной программы {0}.", startPeriod));
                    }

                    if (actualizeStart > endPeriod)
                    {
                        return new BaseDataResult(false, string.Format("Начало периода актуазиации не должно быть больше окончания периода долгосрочной программы {0}.", startPeriod));
                    }   

                    if (actualizeEnd > endPeriod)
                    {
                        return new BaseDataResult(false, string.Format("Окончание периода актуазиации не должно быть больше окончания периода долгосрочной программы {0}.", startPeriod));
                    }    
                }

                if (useValidShortProgram == TypeUsage.Used)
                {
                    var programs = typeWorkSt1Domain.GetAll()
                                     .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                     .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr != TypeProgramStateCr.Close)
                                     .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden)
                                     .WhereIf(
                                         actualizeStart > 0,
                                         x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateStart.Year >= actualizeStart || (x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd.HasValue && x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd.Value.Year >= actualizeStart))
                                     .WhereIf(
                                         actualizeEnd > 0,
                                         x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateStart.Year <= actualizeEnd || (x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd.HasValue && x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd.Value.Year >= actualizeEnd))
                                     .Select(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id)
                                     .Distinct()
                                     .ToList();

                    if (programs.Any())
                    {
                        var names = ProgramCrDomain.GetAll()
                                        .Where(x => programs.Contains(x.Id))
                                        .Select(x => x.Period.Name)
                                        .ToList()
                                        .Distinct()
                                        .Aggregate((x, y) => string.IsNullOrEmpty(y) ? x : y + "," + x);

                        return new BaseDataResult(false, string.Format("За период '{0}' создана краткосрочная программа. Актуализация долгосрочной программы за этот период не может быть проведена. Измените период актуализации и проведите актуализацию повторно.", names));
                    }
                }

                
                return new BaseDataResult(null);
            }
        }

        private List<VersionRecord> ChangeIndexNumber(List<VersionRecord> changedRecs, IQueryable<VersionRecord> verRecQuery)
        {
            var result = new List<VersionRecord>();

            var newRecsMinYear = changedRecs.SafeMin(x => x.Year);

            var recsForChangeIndexNum = verRecQuery
                                            .Where(x => newRecsMinYear > 0 && x.Year > newRecsMinYear)
                                            .ToList();

            var changedRecIds = changedRecs.Select(x => x.Id).ToHashSet();

            changedRecs.AddRange(recsForChangeIndexNum.Where(x => !changedRecIds.Contains(x.Id)));

            var changedRecsByYearDict = changedRecs
                .GroupBy(x => x.Year)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.IndexNumber));

            var maxIndNumByYear = verRecQuery
                .Select(x =>
                    new
                    {
                        x.IndexNumber,
                        x.Year
                    })
                  .AsEnumerable()
                  .GroupBy(x => x.Year)
                  .Select(x => new { Year = x.Key, MaxIndNum = x.Max(y => y.IndexNumber) })
                  .ToList();

            var indexIncrement = 0;

            foreach (var changedRecsByYear in changedRecsByYearDict)
            {
                var yearMaxInd = maxIndNumByYear.Where(x => x.Year <= changedRecsByYear.Key).SafeMax(x => x.MaxIndNum);

                foreach (var changedRec in changedRecsByYear.Value)
                {
                    if (changedRec.Id > 0)
                    {
                        changedRec.IndexNumber += indexIncrement;
                    }
                    else
                    {
                        ++indexIncrement;
                        changedRec.IndexNumber = indexIncrement + yearMaxInd;
                    }

                    result.Add(changedRec);
                }
            }

            return result;
        }
    }
}