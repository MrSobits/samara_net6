namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    using B4.DataAccess;

    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Gkh.Entities;
    using Enums;
    using Domain;

    using Castle.Windsor;
    using CommonParams;
    using Entities;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;
    using Overhaul.Enum;

    public class RealEstateTypeRateService : IRealEstateTypeRateService
    {
        private sealed class ParamConstraint
        {
            public string Code { get; set; }

            public string Min { get; set; }

            public string Max { get; set; } 
        }

        private sealed class ParamValue
        {
            public object Value { get; set; }

            public CommonParamType CommonParamType { get; set; }
        }

        private sealed class RealEstateTypeRateProxy
        {
            public long? RealEstateTypeId { get; set; }

            public decimal? NeedForFunding { get; set; }

            public decimal? TotalArea { get; set; }

            public decimal? ReasonableRate { get; set; }
        }

        private sealed class StructuralElementExists
        {
            public long StructuralElementId { get; set; }

            public bool Exists { get; set; }
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<RealEstateTypeRealityObject> RealEstateTypeRealityObjectDomain { get; set; }

        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }

        /// <summary>
        /// Расчет показателей для типов домов
        /// </summary>
        /// <param name="baseParams">
        /// The base Params.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        public IDataResult CalculateRates(BaseParams baseParams)
        {
            try
            {
                var serviceRealtyObject = Container.Resolve<IDomainService<RealityObject>>();
                var serviceRealityObjectStructuralElement = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
                var serviceRealEstateTypeStructElement = Container.Resolve<IDomainService<RealEstateTypeStructElement>>();
                var serviceRealEstateType = Container.Resolve<IDomainService<RealEstateType>>();

                // Идентификаторы типов домов
                var realEstateTypeIds = serviceRealEstateType.GetAll().Select(x => x.Id).ToList();

                // Типы домов с конструктивными характеристиками
                var realEstateTypeSeExists =
                    serviceRealEstateTypeStructElement.GetAll()
                        .Select(
                            x => new { realEstateTypeId = x.RealEstateType.Id, seId = x.StructuralElement.Id, x.Exists })
                        .AsEnumerable()
                        .GroupBy(x => x.realEstateTypeId)
                        .ToDictionary(
                            x => x.Key,
                            x =>
                                x.Select(
                                    y => new StructuralElementExists
                                         {
                                             StructuralElementId = y.seId, Exists = y.Exists
                                         })
                                    .ToList());

                // Ограничения типа по конструктивным характеристикам
                // Словарь типов домов, со списком Конст. Хар-т (если заданы)
                var realEstateTypeContructiveElementsDict = realEstateTypeIds.ToDictionary(
                    x => x,
                    x => realEstateTypeSeExists.ContainsKey(x)
                             ? realEstateTypeSeExists[x]
                             : new List<StructuralElementExists>());

#warning данный Метод получения Домов участвующих в ДПКР вынести в ДоменСервис, который возвращает IQueryable<RealityObject>

                var realtyObjectAreaLivingDict = serviceRealtyObject.GetAll()
                    .Where(x => !EmergencyDomain.GetAll()
                        .Any(e => e.RealityObject.Id == x.Id) 
                        || EmergencyDomain.GetAll()
                            .Any(e => e.RealityObject.Id == x.Id && (e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)))
                    .Where(x => (x.TypeHouse == TypeHouse.BlockedBuilding || x.TypeHouse == TypeHouse.ManyApartments
                                || x.TypeHouse == TypeHouse.SocialBehavior) && (x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated))
                    .Select(x => new
                    {
                        x.Id,
                        x.AreaLiving
                    })
                    .ToDictionary(x => x.Id, x => x.AreaLiving);

                var realtyObjectIds = realtyObjectAreaLivingDict.Keys.ToList();

                var robjStrElements = serviceRealityObjectStructuralElement.GetAll()
                    .Select(x => new { x.RealityObject.Id, seId = x.StructuralElement.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(z => z.seId).ToList());

                // Словарь домов, в котором каждому дому соответствует словарь типов, с флагом того, что дом подходит типу 
                // согласно наличию конструктивных характеристик
                var robjSatisfyStrElemTypeDictByRoId = realtyObjectIds.ToDictionary(
                    x => x,
                    x =>
                        {
                            var robjHasStructElements = robjStrElements.ContainsKey(x);
                            var structElList = robjHasStructElements ? robjStrElements[x] : new List<long>();
                            return realEstateTypeContructiveElementsDict.ToDictionary(
                                y => y.Key,
                                y => y.Value.Count == 0 || (robjHasStructElements && TestForStructElemSatisfy(y.Value, structElList)));
                        });

                // Список идентификаторов домов, который соответствует хотя бы одному типу по наличию конструктивных характеристик
                var realtyObjSatisfiesAnyType = robjSatisfyStrElemTypeDictByRoId
                    .Where(x => x.Value.Any(y => y.Value))
                    .Select(x => x.Key)
                    .ToList();
                
                var realityObjects = serviceRealtyObject.GetAll()
                    .AsEnumerable()
                    .Where(x => realtyObjSatisfiesAnyType.Contains(x.Id))
                    .ToList();

                var commonParams = Container.ResolveAll<ICommonParam>().ToList<ICommonParam>();

                // Получение значений параметров по каждому дому, который соответствует хотя бы одному типу по наличию конструктивных характеристик
                var realtyObjectParamValuesDict = realityObjects
                    .ToDictionary(
                        x => x.Id, 
                        x => commonParams.ToDictionary(
                            y => y.Code, 
                            y => new ParamValue { Value = y.GetValue(x), CommonParamType = y.CommonParamType }));

                // Ограничения типа по параметрам
                var realEstateTypeParamsDictTemp = Container.Resolve<IDomainService<RealEstateTypeCommonParam>>().GetAll()
                    .Select(x => new
                       {
                           x.CommonParamCode,
                           x.RealEstateType.Id,
                           x.Max,
                           x.Min
                       })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.Select(y => new ParamConstraint
                                {
                                    Code = y.CommonParamCode, 
                                    Max = y.Max, 
                                    Min = y.Min
                                })
                              .ToList());

                var realEstateTypeParamsDict = realEstateTypeIds.ToDictionary(
                    x => x,
                    x => realEstateTypeParamsDictTemp.ContainsKey(x)
                        ? realEstateTypeParamsDictTemp[x]
                        : new List<ParamConstraint>());

                // Словарь домов, в котором каждому дому соответствует словарь типов, с флагом того, что дом подходит типу 
                // согласно наличию конструктивных характеристик и удовлетворению параметров типа
                var robjSatisfyRealEstTypeConditionsDict = 
                    realtyObjectParamValuesDict.ToDictionary(
                        x => x.Key, // RealityObjectId
                        x =>
                            {
                                var typeSatisfiedOnStructElemDict = robjSatisfyStrElemTypeDictByRoId[x.Key];
                                
                                return realEstateTypeParamsDict.ToDictionary(
                                    y => y.Key,
                                    y => typeSatisfiedOnStructElemDict[y.Key] && (y.Value.Count == 0 || TestForSatisfyParams(y.Value, x.Value)));
                            });

                // Потребность в финансировании по домам
                var serviceRealityObjectStructuralElementInProgramm = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();
                var needForFundDict = serviceRealityObjectStructuralElementInProgramm.GetAll()
                    .GroupBy(x => x.RealityObject.Id)
                    .Select(x => new
                        {
                            x.Key,
                            Sum = x.Sum(y => y.Sum)
                        })
                    .ToDictionary(x => x.Key, x => x.Sum);

                var config = Container.GetGkhConfig<OverhaulTatConfig>();
                int period = (config.ProgrammPeriodEnd - config.ProgrammPeriodStart + 1) * 12;

                var typedRealtyObjects =
                    robjSatisfyRealEstTypeConditionsDict
                    .Select(x => new
                            {
                                RealityObjectId = x.Key,
                                RealEstateTypeIds = x.Value.Where(y => y.Value).Select(z => x.Key)
                            })
                        .ToList();

                var roIds = typedRealtyObjects.Where(x => x.RealEstateTypeIds.Any())
                                              .Select(x => x.RealityObjectId)
                                              .ToList();
                
                var untypedRealtyObjectsData = realtyObjectAreaLivingDict
                    .Where(x => !roIds.Contains(x.Key))
                    .GroupBy(x => 1)
                    .Select(x =>
                        {
                            var totalAreaLiving = x.Sum(y => y.Value);
                            var needForFund = x.Sum(y => needForFundDict.ContainsKey(y.Key) ? needForFundDict[y.Key] : 0);
                            var reasonableRate = totalAreaLiving.HasValue && totalAreaLiving.Value != 0
                                                     ? (needForFund / period) / totalAreaLiving.Value
                                                     : 0;
                            return new
                                {
                                    totalAreaLiving,
                                    needForFund,
                                    reasonableRate
                                };
                        })
                    .FirstOrDefault();

                var newRates = new List<RealEstateTypeRateProxy>();

                if (untypedRealtyObjectsData != null)
                {
                    var newRate = new RealEstateTypeRateProxy
                        {
                            RealEstateTypeId = null,
                            NeedForFunding = untypedRealtyObjectsData.needForFund,
                            ReasonableRate = untypedRealtyObjectsData.reasonableRate,
                            TotalArea = untypedRealtyObjectsData.totalAreaLiving
                        };
                    newRates.Add(newRate);
                }


                var typedData = robjSatisfyRealEstTypeConditionsDict
                    .SelectMany(x => x.Value
                        .Where(y => y.Value)
                        .Select(z => new
                            {
                                roId = x.Key, 
                                type = z.Key
                            })
                        .ToList())
                     .GroupBy(x => x.type)
                     .ToDictionary(
                        x => x.Key, 
                        x =>
                            {
                                var totalAreaLiving = x.Sum(y => realtyObjectAreaLivingDict[y.roId]);
                                var needForFund = x.Sum(y => needForFundDict.ContainsKey(y.roId) ? needForFundDict[y.roId] : 0);
                                var reasonableRate = totalAreaLiving.HasValue && totalAreaLiving.Value != 0
                                                         ? (needForFund / period) / totalAreaLiving.Value
                                                         : 0;

                                return new
                                    {
                                        RealitObjectIds = x.Select(y => y.roId).ToList(),
                                        totalAreaLiving,
                                        needForFund,
                                        reasonableRate
                                    };
                            });

                var realEstateTypeRealityObject = new Dictionary<long, List<long>>();
                typedData.ForEach(x =>
                    {
                        var newRate = new RealEstateTypeRateProxy
                        {
                            RealEstateTypeId = x.Key,
                            NeedForFunding = x.Value.needForFund,
                            ReasonableRate = x.Value.reasonableRate,
                            TotalArea = x.Value.totalAreaLiving
                        };
                        newRates.Add(newRate);

                        realEstateTypeRealityObject.Add(x.Key, new List<long>());
                        realEstateTypeRealityObject[x.Key].AddRange(x.Value.RealitObjectIds);
                    });

                UpdateRates(newRates, realEstateTypeRealityObject);

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }

        /// <summary>
        /// Проверка, что дом попадает в заданный тип  дома по конструктивным характеристикам (каждый из КХ типа дома должен быть в доме)
        /// </summary>
        private bool TestForStructElemSatisfy(List<StructuralElementExists> typeStructuralElementsList, List<long> robjStructuralElementsList)
        {
            

            var result = typeStructuralElementsList.Where(x => x.Exists)
                .Select(x => x.StructuralElementId)
                .All(robjStructuralElementsList.Contains);

            result = result
                     && typeStructuralElementsList.Where(x => !x.Exists)
                         .Select(x => x.StructuralElementId)
                         .All(x => !robjStructuralElementsList.Contains(x));

            return result;
        }

        /// <summary>
        /// Проверка параметров типа
        /// </summary>
        private bool TestForSatisfyParams(List<ParamConstraint> typeParamConstraints, Dictionary<string, ParamValue> valuesDict)
        {
            foreach (var paramConstraint in typeParamConstraints)
            {
                if (valuesDict.ContainsKey(paramConstraint.Code))
                {
                    if (!ValueSatisfiesParam(valuesDict[paramConstraint.Code], paramConstraint))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка параметра
        /// </summary>
        private bool ValueSatisfiesParam(ParamValue paramValue, ParamConstraint constraint)
        {
            var maxIsEmpty = string.IsNullOrEmpty(constraint.Max);
            var minIsEmpty = string.IsNullOrEmpty(constraint.Min);
            var result = true;

            if (maxIsEmpty && minIsEmpty)
            {
                return true;
            }

            if (paramValue.Value == null)
            {
                return false;
            }

            switch (paramValue.CommonParamType)
            {
                case CommonParamType.Date:
                    {
                        DateTime value;
                        if (!DateTime.TryParse(paramValue.Value.ToString(), out value))
                        {
                            return false;
                        }
                        
                        if (!maxIsEmpty)
                        {
                            if (constraint.Max.ToDateTime() < value)
                            {
                                return false;
                            }
                        }

                        if (!minIsEmpty)
                        {
                            result = constraint.Min.ToDateTime() <= value;
                        }

                        return result;
                    }

                case CommonParamType.Decimal:
                    {
                        decimal value;
                        if (!decimal.TryParse(paramValue.Value.ToString(), out value))
                        {
                            return false;
                        }

                        if (!maxIsEmpty)
                        {
                            if (constraint.Max.ToDecimal() < value)
                            {
                                return false;
                            }
                        }

                        if (!minIsEmpty)
                        {
                            result = constraint.Min.ToDecimal() <= value;
                        }

                        return result;
                    }

                case CommonParamType.Integer:
                    {
                        int value;
                        if (!int.TryParse(paramValue.Value.ToString(), out value))
                        {
                            return false;
                        }

                        if (!maxIsEmpty)
                        {
                            if (constraint.Max.ToInt() < value)
                            {
                                return false;
                            }
                        }

                        if (!minIsEmpty)
                        {
                            result = constraint.Min.ToInt() <= value;
                        }

                        return result;
                    }

                default:
                    return false;
            }
        }

        private void UpdateRates(List<RealEstateTypeRateProxy> newRates, Dictionary<long, List<long>> realEstateTypeRealityObject)
        {
            var serviceRealEstateTypeRate = Container.Resolve<IDomainService<RealEstateTypeRate>>();

            var realEstateTypeRates = serviceRealEstateTypeRate.GetAll().ToList();

            DeleteTypeRealityObject();

            var listRatesForSave = new List<RealEstateTypeRate>();
            var listEstateTypeRoForSave = new List<RealEstateTypeRealityObject>();

            foreach (var realEstateTypeRate in realEstateTypeRates)
            {
                RealEstateTypeRateProxy newRate;
                if (realEstateTypeRate.RealEstateType != null)
                {
                    newRate = newRates.FirstOrDefault(x => x.RealEstateTypeId == realEstateTypeRate.RealEstateType.Id);
                }
                else
                {
                    newRate = newRates.FirstOrDefault(x => x.RealEstateTypeId == null);
                }

                if (newRate == null)
                {
                    realEstateTypeRate.ReasonableRate = 0;
                    realEstateTypeRate.NeedForFunding = 0;
                    realEstateTypeRate.TotalArea = 0;
                    realEstateTypeRate.RateDeficit = 0;
                }
                else
                {
                    realEstateTypeRate.ReasonableRate = newRate.ReasonableRate;
                    realEstateTypeRate.NeedForFunding = newRate.NeedForFunding;
                    realEstateTypeRate.TotalArea = newRate.TotalArea;

                    if (realEstateTypeRate.SociallyAcceptableRate.HasValue)
                    {
                        realEstateTypeRate.RateDeficit = newRate.ReasonableRate - realEstateTypeRate.SociallyAcceptableRate.Value;
                    }
                    else
                    {
                        realEstateTypeRate.RateDeficit = null;
                    }

                    if (realEstateTypeRate.RealEstateType != null && realEstateTypeRealityObject.ContainsKey(realEstateTypeRate.RealEstateType.Id))
                    {
                        foreach (var realityObjectId in realEstateTypeRealityObject[realEstateTypeRate.RealEstateType.Id])
                        {
                            listEstateTypeRoForSave.Add(new RealEstateTypeRealityObject
                            {
                                RealEstateType = new RealEstateType { Id = realEstateTypeRate.RealEstateType.Id },
                                RealityObject = new RealityObject { Id = realityObjectId }
                            });
                        }
                    }
                }

                listRatesForSave.Add(realEstateTypeRate);
            }

            var repTypeRo = Container.Resolve<IRepository<RealEstateTypeRealityObject>>();
            var repRate = Container.Resolve<IRepository<RealEstateTypeRate>>();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listEstateTypeRoForSave.ForEach(repTypeRo.Save);

                    listRatesForSave.ForEach(repRate.Save);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void DeleteTypeRealityObject()
        {
            var session = Container.Resolve<ISessionProvider>()
                                   .GetCurrentSession();

            session.CreateSQLQuery("delete from OVRHL_REALESTATEREALITYO")
                   .ExecuteUpdate();
        }
    }
}