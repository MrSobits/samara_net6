namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для обработки данных по Ремонту дома и благоустройству территории, средний срок обслуживания МКД
    /// </summary>
    public class FinActivityRepairCategoryService : IFinActivityRepairCategoryService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<FinActivityRepairCategory>();
            var disclosureInfoDomain = Container.ResolveDomain<DisclosureInfo>();

            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<FinActivityRepairCategory>())
                    .ToList();

                var existingCategoryies = service.GetAll()
                        .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                        .AsEnumerable()
                        .GroupBy(x => x.TypeCategoryHouseDi)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var disclosureInfo = disclosureInfoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoId);

                foreach (var rec in records)
                {
                    FinActivityRepairCategory existingCategory = null;

                    if (existingCategoryies.ContainsKey(rec.TypeCategoryHouseDi))
                        existingCategory = existingCategoryies[rec.TypeCategoryHouseDi];

                    if (existingCategory != null)
                    {
                        existingCategory.WorkByRepair = rec.WorkByRepair;
                        existingCategory.WorkByBeautification = rec.WorkByBeautification;
                        existingCategory.PeriodService = rec.PeriodService;

                        service.Update(existingCategory);
                    }
                    else
                    {
                        var newFinActivityRepairCategory = new FinActivityRepairCategory
                        {
                            DisclosureInfo = disclosureInfo,
                            TypeCategoryHouseDi = rec.TypeCategoryHouseDi,
                            WorkByRepair = rec.WorkByRepair,
                            WorkByBeautification = rec.WorkByBeautification,
                            PeriodService = rec.PeriodService
                        };

                        service.Save(newFinActivityRepairCategory);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(service);
                Container.Release(disclosureInfoDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult AddDataByRealityObj(BaseParams baseParams)
        {
            var discolsureInfoDomain = this.Container.ResolveDomain<DisclosureInfo>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var service = this.Container.Resolve<IDomainService<FinActivityRepairCategory>>();

            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var disclosureInfo = discolsureInfoDomain.Get(disclosureInfoId);

                var disclosureInfoRealityObjList = disclosureInfoRelationDomain
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.DisclosureInfoRealityObj)
                    .ToList();


                var to25 =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.To25) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.To25,
                        DisclosureInfo = disclosureInfo
                    };
                var from26To50 =
                    service.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.From26To50) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.From26To50,
                        DisclosureInfo = disclosureInfo
                    };
                var from51To75 =
                    service.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.From51To75) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.From51To75,
                        DisclosureInfo = disclosureInfo
                    };
                var from76 =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.From76) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.From76,
                        DisclosureInfo = disclosureInfo
                    };
                var crashHouse =
                    service.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.CrashHouse) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.CrashHouse,
                        DisclosureInfo = disclosureInfo
                    };

                var summary =
                    service.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury) ??
                    new FinActivityRepairCategory
                    {
                        Id = 0,
                        TypeCategoryHouseDi = TypeCategoryHouseDi.Summury,
                        DisclosureInfo = disclosureInfo
                    };

                var firstIterationTo25Work = true;
                var firstIterationTo25Land = true;
                var firstIterationFrom26To50Work = true;
                var firstIterationFrom26To50Land = true;
                var firstIterationFrom51To75Work = true;
                var firstIterationFrom51To75Land = true;
                var firstIterationFrom76Work = true;
                var firstIterationFrom76Land = true;
                var firstIterationCrashHouseWork = true;
                var firstIterationCrashHouseLand = true;

                foreach (var disclosureInfoRealityObj in disclosureInfoRealityObjList)
                {
                    if (disclosureInfoRealityObj.RealityObject.DateCommissioning.HasValue
                        && disclosureInfoRealityObj.RealityObject.ConditionHouse != ConditionHouse.Emergency)
                    {
                        // Получаем разницу в полных годах
                        var deltaYear = disclosureInfoRealityObj.PeriodDi.DateEnd.Value.Year
                            - disclosureInfoRealityObj.RealityObject.DateCommissioning.Value.Year;
                        if (disclosureInfoRealityObj.PeriodDi.DateEnd.Value.Month < disclosureInfoRealityObj.RealityObject.DateCommissioning.Value.Month
                            || (disclosureInfoRealityObj.PeriodDi.DateEnd.Value.Month
                                >= disclosureInfoRealityObj.RealityObject.DateCommissioning.Value.Month
                                && disclosureInfoRealityObj.PeriodDi.DateEnd.Value.Day < disclosureInfoRealityObj.RealityObject.DateCommissioning.Value.Day))
                        {
                            deltaYear--;
                        }

                        if (deltaYear <= 25)
                        {
                            if (disclosureInfoRealityObj.WorkRepair.HasValue)
                            {
                                if (firstIterationTo25Work)
                                {
                                    to25.WorkByRepair = 0m;
                                    firstIterationTo25Work = false;
                                }

                                to25.WorkByRepair += disclosureInfoRealityObj.WorkRepair.Value;
                            }

                            if (disclosureInfoRealityObj.WorkLandscaping.HasValue)
                            {
                                if (firstIterationTo25Land)
                                {
                                    to25.WorkByBeautification = 0m;
                                    firstIterationTo25Land = false;
                                }

                                to25.WorkByBeautification += disclosureInfoRealityObj.WorkLandscaping.Value;
                            }
                        }

                        if (deltaYear > 25 && deltaYear <= 50)
                        {
                            if (disclosureInfoRealityObj.WorkRepair.HasValue)
                            {
                                if (firstIterationFrom26To50Work)
                                {
                                    from26To50.WorkByRepair = 0m;
                                    firstIterationFrom26To50Work = false;
                                }

                                from26To50.WorkByRepair += disclosureInfoRealityObj.WorkRepair.Value;
                            }

                            if (disclosureInfoRealityObj.WorkLandscaping.HasValue)
                            {
                                if (firstIterationFrom26To50Land)
                                {
                                    from26To50.WorkByBeautification = 0m;
                                    firstIterationFrom26To50Land = false;
                                }

                                from26To50.WorkByBeautification += disclosureInfoRealityObj.WorkLandscaping.Value;
                            }
                        }

                        if (deltaYear > 50 && deltaYear <= 75)
                        {
                            if (disclosureInfoRealityObj.WorkRepair.HasValue)
                            {
                                if (firstIterationFrom51To75Work)
                                {
                                    from51To75.WorkByRepair = 0m;
                                    firstIterationFrom51To75Work = false;
                                }

                                from51To75.WorkByRepair += disclosureInfoRealityObj.WorkRepair.Value;
                            }

                            if (disclosureInfoRealityObj.WorkLandscaping.HasValue)
                            {
                                if (firstIterationFrom51To75Land)
                                {
                                    from51To75.WorkByBeautification = 0m;
                                    firstIterationFrom51To75Land = false;
                                }

                                from51To75.WorkByBeautification += disclosureInfoRealityObj.WorkLandscaping.Value;
                            }
                        }

                        if (deltaYear > 75)
                        {
                            if (disclosureInfoRealityObj.WorkRepair.HasValue)
                            {
                                if (firstIterationFrom76Work)
                                {
                                    from76.WorkByRepair = 0m;
                                    firstIterationFrom76Work = false;
                                }

                                from76.WorkByRepair += disclosureInfoRealityObj.WorkRepair.Value;
                            }

                            if (disclosureInfoRealityObj.WorkLandscaping.HasValue)
                            {
                                if (firstIterationFrom76Land)
                                {
                                    from76.WorkByBeautification = 0m;
                                    firstIterationFrom76Land = false;
                                }

                                from76.WorkByBeautification += disclosureInfoRealityObj.WorkLandscaping.Value;
                            }
                        }
                    }
                    else if (disclosureInfoRealityObj.RealityObject.ConditionHouse == ConditionHouse.Emergency)
                    {
                        if (disclosureInfoRealityObj.WorkRepair.HasValue)
                        {
                            if (firstIterationCrashHouseWork)
                            {
                                crashHouse.WorkByRepair = 0m;
                                firstIterationCrashHouseWork = false;
                            }

                            crashHouse.WorkByRepair += disclosureInfoRealityObj.WorkRepair.Value;
                        }

                        if (disclosureInfoRealityObj.WorkLandscaping.HasValue)
                        {
                            if (firstIterationCrashHouseLand)
                            {
                                crashHouse.WorkByBeautification = 0m;
                                firstIterationCrashHouseLand = false;
                            }

                            crashHouse.WorkByBeautification += disclosureInfoRealityObj.WorkLandscaping.Value;
                        }
                    }
                }

                var finActivityRepairCategoryList = new List<FinActivityRepairCategory>
                {
                    to25,
                    from26To50,
                    from51To75,
                    from76,
                    crashHouse
                };

                var sumElements = finActivityRepairCategoryList.Where(x => x.WorkByBeautification.HasValue);
                summary.WorkByBeautification = sumElements.Any() ? sumElements.Sum(x => x.WorkByBeautification) : null;

                sumElements = finActivityRepairCategoryList.Where(x => x.WorkByRepair.HasValue);
                summary.WorkByRepair = sumElements.Any() ? sumElements.Sum(x => x.WorkByRepair) : null;
                finActivityRepairCategoryList.Add(summary);

                sumElements = finActivityRepairCategoryList.Where(x => x.PeriodService.HasValue);
                summary.PeriodService = sumElements.Any() ? sumElements.Average(x => x.PeriodService).Return(x => (int?)Math.Round(x.Value)) : null;

                foreach (var finActivityRepairCategory in finActivityRepairCategoryList)
                {
                    if (finActivityRepairCategory.WorkByRepair.HasValue || finActivityRepairCategory.WorkByBeautification.HasValue)
                    {
                        if (finActivityRepairCategory.Id == 0)
                        {
                            service.Save(finActivityRepairCategory);
                        }
                        else
                        {
                            service.Update(finActivityRepairCategory);
                        }
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(discolsureInfoDomain);
                this.Container.Release(disclosureInfoRelationDomain);
            }
        }
    }
}
