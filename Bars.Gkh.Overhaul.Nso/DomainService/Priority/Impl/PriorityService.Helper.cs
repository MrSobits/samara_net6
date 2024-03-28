using Bars.B4.Modules.FIAS;

namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Bars.Gkh.Overhaul.Nso.PriorityParams;
    using Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;

    using Enums;
    using NHibernate;

    public partial class PriorityService
    {
        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2Domain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Stage3Service { get; set; }

        private List<RealityObjectStructuralElementInProgrammStage3> 
        SetPriority(Dictionary<string, int> userCriterias, TypePriority method, int startYear)
        {
            var pointsParams = new Dictionary<long, List<StoredPointParam>>();

            var stage3Query = Stage3Service.GetAll();

            var stage2Query = Stage2Service.GetAll()
                    .Select(x => new Stage2Proxy
                    {
                        Stage3Id = x.Stage3.Id,
                        CeoId = x.CommonEstateObject.Id,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable();

            var stage1Proxy = Stage1Service.GetAll()
                .Select(x => new Stage1Proxy
                {
                    Stage3Id = x.Stage2.Stage3.Id,
                    Wearout = x.StructuralElement.Wearout
                })
                .AsEnumerable();

            switch (method)
            {
                case TypePriority.Criteria:
                    {
                        if (userCriterias.Count == 0)
                        {
                            throw new ValidationException("Не указаны параметры");
                        }
                        break;
                    }

                case TypePriority.Points:
                    {
                        pointsParams = GetPoints(stage3Query, stage2Query, stage1Proxy);
                        userCriterias["PointsParam"] = 1;
                        break;
                    }
                case TypePriority.CriteriaAndPoins:
                    {
                        pointsParams = GetPoints(stage3Query, stage2Query, stage1Proxy);
                        break;
                    }
            }

            var points = pointsParams.ToDictionary(x => x.Key, y => y.Value.Sum(x => x.Value));

            // Такой словарик нужен для того чтобы быстро получать элементы при сохранении уже из готовых данных по Id
            var dictStage3 = stage3Query.ToDictionary(x => x.Id);

            var muStage3 = stage3Query.Select(
                x => new Stage3Order
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

            //получение годом капитального ремонта
            var firstPrivYears = GetFirstPrivYears(startYear);

            var lastOvrhlYears = GetLastOvrhlYears();

            var weights = GetWeights();
            
            var yearsWithLifetimes = GetSeYearsWithLifetimes();

            foreach (var item in muStage3)
            {
                var density =
                    item.AreaLiving.HasValue && item.AreaLiving > 0 && item.NumberLiving.HasValue
                        ? item.NumberLiving.Value / item.AreaLiving.Value
                        : 0m;

                var injections = new
                {
                    item.PrivatizDate,
                    DictPoints = points,
                    item.Year,
                    item.BuildYear,
                    StartYear = startYear,
                    Density = density,
                    Weights = weights.ContainsKey(item.Id) ? weights[item.Id] : new[] { 0 },
                    FirstPrivYears = firstPrivYears.ContainsKey(item.Id) ? firstPrivYears[item.Id] : new List<int>(),
                    OverhaulYears = lastOvrhlYears.ContainsKey(item.Id) ? lastOvrhlYears[item.Id] : new List<int>(),
                    OverhaulYearsWithLifetimes = yearsWithLifetimes.ContainsKey(item.Id) ? yearsWithLifetimes[item.Id] : new List<int>()
                };

                CalculateOrder(item, userCriterias.Keys, injections);
            }

            /*
            * Сначала сортируем по плановому году 
            */
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

            var index = 1;
            foreach (var item in proxyList2)
            {
                if (!dictStage3.ContainsKey(item.Id)) continue;

                var st3 = dictStage3[item.Id];

                FillStage3Criteria(st3, item.OrderDict);

                st3.StoredPointParams = pointsParams.ContainsKey(st3.Id) ? pointsParams[st3.Id] : new List<StoredPointParam>();

                st3.IndexNumber = index++;
            }

             return dictStage3.Values.ToList();
        }

        public void CalculateOrder(Stage3Order st3Oreder, IEnumerable<string> keys, object injections)
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

        private IProgrammPriorityParam ResolveEvaluator(string paramCode, object stage3)
        {
            return this.Container.Resolve<IProgrammPriorityParam>(paramCode, new Arguments
            {
                {"stage3", stage3}
            });
        }

        public void FillStage3Criteria(IStage3Entity st3Item, Dictionary<string, object> orderDict)
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

        private Dictionary<long, int> GetCountWorks()
        {
            var result = new Dictionary<long, int>();

            var stage1Recs = Stage1Service.GetAll()
                .Where(x => x.Stage2.Stage3 != null)
                .Select(x => new
                {
                    Stage3Id = x.Stage2.Stage3.Id,
                    x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id));

            var strElWorks = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
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

        private Dictionary<long, IEnumerable<int>> GetWeights()
        {
            return Stage2Domain.GetAll()
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

        private Dictionary<string, int> GetUserCriterias(object[] records)
        {
            var criterias = new Dictionary<string, int>();

            if (records == null)
            {
                return criterias;
            }

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

                criterias.Add(code, dd.Get("Order", 0));
            }

            return criterias;
        }

        private void SaveCriterias(Dictionary<string, int> userCriterias)
        {
            var currPriorityParamsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            InTransaction(() =>
            {
                foreach (var value in currPriorityParamsService.GetAll().ToList())
                {
                    currPriorityParamsService.Delete(value.Id);
                }

                foreach (var value in userCriterias)
                {
                    currPriorityParamsService.Save(new CurrentPrioirityParams { Code = value.Key, Order = value.Value });
                }
            });
        }

        public Dictionary<long, List<StoredPointParam>> GetPoints(IQueryable<IStage3Entity> stage3RecsQuery, IEnumerable<Stage2Proxy> stage2Query, IEnumerable<Stage1Proxy> stage1Query, long? versionId = null)
        {
            var result = new Dictionary<long, List<StoredPointParam>>(); 
            var prParamAdditionDomain = Container.Resolve<IDomainService<PriorityParamAddition>>();
            
            var priorityParamAdditions = prParamAdditionDomain.GetAll().ToDictionary(x => x.Code, y => y);
            PriorityParamAddition addtionInfo = null;

            var roStructElQuery =
                RoStructElDomain.GetAll()
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.Wearout,
                        x.LastOverhaulYear,
                        CeoId = x.StructuralElement.Group.CommonEstateObject.Id
                    })
                    .AsEnumerable();

            var dictRoWearout = roStructElQuery
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y =>
                {
                    var count = y.Count();

                    decimal res = 0;

                    if (count > 0)
                    {
                        res = y.Sum(x => x.Wearout) / count;
                    }
                    return res;
                });

            var roceoDict =
                    stage2Query
                        .Select(x => new
                        {
                            x.Stage3Id,
                            x.CeoId
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Stage3Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.CeoId).ToHashSet());

            var wearout = GetWearout(stage1Query);

            var countWorksByRo = GetCountsWorksByRo();

            var countWorksInYear = GetCountWorksInYear(versionId);

            var usages = GetStrElUsagesDict(versionId);

            var priorParams = Container.ResolveAll<IPriorityParams>(new Arguments
            {
                { "DictRoWearout", dictRoWearout },
                { "Stage3CeoDict", roceoDict },
                { "CountWorksByRo", countWorksByRo },
                { "CountWorksInYear", countWorksInYear },
                { "StructElementUsageValues", usages },
                { "Wearout", wearout }
            }).ToDictionary(x => x.Id);

            var stage3Records = stage3RecsQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.Year,
                        RoId = x.RealityObject.Id,
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
                        x.RealityObject.FiasAddress.PlaceName,
                        MuLevel = (TypeMunicipality?)x.RealityObject.MoSettlement.Level
                    })
                    .AsEnumerable();

            var qualityParams = Container.Resolve<IDomainService<QualityPriorityParam>>().GetAll().ToList();
            var quantParams = Container.Resolve<IDomainService<QuantPriorityParam>>().GetAll().ToList();
            var multiParams = Container.Resolve<IDomainService<MultiPriorityParam>>().GetAll().ToList();

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
                        FiasAddress = new FiasAddress {PlaceName = record.PlaceName},
                        MoSettlement = record.MuLevel.HasValue
                            ? new Municipality {Level = record.MuLevel.Value}
                            : null
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

            return result;
        }

        private Dictionary<long, decimal> GetWearout(IEnumerable<Stage1Proxy> stage1Query)
        {
            return stage1Query
                .Select(
                        x => new
                        {
                            x.Stage3Id,
                            x.Wearout
                        })
                    .ToList()
                    .GroupBy(x => x.Stage3Id)
                    .Select(x => new { x.Key, Wearout = x.Sum(y => y.Wearout / 70) })
                    .ToDictionary(x => x.Key, x => x.Wearout);
        }

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

        private Dictionary<Tuple<int, long>, int> GetCountWorksInYear(long? versionId = null)
        {
            if (!versionId.HasValue)
                return new Dictionary<Tuple<int, long>, int>();

            return Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => x.ProgramVersion.Id == versionId.Value)
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

        private Dictionary<long, decimal> GetStrElUsagesDict(long? versionId = null)
        {
            var result = new Dictionary<long, decimal>();

            if (!versionId.HasValue)
                return result;

            var stage1Items = Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId.Value)
                .Select(x =>
                    new Stage1Node
                    {
                        Stage3Id = x.Stage2Version.Stage3Version.Id,
                        PlanYear = x.Stage2Version.Stage3Version.Year,
                        RoId = x.RealityObject.Id,
                        RoStructElId = x.StructuralElement.Id,
                        Lifetime = x.StructuralElement.StructuralElement.LifeTime,
                        LifetimeAfterRepair = x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                        OverhaulYear = x.StructuralElement.LastOverhaulYear,
                        BuildYear = x.RealityObject.BuildYear
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

        /// <summary>
        /// Метод, для выполнения действий в транзации 
        /// </summary>
        /// <param name="action">Действие</param>
        private void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
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
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        private void InTransaction(Action<IStatelessSession> action)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    action(session);

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }

    public class Stage2Proxy
    {
        public long Stage3Id { get; set; }

        public long CeoId { get; set; }

        public long RoId { get; set; }
    }

    public class Stage1Proxy
    {
        public long Stage3Id { get; set; }

        public decimal Wearout { get; set; }
    }
}
