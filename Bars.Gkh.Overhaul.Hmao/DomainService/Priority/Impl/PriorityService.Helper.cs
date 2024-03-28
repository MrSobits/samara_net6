namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.PriorityParams;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;

    using NHibernate;

    /// <summary>
    /// Сервис расчета очередности
    /// </summary>
    public partial class PriorityService
    {
        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2Domain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Stage3Service { get; set; }

        ///<inheritdoc/>
        public void CalculateOrder(Stage3Order st3Oreder, IEnumerable<string> keys, object injections)
        {
            foreach (var key in keys)
            {
                var eval = this.ResolveEvaluator(key, injections);

                if (eval != null)
                {
                    if (!st3Oreder.OrderDict.ContainsKey(key))
                    {
                        st3Oreder.OrderDict.Add(key, eval.GetValue(st3Oreder.Stage3));
                    }
                }
            }
        }

        ///<inheritdoc/>
        public void FillStage3Criteria(IStage3Entity st3Item, Dictionary<string, object> orderDict)
        {
            st3Item.StoredCriteria = st3Item.StoredCriteria ?? new List<StoredPriorityParam>();
            st3Item.StoredCriteria.Clear();

            foreach (var param in orderDict)
            {
                st3Item.StoredCriteria.Add(
                    new StoredPriorityParam
                    {
                        Criterion = param.Key,
                        Value =

                            //костыль для отображения в экселе
                            param.Key == "DateInventoryParam"
                                ? param.Value.ToInt() > 0
                                    ? (DateTime.MinValue + new TimeSpan(param.Value.ToInt(), 0, 0, 0)).ToShortDateString()
                                    : ""
                                : param.Value.ToString()
                    });
            }
        }

        ///<inheritdoc/>
        public Dictionary<long, List<StoredPointParam>> GetPoints(
            long muId,
            IQueryable<IStage3Entity> stage3RecsQuery,
            IEnumerable<Stage2Proxy> stage2Query,
            IEnumerable<Stage1Proxy> stage1Query)
        {
            var paramAdditionDomain = this.Container.ResolveDomain<PriorityParamAddition>();
            var qualityPriorityParamDomain = this.Container.ResolveDomain<QualityPriorityParam>();
            var quantPriorityParamDomain = this.Container.ResolveDomain<QuantPriorityParam>();
            var multiPriorityParamDomain = this.Container.ResolveDomain<MultiPriorityParam>();

            var result = new Dictionary<long, List<StoredPointParam>>();

            var priorityParamAdditions = paramAdditionDomain.GetAll().ToDictionary(x => x.Code, y => y);

            var priorParams = this.ResolvePriorityParams(muId, stage2Query, stage1Query);

            var stage3Records = stage3RecsQuery
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Year,
                        RoId = x.RealityObject.Id,
                        CapGroupId = (long?) x.RealityObject.CapitalGroup.Id,
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
                        MuLevel = (TypeMunicipality?) x.RealityObject.MoSettlement.Level
                    })
                .AsEnumerable();

            var qualityParams = qualityPriorityParamDomain.GetAll().ToList();
            var quantParams = quantPriorityParamDomain.GetAll().ToList();
            var multiParams = multiPriorityParamDomain.GetAll().ToList();

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
                        CapitalGroup = new CapitalGroup {Id = record.CapGroupId.GetValueOrDefault(), Code = record.CapGroupCode},
                        FiasAddress = new FiasAddress {PlaceName = record.PlaceName},
                        MoSettlement = record.MuLevel.HasValue
                            ? new Municipality {Level = record.MuLevel.Value}
                            : null
                    }
                };

                PriorityParamAddition addtionInfo;
                
                foreach (var param in qualityParams)
                {
                    if (!priorParams.ContainsKey(param.Code))
                    {
                        continue;
                    }

                    var value = (int) priorParams[param.Code].GetValue(stage3Rec);

                    if (value == param.Value)
                    {
                        addtionInfo = priorityParamAdditions.ContainsKey(param.Code) ? priorityParamAdditions[param.Code] : null;
                        result[id].Add(
                            new StoredPointParam
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
                        var additPoint = (addtionInfo != null && addtionInfo.AdditionFactor == PriorityParamAdditionFactor.Using
                            ? addtionInfo.FactorValue
                            : 1);
                        result[id].Add(
                            new StoredPointParam
                            {
                                Code = param.Code,
                                Value =
                                    (addtionInfo != null && addtionInfo.FinalValue == PriorityParamFinalValue.ParameterValue ? value : param.Point)
                                        * additPoint
                            });
                    }
                }

                foreach (var param in multiParams)
                {
                    if (!priorParams.ContainsKey(param.Code))
                    {
                        continue;
                    }

                    if (((IMultiPriorityParam) priorParams[param.Code]).CheckContains(stage3Rec, param.StoredValues))
                    {
                        var first = result[id].FirstOrDefault(x => x.Code == param.Code);
                        if (first != null)
                        {
                            first.Value += param.Point;
                        }
                        else
                        {
                            addtionInfo = priorityParamAdditions.ContainsKey(param.Code) ? priorityParamAdditions[param.Code] : null;
                            result[id].Add(
                                new StoredPointParam
                                {
                                    Code = param.Code,
                                    Value = param.Point *
                                        (addtionInfo != null && addtionInfo.AdditionFactor == PriorityParamAdditionFactor.Using
                                            ? addtionInfo.FactorValue
                                            : 1)
                                });
                        }
                    }
                }
            }

            return result;
        }

        private List<RealityObjectStructuralElementInProgrammStage3>
            SetPriority(Municipality municipality, Dictionary<string, int> userCriterias, TypePriority method, int startYear)
        {
            var muId = municipality.Id;

            var pointsParams = new Dictionary<long, List<StoredPointParam>>();

            var stage3Query = this.Stage3Service.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId || x.RealityObject.MoSettlement.Id == muId);

            var stage2Query = this.Stage2Service.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId || x.RealityObject.MoSettlement.Id == muId)
                .Select(
                    x => new Stage2Proxy
                    {
                        Stage3Id = x.Stage3.Id,
                        CeoId = x.CommonEstateObject.Id,
                        RoId = x.RealityObject.Id
                    })
                .AsEnumerable();

            var stage1Proxy = this.Stage1Service.GetAll()
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId || x.StructuralElement.RealityObject.MoSettlement.Id == muId)
                .Select(
                    x => new Stage1Proxy
                    {
                        Stage3Id = x.Stage2.Stage3.Id,
                        CeoGroupType = x.StructuralElement.StructuralElement.Group.CommonEstateObject.GroupType,
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
                    pointsParams = this.GetPoints(muId, stage3Query, stage2Query, stage1Proxy);
                    userCriterias["PointsParam"] = 1;
                    break;
                }
                case TypePriority.CriteriaAndPoins:
                {
                    pointsParams = this.GetPoints(muId, stage3Query, stage2Query, stage1Proxy);
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
                    AreaLiving = x.RealityObject.AreaLiving,
                    NumberLiving = x.RealityObject.NumberLiving,
                    BuildYear = x.RealityObject.BuildYear,
                    DateTechInspection = x.RealityObject.DateTechInspection,
                    PhysicalWear = x.RealityObject.PhysicalWear,
                    DateCommissioning = x.RealityObject.DateCommissioning
                })
                .ToList();

            //получение годом капитального ремонта
            var firstPrivYears = this.GetFirstPrivYears(municipality, startYear);

            var lastOvrhlYears = this.GetLastOvrhlYears(municipality);

            var weights = this.GetWeights(municipality);

            var countWorks = this.GetCountWorks(municipality);

            var yearsWithLifetimes = this.GetSeYearsWithLifetimes(municipality);

            foreach (var item in muStage3)
            {
                var density =
                    item.AreaLiving.HasValue && item.AreaLiving > 0 && item.NumberLiving.HasValue
                        ? item.NumberLiving.Value / item.AreaLiving.Value
                        : 0m;

                var injections = new
                {
                    item.Id,
                    DictPoints = points,
                    item.Year,
                    item.PhysicalWear,
                    item.DateTechInspection,
                    item.PrivatizDate,
                    item.BuildYear,
                    item.DateCommissioning,
                    Density = density,
                    CountWorks = countWorks.ContainsKey(item.Id) ? countWorks[item.Id] : 0,
                    Weights = weights.ContainsKey(item.Id) ? weights[item.Id] : new[] {0},
                    FirstPrivYears = firstPrivYears.ContainsKey(item.Id) ? firstPrivYears[item.Id] : new List<int>(),
                    OverhaulYears = lastOvrhlYears.ContainsKey(item.Id) ? lastOvrhlYears[item.Id] : new List<int>(),
                    OverhaulYearsWithLifetimes = yearsWithLifetimes.ContainsKey(item.Id) ? yearsWithLifetimes[item.Id] : new List<int>()
                };

                this.CalculateOrder(item, userCriterias.Keys, injections);
            }

            /*
            * Сначала сортируем по плановому году 
            */
            var proxyList2 = muStage3.OrderBy(x => x.Year);
            foreach (var item in userCriterias.OrderBy(x => x.Value))
            {
                var key = item.Key;

                var eval = this.ResolveEvaluator(key, new object());
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
                if (!dictStage3.ContainsKey(item.Id))
                {
                    continue;
                }

                var st3 = dictStage3[item.Id];

                this.FillStage3Criteria(st3, item.OrderDict);

                st3.StoredPointParams = pointsParams.ContainsKey(st3.Id) ? pointsParams[st3.Id] : new List<StoredPointParam>();

                st3.IndexNumber = index++;
            }

            return dictStage3.Values.ToList();
        }

        private IProgrammPriorityParam ResolveEvaluator(string paramCode, object stage3)
        {
            return this.Container.Resolve<IProgrammPriorityParam>(paramCode, new Arguments
            {
                {"stage3", stage3}
            });
        }

        private Dictionary<long, List<int>> GetFirstPrivYears(Municipality municipality, int startYear)
        {
            var muId = municipality.Id;

            var service = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == muId || x.StructuralElement.RealityObject.MoSettlement.Id == muId)
                .Where(x => x.Stage2.Stage3 != null)
                .Select(
                    x => new
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

        private Dictionary<long, List<int>> GetLastOvrhlYears(Municipality municipality)
        {
            var service = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(
                    x => x.StructuralElement.RealityObject.Municipality.Id == municipality.Id
                        || x.StructuralElement.RealityObject.MoSettlement.Id == municipality.Id)
                .Where(x => x.Stage2.Stage3 != null)
                .Select(
                    x => new
                    {
                        RoSeId = x.StructuralElement.Id,
                        Stage3Id = x.Stage2.Stage3.Id,
                        x.Year,
                        x.StructuralElement.LastOverhaulYear,
                        x.StructuralElement.RealityObject.BuildYear
                    })
                .ToList();

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

        private Dictionary<long, int> GetCountWorks(Municipality municipality)
        {
            var result = new Dictionary<long, int>();

            var stage1Recs = this.Stage1Service.GetAll()
                .Where(
                    x =>
                        x.StructuralElement.RealityObject.Municipality.Id == municipality.Id
                            || x.StructuralElement.RealityObject.MoSettlement.Id == municipality.Id)
                .Where(x => x.Stage2.Stage3 != null)
                .Select(
                    x => new
                    {
                        Stage3Id = x.Stage2.Stage3.Id,
                        x.StructuralElement.StructuralElement.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id));

            var strElWorks = this.Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
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

        private Dictionary<long, IEnumerable<int>> GetWeights(Municipality municipality)
        {
            return this.Stage2Domain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == municipality.Id || x.RealityObject.MoSettlement.Id == municipality.Id)
                .Where(x => x.CommonEstateObject != null)
                .Select(
                    x => new
                    {
                        x.Stage3.Id,
                        x.CommonEstateObject.Weight
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Weight));
        }

        private Dictionary<long, List<int>> GetSeYearsWithLifetimes(Municipality municipality)
        {
            var service = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();

            var stage3Years = service.GetAll()
                .Where(
                    x => x.StructuralElement.RealityObject.Municipality.Id == municipality.Id
                        || x.StructuralElement.RealityObject.MoSettlement.Id == municipality.Id)
                .Where(x => x.Stage2.Stage3 != null)
                .Select(
                    x => new
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
            var currPriorityParamsService = this.Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            this.InTransaction(
                () =>
                {
                    foreach (var value in currPriorityParamsService.GetAll().ToList())
                    {
                        currPriorityParamsService.Delete(value.Id);
                    }

                    foreach (var value in userCriterias)
                    {
                        currPriorityParamsService.Save(new CurrentPrioirityParams {Code = value.Key, Order = value.Value});
                    }
                });
        }

        private Dictionary<string, IPriorityParams> ResolvePriorityParams(
            long muId,
            IEnumerable<Stage2Proxy> stage2Query,
            IEnumerable<Stage1Proxy> stage1Query)
        {
            var basePropertyOwnerDecisionDomain = this.Container.ResolveDomain<BasePropertyOwnerDecision>();

            var roStructElBaseQuery = this.RoStructElDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId || x.RealityObject.MoSettlement.Id == muId);

            var roStructElementList = roStructElBaseQuery
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.Wearout,
                        x.LastOverhaulYear,
                        CeoId = x.StructuralElement.Group.CommonEstateObject.Id
                    })
                .ToList();

            var stage1List = stage1Query.ToList();
            var stage2List = stage2Query.ToList();

            var setDecisions = basePropertyOwnerDecisionDomain
                .GetAll()
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                .Where(x => x.RealityObject.Municipality.Id == muId || x.RealityObject.MoSettlement.Id == muId)
                .Select(x => x.RealityObject.Id)
                .ToHashSet();

            var dictLastOverhaulYear = roStructElementList
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Max(x => x.LastOverhaulYear));

            var dictRoCeoCnt = roStructElementList
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.CeoId).Distinct().Count());

            var dictNeedOverhaulPercent = stage2List
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(
                    x => x.Key,
                    y =>
                    {
                        var count = y.Count();

                        var roId = y.First().RoId;

                        if (dictRoCeoCnt.ContainsKey(roId))
                        {
                            return (int) ((decimal) count / dictRoCeoCnt[roId] * 100);
                        }

                        return 0;
                    });

            var dictNeedOverhaulCeo = stage2List
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Count());

            var dictPhysicalWearCeo = stage1List
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Wearout) / y.Count());

            var liftWearoutDict = stage1List
                .Where(x => x.CeoGroupType.Name.Contains("Лифт"))
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Wearout).ToList());

            var dictRoCeoLastOverhaulYear = roStructElementList
                .GroupBy(x => new {x.RoId, x.CeoId})
                .ToDictionary(
                    x => x.Key,
                    z => z.Max(x => x.LastOverhaulYear));

            var dictLastOverhaulYearCeo = stage2List
                .Select(
                    x => new
                    {
                        x.Stage3Id,
                        LastOverhaulYear = dictRoCeoLastOverhaulYear.ContainsKey(new {x.RoId, x.CeoId})
                            ? dictRoCeoLastOverhaulYear[new {x.RoId, x.CeoId}]
                            : 0
                    })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Max(x => x.LastOverhaulYear));

            var roceoDict = stage2List
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.CeoId).ToHashSet());

            var structElDict = roStructElBaseQuery
                .GroupBy(x => Tuple.Create(x.RealityObject.Id, x.StructuralElement.Group.CommonEstateObject.Id))
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.LastOverhaulYear).FirstOrDefault());

            var structuralElementWithLastYearDict = stage2List.Select(
                x => new
                {
                    x.Stage3Id,
                    RoStructuralElement = structElDict.Get(x.RoId, x.CeoId)
                })
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.RoStructuralElement).FirstOrDefault());

            var priorParams = this.Container.ResolveAll<IPriorityParams>(
                    
                new Arguments
                {
                    {"SetDecisions", setDecisions},
                    {"DictLastOverhaulYear",  dictLastOverhaulYear},
                    {"DictNeedOverhaulPercent", dictNeedOverhaulPercent},
                    {"DictNeedOverhaulCeo", dictNeedOverhaulCeo},
                    {"DictPhysicalWearCeo", dictPhysicalWearCeo},
                    {"DictLastOverhaulYearCeo", dictLastOverhaulYearCeo},
                    {"Stage3CeoDict", roceoDict},
                    {"StructuralElementWithLastYearDict", structuralElementWithLastYearDict},
                    {"LiftWearoutDict", liftWearoutDict},
                }
              )
                .ToDictionary(x => x.Id);

            return priorParams;
        }

        /// <summary>
        /// Метод, для выполнения действий в транзации
        /// </summary>
        /// <param name="action">Действие</param>
        private void InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
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
            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

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
        public GroupType CeoGroupType { get; set; }
        public decimal Wearout { get; set; }
    }
}