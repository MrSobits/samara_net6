namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;
    using Bars.Gkh.Utils;
    using NHibernate.Linq;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using B4;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Entities;

    using Castle.Windsor;
    using Entities.Version;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities.CommonEstateObject;
    using GkhCr.Enums;
    using Overhaul.DomainService;
    using Version;
    using Config;

    using GkhCr.Entities;
    using Bars.Gkh.Overhaul.Hmao.Reports;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.MicroKernel;

    using Castle.MicroKernel;

    using Npgsql;

    /// <summary>
    /// Сервис для актуализации версии
    /// </summary>
    public partial class ActualizeVersionService : IActualizeVersionService
    {
        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Параметры ЖКХ
        /// </summary>
        public IGkhParams GkhParams { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public IUserIdentity User { get; set; }

        /// <summary>
        /// Лог сервис
        /// </summary>
        public IActualizeVersionLogService<ActualizeVersionLogRecord, ActualizeVersionLogReport> LogService { get; set; }

        /// <summary>
        /// Домен для Журнала домена 
        /// </summary>
        public IDomainService<VersionActualizeLog> LogDomain { get; set; }

        private WorkpriceMoLevel? workpriceMoLevel = null;

        /// <summary>
        /// Уровень муниципального образования
        /// </summary>
        public WorkpriceMoLevel? WorkpriceMoLevel =>
            (this.workpriceMoLevel.HasValue ? this.workpriceMoLevel.Value : (this.workpriceMoLevel =
                (WorkpriceMoLevel)this.GkhParams.GetParams().GetAs<int>("WorkPriceMoLevel")).Value);
        
        public void CalculateYears(ProgramVersion version, int calcfromyear, int calctoyear, int prsc)
        {
            Dictionary<long, int> weightDict = new Dictionary<long, int>();
            var ceoDomain = Container.ResolveDomain<CommonEstateObject>();
            var structElRoDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var subsidyRecordVersionDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();
            var currYear = calcfromyear - 1;
            decimal coeff = 1;
            if (prsc > 0)
            {
                coeff = 1 + Convert.ToDecimal(prsc) / 100m;
            }
            weightDict = ceoDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Weight = x.Weight > 0 ? x.Weight : 1
                }).AsEnumerable().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.First().Weight);
            var verSt1Query = versSt1Domain.GetAll()
                 .Where(x => x.VersionRecordState != VersionRecordState.NonActual)
                 .Fetch(r => r.StructuralElement)
                 .ThenFetch(e => e.StructuralElement)
                 .ThenFetch(f => f.Group)
                 .Where(x => x.Stage2Version.Stage3Version.ProgramVersion == version && x.Stage2Version.Stage3Version.Year > currYear && x.Stage2Version.Stage3Version.Show && !x.Stage2Version.Stage3Version.FixedYear);

            var subsidyRecDist = subsidyRecordVersionDomain.GetAll().Where(x => x.Version == version)
                .Select(x => new
                {
                    x.SubsidyYear,
                    Record = x
                }).OrderBy(x => x.SubsidyYear).AsEnumerable().GroupBy(x => x.SubsidyYear).ToDictionary(x => x.Key, y => y.First().Record);

            var weroutsByStel = verSt1Query.Select(x => new
            {
                x.StructuralElement.Id,
                x.StructuralElement.WearoutActual,
                x.Stage2Version.Stage3Version.Year,
                CommonEstateObjectId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                x.Stage2Version.Stage3Version.Sum,
                RecId = x.Stage2Version.Stage3Version.Id,
                x.Stage2Version.Stage3Version.CommonEstateObjects,
                RealityObject = x.Stage2Version.Stage3Version.RealityObject.Id,
                BuildYear = x.Stage2Version.Stage3Version.RealityObject.BuildYear
            }).OrderBy(x => x.Year).AsEnumerable();

            List<long> stelList = new List<long>();
            Dictionary<long, decimal> weroutDict = new Dictionary<long, decimal>();

            foreach (var rec in weroutsByStel)
            {
                if (!stelList.Contains(rec.Id))
                {
                    stelList.Add(rec.Id);
                    if (!weroutDict.ContainsKey(rec.RecId))
                        weroutDict.Add(rec.RecId, rec.WearoutActual);
                    else
                    {
                        if (weroutDict[rec.RecId] < rec.WearoutActual)
                        {
                            weroutDict[rec.RecId] = rec.WearoutActual;
                        }
                    }
                }
                else
                {
                    if (!weroutDict.ContainsKey(rec.RecId))
                        weroutDict.Add(rec.RecId, 0m);
                }
            }

            var worksList = weroutsByStel.Select(x => new
            {
                x.Id,
                x.WearoutActual,
                x.Year,
                x.CommonEstateObjectId,
                x.Sum,
                x.RecId,
                x.CommonEstateObjects,
                x.RealityObject,
                Point = (weightDict[x.CommonEstateObjectId] * (weroutDict.ContainsKey(x.RecId) ? weroutDict[x.RecId] : x.WearoutActual)) / (x.BuildYear.HasValue ? x.BuildYear.Value : x.Year)
            }).OrderByDescending(x => x.Point).AsEnumerable();
            // var surrentSaldo = subsidyRecDist[calcfromyear - 1].SaldoBallance;
            decimal surrentSaldo = 0;
            List<long> recs = new List<long>();
            foreach (KeyValuePair<int, SubsidyRecordVersion> key in subsidyRecDist)
            {
                if (key.Key >= calcfromyear && key.Key <= calctoyear)
                {
                    surrentSaldo = surrentSaldo + key.Value.BudgetCr * coeff - key.Value.AdditionalExpences;
                    var worksum = versSt3Domain.GetAll().Where(x => x.ProgramVersion == version && x.Year == key.Key && x.FixedYear && x.Show)
                        .SafeSum(x => x.Sum);
                    surrentSaldo -= worksum;
                    var recentWorks = worksList.Where(x => !recs.Contains(x.RecId)).OrderByDescending(x => x.Point);
                    if (surrentSaldo > 0)
                        foreach (var rec in recentWorks)
                        {

                            if (surrentSaldo > 0 && surrentSaldo > rec.Sum)
                            {
                                if (!recs.Contains(rec.RecId))
                                {
                                    var vr = versSt3Domain.Get(rec.RecId);
                                    if (vr != null)
                                    {
                                        vr.Point = rec.Point;
                                        vr.YearCalculated = key.Key;
                                        vr.Remark = $"Износ {rec.WearoutActual}, Балл {rec.Point}";
                                        if (surrentSaldo - vr.Sum < 0)
                                        {

                                        }
                                        surrentSaldo -= vr.Sum;
                                        versSt3Domain.Update(vr);
                                    }
                                    recs.Add(rec.RecId);
                                }
                            }
                            else
                                break;


                        }
                }
            }

            var lastyear = subsidyRecDist.Max(x => x.Key);
            var lastWorks = worksList.Where(x => !recs.Contains(x.RecId)).OrderByDescending(x => x.Point);
            if (lastyear >= calctoyear)
            {
                lastyear = calctoyear + 1;
            }

            foreach (var rec in lastWorks)
            {
                if (!recs.Contains(rec.RecId))
                {
                    var vr = versSt3Domain.Get(rec.RecId);
                    if (vr != null)
                    {
                        vr.Point = rec.Point;
                        vr.YearCalculated = lastyear;
                        vr.Remark = $"Износ {rec.WearoutActual}, Балл {rec.Point}. Перенесено на последний год.";
                        versSt3Domain.Update(vr);
                    }
                    recs.Add(rec.RecId);
                }

            }
        }

         public void Calculate12Stage(ProgramVersion version, int calcfromyear, int calctoyear, int stage)
        {
            var pointDict = new Dictionary<long, decimal>();
            var ceoDomain = Container.ResolveDomain<CommonEstateObject>();
            var structElRoDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var subsidyRecordVersionDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();
            var criteriasDomain = this.Container.ResolveDomain<CriteriaForActualizeVersion>();
            
            var roChargeAccDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var currYear = calcfromyear - 1;

            var stage1 = versSt1Domain.GetAll()
                 .Where(x => x.VersionRecordState != VersionRecordState.NonActual)
                 .Fetch(r => r.StructuralElement)
                 .ThenFetch(e => e.StructuralElement)
                 .ThenFetch(f => f.Group)
                 .Where(x => x.Stage2Version.Stage3Version.ProgramVersion == version
                          && x.Stage2Version.Stage3Version.Year >= calcfromyear
                          && x.Stage2Version.Stage3Version.Year <= calctoyear
                          && x.Stage2Version.Stage3Version.Show
                          && !x.Stage2Version.Stage3Version.FixedYear
                          && x.Stage2Version.Stage3Version.RealityObject.AccountFormationVariant == CrFundFormationType.RegOpAccount);

            var subsidyRecDict = subsidyRecordVersionDomain.GetAll()
                .Where(x => x.Version == version)
                .Where(x => x.SubsidyYear >= calcfromyear && x.SubsidyYear <= calctoyear)
                .Select(x => new
                {
                    x.SubsidyYear,
                    Record = x
                })
                .OrderBy(x => x.SubsidyYear)
                .GroupBy(x => x.SubsidyYear)
                .ToDictionary(x => x.Key, y => y.First().Record);

            var necessaryData = stage1
                .Select(x => new
                {
                    x.StructuralElement,
                    x.StructuralElement.Wearout,
                    x.Stage2Version.Stage3Version.Sum,
                    x.Stage2Version.Stage3Version.RealityObject,
                    Stage3Id = x.Stage2Version.Stage3Version.Id,
                    x.StructuralElement.LastOverhaulYear,
                    BuildYear = x.Stage2Version.Stage3Version.RealityObject.DateCommissioning.HasValue
                                ? x.Stage2Version.Stage3Version.RealityObject.DateCommissioning.Value.Year
                                : x.Stage2Version.Stage3Version.RealityObject.BuildYear ?? 1999
                })
                .Select(x => new
                {
                    x.StructuralElement,
                    x.Wearout,
                    x.Sum,
                    x.RealityObject,
                    x.Stage3Id,
                    x.LastOverhaulYear,
                    x.BuildYear,
                    NotBeforeYear = x.LastOverhaulYear > x.BuildYear
                                    ? x.LastOverhaulYear + 30 > calctoyear
                                        ? calctoyear
                                        : x.LastOverhaulYear + 30
                                    : x.BuildYear + 30 > calctoyear
                                        ? calctoyear
                                        : x.BuildYear + 30
                })
                .AsEnumerable();

            foreach (var rec in necessaryData)
            {
                var compareYear = rec.BuildYear;
                if (rec.LastOverhaulYear > rec.BuildYear && rec.LastOverhaulYear < 2014)
                {
                    compareYear = rec.LastOverhaulYear;
                }

                var lifetimeCriteria = criteriasDomain.GetAll()
                    .Where(x => x.CriteriaType == CriteriaType.Lifetime)
                    .Where(x => x.ValueFrom <= compareYear && x.ValueTo >= compareYear)
                    .FirstOrDefault();

                var weroutCriteria = criteriasDomain.GetAll()
                    .Where(x => x.CriteriaType == CriteriaType.Werout)
                    .Where(x => x.ValueFrom <= Math.Ceiling(rec.Wearout) && x.ValueTo >= Math.Ceiling(rec.Wearout))
                    .FirstOrDefault();

                var receivedCriteriaPoint = 0;
                var receivedCriteriaWeight = 0m;

                this.Container.UsingForResolved<IDomainService<RealityObjectChargeAccount>>((container, domainService) =>
                {
                    var entity = domainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == rec.RealityObject.Id);

                    if (entity != null)
                    {
                        var chargedTotal = entity.Operations.SafeSum(y => y.ChargedTotal);
                        var receivedPercentage = chargedTotal == 0 ? 0 : entity.PaidTotal / chargedTotal * 100;

                        var receivedCriteria = criteriasDomain.GetAll()
                            .Where(x => x.CriteriaType == CriteriaType.ShareReceived)
                            .Where(x => x.ValueFrom <= Math.Ceiling(receivedPercentage) && x.ValueTo >= Math.Ceiling(receivedPercentage))
                            .FirstOrDefault();

                        receivedCriteriaPoint = receivedCriteria.Points;
                        receivedCriteriaWeight = receivedCriteria.Weight;
                    }
                });

                var totalPoint = lifetimeCriteria.Points * lifetimeCriteria.Weight + weroutCriteria.Points * weroutCriteria.Weight + receivedCriteriaPoint * receivedCriteriaWeight;
                if (!pointDict.Keys.Contains(rec.Stage3Id))
                {
                    pointDict.Add(rec.Stage3Id, totalPoint);
                }
            }

            var worksList = necessaryData
                .Select(x => new
                {
                    x.StructuralElement,
                    x.RealityObject,
                    x.Wearout,
                    x.Sum,
                    x.Stage3Id,
                    x.NotBeforeYear,
                    Point = pointDict.ContainsKey(x.Stage3Id) ? pointDict[x.Stage3Id] :0
                })
                .OrderByDescending(x => x.Point)
                .AsEnumerable();

            decimal currentSaldo = 0;

            var recs = new List<long>();

            foreach (var subsidy in subsidyRecDict)
            {
                currentSaldo = currentSaldo + subsidy.Value.BudgetCr - subsidy.Value.AdditionalExpences;

                var onFixedYearWorks = versSt3Domain.GetAll()
                    .Where(x => x.ProgramVersion == version && x.Year == subsidy.Key && x.FixedYear && x.Show);

                currentSaldo -= onFixedYearWorks.SafeSum(x => x.Sum);

                foreach (var work in onFixedYearWorks)
                {
                    work.YearCalculated = subsidy.Key;
                    work.Remark = $"Год закреплен пользователем";
                    versSt3Domain.Update(work);
                }

                var recentWorks = worksList.Where(x => !recs.Contains(x.Stage3Id));

                foreach (var rec in recentWorks)
                {
                    if (currentSaldo > 0 && subsidy.Key >= rec.NotBeforeYear)
                    {
                        if (!recs.Contains(rec.Stage3Id))
                        {
                            longProgramService.GetDpkrCost(
                                rec.RealityObject.Municipality.Id,
                                0,
                                subsidy.Key,
                                rec.StructuralElement.StructuralElement.Id,
                                rec.RealityObject.Id,
                                rec.RealityObject.CapitalGroup != null ? rec.RealityObject.CapitalGroup.Id : 0,
                                rec.StructuralElement.StructuralElement.CalculateBy,
                                rec.StructuralElement.Volume,
                                rec.RealityObject.AreaLiving,
                                rec.RealityObject.AreaMkd,
                                rec.RealityObject.AreaLivingNotLivingMkd);

                            var vr = versSt3Domain.Get(rec.Stage3Id);
                            if (vr != null)
                            {
                                vr.Point = rec.Point;
                                vr.YearCalculated = subsidy.Key;
                                vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}";
                                currentSaldo -= vr.Sum;
                                versSt3Domain.Update(vr);
                            }
                            recs.Add(rec.Stage3Id);
                        }
                    }
                    else
                    {
                        break;
                    } 
                }
            }

            var lastWorks = worksList
                .Where(x => !recs.Contains(x.Stage3Id));

            if (stage == 1)
            {
                foreach (var rec in lastWorks)
                {
                    if (!recs.Contains(rec.Stage3Id))
                    {
                        var vr = versSt3Domain.Get(rec.Stage3Id);
                        if (vr != null)
                        {
                            vr.Point = rec.Point;
                            vr.YearCalculated = calctoyear;
                            vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}. Перенесено на следующий этап.";
                            versSt3Domain.Update(vr);
                        }
                        recs.Add(rec.Stage3Id);
                    }
                }
            }
            else if (stage == 2)
            {
                foreach (var rec in lastWorks)
                {
                    if (!recs.Contains(rec.Stage3Id))
                    {
                        if (rec.NotBeforeYear > calctoyear + 1)
                        {
                            var vr = versSt3Domain.Get(rec.Stage3Id);
                            if (vr != null)
                            {
                                vr.Point = rec.Point;
                                vr.YearCalculated = rec.NotBeforeYear;
                                vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}. Перенесено на следующий этап.";
                                versSt3Domain.Update(vr);
                            }
                            recs.Add(rec.Stage3Id);
                        }
                        else
                        {
                            var vr = versSt3Domain.Get(rec.Stage3Id);   
                            if (vr != null)
                            {
                                vr.Point = rec.Point;
                                vr.YearCalculated = calctoyear + 1;
                                vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}. Перенесено на следующий этап.";
                                versSt3Domain.Update(vr);
                            }
                            recs.Add(rec.Stage3Id);
                        }
                    }
                }
            }            
        }
         
          public void Calculate3Stage(ProgramVersion version, int calcfromyear, int calctoyear)
        {
            var pointDict = new Dictionary<long, decimal>();
            var ceoDomain = Container.ResolveDomain<CommonEstateObject>();
            var structElRoDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var subsidyRecordVersionDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();
            var criteriasDomain = this.Container.ResolveDomain<CriteriaForActualizeVersion>();
            var roChargeAccDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var currYear = calcfromyear - 1;

            var stage1 = versSt1Domain.GetAll()
                 .Where(x => x.VersionRecordState != VersionRecordState.NonActual)
                 .Fetch(r => r.StructuralElement)
                 .ThenFetch(e => e.StructuralElement)
                 .ThenFetch(f => f.Group)
                 .Where(x => x.Stage2Version.Stage3Version.ProgramVersion == version
                          && x.Stage2Version.Stage3Version.Year >= calcfromyear
                          && x.Stage2Version.Stage3Version.Year <= calctoyear
                          && x.Stage2Version.Stage3Version.Show
                          && !x.Stage2Version.Stage3Version.FixedYear
                          && x.Stage2Version.Stage3Version.RealityObject.AccountFormationVariant == CrFundFormationType.RegOpAccount);

            var necessaryData = stage1
                .Select(x => new
                {
                    x.StructuralElement,
                    x.StructuralElement.Wearout,
                    x.Stage2Version.Stage3Version.Sum,
                    x.Stage2Version.Stage3Version.RealityObject,
                    Stage3Id = x.Stage2Version.Stage3Version.Id,
                    x.StructuralElement.LastOverhaulYear,
                    BuildYear = x.Stage2Version.Stage3Version.RealityObject.DateCommissioning.HasValue
                                ? x.Stage2Version.Stage3Version.RealityObject.DateCommissioning.Value.Year
                                : x.Stage2Version.Stage3Version.RealityObject.BuildYear ?? 1999
                })
                .Select(x => new
                {
                    x.StructuralElement,
                    x.Wearout,
                    x.Sum,
                    x.RealityObject,
                    x.Stage3Id,
                    x.LastOverhaulYear,
                    x.BuildYear,
                    NotBeforeYear = x.LastOverhaulYear > x.BuildYear
                                    ? x.LastOverhaulYear + 30 > calctoyear
                                        ? calctoyear
                                        : x.LastOverhaulYear + 30
                                    : x.BuildYear + 30 > calctoyear
                                        ? calctoyear
                                        : x.BuildYear + 30
                })
                .AsEnumerable();

            foreach (var rec in necessaryData)
            {
                var compareYear = rec.BuildYear;
                if (rec.LastOverhaulYear > rec.BuildYear && rec.LastOverhaulYear < 2014)
                {
                    compareYear = rec.LastOverhaulYear;
                }

                var lifetimeCriteria = criteriasDomain.GetAll()
                    .Where(x => x.CriteriaType == CriteriaType.Lifetime)
                    .Where(x => x.ValueFrom <= compareYear && x.ValueTo >= compareYear)
                    .FirstOrDefault();

                var weroutCriteria = criteriasDomain.GetAll()
                    .Where(x => x.CriteriaType == CriteriaType.Werout)
                    .Where(x => x.ValueFrom <= Math.Ceiling(rec.Wearout) && x.ValueTo >= Math.Ceiling(rec.Wearout))
                    .FirstOrDefault();

                var receivedCriteriaPoint = 0;
                var receivedCriteriaWeight = 0m;

                this.Container.UsingForResolved<IDomainService<RealityObjectChargeAccount>>((container, domainService) =>
                {
                    var entity = domainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == rec.RealityObject.Id);

                    if (entity != null)
                    {
                        var chargedTotal = entity.Operations.SafeSum(y => y.ChargedTotal);
                        var receivedPercentage = chargedTotal == 0 ? 0 : entity.PaidTotal / chargedTotal * 100;

                        var receivedCriteria = criteriasDomain.GetAll()
                            .Where(x => x.CriteriaType == CriteriaType.ShareReceived)
                            .Where(x => x.ValueFrom <= Math.Ceiling(receivedPercentage) && x.ValueTo >= Math.Ceiling(receivedPercentage))
                            .FirstOrDefault();

                        receivedCriteriaPoint = receivedCriteria.Points;
                        receivedCriteriaWeight = receivedCriteria.Weight;
                    }
                });

                var totalPoint = lifetimeCriteria.Points * lifetimeCriteria.Weight + weroutCriteria.Points * weroutCriteria.Weight + receivedCriteriaPoint * receivedCriteriaWeight;
                if (!pointDict.Keys.Contains(rec.Stage3Id))
                {
                    pointDict.Add(rec.Stage3Id, totalPoint);
                }
            }

            var worksList = necessaryData
                .Select(x => new
                {
                    x.StructuralElement,
                    x.RealityObject,
                    x.Wearout,
                    x.Sum,
                    x.Stage3Id,
                    x.NotBeforeYear,
                    Point = pointDict.ContainsKey(x.Stage3Id) ? pointDict[x.Stage3Id] : 0
                })
                .OrderByDescending(x => x.Point)
                .AsEnumerable();

            var worksCount = worksList.Select(x => x.Stage3Id).Distinct().Count();

            var worksPerYear = (int)Math.Ceiling((decimal)worksCount / ((decimal)calctoyear - (decimal)calcfromyear));

            var recs = new List<long>();

            for (var i = calcfromyear; i <= calctoyear; i++)
            {
                var countInYear = 0;

                while (countInYear < worksPerYear)
                {
                    foreach (var rec in worksList)
                    {
                        if (!recs.Contains(rec.Stage3Id))
                        {
                            if (rec.NotBeforeYear <= i)
                            {
                                var vr = versSt3Domain.Get(rec.Stage3Id);
                                if (vr != null)
                                {
                                    vr.Point = rec.Point;
                                    vr.YearCalculated = i;
                                    vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}";
                                    versSt3Domain.Update(vr);
                                }
                                recs.Add(rec.Stage3Id);
                                countInYear++;
                            }
                        }
                    }
                }
            }

            var lastWorks = worksList.Where(x => !recs.Contains(x.Stage3Id)).OrderByDescending(x => x.Point);

            foreach (var rec in lastWorks)
            {
                if (!recs.Contains(rec.Stage3Id))
                {
                    var vr = versSt3Domain.Get(rec.Stage3Id);
                    if (vr != null)
                    {
                        vr.Point = rec.Point;
                        vr.YearCalculated = calctoyear + 1;
                        vr.Remark = $"Износ {rec.Wearout}, Балл {rec.Point}. Перенесено на следующий этап.";
                        versSt3Domain.Update(vr);
                    }
                    recs.Add(rec.Stage3Id);
                }

            }
        }
        
        /// <summary>
        /// Актуализация добавления новых записей
        /// </summary>
        /// <param name="baseParams"> Базовые параметры </param>
        /// <returns> IDataResult </returns>
        public IDataResult AddNewRecords(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versionActualizeLogDomain = this.Container.ResolveDomain<VersionActualizeLog>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var versionId = baseParams.Params.GetAs<long>("versionId");
                var version = programVersionDomain.Get(versionId);
                var yearStart = baseParams.Params.GetAs("yearStart", 0);

                baseParams.Params.Add("needGenerateWorkCode", true);

                List<VersionRecord> ver3S;
                List<VersionRecordStage2> ver2S;
                List<VersionRecordStage1> ver1S;
                var dataResult = this.GetNewRecords(baseParams, out ver3S, out ver2S, out ver1S);

                if (!ver3S.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var volumeDict =
                    ver1S.Select(x => new { x.Stage2Version.Stage3Version, x.Volume })
                         .GroupBy(x => x.Stage3Version)
                         .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume));

                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ActualizeNewRecords,
                    DateAction = DateTime.Now,
                    Municipality = version.Municipality,
                    InputParams = yearStart.ToString(),
                    ProgramVersion = new ProgramVersion { Id = versionId },
                    UserName = this.User.Name
                };

                this.Container.InTransaction(
                    () =>
                        {
                            var ver3SFiltered = ver3S
                                // записываем в лог только те записи из третьей стадии, которые были изменены для второй стадии
                                .Where(st3Elem => ver2S.Select(st2Elem => st2Elem.Stage3Version.Id).Contains(st3Elem.Id));

                            var logRecords = ver3SFiltered
                                .Select(x => new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeNewRecords,
                                    Action = "Добавление",
                                    Description = "Удовлетворяет условиям ДПКР",
                                    Address = x.RealityObject.Address,
                                    Ceo = x.CommonEstateObjects,
                                    PlanYear = x.Year,
                                    Number = x.IndexNumber,
                                    Volume = volumeDict.ContainsKey(x) ? volumeDict[x] : 0m,
                                    Sum = x.Sum
                                }).ToArray();

                            var vaLogRecords = ver3SFiltered
                                .Select(x => new VersionActualizeLogRecord
                                {
                                    ActualizeLog = log,
                                    Action = "Добавление",
                                    RealityObject = x.RealityObject,
                                    WorkCode = x.WorkCode,
                                    Ceo = x.CommonEstateObjects,
                                    PlanYear = x.Year,
                                    Volume = volumeDict.ContainsKey(x) ? volumeDict[x] : 0m,
                                    Sum = x.Sum,
                                    Number = x.IndexNumber
                                });

                            TransactionHelper.InsertInManyTransactions(this.Container, ver3S, useStatelessSession: true);
                            TransactionHelper.InsertInManyTransactions(this.Container, ver2S, useStatelessSession: true);
                            TransactionHelper.InsertInManyTransactions(this.Container, ver1S, useStatelessSession: true);

                            version.ActualizeDate = DateTime.Now;
                            programVersionDomain.Update(version);

                            if (logRecords.Length > 0)
                            {
                                log.CountActions = logRecords.Length;
                                log.LogFile =
                                    this.LogService.CreateLogFile(
                                    logRecords.OrderBy(x => x.Address).ThenBy(x => x.Number),
                                        baseParams);
                            }
                            else
                            {
                                dataResult = new BaseDataResult(
                                    false,
                                    "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                            }

                            versionActualizeLogDomain.Save(log);
                            vaLogRecords.ForEach(x => versionActualizeLogRecordDomain.Save(x));
                        });

                return dataResult;
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(versionActualizeLogDomain);
                this.Container.Release(versionActualizeLogRecordDomain);
                this.Container.Release(roDomain);
            }
        }

        /// <summary>
        /// Актуализация суммы
        /// </summary>
        /// <param name="baseParams"> Базовые параметры </param>
        /// <returns> IDataResult </returns>
        public IDataResult ActualizeSum(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var workPriceDomain = this.Container.ResolveDomain<HmaoWorkPrice>();
            var structElementWorkDomain = this.Container.ResolveDomain<StructuralElementWork>();
            var realEstService = this.Container.Resolve<IRealEstateTypeService>();
            var correctDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            using (this.Container.Using(programVersionDomain, versSt1Domain, versSt2Domain, versSt3Domain, sessionProvider,
                workPriceDomain, structElementWorkDomain, realEstService, versionActualizeLogRecordDomain, publishedProgramRecordDomain))
            {
                var version = programVersionDomain.Get(versionId);
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var periodStartYear = config.ProgrammPeriodStart;
                var isWorkPriceFirstYear = config.WorkPriceCalcYear == WorkPriceCalcYear.First;
                var workPriceDetermineType = config.WorkPriceDetermineType;
                var servicePercent = config.ServiceCost;
                var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
                var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);
                var selectUsingFilters = baseParams.Params.GetAs("selectUsingFilters", true);

                var st3Ids = selectUsingFilters
                    ? this.GetActualizeSumEntriesQueryable(baseParams).Select(x => x.Id).ToArray()
                    : baseParams.Params.GetAs<long[]>("st3Ids");

                var verSt1Query = versSt1Domain.GetAll()
                    .Fetch(r => r.StructuralElement)
                    .ThenFetch(e => e.StructuralElement)
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x =>
                            (correctDomain.GetAll()
                                .Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart && y.PlanYear <= actualizeEnd))
                            || (!correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id)
                            && x.Stage2Version.Stage3Version.Year >= actualizeStart && x.Stage2Version.Stage3Version.Year <= actualizeEnd))
                    .WhereContains(x => x.Stage2Version.Stage3Version.Id, st3Ids);

                // фиксируем старые значения для того чтобы в логи записать информацию об изменении
                var oldDataSt1 = verSt1Query
                    .Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume, x.Sum })
                    .AsEnumerable();

                var versSt1Recs = verSt1Query
                    .ToArray();

                var st1RecToUpdate = new List<VersionRecordStage1>();
                var st2RecToUpdate = new Dictionary<long, VersionRecordStage2>();
                var st3RecToUpdate = new Dictionary<long, VersionRecord>();

                var roQuery = realObjDomain.GetAll()
                    .Where(x => versSt1Domain.GetAll()
                    .Where(y => y.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(y => y.Stage2Version.Stage3Version.Year >= periodStartYear)
                    .Any(y => y.Stage2Version.Stage3Version.RealityObject.Id == x.Id));

                // получаем типы домов
                var dictRealEstTypes = realEstService.GetRealEstateTypes(roQuery);

                var workPricesByMu = workPriceDomain.GetAll()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(x => x.Year)
                            .ToDictionary(x => x.Key, z => z.AsEnumerable()));

                var dictStructElWork = structElementWorkDomain.GetAll()
                    .Select(x => new
                    {
                        SeId = x.StructuralElement.Id,
                        JobId = x.Job.Id,
                        JobName = x.Job.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SeId)
                    .ToDictionary(x => x.Key, y => y
                        .GroupBy(x => x.JobId)
                        .ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First()));

                foreach (var st1Rec in versSt1Recs)
                {
                    var jobs = dictStructElWork.ContainsKey(st1Rec.StructuralElement.StructuralElement.Id)
                                    ? dictStructElWork[st1Rec.StructuralElement.StructuralElement.Id]
                                    : new Dictionary<long, string>();

                    var realEstTypes = dictRealEstTypes.ContainsKey(st1Rec.RealityObject.Id)
                       ? dictRealEstTypes[st1Rec.RealityObject.Id]
                       : null;

                    var muId = st1Rec.RealityObject.Municipality.Id;
                    long settlementId = st1Rec.RealityObject.MoSettlement.ReturnSafe(x => x.Id);

                    var workPriceYear = isWorkPriceFirstYear ? periodStartYear : st1Rec.Year;

                    Dictionary<int, IEnumerable<HmaoWorkPrice>> workPricesByYear;

                    // берем расценку по району
                    if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.MunicipalUnion)
                    {
                        workPricesByYear = workPricesByMu.Get(muId);
                    }
                    else if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.Settlement)
                    {
                        // берем расценку по мо
                        workPricesByYear = workPricesByMu.Get(settlementId);
                    }
                    else
                    {
                        // берем расценку по мо, если  ее нет - по району
                        workPricesByYear = workPricesByMu.Get(settlementId) ?? workPricesByMu.Get(muId);
                    }

                    var workPrices = this.GetWorkPrices(workPricesByYear,
                                                    workPriceYear,
                                                    jobs.Keys.AsEnumerable(),
                                                    workPriceDetermineType,
                                                    st1Rec.RealityObject,
                                                    realEstTypes);

                    var sum = 0M;
                    if (workPrices.Any())
                    {
                        switch (st1Rec.StructuralElement.StructuralElement.CalculateBy)
                        {
                            case PriceCalculateBy.Volume:
                                sum = workPrices.Sum(x => x.NormativeCost * st1Rec.StructuralElement.Volume).RoundDecimal(2);
                                break;
                            case PriceCalculateBy.LivingArea:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaLiving.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                            case PriceCalculateBy.TotalArea:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaMkd.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                            case PriceCalculateBy.AreaLivingNotLivingMkd:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaLivingNotLivingMkd.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                        }
                    }

                    var serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                    if (st1Rec.Sum != sum || st1Rec.SumService != serviceCost)
                    {
                        var st3Rec = st3RecToUpdate.Get(st1Rec.Stage2Version.Stage3Version.Id) ?? st1Rec.Stage2Version.Stage3Version;
                        st3Rec.Changes = string.Format("Изменено: {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        st3Rec.IsChangeSumOnActualize = true;
                        st3Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                        st3Rec.ObjectEditDate = DateTime.Now;

                        if (!st3RecToUpdate.ContainsKey(st3Rec.Id))
                        {
                            st3RecToUpdate.Add(st3Rec.Id, st3Rec);
                        }

                        var st2Rec = st2RecToUpdate.Get(st1Rec.Stage2Version.Id) ?? st1Rec.Stage2Version;
                        st2Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                        st2Rec.ObjectEditDate = DateTime.Now;

                        if (!st2RecToUpdate.ContainsKey(st2Rec.Id))
                        {
                            st2RecToUpdate.Add(st2Rec.Id, st2Rec);
                        }

                        st1Rec.Sum = sum;
                        st1Rec.SumService = serviceCost;
                        st1Rec.ObjectEditDate = DateTime.Now;
                        st1RecToUpdate.Add(st1Rec);
                    }
                }

                if (!st3RecToUpdate.Any())
                {
                    return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                }

                // Старое значение объемов
                var volumeDict = oldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // Старое значение суммы
                var sumDict = oldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

                // новое значение объемов
                var newVolumeDict =
                    st1RecToUpdate.Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume })
                                  .GroupBy(x => x.St3Id)
                                  .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // новое значение 
                var roAddress =
                    verSt1Query.Select(x => new { roId = x.RealityObject.Id, x.RealityObject.Address })
                               .AsEnumerable()
                               .GroupBy(x => x.roId)
                               .ToDictionary(x => x.Key, y => y.Select(z => z.Address).FirstOrDefault());

                var publishYearDict = publishedProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain == version.IsMain && x.PublishedProgram.ProgramVersion.Municipality == version.Municipality)
                    .Where(x => x.PublishedProgram.ProgramVersion.Id == versionId)
                    .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                    .ToDictionary(x => x.Id, y => y.PublishedYear);

                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ActualizeSum,
                    DateAction = DateTime.Now,
                    Municipality = version.Municipality,
                    ProgramVersion = version,
                    UserName = this.User.Name,
                    InputParams = $"{actualizeStart}-{actualizeEnd}"
                };

                // Формируем данные для логов
                var logData = st3RecToUpdate.Values
                    .Select(rec =>
                    {
                        var oldVolume = volumeDict.ContainsKey(rec.Id) ? volumeDict[rec.Id] : 0m;
                        var newVolume = newVolumeDict.ContainsKey(rec.Id) ? newVolumeDict[rec.Id] : 0m;
                        var oldSum = sumDict.ContainsKey(rec.Id) ? sumDict[rec.Id] : 0m;

                        return new
                        {
                            TypeAction = VersionActualizeType.ActualizeSum,
                            Action = "Изменение",
                            Description = "Актуализация стоимости",
                            rec.RealityObject,
                            Address = roAddress.ContainsKey(rec.RealityObject.Id) ? roAddress[rec.RealityObject.Id] : rec.RealityObject.Address,
                            Ceo = rec.CommonEstateObjects,
                            PlanYear = rec.Year,
                            PublishYear = publishYearDict.Get(rec.Id),
                            rec.WorkCode,
                            Volume = oldVolume,
                            Number = rec.IndexNumber,
                            Sum = oldSum,
                            ChangeVolume = oldVolume != newVolume ? newVolume : 0m, // Проставляем только в случае изменения
                            ChangeSum = oldSum != rec.Sum ? rec.Sum : 0m // проставляем только в случае изменения
                        };
                    });

                var logRecords = logData
                    .Select(x => new ActualizeVersionLogRecord
                    {
                        TypeAction = x.TypeAction,
                        Action = x.Action,
                        Description = x.Description,
                        Address = x.Address,
                        Ceo = x.Ceo,
                        PlanYear = x.PlanYear,
                        Volume = x.Volume,
                        Number = x.Number,
                        Sum = x.Sum,
                        ChangeVolume = x.ChangeVolume,
                        ChangeSum = x.ChangeSum
                    })
                    .ToList();

                var vaLogRecords = logData
                    .Select(x => new VersionActualizeLogRecord
                    {
                        ActualizeLog = log,
                        Action = x.Action,
                        RealityObject = x.RealityObject,
                        WorkCode = x.WorkCode,
                        Ceo = x.Ceo,
                        PlanYear = x.PlanYear,
                        PublishYear = x.PublishYear,
                        Volume = x.Volume,
                        ChangeVolume = x.ChangeVolume,
                        Sum = x.Sum,
                        ChangeSum = x.ChangeSum,
                        Number = x.Number
                    });

                log.CountActions = logRecords.Count;
                log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        st3RecToUpdate.Values.ForEach(versSt3Domain.Update);
                        st2RecToUpdate.Values.ForEach(versSt2Domain.Update);
                        st1RecToUpdate.ForEach(versSt1Domain.Update);

                        this.LogDomain.Save(log);
                        vaLogRecords.ForEach(x => versionActualizeLogRecordDomain.Save(x));

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

        /// <summary>
        /// Актуализация года
        /// </summary>
        /// <param name="baseParams"> Базовые параметры </param>
        /// <returns></returns>
        public IDataResult ActualizeYear(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var publishProgRecDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var dpkrCorrectionDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var shortProgramRecordDomain = this.Container.ResolveDomain<ShortProgramRecord>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var ownerDecisionDomain = this.Container.ResolveDomain<ChangeYearOwnerDecision>();

            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var unProxy = this.Container.Resolve<IUnProxy>();
            var versionId = baseParams.Params.GetAs<long>("versionId");

            try
            {
                var version = programVersionDomain.Get(versionId);
                var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
                var crEndYear = this.GetPeriodStartYear(version) - 1;

                var selectUsingFilters = baseParams.Params.GetAs("selectUsingFilters", true);
                var st3Ids = baseParams.Params.GetAs<long[]>("st3Ids") ?? new long[0];

                if (crEndYear >= actualizeStart)
                {
                    return new BaseDataResult(false, String.Format("Выбранный год начала актуализации: {0}, не может быть равен или меньше года, в который выгружались данные из КПКР: {1}.", actualizeStart, crEndYear));
                }

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
                    .Select(x => new { st2Id = x.Stage2.Id, x.PlanYear })
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.PlanYear).FirstOrDefault());

                var idsWhereCorrectedDateGreater = dpkrCorrectionDomain.GetAll()
                    .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == versionId && y.PlanYear >= actualizeStart)
                    .Select(y => y.Stage2.Stage3Version.Id)
                    .AsQueryable();

                var st3Query = this.GetActualizeYearEntriesQueryable(baseParams)
                    .WhereIfContains(!selectUsingFilters, x => x.Id, st3Ids);

                var roStructElQuery = versSt1Domain.GetAll()
                    .Where(x => st3Query.Any(y => y.Id == x.Stage2Version.Stage3Version.Id))
                    .Select(x => x.StructuralElement);
                   
                var st1List = versSt1Domain.GetAll()
                  .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Year > crEndYear)
                  .Where(y => idsWhereCorrectedDateGreater.Any(idy => idy == y.Stage2Version.Stage3Version.Id))
                  .Select(
                     x =>
                     new
                     {
                         RoSeId = x.StructuralElement.Id,
                         x.StructuralElement.StructuralElement.LifeTime,
                         x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                         x.Stage2Version.Stage3Version.FixedYear, //Пометка для фиксирования (запрета изменения,перемещения по году) этим записям.
                         CorrectYear = correctionYear.ContainsKey(x.Stage2Version.Id) ? correctionYear[x.Stage2Version.Id] : x.Year,
                         x.Year,
                         St1Id = x.Id,
                         St1Sum = x.Sum,
                         St1SumService = x.SumService,
                         St2Id = x.Stage2Version.Id,
                         St2Sum = x.Stage2Version.Sum,
                         St3Id = x.Stage2Version.Stage3Version.Id,
                         St3Sum = x.Stage2Version.Stage3Version.Sum
                     })
                 .ToList();

                var stage1VerRecs = st1List
                    .Where(x => x.CorrectYear >= actualizeStart)
                    .Select(x => new { Key = "{0}_{1}".FormatUsing(x.RoSeId, x.Year), x.RoSeId })
                    .Distinct()
                    .ToDictionary(x => x.Key, y => y.RoSeId);

                var allStage1VerRecs = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Stage2Version.Stage3Version.Year > crEndYear)
                    .Where(y => idsWhereCorrectedDateGreater.Any(idy => idy == y.Stage2Version.Stage3Version.Id))
                    .Select(x => new
                    {
                        RoSeId = x.StructuralElement.Id,
                        St1Id = x.Id,
                        CorrectYear = correctionYear.ContainsKey(x.Stage2Version.Id) ? correctionYear[x.Stage2Version.Id] : x.Year,
                        St1Sum = x.Sum,
                        St2Id = x.Stage2Version.Id,
                        St2Sum = x.Stage2Version.Sum,
                        St3Id = x.Stage2Version.Stage3Version.Id,
                        St3Sum = x.Stage2Version.Stage3Version.Sum,
                        x.Stage2Version.Stage3Version.FixedYear //Пометка для фиксирования (запрета изменения,перемещения по году) этим записям.
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoSeId)
                    .ToDictionary(x => x.Key);


                // получаем список отремонтированных структурных элементов со своими сроками эксплуатации начиная с указанного периода и не трогаем записи, которые нельзя перемещать вне зависиомсти от из года.
                var st1Repaired =
                    st1List.Where(x => x.CorrectYear < actualizeStart || x.FixedYear)
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

                var stage1ToSave = longProgramService.GetStage1(actualizeStart, version.Municipality.Id, roStructElQuery);

                var changedRoStrEls = new HashSet<long>();

                stage1ToSave.Where(x => x.Year > crEndYear).ForEach(x =>
                {
                    var key = "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year);
                    if (stage1VerRecs.ContainsKey(key))
                    {
                        stage1VerRecs.Remove(key);
                    }
                    else
                    {
                        if (!st1Repaired.ContainsKey(x.StructuralElement.Id))
                        {
                            changedRoStrEls.Add(x.StructuralElement.Id);
                        }
                    }
                });


                var actualRoSeDict = stage1ToSave.Select(x => x.StructuralElement.Id).Distinct().ToDictionary(x => x);

                stage1VerRecs.Where(x => actualRoSeDict.ContainsKey(x.Value)).Select(x => x.Value).ForEach(
                    x =>
                    {
                        if (!st1Repaired.ContainsKey(x))
                        {
                            changedRoStrEls.Add(x);
                        }
                    });

                // значения объемов по 3 этапу
                var oldData = this.GetOldVersionRecordBySt3(versSt1Domain, versionId);
                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ActualizeYear,
                    DateAction = DateTime.Now,
                    Municipality = version.Municipality,
                    ProgramVersion = version,
                    UserName = this.User.Name
                };

                var logRecords = new List<ActualizeVersionLogRecord>();
                var listChangeSt3 = new List<long>();
                var listDeletedSt3 = new List<long>();

                if (changedRoStrEls.Any())
                {
                    using (var transaction = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            allStage1VerRecs.Where(x => changedRoStrEls.Contains(x.Key))
                                .ForEach(x => x.Value.ForEach(y =>
                                {
                                    if (y.CorrectYear >= actualizeStart)
                                    {
                                        ownerDecisionDomain.GetAll()
                                        .Where(z => z.VersionRecordStage1.Id == y.St1Id)
                                        .Select(z => z.Id)
                                        .ForEach(z => ownerDecisionDomain.Delete(z));

                                        versSt1Domain.Delete(y.St1Id);
                                        var st2 = stage2Dict[y.St2Id];
                                        st2.Sum = y.St2Sum - y.St1Sum;
                                        versSt2Domain.Update(st2);

                                        var st3 = stage3Dict[y.St3Id];
                                        st3.Sum = y.St3Sum - y.St1Sum;
                                        versSt3Domain.Update(st3);

                                        listChangeSt3.Add(st3.Id);
                                    }

                                }));

                            var st2ForDelete =
                                versSt2Domain.GetAll()
                                    .Where(
                                        x =>
                                            !versSt1Domain.GetAll().Any(y => y.Stage2Version.Id == x.Id) &&
                                            x.Stage3Version.ProgramVersion.Id == versionId);
                            shortProgramRecordDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                .ForEach(x => shortProgramRecordDomain.Delete(x.Id));
                            dpkrCorrectionDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                .ForEach(x => dpkrCorrectionDomain.Delete(x.Id));
                            publishProgRecDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.PublishedProgram.ProgramVersion.Id == versionId)
                                .ForEach(x => publishProgRecDomain.Delete(x.Id));
                            st2ForDelete.ForEach(x => versSt2Domain.Delete(x.Id));

                            versSt3Domain.GetAll()
                                .Where(
                                    x =>
                                        !versSt2Domain.GetAll().Any(y => y.Stage3Version.Id == x.Id) &&
                                        x.ProgramVersion.Id == versionId)
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
                                            Address = x.RealityObject.Address,
                                            Ceo = oldValues != null ? oldValues.CeoNames : string.Empty,
                                            PlanYear = oldValues != null ? oldValues.Year : 0,
                                            Volume = oldValues != null ? oldValues.Volume : 0,
                                            Sum = oldValues != null ? oldValues.Sum : 0,
                                            Number = oldValues != null ? oldValues.Number : 0
                                        };

                                        logRecords.Add(logRecord);

                                        listDeletedSt3.Add(x.Id);

                                        versSt3Domain.Delete(x.Id);
                                    });

                            transaction.Commit();
                        }
                        catch (Exception)
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
                                            .Where(
                                                x => listChangeSt3.Contains(x.Stage2Version.Stage3Version.Id))
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
                                                        ceoNames = y.Select(z => z.ceoNames).ToList().Any()
                                                            ? y.Select(z => z.ceoNames)
                                                                .Distinct()
                                                                .ToList()
                                                                .Aggregate(
                                                                    (str, result) =>
                                                                        string.IsNullOrEmpty(result)
                                                                            ? str
                                                                            : result + ", " + str)
                                                            : string.Empty,
                                                        volume = y.Sum(z => z.Volume),
                                                        year = y.Select(z => z.Year).FirstOrDefault(),
                                                        sum = y.Select(z => z.Sum).FirstOrDefault(),
                                                        number = y.Select(z => z.IndexNumber).FirstOrDefault()
                                                    });

                                    foreach (var kvp in changesData)
                                    {
                                        var st3 = versSt3Domain.Load(kvp.Key);
                                        st3.CommonEstateObjects = kvp.Value.ceoNames;
                                        versSt3Domain.Update(st3);

                                        var oldAddress = string.Empty;
                                        var oldCeoNames = string.Empty;
                                        var oldYear = 0;
                                        var oldVolume = 0m;
                                        var oldSum = 0m;
                                        var oldNumber = 0;

                                        if (oldData.ContainsKey(kvp.Key))
                                        {
                                            var oldValues = oldData[kvp.Key];

                                            oldAddress = oldValues.Address;
                                            oldCeoNames = oldValues.CeoNames;
                                            oldYear = oldValues.Year;
                                            oldVolume = oldValues.Volume;
                                            oldSum = oldValues.Sum;
                                            oldNumber = oldValues.Number;
                                        }

                                        var newValues = kvp.Value;

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

                    List<VersionRecord> ver3S = null;
                    List<VersionRecordStage2> ver2S = null;
                    List<VersionRecordStage1> ver1S = null;

                    var forDeleting = new List<long>();
                    forDeleting.AddRange(changedRoStrEls);

                    if (st3Ids.Length == 0)
                    {
                        // и оставшиеся записи в версии также помечаем как необзходимые на удаление
                        foreach (var roStId in stage1VerRecs.Values.Distinct())
                        {
                            if (!forDeleting.Contains(roStId))
                            {
                                forDeleting.Add(roStId);
                            }
                        }
                    }
                   
                    baseParams.Params.Add("FromActualizeYear", true);
                    var dataResult = this.GetNewRecords(baseParams, out ver3S, out ver2S, out ver1S, st1Repaired, forDeleting);

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

                                                oldAddress = oldValues.Address;
                                                oldCeoNames = oldValues.CeoNames;
                                                oldYear = oldValues.Year;
                                                oldVolume = oldValues.Volume;
                                                oldSum = oldValues.Sum;
                                                oldNumber = oldValues.Number;
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
                                                ChangeCeo = x.CommonEstateObjects != oldCeoNames
                                                        ? x.CommonEstateObjects : null,
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
                        log.CountActions = logRecords.Count;
                        log.LogFile = this.LogService.CreateLogFile(
                            logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                    }

                    this.LogDomain.Save(log);

                    return new BaseDataResult();
                }
                else
                {
                    return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                }
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(roStructElDomain);
                this.Container.Release(longProgramService);
                this.Container.Release(versSt1Domain);
                this.Container.Release(versSt2Domain);
                this.Container.Release(publishProgRecDomain);
                this.Container.Release(dpkrCorrectionDomain);
                this.Container.Release(versSt3Domain);
                this.Container.Release(sessionProvider);
                this.Container.Release(shortProgramRecordDomain);
                this.Container.Release(ownerDecisionDomain);
            }
        }

        /// <summary>
        ///  Костыльная актуализация года (для Ставрополи). 
        ///  Не пытайтесь понять бред заказчика, если поняли, то забудьте
        /// </summary>
        /// <param name="baseParams"> Базовые параметры</param>
        /// <returns> IDataResult </returns>
        public IDataResult ActualizeYearForStavropol(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var publishProgRecDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var publishProgDomain = this.Container.ResolveDomain<PublishedProgram>();
            var dpkrCorrectionDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var shortProgramRecordDomain = this.Container.ResolveDomain<ShortProgramRecord>();
            var ownerDecisionDomain = this.Container.ResolveDomain<ChangeYearOwnerDecision>();

            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var unProxy = this.Container.Resolve<IUnProxy>();
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            try
            {
                var version = programVersionDomain.Get(versionId);
                var publishProg = publishProgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == versionId);
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var startYear = config.ProgrammPeriodStart;
                var endYear = config.ProgrammPeriodEnd;
                var useValidShortProgram = config.ActualizeConfig.ActualizeUseValidShortProgram == TypeUsage.Used;

                var crEndYear = this.GetPeriodStartYear(version) - 1;

                if (useValidShortProgram && crEndYear >= actualizeStart)
                {
                    return new BaseDataResult(false,
                        $"Выбранный год начала актуализации: {actualizeStart}, не может быть равен или меньше года, в который выгружались данные из КПКР: {crEndYear}.");
                }

                var useSelectiveActualize = config.ActualizeConfig.UseSelectiveActualize == TypeUsage.Used;
				var selectUsingFilters = baseParams.Params.GetAs("selectUsingFilters", true);
				var st3Ids = baseParams.Params.GetAs<long[]>("st3Ids");

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
                
                var publishYearsList = publishProgRecDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                    .Join(versSt1Domain.GetAll(),
                        x => x.Stage2.Id,
                        y => y.Stage2Version.Id,
                        (x, y) => new
                        {
                            x.PublishedYear,
                            y.StructuralElement.Id,
                            VersId = y.Stage2Version.Stage3Version.ProgramVersion.Id,
                            St2Id = y.Stage2Version.Id,
                            y.Year,
                            y.Stage2Version.Stage3Version.IsChangedPublishYear
                        })
                    .Where(x => x.VersId == versionId && (x.IsChangedPublishYear || x.Year > crEndYear))
                    .ToList();

                // получаем опубликованные года для 2 этапа 
                var dictSt2PublishYears = publishYearsList
                    .GroupBy(x => x.St2Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

                var publishYears = publishYearsList
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).AsEnumerable().Distinct().OrderBy(x => x).ToList());

                var realObjInfo = realObjDomain.GetAll()
                    .Where(x => x.Municipality.Id == version.Municipality.Id || x.MoSettlement.Id == version.Municipality.Id)
                    .Select(x => new
                    {
                        x.Id,
                        Locality = x.FiasAddress.PlaceName,
                        Street = x.FiasAddress.StreetName,
                        x.FiasAddress.House,
                        x.FiasAddress.Housing,
                        Address = x.FiasAddress.AddressName,
                        CommissioningYear = x.BuildYear ?? 0
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

                IQueryable<RealityObjectStructuralElement> roStructElQuery;
                if (useSelectiveActualize)
                {
                    var st3Query = this.GetActualizeYearChangeEntriesQueryable(baseParams)
                        .WhereIfContains(!selectUsingFilters, x => x.Id, st3Ids);

                    roStructElQuery = versSt1Domain.GetAll()
                        .Where(x => st3Query.Any(y => y.Id == x.Stage2Version.Stage3Version.Id))
                        .Select(x => x.StructuralElement);
                }
                else
                {
                    roStructElQuery = roStructElDomain.GetAll();
                }

                var st1Query = versSt1Domain.GetAll().Where(y => y.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && !y.Stage2Version.Stage3Version.IsChangedPublishYear);

                // берем только те конструктивы, для которых нет ни одной записи в периоде КПКР
                roStructElQuery = roStructElQuery
                    .Where(x => st1Query.Any(y => y.StructuralElement.Id == x.Id))
                    .Where(x => !st1Query.Where(y => y.Stage2Version.Stage3Version.Year <= crEndYear).Any(y => y.StructuralElement.Id == x.Id));

                var st1List = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Year > crEndYear)
                    .Select(x => new 
                    {
                        RoSeId = x.StructuralElement.Id,
                        x.StructuralElement.StructuralElement.LifeTime,
                        x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                        x.Stage2Version.Stage3Version.FixedYear,
                        x.Year,
                        St1Id = x.Id,
                        St1Sum = x.Sum,
                        St1SumService = x.SumService,
                        St2Id = x.Stage2Version.Id,
                        St2Sum = x.Stage2Version.Sum,
                        St3Id = x.Stage2Version.Stage3Version.Id,
                        St3Sum = x.Stage2Version.Stage3Version.Sum 
                    })
                    .ToList();

                var stage1VerRecs = new Dictionary<string, long>();

                st1List.Select(x => new
                {
                    PublishYear = dictSt2PublishYears.ContainsKey(x.St2Id) ? dictSt2PublishYears[x.St2Id] : 0,
                    x.RoSeId,
                    x.Year
                })
                .OrderBy(x => x.RoSeId)
                .ThenBy(x => x.Year)
                .ForEach(x =>
                {
                    var key = "{0}_{1}".FormatUsing(x.RoSeId, x.Year);

                    if (!stage1VerRecs.ContainsKey(key))
                    {
                       stage1VerRecs.Add(key, x.RoSeId);
                    }
                });

                var allStage1VerRecs = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Stage2Version.Stage3Version.Year > crEndYear)
                    .SelectMany(x => publishProgRecDomain.GetAll()
                        .Where(y => y.Stage2.Id == x.Stage2Version.Id)
                        .DefaultIfEmpty(),
                        (x, y) => new
                        {
                            Stage1 = x,
                            PublishedYear = y != null ? y.PublishedYear : 0
                        })
                    .Select(x => new
                    {
                        RoSeId = x.Stage1.StructuralElement.Id,
                        St1Id = x.Stage1.Id,
                        St1Sum = x.Stage1.Sum + x.Stage1.SumService,
                        St2Id = x.Stage1.Stage2Version.Id,
                        St3Id = x.Stage1.Stage2Version.Stage3Version.Id,
                        x.Stage1.Stage2Version.Stage3Version.FixedYear,
                        x.PublishedYear
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoSeId)
                    .ToDictionary(x => x.Key);

                // поулаем список отремонтированных структурных элементов со своими сроками эксплуатации
                var st1Repaired =
                    st1List.Where(x => x.Year < startYear || x.FixedYear)
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
                                       LastYearRepair = z.Year
                                   })
                                .OrderByDescending(z => z.LastYearRepair)
                                .FirstOrDefault());

                //тут беру окончание периода с запасом потмоучт нужно после вычисления проверить изменившиеся года у опубликованных записей
                var stage1ToSave = longProgramService.GetStage1(startYear, version.Municipality.Id, roStructElQuery, endYear + 30, true);

                var publishYears2 = publishYearsList
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).AsEnumerable().Distinct().OrderBy(x => x).ToList());

                foreach (var stage1 in stage1ToSave.OrderBy(x => x.StructuralElement.Id).ThenBy(x => x.Year))
                {
                    var tempPublishYear = publishYears2.Get(stage1.StructuralElement.Id);
                    var publishYear = 0;

                    if (tempPublishYear != null && tempPublishYear.Count > 0)
                    {
                        publishYear = tempPublishYear.First();
                        tempPublishYear.Remove(publishYear);
                    }

                    if (publishYear >= startYear && stage1.Year > publishYear)
                    {
                        stage1.Year = publishYear;
                    }

                    stage1.ObjectVersion = publishYear;
                }

                stage1ToSave = stage1ToSave.Where(x => x.Year <= endYear).ToList();

                var changedRoStrEls = new HashSet<long>();

                stage1ToSave.Where(x => x.Year > crEndYear).ForEach(x =>
                {
                    var key = "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year);

                    if (stage1VerRecs.ContainsKey(key))
                    {
                        stage1VerRecs.Remove(key);
                    }
                    else
                    {
                        changedRoStrEls.Add(x.StructuralElement.Id);
                    }
                });

                var actualRoSeDict = stage1ToSave.Select(x => x.StructuralElement.Id).Distinct().ToDictionary(x => x);

                stage1VerRecs.Where(x => actualRoSeDict.ContainsKey(x.Value)).Select(x => x.Value).ForEach(x => changedRoStrEls.Add(x));

                // значения объемов по 3 этапу
                var oldData = this.GetOldVersionRecordBySt3(versSt1Domain, versionId);
                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ActualizeChangeYear,
                    DateAction = DateTime.Now,
                    Municipality = version.Municipality,
                    ProgramVersion = version,
                    UserName = this.User.Name,
                    InputParams = $"{startYear}-{endYear}"
                };

                var fileLogRecords = new List<ActualizeVersionLogRecord>();
                var logData= new List<Tuple<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>>();
                var listChangeSt3 = new List<long>();
                var listDeletedSt3 = new List<long>();

                var listChangedPublishedYear = versSt3Domain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId && x.IsChangedPublishYear)
                    .Select(x => new
                    {
                        x.Id
                    }).ToList();

                if (changedRoStrEls.Any() || listChangedPublishedYear.Any())
                {
                    using (var transaction = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            var publishedYearsDict = new Dictionary<long, int>();
                            
                            allStage1VerRecs.Where(x => changedRoStrEls.Contains(x.Key))
                                .ForEach(x => x.Value.ForEach(y =>
                                {
                                    ownerDecisionDomain.GetAll()
                                        .Where(z => z.VersionRecordStage1.Id == y.St1Id)
                                        .Select(z => z.Id)
                                        .ForEach(z => ownerDecisionDomain.Delete(z));

                                    versSt1Domain.Delete(y.St1Id);
                                    var st2 = stage2Dict[y.St2Id];
                                    st2.Sum -= y.St1Sum;
                                    versSt2Domain.Update(st2);

                                    var st3 = stage3Dict[y.St3Id];

                                    st3.Sum -= y.St1Sum;
                                    versSt3Domain.Update(st3);

                                    publishedYearsDict.Add(st3.Id, y.PublishedYear);
                                    listChangeSt3.Add(st3.Id);
                                }));

                            foreach (var s in listChangedPublishedYear)
                            {
                                if(!listChangeSt3.Contains(s.Id)) 
                                    listChangeSt3.Add(s.Id);
                            }
                            
                            var st2ForDelete =
                                versSt2Domain.GetAll()
                                    .Where(
                                        x =>
                                            !versSt1Domain.GetAll().Any(y => y.Stage2Version.Id == x.Id) &&
                                            x.Stage3Version.ProgramVersion.Id == versionId && !x.Stage3Version.IsChangedPublishYear);
                            shortProgramRecordDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                .ForEach(x => shortProgramRecordDomain.Delete(x.Id));
                            dpkrCorrectionDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                .ForEach(x => dpkrCorrectionDomain.Delete(x.Id));
                            publishProgRecDomain.GetAll()
                                .Where(
                                    x =>
                                        st2ForDelete.Any(y => y.Id == x.Stage2.Id) &&
                                        x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                .ForEach(x => publishProgRecDomain.Delete(x.Id));
                            st2ForDelete.ForEach(x => versSt2Domain.Delete(x.Id));

                            versSt3Domain.GetAll()
                                .Where(
                                    x =>
                                        !versSt2Domain.GetAll().Any(y => y.Stage3Version.Id == x.Id) &&
                                        x.ProgramVersion.Id == versionId)
                                .ForEach(
                                    x =>
                                    {
                                        // логируем Удаление Записи
                                        var oldValues = oldData.ContainsKey(x.Id) ? oldData[x.Id] : null;

                                        var logRecord = new ActualizeVersionLogRecord
                                        {
                                            TypeAction = VersionActualizeType.ActualizeChangeYear,
                                            Action = "Удаление в связи с актуализацией года",
                                            Description = "Изменение при Актуализации изменения года",
                                            Address = x.RealityObject.Address,
                                            Ceo = oldValues?.CeoNames ?? string.Empty,
                                            PlanYear = oldValues?.Year ?? 0,
                                            Volume = oldValues?.Volume ?? 0,
                                            Sum = oldValues?.Sum ?? 0,
                                            Number = oldValues?.Number ?? 0
                                        };

                                        fileLogRecords.Add(logRecord);
                                        
                                        logData.Add(new Tuple<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>(logRecord, new VersionActualizeLogRecordAdditionInfo
                                        {
                                            RealityObject = x.RealityObject,
                                            WorkCode = x.WorkCode,
                                            PublishedYear = publishedYearsDict.Get(x.Id)
                                        }));

                                        listDeletedSt3.Add(x.Id);

                                        versSt3Domain.Delete(x.Id);
                                    });

                            transaction.Commit();
                        }
                        catch
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
                                            .Select(x => new
                                            {
                                                st3Id = x.Stage2Version.Stage3Version.Id,
                                                ceoNames = x.Stage2Version.CommonEstateObject.Name,
                                                x.Volume,
                                                x.Stage2Version.Stage3Version.Sum,
                                                x.Year,
                                                x.Stage2Version.Stage3Version.IndexNumber,
                                                st2Id = x.Stage2Version.Id,
                                                x.Stage2Version.Stage3Version.IsChangedPublishYear
                                            })
                                            .AsEnumerable()
                                            .GroupBy(x => x.st3Id)
                                            .ToDictionary(
                                                x => x.Key,
                                                y =>
                                                    new
                                                    {
                                                        ceoNames = y.Select(z => z.ceoNames).ToList().Any()
                                                            ? y.Select(z => z.ceoNames)
                                                                .Distinct()
                                                                .ToList()
                                                                .Aggregate(
                                                                    (str, result) =>
                                                                        string.IsNullOrEmpty(result)
                                                                            ? str
                                                                            : result + ", " + str)
                                                            : string.Empty,
                                                        volume = y.Sum(z => z.Volume),
                                                        year = y.Select(z => z.IsChangedPublishYear
                                                            ? dictSt2PublishYears[z.st2Id]
                                                            : z.Year).FirstOrDefault(),
                                                        sum = y.Select(z => z.Sum).FirstOrDefault(),
                                                        number = y.Select(z => z.IndexNumber).FirstOrDefault()
                                                    });

                                    var publishedYearsDict = publishProgRecDomain.GetAll()
                                        .Where(x =>listChangeSt3.Contains(x.Stage2.Stage3Version.Id))
                                        .ToDictionary(x => x.Stage2.Stage3Version.Id, x => x.PublishedYear);

                                    foreach (var kvp in changesData)
                                    {
                                        var st3 = versSt3Domain.Load(kvp.Key);
                                        st3.CommonEstateObjects = kvp.Value.ceoNames;
                                        if (st3.IsChangedPublishYear)
                                        {
                                            st3.Year = kvp.Value.year;
                                        }

                                        versSt3Domain.Update(st3);

                                        var oldAddress = string.Empty;
                                        var oldCeoNames = string.Empty;
                                        var oldYear = 0;
                                        var oldVolume = 0m;
                                        var oldSum = 0m;
                                        var oldNumber = 0;

                                        if (oldData.ContainsKey(kvp.Key))
                                        {
                                            var oldValues = oldData[kvp.Key];

                                            oldAddress = oldValues.Address;
                                            oldCeoNames = oldValues.CeoNames;
                                            oldYear = oldValues.Year;
                                            oldVolume = oldValues.Volume;
                                            oldSum = oldValues.Sum;
                                            oldNumber = oldValues.Number;
                                        }

                                        var newValues = kvp.Value;

                                        if (newValues.year != 0 && newValues.year != oldYear)
                                        {
                                            // логируем Изменение Записи
                                            var logRecord = new ActualizeVersionLogRecord
                                            {
                                                TypeAction = VersionActualizeType.ActualizeChangeYear,
                                                Action = "Изменение в связи с актуализацией года",
                                                Description = "Изменение при Актуализации изменения года",
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

                                            fileLogRecords.Add(logRecord);
                                            logData.Add(new Tuple<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>(logRecord, new VersionActualizeLogRecordAdditionInfo
                                            {
                                                RealityObject = st3.RealityObject,
                                                WorkCode = st3.WorkCode,
                                                PublishedYear = publishedYearsDict.Get(st3.Id)
                                            }));
                                        }
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

                    List<VersionRecord> ver3S = null;
                    List<VersionRecordStage2> ver2S = null;
                    List<VersionRecordStage1> ver1S = null;

                    var forDeleting = new List<long>();
                    forDeleting.AddRange(changedRoStrEls);

                    baseParams.Params.Add("FromActualizeYear", true);
                    this.GetNewRecordsForStavropol(baseParams, out ver3S, out ver2S, out ver1S, st1Repaired, forDeleting, publishYears, dictSt2PublishYears);

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
                                var ver3sIds = ver3S.Select(y => y.Id).ToList();
                                
                                var publishedYearsDict = publishProgRecDomain.GetAll()
                                    .Where(x => ver3sIds.Contains(x.Stage2.Stage3Version.Id))
                                    .ToDictionary(x => x.Stage2.Stage3Version.Id, x => x.PublishedYear);

                                ver3S.ForEach(x =>
                                {
                                    if (x.Id > 0)
                                    {
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

                                            oldAddress = oldValues.Address;
                                            oldCeoNames = oldValues.CeoNames;
                                            oldYear = oldValues.Year;
                                            oldVolume = oldValues.Volume;
                                            oldSum = oldValues.Sum;
                                            oldNumber = oldValues.Number;
                                        }

                                        if (x.Year != 0 && x.Year != oldYear)
                                        {
                                            // логируем Изменение Записи
                                            var logRecord = new ActualizeVersionLogRecord
                                            {
                                                TypeAction = VersionActualizeType.ActualizeChangeYear,
                                                Action = "Изменение в связи с актуализацией года",
                                                Description = "Изменение при Актуализации изменения года",
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

                                            fileLogRecords.Add(logRecord);
                                            logData.Add(new Tuple<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>(logRecord, new VersionActualizeLogRecordAdditionInfo
                                            {
                                                RealityObject = x.RealityObject,
                                                WorkCode = x.WorkCode,
                                                PublishedYear = publishedYearsDict.Get(x.Id)
                                            }));
                                        }
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
                                            TypeAction = VersionActualizeType.ActualizeChangeYear,
                                            Action = "Добавление в связи с актуализацией года",
                                            Description = "Изменение при Актуализации изменения года",
                                            Address = x.RealityObject.Address,
                                            Ceo = x.CommonEstateObjects,
                                            PlanYear = x.Year,
                                            Volume = newVolume,
                                            Sum = x.Sum,
                                            Number = x.IndexNumber
                                        };

                                        fileLogRecords.Add(logRecord);
                                        logData.Add(new Tuple<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>(logRecord, new VersionActualizeLogRecordAdditionInfo
                                        {
                                            RealityObject = x.RealityObject,
                                            WorkCode = x.WorkCode,
                                            PublishedYear = publishedYearsDict.Get(x.Id)
                                        }));
                                    }
                                });

                                ver2S.ForEach(x =>
                                {
                                    PublishedProgramRecord newRec = null;

                                    if (x.ObjectVersion > 0)
                                    {
                                        var tempRealObjInfo = realObjInfo.Get(x.Stage3Version.RealityObject.Id);

                                        newRec = new PublishedProgramRecord
                                        {
                                            PublishedProgram = publishProg,
                                            Stage2 = x,
                                            RealityObject = x.Stage3Version.RealityObject,
                                            PublishedYear = x.ObjectVersion,
                                            IndexNumber = x.Stage3Version.IndexNumber,
                                            Locality = tempRealObjInfo.Return(z => z.Locality),
                                            Street = tempRealObjInfo.Return(z => z.Street),
                                            House = tempRealObjInfo.Return(z => z.House),
                                            Housing = tempRealObjInfo.Return(z => z.Housing),
                                            Address = tempRealObjInfo.Return(z => z.Address),
                                            CommonEstateobject = x.Stage3Version.CommonEstateObjects,
                                            CommissioningYear = tempRealObjInfo.Return(z => z.CommissioningYear),
                                            Sum = x.Sum
                                        };
                                    }

                                    x.ObjectVersion = 0;

                                    if (x.Id > 0)
                                    {
                                        session.Update(x);
                                    }
                                    else
                                    {
                                        session.Insert(x);
                                    }

                                    if (newRec != null)
                                    {
                                        session.Insert(newRec);
                                    }

                                });

                                ver1S.ForEach(x =>
                                {
                                    x.ObjectVersion = 0;
                                    session.Insert(x);
                                });
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                    if (fileLogRecords.Any())
                    {
                        log.CountActions = fileLogRecords.Count;
                        log.LogFile = this.LogService.CreateLogFile(
                            fileLogRecords
                                .OrderBy(x => x.Address)
                                .ThenBy(x => x.Ceo)
                                .ThenByDescending(x => x.Action)
                                .ThenBy(x => x.Number),
                            baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                    }

                    this.LogDomain.Save(log);

                    if (logData.Any())
                    {
                        var connection = sessionProvider.GetCurrentSession().Connection as NpgsqlConnection;

                        using (var importer = connection.BeginBinaryImport(
                                   @"COPY OVRHL_ACTUALIZE_LOG_RECORD 
                                   (object_version, object_create_date, object_edit_date, actualize_log_id, action, 
                                    reality_object_id, work_code, ceo, plan_year, change_plan_year, publish_year, change_publish_year, volume, 
                                    change_volume, sum, change_sum, number, change_number) FROM STDIN (FORMAT BINARY)"))
                        {
                            logData.ForEach(x => importer.WriteRow(0L, DateTime.Now, DateTime.Now, log.Id, x.Item1.Action, x.Item2.RealityObject.Id, x.Item2.WorkCode, x.Item1.Ceo,
                                x.Item1.PlanYear, x.Item1.ChangePlanYear, x.Item2.PublishedYear, 0, x.Item1.Volume, x.Item1.ChangeVolume, x.Item1.Sum, x.Item1.ChangeSum, x.Item1.Number, x.Item1.ChangeNumber));
                            
                            importer.Complete();
                        }
                    }
                    
                    return new BaseDataResult();
                }
                else
                {
                    return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                }
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(roStructElDomain);
                this.Container.Release(longProgramService);
                this.Container.Release(versSt1Domain);
                this.Container.Release(versSt2Domain);
                this.Container.Release(publishProgRecDomain);
                this.Container.Release(dpkrCorrectionDomain);
                this.Container.Release(versSt3Domain);
                this.Container.Release(sessionProvider);
                this.Container.Release(shortProgramRecordDomain);
                this.Container.Release(publishProgDomain);
                this.Container.Release(ownerDecisionDomain);
            }
        }

        /// <summary>
        /// Получить запрос списка записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IQueryable<ActualizeSumEntriesDto> GetActualizeSumEntriesQueryable(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var workPriceDomain = this.Container.ResolveDomain<HmaoWorkPrice>();
            var structElementWorkDomain = this.Container.ResolveDomain<StructuralElementWork>();
            var realEstService = this.Container.Resolve<IRealEstateTypeService>();
            var correctDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            using (this.Container.Using(programVersionDomain, versSt1Domain, versSt2Domain, versSt3Domain, sessionProvider, workPriceDomain, structElementWorkDomain, realEstService))
            {
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var periodStartYear = config.ProgrammPeriodStart;
                var isWorkPriceFirstYear = config.WorkPriceCalcYear == WorkPriceCalcYear.First;
                var workPriceDetermineType = config.WorkPriceDetermineType;
                var servicePercent = config.ServiceCost;
                var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
                var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);
                var loadParams = baseParams.GetLoadParam();

                var verSt1Query = versSt1Domain.GetAll()
                    .Fetch(r => r.StructuralElement)
                    .ThenFetch(e => e.StructuralElement)
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x =>
                            (correctDomain.GetAll()
                                .Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart && y.PlanYear <= actualizeEnd))
                            || (!correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id) && x.Stage2Version.Stage3Version.Year >= actualizeStart && x.Stage2Version.Stage3Version.Year <= actualizeEnd));

                var versSt1Recs = verSt1Query
                    .ToArray();

                var st3RecToUpdate = new Dictionary<long, ActualizeSumEntriesDto>();

                var roQuery = realObjDomain.GetAll()
                    .Where(x => versSt1Domain.GetAll()
                            .Where(y => y.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                            .Where(y => y.Stage2Version.Stage3Version.Year >= periodStartYear).Any(y => y.Stage2Version.Stage3Version.RealityObject.Id == x.Id));

                // получаем типы домов
                var dictRealEstTypes = realEstService.GetRealEstateTypes(roQuery);

                var workPricesByMu = workPriceDomain.GetAll()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(x => x.Year)
                            .ToDictionary(x => x.Key, z => z.AsEnumerable()));

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

                foreach (var st1Rec in versSt1Recs)
                {
                    var jobs = dictStructElWork.ContainsKey(st1Rec.StructuralElement.StructuralElement.Id)
                                    ? dictStructElWork[st1Rec.StructuralElement.StructuralElement.Id]
                                    : new Dictionary<long, string>();

                    var realEstTypes = dictRealEstTypes.ContainsKey(st1Rec.RealityObject.Id)
                       ? dictRealEstTypes[st1Rec.RealityObject.Id]
                       : null;

                    var muId = st1Rec.RealityObject.Municipality.Id;
                    long settlementId = st1Rec.RealityObject.MoSettlement.ReturnSafe(x => x.Id);

                    var workPriceYear = isWorkPriceFirstYear ? periodStartYear : st1Rec.Year;

                    Dictionary<int, IEnumerable<HmaoWorkPrice>> workPricesByYear;

                    // берем расценку по району
                    if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.MunicipalUnion)
                    {
                        workPricesByYear = workPricesByMu.Get(muId);
                    }
                    else if (this.WorkpriceMoLevel == Enums.WorkpriceMoLevel.Settlement)
                    {
                        // берем расценку по мо
                        workPricesByYear = workPricesByMu.Get(settlementId);
                    }
                    else
                    {
                        // берем расценку по мо, если  ее нет - по району
                        workPricesByYear = workPricesByMu.Get(settlementId) ?? workPricesByMu.Get(muId);
                    }

                    var workPrices = this.GetWorkPrices(workPricesByYear,
                                                    workPriceYear,
                                                    jobs.Keys.AsEnumerable(),
                                                    workPriceDetermineType,
                                                    st1Rec.RealityObject,
                                                    realEstTypes);

                    var sum = 0M;
                    if (workPrices.Any())
                    {
                        switch (st1Rec.StructuralElement.StructuralElement.CalculateBy)
                        {
                            case PriceCalculateBy.Volume:
                                sum = workPrices.Sum(x => x.NormativeCost * st1Rec.StructuralElement.Volume).RoundDecimal(2);
                                break;
                            case PriceCalculateBy.LivingArea:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaLiving.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                            case PriceCalculateBy.TotalArea:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaMkd.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                            case PriceCalculateBy.AreaLivingNotLivingMkd:
                                sum = workPrices.Sum(x => x.SquareMeterCost * st1Rec.RealityObject.AreaLivingNotLivingMkd.ToDecimal()).ToDecimal().RoundDecimal(2);
                                break;
                        }
                    }

                    var serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                    if (st1Rec.Sum != sum || st1Rec.SumService != serviceCost)
                    {
                        var st3Rec = st3RecToUpdate.Get(st1Rec.Stage2Version.Stage3Version.Id);
                        if (st3Rec == null)
                        {
                            var rec = st1Rec.Stage2Version.Stage3Version;
                            st3Rec = new ActualizeSumEntriesDto
                            {
                                Id = rec.Id,
                                RealityObject = rec.RealityObject.Address,
                                CommonEstateObjects = rec.CommonEstateObjects,
                                Year = rec.Year,
                                IndexNumber = rec.IndexNumber,
                                Sum = rec.Sum
                            };
                        }
                        
                        st3Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;

                        if (!st3RecToUpdate.ContainsKey(st3Rec.Id))
                        {
                            st3RecToUpdate.Add(st3Rec.Id, st3Rec);
                        }
                    }
                }

                var result = st3RecToUpdate.Values
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return result;
            }
        }

        /// <summary>
        /// Актуализация очередности
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат запроса</returns>
        /// <exception cref="ValidationException">Результат проверки входных данных</exception>
        public IDataResult ActualizePriority(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var method = config.MethodOfCalculation;

            var versionParamsDomain = this.Container.ResolveDomain<VersionParam>();
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var versionRecordSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versionRecordSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var correctDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            var priorityService = this.Container.Resolve<IPriorityService>();
            try
            {
                var priorityParamsQuery = versionParamsDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId);

                var priorityParams = priorityParamsQuery
                    .ToDictionary(x => x.Code, y => y.Weight);

                var version = programVersionDomain.Get(versionId);
                var muId = version.Municipality.Id;

                var pointsParams = new Dictionary<long, List<StoredPointParam>>();

                var excludeIds = correctDomain.GetAll()
                    .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == versionId && y.PlanYear < actualizeStart)
                    .Select(y => y.Stage2.Stage3Version.Id)
                    .ToList();

                var versionSt3Query = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Where(x => !excludeIds.Contains(x.Id));

                var maxIndex = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Where(x => excludeIds.Contains(x.Id))
                    .SafeMax(x => x.IndexNumber);

                var versionSt2Query = versionRecordSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(
                        x => new Stage2Proxy
                        {
                            Stage3Id = x.Stage3Version.Id,
                            CeoId = x.CommonEstateObject.Id,
                            RoId = x.Stage3Version.RealityObject.Id
                        })
                    .AsEnumerable();

                var versionSt1Query = versionRecordSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(
                        x => new Stage1Proxy
                        {
                            Stage3Id = x.Stage2Version.Stage3Version.Id,
                            CeoGroupType = x.StructuralElement.StructuralElement.Group.CommonEstateObject.GroupType,
                            Wearout = x.StructuralElement.Wearout
                        })
                    .AsEnumerable();

                var sbLogDescription = new StringBuilder("Расчет очередности");
                switch (method)
                {
                    case TypePriority.Criteria:
                        sbLogDescription.Append(" по критериям");
                        if (priorityParams.Count == 0)
                        {
                            throw new ValidationException("Не указаны параметры");
                        }
                        break;

                    case TypePriority.Points:
                        sbLogDescription.Append(" по баллам");
                        pointsParams = priorityService.GetPoints(muId, versionSt3Query, versionSt2Query, versionSt1Query);
                        priorityParams["PointsParam"] = 1;
                        break;

                    case TypePriority.CriteriaAndPoins:
                        sbLogDescription.Append(" по критериям и баллам");
                        pointsParams = priorityService.GetPoints(muId, versionSt3Query, versionSt2Query, versionSt1Query);
                        break;
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
                var dictStage3 = versionSt3Query.ToDictionary(x => x.Id);

                var muStage3 = versionSt3Query
                    .Select(
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

                var oldData = this.GetOldVersionRecordBySt3(versionRecordSt1Domain, versionId);
                //получение годом капитального ремонта
                var firstPrivYears = this.GetFirstPrivYears(version, startYear);

                var lastOvrhlYears = this.GetLastOvrhlYears(version);

                var weights = this.GetWeights(version);

                var countWorks = this.GetCountWorks(version);

                var yearsWithLifetimes = this.GetSeYearsWithLifetimes(version);

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

                    var eval = this.Container.Resolve<IProgrammPriorityParam>(key,
                        new Arguments
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

                var index = maxIndex + 1;
                foreach (var item in proxyList2.ThenBy(x => x.RoId).ThenBy(x => x.Stage3.Id))
                {
                    if (!dictStage3.ContainsKey(item.Id))
                    {
                        continue;
                    }

                    var st3 = dictStage3[item.Id];

                    priorityService.FillStage3Criteria(st3, item.OrderDict);

                    st3.StoredPointParams = pointsParams.ContainsKey(st3.Id) ? pointsParams[st3.Id] : new List<StoredPointParam>();

                    st3.IndexNumber = index++;
                }

                var publishYearDict = publishedProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain == version.IsMain && x.PublishedProgram.ProgramVersion.Municipality == version.Municipality)
                    .Where(x => x.PublishedProgram.ProgramVersion.Id == versionId)
                    .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                    .ToDictionary(x => x.Id, y => y.PublishedYear);

                var logData = dictStage3
                    .Join(
                        oldData,
                        y => y.Key,
                        x => x.Key,
                        (y, x) =>
                        {
                            if (x.Value.Number == y.Value.IndexNumber)
                            {
                                return null;
                            }
                            else
                            {
                                return new
                                {
                                    TypeAction = VersionActualizeType.ActualizeOrder,
                                    Action = "Изменение",
                                    Description = logDescription,
                                    y.Value.RealityObject,
                                    x.Value.Address,
                                    Ceo = x.Value.CeoNames,
                                    PlanYear = x.Value.Year,
                                    PublishYear = publishYearDict.Get(y.Value.Id),
                                    x.Value.Sum,
                                    x.Value.Number,
                                    ChangePlanYear = x.Value.Year != y.Value.Year ? y.Value.Year : 0,
                                    ChangeSum = x.Value.Sum != y.Value.Sum ? y.Value.Sum : 0m,
                                    ChangeNumber = x.Value.Number != y.Value.IndexNumber ? y.Value.IndexNumber : 0,
                                    x.Value.WorkCode
                                };
                            }
                        }
                    )
                    .Where(x => x != null)
                    .ToList();

                var logRecords = logData
                    .Select(x => new ActualizeVersionLogRecord
                    {
                        TypeAction = x.TypeAction,
                        Action = x.Action,
                        Description = x.Description,
                        Address = x.Address,
                        Ceo = x.Ceo,
                        PlanYear = x.PlanYear,
                        Sum = x.Sum,
                        Number = x.Number,
                        ChangePlanYear = x.ChangePlanYear,
                        ChangeSum = x.ChangeSum,
                        ChangeNumber = x.ChangeNumber
                    })
                    .ToList();

                var log = new VersionActualizeLog
                {
                    ActualizeType = VersionActualizeType.ActualizeOrder,
                    DateAction = DateTime.Now,
                    Municipality = version.Municipality,
                    ProgramVersion = version,
                    UserName = this.User.Name,
                    InputParams = actualizeStart.ToString()
                };
                if (logRecords.Any(x => x.Number != x.ChangeNumber))
                {
                    log.CountActions = logRecords.Count;
                    log.LogFile = this.LogService.CreateLogFile(
                        logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                }

                this.LogDomain.Save(log);

                var vaLogRecords = logData
                    .Select(x => new VersionActualizeLogRecord
                    {
                        ActualizeLog = log,
                        Action = x.Action,
                        RealityObject = x.RealityObject,
                        Ceo = x.Ceo,
                        PlanYear = x.PlanYear,
                        PublishYear = x.PublishYear,
                        ChangePlanYear = x.ChangePlanYear,
                        Sum = x.Sum,
                        ChangeSum = x.ChangeSum,
                        Number = x.Number,
                        ChangeNumber = x.ChangeNumber,
                        WorkCode = x.WorkCode
                    });

                vaLogRecords.ForEach(x => versionActualizeLogRecordDomain.Save(x));

                TransactionHelper.InsertInManyTransactions(this.Container, dictStage3.Values, 10000, true, true);
            }
            finally
            {
                this.Container.Release(versionParamsDomain);
                this.Container.Release(versionRecordDomain);
                this.Container.Release(versionRecordSt2Domain);
                this.Container.Release(versionRecordSt1Domain);
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedProgramRecordDomain);
                this.Container.Release(versionActualizeLogRecordDomain);
                this.Container.Release(priorityService);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получение неактуальных записей
        /// </summary>
        /// <param name="baseParams"> Базовые параметры </param>
        /// <returns> IDataResult </returns>
        public IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(BaseParams baseParams)
        {
            var roStructElDomain = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roStrElService = this.Container.Resolve<IRealityObjectStructElementService>();
            var versStage1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();
            int minimumCountApartments;
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            try
            {
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                minimumCountApartments = config.HouseAddInProgramConfig.MinimumCountApartments;
            }
            catch (KeyNotFoundException)
            {
                minimumCountApartments = 0;
            }

            try
            {
                var roStrElQuery = roStrElService.GetElementsForLongProgram(roStructElDomain)
                    .WhereIf(minimumCountApartments > 0, x => x.RealityObject.NumberApartments >= minimumCountApartments)
                    .Where(x => !x.RealityObject.State.FinalState);

                // Получаем те записи 1го этапа версии для которые уже считаются не актуальными
                return versStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x => x.Stage2Version.Stage3Version.Year >= actualizeStart || x.RealityObject.State.FinalState)
                    .Where(x => !roStrElQuery.Any(y => y.Id == x.StructuralElement.Id));
            }
            finally
            {
                this.Container.Release(roStructElDomain);
                this.Container.Release(roStrElService);
                this.Container.Release(versStage1Domain);
            }
        }

        /// <summary>
        /// Получить запрос списка записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="afterRepaired">Признак После ремонта</param>
        /// <param name="roStrElList">Список КЭ дома</param>
        /// <returns></returns>
        public IQueryable<RealityObjectStructuralElement> GetAddEntriesQueryable(
            BaseParams baseParams, 
            Dictionary<long, RoStrElAfterRepair> afterRepaired = null, 
            List<long> roStrElList = null)
        {
            var loadParams = baseParams.GetLoadParam();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var fromActualizeYear = baseParams.Params.GetAs<bool>("FromActualizeYear");

            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var correctDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var roStructElService = this.Container.Resolve<IRealityObjectStructElementService>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgService =  this.Container.Resolve<ILongProgramService>();

            using (this.Container.Using(programVersionDomain, versSt1Domain, correctDomain, roStructElService, roStructElDomain, longProgService))
            {
                var version = programVersionDomain.Get(versionId);
                var startYear = this.GetPeriodStartYear(version);

                var versSt1Query = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .WhereIf(afterRepaired != null && afterRepaired.Any(),
                                     x => x.Stage2Version.Stage3Version.Year >= startYear &&
                            ((correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart))
                            || !correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id)));

                var newStructQuery = roStructElService.GetElementsForLongProgram(roStructElDomain)
                    .Where(
                        x =>
                            x.RealityObject.Municipality.Id == version.Municipality.Id ||
                                x.RealityObject.MoSettlement.Id == version.Municipality.Id)
                    .WhereIf(
                        roStrElList != null,
                        x => roStrElList.Contains(x.Id) || !versSt1Query.Any(y => y.StructuralElement.Id == x.Id))
                    .WhereIf(roStrElList == null, x => !versSt1Query.Any(y => y.StructuralElement.Id == x.Id))
                    .Where(
                        x =>
                            !versSt1Query.Any(
                                y => x.Id == y.StructuralElement.Id && (!fromActualizeYear
                                    || (y.Stage2Version.Stage3Version.Year >= startYear && y.Stage2Version.Stage3Version.Year >= actualizeStart))))

                    .Select(
                        x => new
                        {
                            roStructEl = x,
                            RealityObject = x.RealityObject.Address,
                            CommonEstateObject = x.StructuralElement.Group.CommonEstateObject.Name,
                            StructuralElement = x.StructuralElement.Name
                        })
                    .Filter(loadParams, this.Container)
                    .Select(x => x.roStructEl);

                var stage1 = longProgService.GetStage1(actualizeStart, version.Municipality.Id, newStructQuery);

                var result = stage1.Select(x => x.StructuralElement)
                    .Distinct()
                    .AsQueryable();

                return result;
            }
        }

        /// <summary>
        /// Получить запрос списка записей на актуализацию года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IQueryable<VersionRecord> GetActualizeYearEntriesQueryable(BaseParams baseParams)
        {
			var loadParams = baseParams.GetLoadParam();
			var version = baseParams.Params.GetAs<long>("versionId");
			var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();

			try
			{
				var st3Query = versSt3Domain.GetAll()
					.Where(x => x.ProgramVersion.Id == version)
					.Select(x => new
					{
						Stage3 = x,
						RealityObject = x.RealityObject.Address,
						CommonEstateObject = x.CommonEstateObjects,
						x.Year,
						x.Sum,
                        x.Changes,
                        x.IsChangedYear
                    })
					.Filter(loadParams, this.Container)
					.Select(x => x.Stage3);

				return st3Query;
			}
			finally
			{
			    this.Container.Release(versSt3Domain);
			}
		}

        /// <summary>
        /// Получить запрос списка записей на актуализацию изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IQueryable<VersionRecord> GetActualizeYearChangeEntriesQueryable(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
			var version = baseParams.Params.GetAs<long>("versionId");
			var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();

	        try
	        {
		        var st3Query = versSt3Domain.GetAll()
			        .Where(x => x.ProgramVersion.Id == version)
			        .Select(x => new
				        {
					        Stage3 = x,
					        RealityObject = x.RealityObject.Address,
					        CommonEstateObject = x.CommonEstateObjects,
					        x.Year,
					        x.Sum,
                            x.Changes,
                            x.IsChangedYear
				        })
			        .Filter(loadParams, this.Container)
			        .Select(x => x.Stage3);

				return st3Query;
	        }
	        finally
	        {
	            this.Container.Release(versSt3Domain);
	        }
        }

        /// <summary>
		/// Актуализация удаления неактуальных записей
		/// </summary>
		/// <param name="baseParams"> Базовые параметры </param>
		/// <returns> IDataResult </returns>
		public IDataResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var stage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var logRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            var publishProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();
            var programVersionService = this.Container.Resolve<IProgramVersionService>();
            var unProxy = this.Container.Resolve<IUnProxy>();

            try
            {
                var versionId = baseParams.Params.GetAs<long>("versionId");
                var selectUsingFilters = baseParams.Params.GetAs("selectUsingFilters", true);
                var actualizeYearStart = baseParams.Params.GetAs("yearStart", 0);
                var actualizeYearEnd = baseParams.Params.GetAs("yearEnd", 0);
                
                var st3Ids = selectUsingFilters
                    ? programVersionService.GetDeletedEntriesQuery(baseParams).Select(x => x.Id).ToArray()
                    : baseParams.Params.GetAs<long[]>("st3Ids");

                var query = this.GetDeletedEntriesQueryable(baseParams)
                    .WhereContains(x => x.Stage2Version.Stage3Version.Id, st3Ids);

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

                    var log = new VersionActualizeLog
                    {
                        ActualizeType = VersionActualizeType.ActualizeDeletedEntries,
                        DateAction = DateTime.Now,
                        ProgramVersion = new ProgramVersion() { Id = versionId },
                        UserName = this.User.Name
                    };

                    var logRecordsDict = new Dictionary<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>();

                    log.Municipality = st3Dict.First().Key.ProgramVersion.Municipality;

                    var st3IdList = st3Dict.Keys.Select(x => x.Id).ToArray(); 

                    var publishedProgramYearsDictBySt3 = publishProgramRecordDomain.GetAll()
                        .Where(x => x.Stage2 != null)
                        .Where(x => st3IdList.Contains(x.Stage2.Stage3Version.Id))
                        .Select(x => new
                        {
                            Stage3Id = x.Stage2.Stage3Version.Id,
                            x.PublishedYear
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Stage3Id)
                        .ToDictionary(x => x.Key,
                            y => y.Select(x => x.PublishedYear).First());
                    
                    var st3ToUpdate = new List<VersionRecord>();
                    var st2ToUpdate = new List<VersionRecordStage2>();
                    foreach (var st3 in st3Dict)
                    {
                        var versionRecord = st3.Key;
                        var oldVolume = volumeDict.ContainsKey(versionRecord.Id) ? volumeDict[versionRecord.Id] : 0m;
                        var deleteVolume = deleteVolumeDict.ContainsKey(versionRecord.Id) ? deleteVolumeDict[versionRecord.Id] : 0m;

                        if (stage3IdsForDelete.Contains(versionRecord.Id))
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Удаление",
                                Description = "Не удовлетворяет условиям ДПКР",
                                Address = versionRecord.RealityObject.Address,
                                Ceo = versionRecord.CommonEstateObjects,
                                PlanYear = versionRecord.Year,
                                Volume = oldVolume,
                                Sum = versionRecord.Sum,
                                Number = versionRecord.IndexNumber
                            };

                            logRecordsDict.Add(logRecord, new VersionActualizeLogRecordAdditionInfo
                            {
                                RealityObject = versionRecord.RealityObject,
                                WorkCode = versionRecord.WorkCode,
                                PublishedYear = publishedProgramYearsDictBySt3.Get(versionRecord.Id)
                            });
                        }
                        else
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Изменение",
                                Description = "Часть КЭ не удовлетворяет условиям ДПКР",
                                Address = versionRecord.RealityObject.Address,
                                Ceo = versionRecord.CommonEstateObjects,
                                PlanYear = versionRecord.Year,
                                Volume = oldVolume,
                                Sum = versionRecord.Sum,
                                Number = versionRecord.IndexNumber
                            };
                            
                            versionRecord.Sum = st3.Value.SafeSum(y => y.Sum);
                            versionRecord.CommonEstateObjects = st3.Value.Select(y => y.CommonEstateObject.Name).Distinct().AggregateWithSeparator(", ");

                            logRecord.ChangeCeo = versionRecord.CommonEstateObjects;
                            logRecord.ChangeSum = versionRecord.Sum;
                            logRecord.ChangeVolume = oldVolume - deleteVolume;

                            logRecordsDict.Add(logRecord, new VersionActualizeLogRecordAdditionInfo
                            {
                                RealityObject = versionRecord.RealityObject,
                                WorkCode = versionRecord.WorkCode,
                                PublishedYear = publishedProgramYearsDictBySt3.Get(versionRecord.Id)
                            });

                            st3ToUpdate.Add(versionRecord);
                            st2ToUpdate.AddRange(st3.Value);
                        }
                    }

                    if (logRecordsDict.Any())
                    {
                        log.CountActions = logRecordsDict.Keys.Count;
                        log.InputParams = $"{actualizeYearStart}-{actualizeYearEnd}";
                        log.LogFile = this.LogService.CreateLogFile(logRecordsDict.Keys
                                .OrderBy(x => x.Address)
                                .ThenBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                    }

                    var sessionProvider = this.Container.Resolve<ISessionProvider>();
                    var session = sessionProvider.OpenStatelessSession();

                    try
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

                                    session.CreateSQLQuery(
                                        string.Format(@"delete from ovrhl_change_year_owner_decision where stage1_id = {0}", id))
                                           .ExecuteUpdate();

                                    // удаляем запись 1 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE1_VERSION where id = {0}", id)).ExecuteUpdate();
                                }

                                foreach (var id in stage2IdsForDelete)
                                {

                                    // удаляем записи краткосрочной программы
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_rec where stage2_id = {0}", id)).ExecuteUpdate();

                                    // удаляем Корректировки по версии
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_dpkr_correct_st2 where st2_version_id  = {0}", id)).ExecuteUpdate();

                                    // удаляем версию 1 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage1_version where stage2_version_id = {0}", id)).ExecuteUpdate();

                                    // удаляем записи опубликованной программы
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg_rec where stage2_id = {0} ", id)).ExecuteUpdate();

                                    // удаляем запись 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE2_VERSION where id = {0}", id)).ExecuteUpdate();
                                }

                                foreach (var id in stage3IdsForDelete)
                                {
                                    // удаляем версию 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where st3_version_id = {0}", id)).ExecuteUpdate();

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

                                this.LogDomain.Save(log);
                                
                                logRecordsDict.ForEach(x =>
                                {
                                    var logRecord = new VersionActualizeLogRecord
                                    {
                                        ActualizeLog = log,
                                        Action = x.Key.Action,
                                        RealityObject = x.Value.RealityObject,
                                        WorkCode = x.Value.WorkCode,
                                        Ceo = x.Key.Ceo,
                                        PlanYear = x.Key.PlanYear,
                                        ChangePlanYear = x.Key.ChangePlanYear,
                                        PublishYear = x.Key.PlanYear,
                                        Volume = x.Key.Volume,
                                        ChangeVolume = x.Key.ChangeVolume,
                                        Sum = x.Key.Sum,
                                        ChangeSum = x.Key.ChangeSum,
                                        Number = x.Key.Number,
                                        ChangeNumber = x.Key.ChangeNumber
                                    };
                                    
                                    logRecordDomain.Save(logRecord);
                                });

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
                        sessionProvider.CloseCurrentSession();
                    }

                    // Поскольку удалили чтото из версии то над опересчитать заново индекс
                    this.UpdateIndexNumber(versionId);
                }

                return new BaseDataResult();

            }
            finally
            {
                this.Container.Release(stage1Domain);
                this.Container.Release(municipalityDomain);
                this.Container.Release(logRecordDomain);
                this.Container.Release(publishProgramRecordDomain);
                this.Container.Release(programVersionService);
            }
        }

        ///<inheritdoc/>
        public IDataResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var logDomain = this.Container.ResolveDomain<VersionActualizeLog>();
            var logRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var twpHistoryDomain = this.Container.ResolveDomain<TypeWorkCrHistory>();
            var versionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var programCrDomain = this.Container.ResolveDomain<ProgramCr>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();
            var roStElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var typeWorkCrRemovalDomain = this.Container.ResolveDomain<TypeWorkCrRemoval>();
            var typeWorkCrRefDomain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var typeWorkCrDomain = this.Container.ResolveRepository<TypeWorkCr>();
            var strElWorksDomain = this.Container.ResolveDomain<StructuralElementWork>();
            var shortPrgRecordDomain = this.Container.ResolveRepository<ShortProgramRecord>();
            var correctionDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var publishPrgDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();

            try
            {
                var versionId = baseParams.Params.GetAsId("versionId");

                var programCrId = baseParams.Params.GetAsId("programCrId");
                var endPeriod = config.ProgrammPeriodEnd;

                var version = versionDomain.FirstOrDefault(x => x.Id == versionId);

                if (version == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить версию по Id {0}", versionId));
                }

                var programCr = programCrDomain.FirstOrDefault(x => x.Id == programCrId);

                if (programCr == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить программу по Id {0}", programCrId));
                }

                var crEndYear = programCr.Period.DateEnd?.Year ?? endPeriod;

                var stage1ToSave = new List<VersionRecordStage1>();
                var stage2ToSave = new List<VersionRecordStage2>();
                var stage3ToSave = new List<VersionRecord>();
                var stage1ToDelete = new List<long>();
                var stage2ToDelete = new List<long>();
                var stage3ToDelete = new List<long>();
                var stage1ExcludedFromShortProgram = new List<long>();
                var typeWorkStage1ToSave = new Dictionary<VersionRecordStage1, TypeWorkCrVersionStage1>();

                #region Подготовка данных
                // если в какойто из работ все таки небудет указан плановый год ремонта, то тогда 
                var defaultYear = programCr.Period.DateStart.Year;

                // Запрос на записи добавленные в программе
                var addHistoryQuery = twpHistoryDomain.GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId && x.TypeAction == TypeWorkCrHistoryAction.Creation);

                var ocrQuery = objectCrDomain.GetAll()
                    .Where(y => y.ProgramCr.Id == programCrId)
                    .Where(y => y.RealityObject.Municipality.Id == version.Municipality.Id
                        || y.RealityObject.MoSettlement.Id == version.Municipality.Id);

                var typeWorkQuery = typeWorkCrDomain.GetAll()
                    .Where(x => ocrQuery.Any(y => y.Id == x.ObjectCr.Id));

                var stage1ByTypeWork = typeWorkCrRefDomain.GetAll()
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x => twpHistoryDomain.GetAll().Any(y => y.TypeWorkCr.Id == x.TypeWorkCr.Id))
                    .Where(x => ocrQuery.Any(y => y.Id == x.TypeWorkCr.ObjectCr.Id))
                    .Select(x => new
                    {
                        x.TypeWorkCr.Id,
                        x.Stage1Version
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Stage1Version).ToList());

                // Получаем только работы собсвенно, которые и будут либо изменены, либо добавлены, либо удалены из ДПКР 
                var dataWorks = typeWorkQuery
                    .Where(x => x.Work.TypeWork == TypeWork.Work && x.IsActive)
                    .Where(x => twpHistoryDomain.GetAll().Any(y => y.TypeWorkCr.Id == x.Id))
                    .Select(x => new ShortRecordProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.ObjectCr.RealityObject.Id,
                        WorkId = x.Work.Id,
                        WorkName = x.Work.Name,
                        Cost = x.Sum ?? 0m,
                        TotalCost = x.Sum ?? 0m,
                        Volume = x.Volume ?? 0m,
                        ShortYearStart = x.YearRepair ?? defaultYear
                    })
                    .AsEnumerable()
                    .Select(x => new ShortRecordProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RealityObjectId,
                        WorkId = x.WorkId,
                        WorkName = x.WorkName,
                        Cost = x.Cost,
                        TotalCost = x.TotalCost,
                        Volume = x.Volume,
                        ShortYearStart = x.ShortYearStart,
                        Stage1List = stage1ByTypeWork.Get(x.Id)
                    })
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(x => x.Key, y => y.ToList());

                // полчучаем все записи 1 этапа
                var st1Query = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId);

                // получаем id записей которые были добавлены ы краткосрочке
                var st1DictHistoryCreation = typeWorkCrRefDomain.GetAll()
                    .Where(x => addHistoryQuery.Any(y => x.TypeWorkCr.Id == y.TypeWorkCr.Id))
                    .Select(x => x.Stage1Version.Id)
                    .Distinct()
                    .AsEnumerable()
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // значения старые по 3 этапу
                var oldData = this.GetOldVersionRecordBySt3(versSt1Domain, versionId);

                var st1List = st1Query.ToList();

                // получаем все записи 1 этапа сгруппированные по КЭ
                // нужно чтобы понимать по каким записям бежать если нужно удалить все элементы
                var st1DictByStrEl = st1Query
                  	.OrderBy(x => x.Year)
                    .Select(x => new
                    {
                        x.Id,
                        strElId = x.StructuralElement.StructuralElement.Id,
                        roId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId + "_" + x.strElId)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Id).ToList());

                var st1Dict = st1List.ToDictionary(x => x.Id);

                var st2Query = versSt2Domain.GetAll().Where(x => x.Stage3Version.ProgramVersion.Id == versionId);

                var st3Query = versSt3Domain.GetAll().Where(x => x.ProgramVersion.Id == versionId);

                var st2Data = st2Query
                    .Select(x => new
                    {
                        x.Id,
                        roId = x.Stage3Version.RealityObject.Id,
                        ceoId = x.CommonEstateObject.Id,
                        ceoWeight = x.CommonEstateObject.Weight,
                        x.Stage3Version.Year,
                        st2 = x
                    })
                    .ToList();

                var correctionDictBySt2 = correctionDomain.GetAll()
                    .Where(x => st2Query.Any(y => y.Id == x.Stage2.Id))
                    .Select(x => new { st2Id = x.Stage2.Id, x.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

                var publishPrgDictBySt2 = publishPrgDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                    .Where(x => st2Query.Any(y => y.Id == x.Stage2.Id))
                    .Select(x => new { st2Id = x.Stage2.Id, x.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

                var st2List = st2Data.Select(x => x.st2).ToList();

                // получаем запись 2 этапа по ключу Дом+ООИ+Год
                var st2DictByCeo = st2Data
                    .GroupBy(x => x.roId + "_" + x.ceoId + "_" + x.Year)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.st2).First());

                // получаем запись второго этапа по Id 
                var st2DictById = st2Data
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.st2).First());

                var st3DictById = st3Query
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Получаем только работы которые удалены из краткосрочки
                var dataWorksRemoval = typeWorkQuery
                    .Where(x => x.Work.TypeWork == TypeWork.Work && !x.IsActive)
                    .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.ObjectCr.RealityObject.Id,
                        WorkId = x.Work.Id,
                        Cost = x.Sum ?? 0m,
                        Volume = x.Volume ?? 0m,
                        ShortYearStart = x.YearRepair ?? defaultYear
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(x => x.Key, y => y.ToList());

                // поулчаем связку вида работ и записи 1 этапа ДПКР
                // может быть либо 1 либо несколкь оссылок на один вид работы
                // поскольку может быть 4 лифта в ДПКР по одному году, но в Объекте кр будет только 1 вид работы по Ремонту лифта
                // следовательно для удобства дальнейшей обработки я привожу либо в Тип  либ ок списку
                // чаще всег обудет Id в 1 экземпляре тоесть будет Тип
                var refQuery = typeWorkCrRefDomain.GetAll()
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId);

                var typeRefList = refQuery
                    .Select(x => new
                    {
                        x.Id,
                        TwcrId = x.TypeWorkCr.Id,
                        st1Id = x.Stage1Version.Id
                    })
                    .ToList();
                
                var typeWorkCrRefDpkr = typeRefList
                    .Select(x => new
                    {
                        x.TwcrId,
                        Stage1Version = st1Dict[x.st1Id],
                    })
                    .GroupBy(x => x.TwcrId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        Stage1 = y.Select(z => z.Stage1Version).FirstOrDefault(),
                        Stage1List = y.Select(z => z.Stage1Version).Distinct().ToList()
                    });

                // получаем по WorkId Сруктурный Элемент и ООИ в котором он находится 
                // страшная жесть но сказали что на одну работу должен быть 1 КЭ и 1 ООИ
                var strElEnumerable = strElWorksDomain.GetAll()
                    .Select(x => new WorkStrElInfo
                    {
                        WorkId = x.Job.Work.Id,
                        StrElId = x.StructuralElement.Id,
                        CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                        CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                        CeoWeight = x.StructuralElement.Group.CommonEstateObject.Weight
                    })
                    .ToList();

                // слвоарь для быстрого получения по КЭ нужное ООИ
                var ceoByStrEl = strElEnumerable
                    .GroupBy(x => x.StrElId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        CeoWeight = y.Select(z => z.CeoWeight).FirstOrDefault(),
                        CeoId = y.Select(z => z.CeoId).FirstOrDefault(),
                        CeoName = y.Select(z => z.CeoName).FirstOrDefault()
                    });

                // словарь для получения причины удаления вида работ из объекта кр
                var removalDict = typeWorkCrRemovalDomain.GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                    .AsEnumerable()
                    .GroupBy(x => x.TypeWorkCr.Id)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(o => o.ObjectEditDate).FirstOrDefault());

                #endregion

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        #region Создаем или изменяем работы в ДПКР
                        foreach (var kvp in dataWorks)
                        {
                            // После того как подсчитаны Итоговые стоимости, нужно либо изменить существующие записи
                            // Либо создать их 
                            foreach (var workData in kvp.Value)
                            {
                                if (workData.Volume == 0 && config.ActualizeConfig.ActualizeFromCrConfig.TypeSumAndVolumeUpdate == TypeUpdate.Update)
                                {
                                    return new BaseDataResult(false, "Имеются объекты в программе капитального ремонта с нулевыми объемом или суммой. Актуализация невозможна.");
                                }

                                if (workData.Stage1List != null && workData.Stage1List.Count == 1)
                                {
                                    var st1 = workData.Stage1List.First();
                                    var st2 = new VersionRecordStage2();
                                    var st3 = new VersionRecord();

                                    if (st2DictById.ContainsKey(st1.Stage2Version.Id))
                                    {
                                        st2 = st2DictById[st1.Stage2Version.Id];
                                    }
                                    if (st2.Stage3Version != null && st3DictById.ContainsKey(st2.Stage3Version.Id))
                                    {
                                        st3 = st3DictById[st2.Stage3Version.Id];
                                    }
                                    var newSt1Year = workData.ShortYearStart;
                                    var isSt1Changed = false;

                                    // Если был произведен перенос КЭ на новый год
                                    if (st1.Year != newSt1Year || st1DictHistoryCreation.ContainsKey(st1.Id))
                                    {
                                        if (st1List.Count(x => x.Stage2Version.Id == st2.Id) > 1)
                                        {
                                            // Текущий КЭ принадлежит ООИ содержащему несколько КЭ

                                            VersionRecordStage2 newSt2 = null;

                                            var keySt2 =
                                                st1.RealityObject.Id
                                                + "_" + st2.CommonEstateObject.Id
                                                + "_" + newSt1Year;

                                            // Ищем ООИ подходящий по новому году
                                            if (st2DictByCeo.ContainsKey(keySt2))
                                            {
                                                newSt2 = st2DictByCeo[keySt2];
                                            }

                                            // Создаем новый ООИ если нет подходящего
                                            if (newSt2 == null)
                                            {
                                                var newSt3 = new VersionRecord
                                                {
                                                    Year = newSt1Year,
                                                    FixedYear = true,
                                                    ProgramVersion = new ProgramVersion { Id = versionId },
                                                    RealityObject = new RealityObject { Id = st1.RealityObject.Id },
                                                    IsChangedYear = true
                                                };

                                                var ceoId = st2.CommonEstateObject.Id;
                                                var ceoName = st2.CommonEstateObject.Name;
                                                var ceoWeight = st2.CommonEstateObject.Weight;

                                                newSt2 = new VersionRecordStage2
                                                {
                                                    CommonEstateObject = new CommonEstateObject { Id = ceoId, Name = ceoName },
                                                    CommonEstateObjectWeight = ceoWeight,
                                                    Stage3Version = newSt3
                                                };

                                                st2List.Add(newSt2);
                                                st2DictByCeo.Add(keySt2, newSt2);
                                            }

                                            st1.Stage2Version = newSt2;
                                        }
                                        else
                                        {
                                            // Текущий КЭ единственный в составе ООИ
                                            st3.Year = newSt1Year;
                                            st3.IsChangedYear = true;
                                            st3.FixedYear = true;
                                            st3.IsChangedPublishYear = false;

                                            oldData[st3.Id].NewYear = newSt1Year;
                                        }

                                        st1.Year = newSt1Year;
                                        isSt1Changed = true;
                                    }

                                    if (config.ActualizeConfig.ActualizeFromCrConfig.TypeSumAndVolumeUpdate == TypeUpdate.Update)
                                    {
                                        if (st1.Sum != workData.Cost)
                                        {
                                            st1.Sum = workData.Cost;
                                            isSt1Changed = true;
                                        }

                                        if (st1.SumService != workData.ServiceCost)
                                        {
                                            st1.SumService = workData.ServiceCost;
                                            isSt1Changed = true;
                                        }

                                        if (st1.Volume != workData.Volume)
                                        {
                                            st1.Volume = workData.Volume;
                                            isSt1Changed = true;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(st3.CommonEstateObjects) && st3.CommonEstateObjects.Contains("Grouping"))
                                    {
                                        isSt1Changed = true;
                                    }

                                    if (isSt1Changed)
                                    {
                                        stage1ToSave.Add(st1);
                                    }
                                }
                                else if (workData.Stage1List != null && workData.Stage1List.Count > 1)
                                {
                                    // КЭ больше чем 1 по данному виду работы то тогда начинается самое прикольное в этом методе 
                                    // Поскольку объемы ставят на 1 работу а в каждом КЭ есть свои объемы
                                    // то не обходимо получить долю объема по КЭ относителньо объема по работе 
                                    var cnt = workData.Stage1List.Count;

                                    var i = 0;
                                    var tempCost = 0m;
                                    var tempSumService = 0m;
                                    var tempVolume = 0m;

                                    var volumeByStrEl = workData.Stage1List.Sum(x => x.Volume); // объемы забитые в домах в КЭ
                                    var proportionVolume = 0m;

                                    if (workData.Volume != volumeByStrEl)
                                    {
                                        proportionVolume = workData.Volume / cnt; // поскольку объем забитый ы ыиде работ не соответсвует объемам забитым в КЭ
                                    }

                                    // нужно разделить пропорционально стоиомсть между всеми записями
                                    foreach (var st1 in workData.Stage1List)
                                    {
                                        var oldSum = st1.Sum;
                                        var oldService = st1.SumService;
                                        var oldVolume = st1.Volume;

                                        i++;

                                        decimal currentCost = 0;
                                        decimal currentServiceCost = 0;
                                        decimal curretVolume = 0;

                                        if (i == cnt)
                                        {
                                            // Если работа последняя, то избегаем ошибок округления
                                            curretVolume = workData.Volume - tempVolume;
                                            currentCost = workData.Cost - tempCost;
                                            currentServiceCost = workData.ServiceCost - tempSumService;
                                        }
                                        else if (config.ActualizeConfig.ActualizeFromCrConfig.TypeSumAndVolumeUpdate == TypeUpdate.Update)
                                        {
                                            // вычисляем сумму услуги исходя из доли каждой работы в общей стоимости
                                            curretVolume = proportionVolume > 0 ? proportionVolume : st1.Volume;
                                            currentCost = ((curretVolume / workData.Volume) * workData.Cost).RoundDecimal(2);
                                            currentServiceCost = ((curretVolume / workData.Volume) * workData.ServiceCost).RoundDecimal(2);
                                        }

                                        if (config.ActualizeConfig.ActualizeFromCrConfig.TypeSumAndVolumeUpdate == TypeUpdate.Update)
                                        {
                                            st1.Sum = currentCost;
                                            st1.SumService = currentServiceCost;
                                            st1.Volume = curretVolume;
                                        }

                                        if (st1.Id > 0 && st1.Stage2Version.Id > 0)
                                        {
                                            var st2 = st2DictById[st1.Stage2Version.Id];
                                            var st3 = st3DictById[st2.Stage3Version.Id];
                                            var newYear = 0;

                                            if (st3.Year != workData.ShortYearStart || st1DictHistoryCreation.ContainsKey(st1.Id) || (!string.IsNullOrEmpty(st3.CommonEstateObjects) && st3.CommonEstateObjects.Contains("Grouping")))
                                            {
                                                newYear = workData.ShortYearStart;
                                                if (st3.Year != 0 && st3.Year != newYear)
                                                {
                                                    st3.IsChangedYear = true;
                                                }
                                                st3.Year = newYear;
                                                st3.FixedYear = true;
                                                st3.IsChangedPublishYear = false;
                                                st2.Stage3Version = st3;
                                            }

                                            if (st1.Sum != oldSum || st1.SumService != oldService || st1.Volume != oldVolume || newYear > 0)
                                            {
                                                // добавляем в список на сохранение
                                                if (newYear > 0)
                                                {
                                                    st1.Year = newYear;
                                                }

                                                stage1ToSave.Add(st1);
                                            }

                                        }

                                        // Для того чтобы не было ошибок округления накапливаем Стоимость услуги
                                        tempCost += currentCost;
                                        tempSumService += currentServiceCost;
                                        tempVolume += curretVolume;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Удаляем работы из Дпкр

                        var st2Disconnected = new List<long>();

                        foreach (var kvp in dataWorksRemoval)
                        {
                            var roId = kvp.Key;

                            foreach (var work in kvp.Value)
                            {
                                if (!removalDict.ContainsKey(work.Id))
                                {
                                    continue;
                                }

                                var removal = removalDict[work.Id];

                                switch (removal.TypeReason)
                                {
                                    // Перенос срока ремонта на более поздний срок
                                    case TypeWorkCrReason.NewYear:
                                        {
                                            if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                            {
                                                var refData = typeWorkCrRefDpkr[work.Id];

                                                var newYearRepair = removal.NewYearRepair.GetValueOrDefault();

                                                foreach (var refSt1 in refData.Stage1List)
                                                {
                                                    // получаем записи которые хотят перекинуть на новый год
                                                    // соответственно получаем что необходимо сдвинуть на нужное количество лет все КЭ программы, 
                                                    // а это значит, что возможно нужно будет и перегруппировать 1, 2, 3 этапы

                                                    var delta = newYearRepair - refSt1.Stage2Version.Stage3Version.Year;

                                                    if (delta == 0)
                                                    {
                                                        continue;
                                                    }

                                                    var key = roId + "_" + refSt1.StructuralElement.StructuralElement.Id;

                                                    if (!st1DictByStrEl.ContainsKey(key))
                                                    {
                                                        continue;
                                                    }

                                                    var ceoId = 0L;
                                                    var ceoWeight = 0;
                                                    var ceoName = string.Empty;
                                                    if (ceoByStrEl.ContainsKey(refSt1.StructuralElement.StructuralElement.Id))
                                                    {
                                                        ceoId = ceoByStrEl[refSt1.StructuralElement.StructuralElement.Id].CeoId;
                                                        ceoWeight = ceoByStrEl[refSt1.StructuralElement.StructuralElement.Id].CeoWeight;
                                                        ceoName = ceoByStrEl[refSt1.StructuralElement.StructuralElement.Id].CeoName;
                                                    }

                                                    foreach (var st1Id in st1DictByStrEl[key])
                                                    {
                                                        var st1 = st1Dict[st1Id];

                                                        var oldSt2 = st2DictById[st1.Stage2Version.Id];
                                                        var oldSt3 = st3DictById[oldSt2.Stage3Version.Id];
                                                        var oldYear = oldSt3.Year;

                                                        var fixedYear = st1Id == refSt1.Id;

                                                        var newYear = oldYear + delta;

                                                        if (oldYear >= newYear)
                                                        {
                                                            continue;
                                                        }

                                                        if (newYear > endPeriod && oldYear > endPeriod)
                                                        {
                                                            stage1ToDelete.Add(st1Id);
                                                            continue;
                                                        }

                                                        var keySt2 = roId + "_" + ceoId + "_" + newYear;

                                                        VersionRecordStage2 st2;
                                                        if (st2DictByCeo.ContainsKey(keySt2))
                                                        {
                                                            st2 = st2DictByCeo[keySt2];

                                                            if (st3DictById.ContainsKey(st2.Stage3Version.Id))
                                                            {
                                                                var st3 = st3DictById[st2.Stage3Version.Id];
                                                                if (st3.Year != 0 && st3.Year != newYear)
                                                                {
                                                                    st3.IsChangedYear = true;
                                                                }
                                                                st3.Year = newYear;
                                                                st3.FixedYear = fixedYear;
                                                                st3.IsChangedPublishYear = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oldData[oldSt2.Stage3Version.Id].NewYear = newYear;

                                                            var st3 = new VersionRecord
                                                            {
                                                                Year = newYear,
                                                                FixedYear = fixedYear,
                                                                ProgramVersion = new ProgramVersion { Id = versionId },
                                                                RealityObject = new RealityObject { Id = roId },
                                                                IsChangedYear = true
                                                            };

                                                            st2 = new VersionRecordStage2
                                                            {
                                                                CommonEstateObject = new CommonEstateObject { Id = ceoId, Name = ceoName },
                                                                CommonEstateObjectWeight = ceoWeight,
                                                                Stage3Version = st3
                                                            };

                                                            st2List.Add(st2);
                                                            stage2ToSave.Add(st2);

                                                            st2DictByCeo.Add(keySt2, st2);
                                                        }

                                                        st1.Year = newYear;
                                                        st1.Stage2Version = st2;
                                                        st2Disconnected.Add(oldSt2.Id);
                                                        stage1ToSave.Add(st1);

                                                        if (newYearRepair > crEndYear)
                                                        {
                                                            stage1ExcludedFromShortProgram.Add(st1.Id);
                                                        }

                                                        if (config.ActualizeConfig.ActualizeFromCrConfig.RecalcOtherEntries == TypeUsage.NoUsed)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (st2Disconnected.Any())
                                                {
                                                    st2Disconnected.RemoveAll(x => st1List.Any(s => s.Stage2Version.Id == x));
                                                }
                                            }
                                        }
                                        break;

                                    // Не требуется ремонт в рамках Долгосрочной программы
                                    // Значит необходимо найти все КЭ в ДПКР и удалить их
                                    case TypeWorkCrReason.NotRequiredDpkr:
                                        {
                                            if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                            {
                                                var refData = typeWorkCrRefDpkr[work.Id];
                                               
                                                foreach (var refSt1 in refData.Stage1List)
                                                {
                                                    var key = roId + "_" + refSt1.StructuralElement.StructuralElement.Id;
                                                    
                                                    if (st1DictByStrEl.ContainsKey(key))
                                                    {
                                                        // для каждого КЭ получаем все записи 1 этапа и удаляем их
                                                        foreach (var st1Id in st1DictByStrEl[key])
                                                        {
                                                            stage1ToDelete.Add(st1Id);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;

                                    // Не требуется ремонт в рамках текущей Краткосрочной программы
                                    // Значит необходимо удалиить только запись по ссылке на ДПКР ,но удалять другие КЭ  других годах ненужно
                                    case TypeWorkCrReason.NotRequiredShortProgram:
                                        {
                                            if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                            {
                                                var refData = typeWorkCrRefDpkr[work.Id];

                                                foreach (var refSt1 in refData.Stage1List)
                                                {
                                                    stage1ToDelete.Add(refSt1.Id);
                                                }
                                            }
                                        }
                                        break;

                                }
                            }
                        }
                        #endregion

                        // после того как определили полный список добавляемых, изменяемых и удаляемых объектов 1 этапа 
                        // не обходимо либо обновить или удалить записи 2го и 3го этапав

                        #region обрабатываем записи 2 этапа
                        var st2NewData = st1List
                                            .Where(x => !stage1ToDelete.Contains(x.Id) && !st2Disconnected.Contains(x.Stage2Version.Id))
                                            .GroupBy(x => x.Stage2Version)
                                            .ToDictionary(x => x.Key, y => new
                                            {
                                                sum = y.SafeSum(z => z.Sum + z.SumService),
                                                volume = y.SafeSum(z => z.Volume)
                                            });

                        foreach (var kvp in st2NewData)
                        {
                            var st2 = kvp.Key;
                            var oldSum = st2.Sum;
                            st2.Sum = kvp.Value.sum;

                            if (st2.Id == 0)
                            {
                                stage2ToSave.Add(st2);
                            }
                            else if (oldSum != st2.Sum)
                            {
                                stage2ToSave.Add(st2);
                            }
                        }

                        stage2ToDelete.AddRange(st2DictById.Keys.Where(x => !st2NewData.Keys.Any(y => y.Id == x)).ToList().Union(st2Disconnected.ToList()));
                        #endregion

                        // формируем логи актуализации
                        var logRecordsDict = new Dictionary<ActualizeVersionLogRecord, VersionActualizeLogRecordAdditionInfo>();

                        #region обрабатываем записи 3 этапа
                        var st3NewData = st2List
                                            .Where(x => !stage2ToDelete.Contains(x.Id))
                                            .GroupBy(x => x.Stage3Version)
                                            .ToDictionary(x => x.Key, y => new
                                            {
                                                sum = y.Sum(st2 => st2.Sum),
                                                volume = y.SafeSum(st2 => st1List.Where(st1 => st1.Stage2Version.Equals(st2) && !stage1ToDelete.Contains(st1.Id)).Select(st1 => st1.Volume).SafeSum()),
                                                ceoNames = y.Select(st2 => st2.CommonEstateObject.Name).ToList().Any() ?
                                                                y.Select(st2 => st2.CommonEstateObject.Name).ToList().Distinct().Aggregate((str, res) => !string.IsNullOrEmpty(res) ? res + ", " + str : res) : string.Empty,
                                                ceoIds = y.Select(st2 => st2.CommonEstateObject.Id).ToArray()
                                            });

                        var publishedProgramYearsDictBySt3 = publishPrgDomain.GetAll()
                            .Where(x => x.Stage2 != null)
                            .Where(x => st3DictById.Keys.Contains(x.Stage2.Stage3Version.Id))
                            .Select(x => new
                            {
                                Stage3Id = x.Stage2.Stage3Version.Id,
                                x.PublishedYear
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.Stage3Id)
                            .ToDictionary(x => x.Key,
                                y => y.Select(x => x.PublishedYear).First());

                        foreach (var kvp in st3NewData)
                        {
                            var st3 = kvp.Key;

                            st3.Sum = kvp.Value.sum;
                            st3.CommonEstateObjects = kvp.Value.ceoNames;

                            if (st3.Id == 0)
                            {
                                stage3ToSave.Add(st3);

                                var ro = roDomain.Load(st3.RealityObject.Id);

                                var logRecord = new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                    Action = "Добавление",
                                    Description = "Добавление при Актуализации из КПКР",
                                    Address = ro.Address,
                                    Ceo = st3.CommonEstateObjects,
                                    PlanYear = st3.Year,
                                    Sum = st3.Sum,
                                    Volume = kvp.Value.volume,
                                    ChangeVolume = 0
                                };

                                logRecordsDict.Add(logRecord, new VersionActualizeLogRecordAdditionInfo
                                {
                                    RealityObject = st3.RealityObject,
                                    WorkCode = st3.WorkCode,
                                    PublishedYear = publishedProgramYearsDictBySt3[st3.Id]
                                });
                            }
                            else
                            {
                                var affectedWorks = strElEnumerable
                                        .Where(i => kvp.Value.ceoIds.Contains(i.CeoId))
                                        .Select(e => e.WorkId)
                                        .ToArray();

                                var newVolume = 0M;

                                if (config.ActualizeConfig.ActualizeFromCrConfig.TypeSumAndVolumeUpdate == TypeUpdate.Update)
                                {
                                    var newWorkData = dataWorks.Get(kvp.Key.RealityObject.Id) ?? new List<ShortRecordProxy>();
                                    IEnumerable<ShortRecordProxy> affectedWorksData = newWorkData.Where(i => affectedWorks.Contains(i.WorkId) && i.ShortYearStart == st3.Year);
                                    newVolume = affectedWorksData.Any() ? affectedWorksData.SafeSum(w => w.Volume) : -1;
                                }
                                else
                                {
                                    newVolume = kvp.Value.volume;
                                }

                                if (oldData.ContainsKey(st3.Id)
                                                  && (
                                                      oldData[st3.Id].Sum != st3.Sum
                                                      || oldData[st3.Id].CeoNames != st3.CommonEstateObjects
                                                      || oldData[st3.Id].Year != st3.Year
                                                      || oldData[st3.Id].FixedYear != st3.FixedYear
                                                      || (newVolume != -1 && oldData[st3.Id].Volume != newVolume)
                                                      ))
                                {

                                    newVolume = Math.Max(newVolume, 0);

                                    var old = oldData[kvp.Key.Id];
                                    var logRecord = new ActualizeVersionLogRecord
                                    {
                                        TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                        Action = "Изменение" + " (" + programCr.Name + ")",
                                        Description = "Изменение при Актуализации из КПКР",
                                        Address = old.Address,
                                        Ceo = old.CeoNames,
                                        PlanYear = old.Year,
                                        Volume = old.Volume,
                                        Sum = old.Sum,
                                        ChangeVolume = newVolume != old.Volume ? newVolume : 0,
                                        Number = old.Number,
                                        ChangeNumber = st3.IndexNumber != old.Number ? st3.IndexNumber : 0,
                                        ChangeSum = st3.Sum != old.Sum ? st3.Sum : 0m,
                                        ChangeCeo =
                                            st3.CommonEstateObjects != old.CeoNames
                                                ? st3.CommonEstateObjects
                                                : null,
                                        ChangePlanYear = st3.Year != old.Year ? st3.Year : 0
                                    };

                                    logRecordsDict.Add(logRecord, new VersionActualizeLogRecordAdditionInfo
                                    {
                                        RealityObject = new RealityObject { Id = st3.RealityObject.Id },
                                        WorkCode = old.WorkCode,
                                        PublishedYear = publishedProgramYearsDictBySt3[st3.Id]
                                    });

                                    //планируемый год изменился
                                    st3.IsChangedYear = st3.Year != old.Year;
                                    stage3ToSave.Add(st3);
                                }
                            }
                        }

                        var notExistSt3Ids = st3DictById.Keys.Where(x => !st3NewData.Keys.Any(y => y.Id == x)).ToList();

                        foreach (var delId in notExistSt3Ids)
                        {
                            if (!oldData.ContainsKey(delId))
                            {
                                continue;
                            }

                            var old = oldData[delId];

                            if (old.NewYear == 0)
                            {
                                var logRecord = new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                    Action = "Удаление",
                                    Description = "Удаление при Актуализации из КПКР",
                                    Address = old.Address,
                                    Ceo = old.CeoNames,
                                    PlanYear = old.Year,
                                    Volume = old.Volume,
                                    Sum = old.Sum,
                                    Number = old.Number
                                };

                                logRecordsDict.Add(logRecord, new VersionActualizeLogRecordAdditionInfo
                                {
                                    RealityObject = new RealityObject { Id = old.RoId },
                                    WorkCode = old.WorkCode,
                                    PublishedYear = publishedProgramYearsDictBySt3[delId]
                                });
                            }
                            else
                            {
                                //Search existing logRecord by ceoName, NewYear, Address
                                //Change TypeAction
                                var duplicate = logRecordsDict.Keys.FirstOrDefault(x => x.Ceo == old.CeoNames && x.Address == old.Address && x.PlanYear == old.NewYear);
                                duplicate.Action = "Изменение";
                                duplicate.Description = "Изменение при Актуализации из КПКР";
                                duplicate.ChangePlanYear = old.NewYear;
                                duplicate.PlanYear = old.Year;
                            }
                        }

                        stage3ToDelete.AddRange(notExistSt3Ids);
                        #endregion

                        if (!logRecordsDict.Any())
                        {
                            return new BaseDataResult(false, "Изменений в программе капитального ремонта не обнаружено. Актуализация не будет проведена.");
                        }

                        if (!stage3ToSave.Any()
                            && !stage2ToSave.Any()
                            && !stage1ToSave.Any()
                            && !typeWorkStage1ToSave.Any()
                            && !stage1ToDelete.Any()
                            && !stage2ToDelete.Any()
                            && !stage3ToDelete.Any())
                        {
                            return new BaseDataResult(false, "Нет записей для сохранения");
                        }

                        stage3ToSave.ForEach(vr => vr.Changes = string.Format("Изменено: {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));

                        stage3ToSave.ForEach(versSt3Domain.SaveOrUpdate);

                        stage2ToSave.ForEach(versSt2Domain.SaveOrUpdate);

                        stage1ToSave.ForEach(versSt1Domain.SaveOrUpdate);

                        typeWorkStage1ToSave.Values.ForEach(x => typeWorkCrRefDomain.Save(x));

                        if (stage1ToDelete.IsNotEmpty())
                        {
                            typeWorkCrRefDomain.GetAll()
                                .Where(x => stage1ToDelete.Contains(x.Stage1Version.Id))
                                .Select(x => x.Id)
                                .AsEnumerable()
                                .ForEach(x => typeWorkCrRefDomain.Delete(x));
                            
                            stage1ToDelete.ForEach(x => versSt1Domain.Delete(x));
                        }

                        //удаляем связь с КПКР, если работы вынесли за пределы КПКР
                        typeWorkCrRefDomain.GetAll()
                        .WhereContains(x => x.Stage1Version.Id, stage1ExcludedFromShortProgram)
                        .Select(x => x.Id)
                        .ForEach(x => typeWorkCrRefDomain.Delete(x));

                        foreach (var id in stage2ToDelete)
                        {
                            // сначала удаляем связи
                            if (correctionDictBySt2.ContainsKey(id))
                            {
                                correctionDomain.Delete(correctionDictBySt2[id]);
                            }

                            if (publishPrgDictBySt2.ContainsKey(id))
                            {
                                publishPrgDomain.Delete(publishPrgDictBySt2[id]);
                            }

                            var shortIds = shortPrgRecordDomain.GetAll()
                                .Where(x => x.Stage2.Id == id)
                                .Select(x => x.Id)
                                .ToArray();

                            // необходимо очистить подчиненные записи для VersionRecordStage
                            foreach (var recordId in shortIds)
                            {
                                shortPrgRecordDomain.Delete(recordId);
                            }

                            versSt2Domain.Delete(id);
                        }

                        stage3ToDelete.ForEach(id => versSt3Domain.Delete(id));

                        var logFile = this.LogService.CreateLogFile(
                            logRecordsDict.Keys.OrderBy(x => x.Address).ThenBy(x => x.Number),
                            baseParams);

                        var log = new VersionActualizeLog
                        {
                            ActualizeType = VersionActualizeType.ActualizeFromShortCr,
                            DateAction = DateTime.Now,
                            Municipality = version.Municipality,
                            ProgramVersion = new ProgramVersion { Id = versionId },
                            UserName = this.User.Name,
                            CountActions = logRecordsDict.Keys.Count,
                            ProgramCrName = programCr.Name,
                            InputParams = programCr.Name,
                            LogFile = logFile
                        };

                        logDomain.Save(log);

                        logRecordsDict.ForEach(x =>
                        {
                            var vaLogRecord = new VersionActualizeLogRecord
                            {
                                ActualizeLog = log,
                                Action = x.Key.Action,
                                RealityObject = x.Value.RealityObject,
                                WorkCode = x.Value.WorkCode,
                                Ceo = x.Key.Ceo,
                                PlanYear = x.Key.PlanYear,
                                ChangePlanYear = x.Key.ChangePlanYear,
                                PublishYear = x.Value.PublishedYear,
                                Volume = x.Key.Volume,
                                ChangeVolume = x.Key.ChangeVolume,
                                Sum = x.Key.Sum,
                                ChangeSum = x.Key.ChangeSum,
                                Number = x.Key.Number,
                                ChangeNumber = x.Key.ChangeNumber
                            };

                            logRecordDomain.Save(vaLogRecord);
                        });

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
                this.Container.Release(logDomain);
                this.Container.Release(logRecordDomain);
                this.Container.Release(programCrDomain);
                this.Container.Release(versSt3Domain);
                this.Container.Release(versSt2Domain);
                this.Container.Release(versSt1Domain);
                this.Container.Release(versionDomain);
                this.Container.Release(typeWorkCrDomain);
                this.Container.Release(typeWorkCrRefDomain);
                this.Container.Release(typeWorkCrRemovalDomain);
                this.Container.Release(strElWorksDomain);
                this.Container.Release(roStElDomain);
                this.Container.Release(objectCrDomain);
                this.Container.Release(correctionDomain);
                this.Container.Release(publishPrgDomain);
                this.Container.Release(shortPrgRecordDomain);
            }

            return new BaseDataResult();
        }

        private void UpdateIndexNumber(long versionId)
        {
            /*
             Тут короче я прохожу по всем записям версии и просто проверяют если ест ькакието пустоты то заполняю их нужными индексами
             следовательно вся очередь уплотняется к наименьшему значению
            */

            var stage3Domain = this.Container.Resolve<IDomainService<VersionRecord>>();

            try
            {
                var data = stage3Domain.GetAll().Where(x => x.ProgramVersion.Id == versionId)
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

                using (var tr = this.Container.Resolve<IDataTransaction>())
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
                this.Container.Release(stage3Domain);
            }
        }

        /// <summary>
        /// Получение сообщения
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetWarningMessage(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var action = baseParams.Params.GetAs<VersionActualizeType>("action");
            var typeWorkSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var stage3Period = config.GroupByRoPeriod;

            using (this.Container.Using(typeWorkSt1Domain))
            {
                if (stage3Period > 0)
                {
                    return new BaseDataResult(false, @"Актуализация недоступна при периоде группировки больше 0");
                }
                if (config.ActualizeConfig.TransferSingleStructElAtCostUpdating == TypeUsage.Used && action == VersionActualizeType.ActualizeSum)
                {

                    var years = typeWorkSt1Domain.GetAll()
                         .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                            .Where(
                                x =>
                                    x.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr !=
                                    TypeVisibilityProgramCr.Hidden ||
                             x.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr != TypeProgramStateCr.Close)
                         .Select(x => new
                         {
                             x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateStart,
                             x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd
                         })
                         .ToList();

                    if (years.Any())
                    {
                        var minYear = years.SafeMin(x => x.DateStart).Return(x => x.Year);
                        var maxYear = years.SafeMax(x => x.DateEnd.ToDateTime()).Return(x => x.Year);

                        var period = minYear == maxYear ? minYear.ToStr() : "{0}-{1}".FormatUsing(minYear, maxYear);

                        return new BaseDataResult(true, @"Создана краткосрочная программа за период {0}. 
                                    Данные за этот период актуализированы не будут. Продолжить?".FormatUsing(period));
                    }
                }
                return new BaseDataResult(null);
            }
        }

        /// <inheritdoc />
        public IDataResult SplitWork(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAsId("versionId");
            var stage1Id = baseParams.Params.GetAsId("stage1Id");

            var versionRecordStage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();

            using (this.Container.Using(versionRecordStage1Domain, publishedProgramRecordDomain, programVersionDomain))
            {
                var stage1 = versionRecordStage1Domain.Get(stage1Id);

                if (stage1.IsNull())
                {
                    return new BaseDataResult(false, "Не удалось найти работу");
                }

                var stage2 = stage1.Stage2Version;
                var stage3 = stage2.Stage3Version;

                var publishedProgramRecord = publishedProgramRecordDomain
                    .FirstOrDefault(x => x.Stage2 != null &&
                        x.Stage2.Id == stage2.Id);

                if (publishedProgramRecord.IsNotNull() &&
                    publishedProgramRecord.PublishedYear <= DateTime.Now.Year)
                {
                    return new BaseDataResult(false, "Работа была завершена, разделение недоступно");
                }

                var version = programVersionDomain.Get(versionId);

                var workCodes = versionRecordStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion == version)
                    .Where(x => x.RealityObject == stage1.RealityObject)
                    .Where(x => x.StructuralElement == stage1.StructuralElement)
                    .Where(x => !publishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PublishedYear <= DateTime.Now.Year))
                    .AsEnumerable()
                    .Select(x => this.SplitWork(x, version))
                    .ToList();

                return new BaseDataResult(new { WorkCodes = string.Join(", ", workCodes) });
            }
        }

        /// <summary>
        /// Разделение работы
        /// </summary>
        /// <param name="stage1">Работа по 1 этапу</param>
        /// <param name="version">Версия программы</param>
        /// <returns>Код новой работы</returns>
        private string SplitWork(VersionRecordStage1 stage1, ProgramVersion version)
        {
            var versionRecordStage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versionRecordStage2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var versionActualizeLogDomain = this.Container.ResolveDomain<VersionActualizeLog>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();

            using (this.Container.Using(versionRecordStage1Domain, versionRecordStage2Domain,
                versionRecordDomain, versionActualizeLogDomain, versionActualizeLogRecordDomain))
            {
                var stage2 = stage1.Stage2Version;
                var stage3 = stage2.Stage3Version;

                var oldWorkInfo = versionRecordStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Id == stage2.Id)
                    .Select(x => new
                    {
                        Stage2Id = x.Stage2Version.Id,
                        x.Sum,
                        x.SumService,
                        x.Volume
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Stage2Id)
                    .Select(x => new
                    {
                        Sum = x.Select(y => y.Sum + y.SumService).Sum(),
                        Volume = x.Select(y => y.Volume).Sum()
                    }).First();

                var ceoCode = stage2.CommonEstateObject.Code.PadLeft(3, '0');

                var stage3Query = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == version.Id);

                var lastWorkCode = stage3Query
                    .Where(x => x.RealityObject.Id == stage3.RealityObject.Id && x.WorkCode != null)
                    .OrderBy(x => x.WorkCode)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        CeoCode = x.WorkCode.Substring(x.WorkCode.Length - 5, 3),
                        Order = x.WorkCode.Substring(x.WorkCode.Length - 2).ToInt()
                    })
                    .Last(x => x.CeoCode == ceoCode);

                var newStage3 = new VersionRecord
                {
                    ProgramVersion = stage3.ProgramVersion,
                    RealityObject = stage3.RealityObject,
                    Year = stage3.Year,
                    CommonEstateObjects = stage3.CommonEstateObjects,
                    Sum = stage1.Sum + stage1.SumService,
                    Point = stage3.Point,
                    StoredCriteria = stage3.StoredCriteria,
                    StoredPointParams = stage3.StoredPointParams,
                    Changes = $"Добавлено: {DateTime.Now:dd.MM.yyyy HH:mm}",
                    WorkCode = $"{stage3.RealityObject.Id}{lastWorkCode.CeoCode}{lastWorkCode.Order + 1:00}"
                };

                var newStage2 = new VersionRecordStage2
                {
                    CommonEstateObject = stage2.CommonEstateObject,
                    Stage3Version = newStage3,
                    CommonEstateObjectWeight = stage2.CommonEstateObjectWeight,
                    Sum = stage1.Sum + stage1.SumService
                };

                stage1.Stage2Version = newStage2;

                // Корректируем суммы после переноса работы
                stage2.Sum = oldWorkInfo.Sum - newStage2.Sum;
                stage3.Sum = oldWorkInfo.Sum - newStage3.Sum;
                stage3.Changes = $"Изменено: {DateTime.Now:dd.MM.yyyy HH:mm}";

                // Пересчитываем порядок работ
                this.ChangeIndexNumber(new List<VersionRecord> { stage3, newStage3 }, stage3Query);

                this.Container.InTransaction(() =>
                {
                    var log = new VersionActualizeLog
                    {
                        ActualizeType = VersionActualizeType.SplitWork,
                        DateAction = DateTime.Now,
                        Municipality = version.Municipality,
                        ProgramVersion = version,
                        UserName = this.User.Name,
                        CountActions = 2 //Изменение разделяемой + добавление новой
                    };

                    versionActualizeLogDomain.Save(log);

                    new[]
                    {
                        new VersionActualizeLogRecord
                        {
                            ActualizeLog = log,
                            Action = "Изменение",
                            RealityObject = stage3.RealityObject,
                            WorkCode = stage3.WorkCode,
                            Ceo = stage3.CommonEstateObjects,
                            PlanYear = stage3.Year,
                            Volume = oldWorkInfo.Volume,
                            ChangeVolume = oldWorkInfo.Volume - stage1.Volume,
                            Sum = oldWorkInfo.Sum,
                            ChangeSum = stage3.Sum,
                            Number = stage3.IndexNumber
                        },
                        new VersionActualizeLogRecord
                        {
                            ActualizeLog = log,
                            Action = "Добавление",
                            RealityObject = newStage3.RealityObject,
                            WorkCode = newStage3.WorkCode,
                            Ceo = newStage3.CommonEstateObjects,
                            PlanYear = newStage3.Year,
                            Volume = stage1.Volume,
                            Sum = newStage3.Sum,
                            Number = newStage3.IndexNumber
                        }
                    }.ForEach(x =>
                    {
                        versionActualizeLogRecordDomain.Save(x);
                    });

                    versionRecordStage2Domain.Update(stage2);
                    versionRecordDomain.Update(stage3);

                    versionRecordDomain.Save(newStage3);
                    versionRecordStage2Domain.Save(newStage2);
                    versionRecordStage1Domain.Update(stage1);
                });

                return newStage3.WorkCode;
            }
        }

        private IDataResult GetNewRecords(
            BaseParams baseParams,
            out List<VersionRecord> ver3S,
            out List<VersionRecordStage2> ver2S,
            out List<VersionRecordStage1> ver1S,
            Dictionary<long, RoStrElAfterRepair> afterRepaired = null,
            List<long> roStrElList = null,
            List<string> existRoStrInYear = null)
        {
            ver3S = new List<VersionRecord>();
            ver2S = new List<VersionRecordStage2>();
            ver1S = new List<VersionRecordStage1>();

            var lastWorkCodeOrderDict = new Dictionary<string, int>();
            
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var unProxy = this.Container.Resolve<IUnProxy>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var correctDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var roStructElService = this.Container.Resolve<IRealityObjectStructElementService>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            using (this.Container.Using(programVersionDomain,
                    roStructElDomain,
                    longProgramService,
                    versSt1Domain,
                    versSt2Domain,
                    versSt3Domain,
                    sessionProvider,
                    unProxy,
                    correctDomain,
                    roStructElService))
            {
                var version = programVersionDomain.Get(versionId);

                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var stage3Period = config.GroupByRoPeriod;
                var fromActualizeYear = baseParams.Params.GetAs<bool>("FromActualizeYear");

                // Генерировать WorkCode для VersionRecord
                var needGenerateWorkCode = baseParams.Params.GetAs<bool>("needGenerateWorkCode");

                var selectUsingFilters = baseParams.Params.GetAs("selectUsingFilters", true);
                var roStructElIds = baseParams.Params.GetAs<long[]>("roStructElIds");

                var useSelectiveActualize = config.ActualizeConfig.UseSelectiveActualize == TypeUsage.Used;

                IQueryable<RealityObjectStructuralElement> newStructQuery;

                if (useSelectiveActualize)
                {
                    newStructQuery = this.GetAddEntriesQueryable(baseParams, afterRepaired, roStrElList)
                        .WhereIfContains(roStrElList != null, x => x.Id, roStrElList)
                        .WhereIfContains(!selectUsingFilters && roStructElIds.IsNotEmpty(), x => x.Id, roStructElIds);
                }
                else
                {
                    var startYear = this.GetPeriodStartYear(version);

                    var versSt1Query = versSt1Domain.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                        .WhereIf(
                            afterRepaired != null && afterRepaired.Any(),
                            x => x.Stage2Version.Stage3Version.Year >= startYear &&
                                ((correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && y.PlanYear >= actualizeStart))
                                    || !correctDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id)));

                    newStructQuery = roStructElService.GetElementsForLongProgram(roStructElDomain)
                        .Where(x => !x.RealityObject.State.FinalState)
                        .Where(
                            x =>
                                x.RealityObject.Municipality.Id == version.Municipality.Id ||
                                    x.RealityObject.MoSettlement.Id == version.Municipality.Id)
                        .WhereIf(
                            roStrElList != null,
                            x => roStrElList.Contains(x.Id) || !versSt1Query.Any(y => y.StructuralElement.Id == x.Id))
                        .WhereIf(roStrElList == null, x => !versSt1Query.Any(y => y.StructuralElement.Id == x.Id))
                        .Where(
                            x =>
                                !versSt1Query.Any(
                                    y =>
                                        x.Id == y.StructuralElement.Id
                                            && (!fromActualizeYear
                                                || (y.Stage2Version.Stage3Version.Year >= startYear && y.Stage2Version.Stage3Version.Year >= actualizeStart))));
                }

                if (!newStructQuery.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var stage2VerRecs = versSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new
                    {
                        Record = x,
                        RoId = x.Stage3Version.RealityObject.Id,
                        CeoId = x.CommonEstateObject.Id,
                        x.Stage3Version.Year
                    })
                    .ToList()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.RoId, x.CeoId, x.Year))
                    .ToDictionary(
                        x => x.Key,
                        y => y.First().Record);

                var versSt3Query = versSt3Domain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId);

                var stage3VerRecs = versSt3Query
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}".FormatUsing(x.RealityObject.Id, x.Year))
                    .ToDictionary(x => x.Key, y => y.First());

                var stage1ToSave = longProgramService.GetStage1(actualizeStart, version.Municipality.Id,
                    newStructQuery, null, true);
                
                if (afterRepaired != null && afterRepaired.Any())
                {
                    var stage1ByRoStrEl = stage1ToSave.GroupBy(x => x.StructuralElement.Id)
                                                      .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Year).ToList());

                    // необходимо понят ькакие записи уже существуют в нетронутых годах и относительно сроков эксплуатации сдвигать последующие записи
                    foreach (var kvp in afterRepaired)
                    {
                        if (!stage1ByRoStrEl.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        // отталкиваемся от того что данный кэ уже учтен в какихто годах а значит берем этот год как год ремонта и прибавляем срок эксплуатации
                        var nextYear = kvp.Value.LastYearRepair + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);

                        // всем записям которые неполностью удалились из дпкрприсваиваем год относителньо года последней записи находящейся в версии
                        foreach (var roStrEl in stage1ByRoStrEl[kvp.Key])
                        {
                            roStrEl.Year = nextYear;

                            nextYear = roStrEl.Year + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);
                        }
                    }
                }

                if (!existRoStrInYear.IsEmpty())
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

                var stage2ToSave = longProgramService.GetStage2(stage1ToSave);
                var stage3ToSave = longProgramService.GetStage3(stage2ToSave);

                var stage2Dict = stage2ToSave
                    .GroupBy(x => x.Stage3)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                var stage1Dict = stage1ToSave
                    .GroupBy(x => x.Stage2)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                if (needGenerateWorkCode)
                {
                    lastWorkCodeOrderDict = this.GetLastWorkCodeOrderDict(stage3VerRecs);
                }

                var stage3Ordered = stage3ToSave.Where(x => !fromActualizeYear || x.Year >= actualizeStart)
                    .OrderBy(x => x.Year);

                foreach (var stage3 in stage3Ordered)
                {
                    var ver3 = stage3Period > 0
                        ? stage3VerRecs.Get("{0}_{1}".FormatUsing(stage3.RealityObject.Id, stage3.Year))
                        : null;

                    var realityObject = roDomain.Get(stage3.RealityObject.Id);

                    ver3 = ver3 ?? new VersionRecord
                    {
                        ProgramVersion = version,
                        RealityObject = realityObject ?? new RealityObject { Id = stage3.RealityObject.Id, Address = stage3.RealityObject.Address },
                        Year = stage3.Year,
                        IndexNumber = 0,
                        CommonEstateObjects = stage3.CommonEstateObjects,
                        Point = stage3.Point,
                        StoredCriteria = stage3.StoredCriteria,
                        StoredPointParams = stage3.StoredPointParams,
                        ObjectCreateDate = DateTime.Now,
                        Changes = $"Добавлено: {DateTime.Now:dd.MM.yyyy HH:mm}"
                    };

                    ver3.Sum += stage3.Sum;
                    ver3.ObjectEditDate = DateTime.Now;

                    if (stage2Dict.ContainsKey(stage3))
                    {
                        var st2RecList = stage2Dict[stage3];

                        foreach (var stage2 in st2RecList)
                        {
                            if (needGenerateWorkCode && ver3.WorkCode.IsEmpty())
                            {
                                ver3.WorkCode = this.GenerateWorkCode(ref lastWorkCodeOrderDict, stage2, ver3);
                            }

                            var st2Version =
                                stage2VerRecs.Get("{0}_{1}_{2}".FormatUsing(stage2.RealityObject.Id,
                                    stage2.CommonEstateObject.Id, stage2.Year));

                            if (st2Version == null)
                            {
                                if (ver3.Id > 0)
                                {
                                    ver3.CommonEstateObjects = string.Format("{0}, {1}", ver3.CommonEstateObjects,
                                        stage2.CommonEstateObject.Name);
                                }

                                st2Version = new VersionRecordStage2
                                {
                                    CommonEstateObject = new CommonEstateObject { Id = stage2.CommonEstateObject.Id },
                                    Stage3Version = ver3,
                                    CommonEstateObjectWeight = stage2.CommonEstateObject.Weight,
                                    ObjectCreateDate = DateTime.Now
                                };
                            }
                            else if (stage3Period == 0)
                            {
                                ver3 = st2Version.Stage3Version;
                                ver3.Sum += stage3.Sum;
                                ver3.ObjectEditDate = DateTime.Now;
                                ver3.Changes = $"Добавлено: {DateTime.Now:dd.MM.yyyy HH:mm}";
                                ver3.IsAddedOnActualize = !fromActualizeYear;
                                ver3.IsChangedYearOnActualize = fromActualizeYear;
                            }

                            st2Version.Sum += stage2.Sum;
                            st2Version.ObjectEditDate = DateTime.Now;
                            ver2S.Add(st2Version);

                            if (stage1Dict.ContainsKey(stage2))
                            {
                                var st1RecList = stage1Dict[stage2];

                                foreach (var stage1 in st1RecList)
                                {
                                    ver1S.Add(new VersionRecordStage1
                                    {
                                        RealityObject =
                                            new RealityObject { Id = stage1.StructuralElement.RealityObject.Id },
                                        Stage2Version = st2Version,
                                        Year = stage1.Year,
                                        StructuralElement =
                                            new RealityObjectStructuralElement { Id = stage1.StructuralElement.Id },
                                        Sum = stage1.Sum,
                                        SumService = stage1.ServiceCost,
                                        Volume = stage1.StructuralElement.Volume,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now
                                    });
                                }
                            }
                        }
                    }

                    ver3S.Add(ver3);
                }

                ver3S = this.ChangeIndexNumber(ver3S, versSt3Query);
            }

            return new BaseDataResult();
        }
        
        private IDataResult GetNewRecordsForStavropol(
                      BaseParams baseParams,
                      out List<VersionRecord> ver3S,
                      out List<VersionRecordStage2> ver2S,
                      out List<VersionRecordStage1> ver1S,
                      Dictionary<long, RoStrElAfterRepair> afterRepaired = null,
                      List<long> roStrElList = null,
                      Dictionary<long, List<int>> publishYears = null,
                      Dictionary<long, int> publishYearsBySt2 = null)
        {
            ver3S = new List<VersionRecord>();
            ver2S = new List<VersionRecordStage2>();
            ver1S = new List<VersionRecordStage1>();

            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var roStructElService = this.Container.Resolve<IRealityObjectStructElementService>();
            var unProxy = this.Container.Resolve<IUnProxy>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            using (
                this.Container.Using(programVersionDomain, roStructElDomain, longProgramService, versSt2Domain, versSt3Domain,
                    sessionProvider, unProxy))
            {
                var version = programVersionDomain.Get(versionId);

                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var stage3Period = config.GroupByRoPeriod;
                var fromActualizeYear = baseParams.Params.GetAs<bool>("FromActualizeYear");
                var programmPeriodEnd = config.ProgrammPeriodEnd;
                var crEndYear = this.GetPeriodStartYear(version) - 1;

                var newStructQuery = roStructElService.GetElementsForLongProgram(roStructElDomain)
                    .Where(
                        x =>
                            x.RealityObject.Municipality.Id == version.Municipality.Id ||
                            x.RealityObject.MoSettlement.Id == version.Municipality.Id)
                    .Where(x => roStrElList.Contains(x.Id));

                if (!newStructQuery.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var stage2VerRecs = versSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new
                    {
                        Record = x,
                        RoId = x.Stage3Version.RealityObject.Id,
                        CeoId = x.CommonEstateObject.Id,
                        x.Stage3Version.Year
                    })
                    .ToList()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.RoId, x.CeoId, x.Year))
                    .ToDictionary(
                        x => x.Key,
                        y => y.First().Record);

                var versSt3Query = versSt3Domain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId);

                var stage3VerRecs = versSt3Query
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}".FormatUsing(x.RealityObject.Id, x.Year))
                    .ToDictionary(x => x.Key, y => y.First());

                // Вообщем Проблема актуализации года для Саратова втом что при изменеии срока Эксплуатации в большую сторону работа выпихивается за пределы окончания ДПКР
                // А поскольку метод GetStage1 заного формирует новый срез данных то теряются работы которе были опубликованы
                // Я пока пошел легким путем и просто увеличил при расчете 1 жата период окончания на 50 лет, чтобы расчитались работы сдвинутые а также обратно проставились Года опубликования
                var stage1ToSave = longProgramService.GetStage1(fromActualizeYear ? actualizeStart : crEndYear,
                    version.Municipality.Id, newStructQuery, programmPeriodEnd + 30, true)
                    .Where(x => x.Year > crEndYear);

                if (afterRepaired != null && afterRepaired.Any())
                {
                    var stage1ByRoStrEl = stage1ToSave.GroupBy(x => x.StructuralElement.Id)
                                                      .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Year).ToList());

                    // необходимо понят ькакие записи уже существуют в нетронутых годах и относительно сроков эксплуатации сдвигать последующие записи
                    foreach (var kvp in afterRepaired)
                    {
                        if (!stage1ByRoStrEl.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        // отталкиваемся от того что данный кэ уже учтен в какихто годах а значит берем этот год как год ремонта и прибавляем срок эксплуатации
                        var nextYear = kvp.Value.LastYearRepair + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);

                        // всем записям которые неполностью удалились из дпкрприсваиваем год относителньо года последней записи находящейся в версии
                        foreach (var roStrEl in stage1ByRoStrEl[kvp.Key])
                        {
                            roStrEl.Year = nextYear;

                            nextYear = roStrEl.Year + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);
                        }
                    }
                }

                if (!stage1ToSave.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                foreach (var stage1 in stage1ToSave)
                {
                    var tempPublishYear = publishYears.Get(stage1.StructuralElement.Id);
                    var publishYear = 0;

                    if (tempPublishYear != null && tempPublishYear.Count > 0)
                    {
                        publishYear = tempPublishYear.First();
                        tempPublishYear.Remove(publishYear);
                    }

                    if (publishYear > 0 && stage1.Year > publishYear)
                    {
                        stage1.Year = publishYear;
                    }

                    stage1.ObjectVersion = publishYear;
                }

                // А вто теперь Забираю только те записи 1 этапа которые уже реально не выходят за пределы ДПКР
                stage1ToSave = stage1ToSave.Where(x => x.Year <= programmPeriodEnd).ToList();

                var stage2ToSave = this.GetStage2ForStavropol(stage1ToSave);
                var stage3ToSave = longProgramService.GetStage3(stage2ToSave);

                var stage2Dict = stage2ToSave
                    .GroupBy(x => x.Stage3)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                var stage1Dict = stage1ToSave
                    .GroupBy(x => x.Stage2)
                    .ToDictionary(x => x.Key, y => y.ToArray());
                
                var lastWorkCodeOrderDict = this.GetLastWorkCodeOrderDict(stage3VerRecs);

                foreach (var stage3 in stage3ToSave.Where(x => !fromActualizeYear || x.Year >= actualizeStart))
                {
                    var ver3 = stage3Period > 0
                        ? stage3VerRecs.Get("{0}_{1}".FormatUsing(stage3.RealityObject.Id, stage3.Year))
                        : null;

                    var newVers3 = new VersionRecord
                    {
                        ProgramVersion = version,
                        RealityObject = new RealityObject
                        {
                            Id = stage3.RealityObject.Id,
                            Address = stage3.RealityObject.Address
                        },
                        Year = stage3.Year,
                        IndexNumber = 0,
                        CommonEstateObjects = stage3.CommonEstateObjects,
                        Point = stage3.Point,
                        StoredCriteria = stage3.StoredCriteria,
                        StoredPointParams = stage3.StoredPointParams,
                        ObjectCreateDate = DateTime.Now,
                        Changes = $"Добавлено: {DateTime.Now:dd.MM.yyyy HH:mm}"
                    };

                    ver3 = ver3 ?? newVers3;

                    ver3.Sum += stage3.Sum;
                    ver3.ObjectEditDate = DateTime.Now;

                    if (stage2Dict.ContainsKey(stage3))
                    {
                        var st2RecList = stage2Dict[stage3];

                        foreach (var stage2 in st2RecList)
                        {
                            if (ver3.WorkCode.IsEmpty())
                            {
                                ver3.WorkCode = this.GenerateWorkCode(ref lastWorkCodeOrderDict, stage2, ver3);
                            }
                            
                            var st2Version =
                                stage2VerRecs.Get("{0}_{1}_{2}".FormatUsing(stage2.RealityObject.Id,
                                    stage2.CommonEstateObject.Id, stage2.Year));

                            RealityObjectStructuralElementInProgramm[] st1RecList = null;
                            int publYear;
                            if (stage1Dict.ContainsKey(stage2))
                            {
                                st1RecList = stage1Dict[stage2];
                                publYear = st1RecList.SafeMax(x => x.ObjectVersion);

                                if (st2Version != null && publishYearsBySt2.Get(st2Version.Id) != publYear)
                                {
                                    st2Version = null;
                                }
                            }

                            if (st2Version == null)
                            {
                                if (ver3.Id > 0)
                                {
                                    ver3.CommonEstateObjects = $"{ver3.CommonEstateObjects}, {stage2.CommonEstateObject.Name}";
                                }

                                st2Version = new VersionRecordStage2
                                {
                                    CommonEstateObject = new CommonEstateObject { Id = stage2.CommonEstateObject.Id },
                                    Stage3Version = ver3,
                                    CommonEstateObjectWeight = stage2.CommonEstateObject.Weight,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectVersion = stage2.ObjectVersion
                                };
                            }
                            else if (stage3Period == 0)
                            {
                                ver3 = st2Version.Stage3Version;
                                ver3.Sum += stage3.Sum;
                                ver3.ObjectEditDate = DateTime.Now;
                                ver3.Changes = $"Добавлено: {DateTime.Now:dd.MM.yyyy HH:mm}";
                                ver3.IsAddedOnActualize = !fromActualizeYear;
                                ver3.IsChangedYearOnActualize = fromActualizeYear;
                            }

                            st2Version.Sum += stage2.Sum;
                            st2Version.ObjectEditDate = DateTime.Now;
                            ver2S.Add(st2Version);

                            if (st1RecList != null)
                            {
                                foreach (var stage1 in st1RecList)
                                {
                                    ver1S.Add(new VersionRecordStage1
                                    {
                                        RealityObject =
                                            new RealityObject
                                            {
                                                Id = stage1.StructuralElement.RealityObject.Id,
                                                Address = stage1.StructuralElement.RealityObject.Address
                                            },
                                        Stage2Version = st2Version,
                                        Year = stage1.Year,
                                        StructuralElement =
                                            new RealityObjectStructuralElement { Id = stage1.StructuralElement.Id },
                                        Sum = stage1.Sum,
                                        SumService = stage1.ServiceCost,
                                        Volume = stage1.StructuralElement.Volume,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now
                                    });
                                }
                            }
                        }
                    }

                    ver3S.Add(ver3);
                }

                ver3S = this.ChangeIndexNumber(ver3S, versSt3Query);
            }

            return new BaseDataResult();
        }

        private IEnumerable<RealityObjectStructuralElementInProgrammStage2> GetStage2ForStavropol(IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;

            var result = new ConcurrentBag<RealityObjectStructuralElementInProgrammStage2>();

            var dictCeo =
                this.Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                    .ToDictionary(x => x.Id);

            var elements =
                stage1Records
                    .Where(x => x.Year >= startYear && x.Year <= endYear)
                    .Select(x => new
                    {
                        x.Id,
                        x.ObjectVersion,
                        x.Year,
                        Address = x.StructuralElement.RealityObject.Address,
                        RoId = x.StructuralElement.RealityObject.Id,
                        CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                        x.Sum,
                        x.ServiceCost,
                        Stage1 = x
                    })
                    .GroupBy(x => new { x.RoId, x.CeoId, x.Year, x.ObjectVersion });

            foreach (var item in elements)
            {
                var ceo = dictCeo.ContainsKey(item.Key.CeoId) ? dictCeo[item.Key.CeoId] : null;

                var stage2 = new RealityObjectStructuralElementInProgrammStage2
                {
                    StructuralElements = string.Empty,
                    CommonEstateObject = ceo,
                    RealityObject = new RealityObject
                    {
                        Id = item.Key.RoId,
                        Address = item.Select(x => x.Address).FirstOrDefault()
                    },
                    Sum = 0M
                };

                var listYears = new List<int>();

                foreach (var val in item)
                {
                    var stage1 = val.Stage1;
                    stage1.Stage2 = stage2;

                    listYears.Add(val.Year);
                    stage2.Sum += val.ServiceCost + val.Sum;
                    stage2.ObjectVersion = val.ObjectVersion;
                }

                stage2.Year = Math.Round(listYears.Average(x => x), MidpointRounding.ToEven).ToInt();

                result.Add(stage2);
            };


            return result;
        }

        private int GetPeriodStartYear(ProgramVersion version)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var typeWorkSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();

            using (this.Container.Using(typeWorkSt1Domain))
            {
                var typeWorkQuery = typeWorkSt1Domain.GetAll()
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id);

                var maxYear = 0;

                if (typeWorkQuery.Any(x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd == null))
                {
                    maxYear = typeWorkQuery
                     .Select(x => x.Stage1Version.Stage2Version.Stage3Version.Year)
                     .SafeMax(x => x);
                }
                else
                {
                    var maxDate = typeWorkQuery
                        .Select(x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateEnd)
                        .OrderByDescending(x => x).FirstOrDefault();

                    if (maxDate != null)
                    {
                        maxYear = maxDate.Value.Year;
                    }
                }

                return maxYear < 2013 || maxYear > 2100 ? startYear : maxYear + 1;
            }
        }

        /// <summary>
        /// Пересчитать и проставить порядковые номера работ
        /// </summary>
        /// <param name="changedRecs">Измененные записи</param>
        /// <param name="verRecQuery">Запрос с работами версии</param>
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
                .Select(x => new
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

        private IEnumerable<HmaoWorkPrice> GetWorkPrices(
            Dictionary<int, IEnumerable<HmaoWorkPrice>> workPricesByYear,
            int year,
            IEnumerable<long> jobs,
            WorkPriceDetermineType type,
            RealityObject ro,
            IEnumerable<long> realEstTypes)
        {
            var capitalGroupId = ro.CapitalGroup != null ? ro.CapitalGroup.Id : 0;

            if (workPricesByYear.ContainsKey(year))
            {
                var list = workPricesByYear[year]
                    .Where(x => jobs.Contains(x.Job.Id));

                var prices =
                    list.AsQueryable()
                        .WhereIf(type == WorkPriceDetermineType.WithoutCapitalGroup, x => x.CapitalGroup == null)
                        .WhereIf(type == WorkPriceDetermineType.WithCapitalGroup,
                            x => x.CapitalGroup != null && x.CapitalGroup.Id == capitalGroupId)
                        .WhereIf(type == WorkPriceDetermineType.WithRealEstType && realEstTypes != null,
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

        private Dictionary<long, List<int>> GetFirstPrivYears(ProgramVersion version, int startYear)
        {
            var versSt1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();

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
                this.Container.Release(versSt1Domain);
            }
        }

        private Dictionary<long, List<int>> GetLastOvrhlYears(ProgramVersion version)
        {
            var versSt1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();

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
                this.Container.Release(versSt1Domain);
            }
        }

        private Dictionary<long, int> GetCountWorks(ProgramVersion version)
        {
            var versSt1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();
            var structuralElWorkDomain = this.Container.Resolve<IDomainService<StructuralElementWork>>();

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
                this.Container.Release(versSt1Domain);
                this.Container.Release(structuralElWorkDomain);
            }
        }

        private Dictionary<long, IEnumerable<int>> GetWeights(ProgramVersion version)
        {
            var versSt2Domain = this.Container.Resolve<IDomainService<VersionRecordStage2>>();
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
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Weight));
            }
            finally
            {
                this.Container.Release(versSt2Domain);
            }
        }

        private Dictionary<long, List<int>> GetSeYearsWithLifetimes(ProgramVersion version)
        {
            var versSt1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();

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
                this.Container.Release(versSt1Domain);
            }
        }
        
        /// <summary>
        /// Возвращает старые записи по 3 этапу
        /// </summary>
        private IDictionary<long, St1OldData> GetOldVersionRecordBySt3(IDomainService<VersionRecordStage1> versionRecordSt1Domain, long versionId)
        {
            // полчучаем все записи 1 этапа
            var st1Query = versionRecordSt1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId);

            var st1Data = versionRecordSt1Domain.GetAll()
                .Where(x => st1Query.Any(y => y.Id == x.Id))
                .Select(x => new
                {
                    St2Id = x.Stage2Version.Id,
                    St3Id = x.Stage2Version.Stage3Version.Id,
                    x.Stage2Version.Stage3Version.WorkCode,
                    x.Volume,
                    RoId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    CeoNames = x.Stage2Version.Stage3Version.CommonEstateObjects,
                    x.Stage2Version.Stage3Version.Sum,
                    x.Stage2Version.Stage3Version.Year,
                    x.Stage2Version.Stage3Version.IndexNumber,
                    x.Stage2Version.Stage3Version.FixedYear
                })
                .AsEnumerable();

            return st1Data
                .GroupBy(x => x.St3Id)
                .ToDictionary(
                    x => x.Key,
                    y => new St1OldData
                    {
                        RoId = y.Select(x => x.RoId).FirstOrDefault(),
                        WorkCode = y.Select(x => x.WorkCode).FirstOrDefault(),
                        CeoNames = y.Select(z => z.CeoNames).ToList().Any()
                            ? y.Select(z => z.CeoNames)
                                .Distinct()
                                .ToList()
                                .Aggregate((str, result) => string.IsNullOrEmpty(result) ? str : result + ", " + str)
                            : string.Empty,
                        Volume = y.Sum(z => z.Volume),
                        Sum = y.Select(z => z.Sum).FirstOrDefault(),
                        Number = y.Select(z => z.IndexNumber).FirstOrDefault(),
                        Year = y.Select(z => z.Year).FirstOrDefault(),
                        Address = y.Select(z => z.Address).FirstOrDefault(),
                        FixedYear = y.Select(z => z.FixedYear).FirstOrDefault(),
                        NewYear = 0
                    });
        }
        
        /// <summary>
        /// Генерация кода работы
        /// </summary>
        /// <param name="lastWorkCodeOrderDict">Словарь с последними порядковыми номерами кодов работ</param>
        /// <param name="stage2">Конструктивный элемент второго этапа</param>
        /// <param name="versionRecord">Запись в версии программы</param>
        /// <returns>Код работы</returns>
        private string GenerateWorkCode(ref Dictionary<string, int> lastWorkCodeOrderDict, 
            RealityObjectStructuralElementInProgrammStage2 stage2, 
            VersionRecord versionRecord)
        {
            var roWithCeo = $"{versionRecord.RealityObject.Id}{stage2.CommonEstateObject.Code.PadLeft(3, '0')}";
            var order = lastWorkCodeOrderDict.Get(roWithCeo) + 1;
            lastWorkCodeOrderDict[roWithCeo] = order;
            return $"{roWithCeo}{order:00}";
        }

        /// <summary>
        /// Получить словарь с последними порядковыми номерами кодов работ
        /// </summary>
        /// <param name="stage3VerRecs"></param>
        private Dictionary<string, int> GetLastWorkCodeOrderDict<T>(Dictionary<T, VersionRecord> stage3VerRecs)
        {
            return stage3VerRecs.Values
                .Where(x => x.WorkCode.IsNotEmpty())
                .Select(x => new
                {
                    x.Year,
                    x.RealityObject,
                    x.WorkCode,
                    CeoCode = x.WorkCode.Substring(x.WorkCode.Length - 5, 3)
                })
                .OrderBy(x => x.WorkCode)
                .GroupBy(x => $"{x.RealityObject.Id}{x.CeoCode.PadLeft(3, '0')}")
                .ToDictionary(x => x.Key,
                    y =>
                    {
                        var workCode = y.Last().WorkCode;
                        return workCode.Substring(workCode.Length - 2, 2).ToInt();
                    });
        }
    }
    
    public class ShortRecordProxy
    {
        public long Id { get; set; }

        public long RealityObjectId { get; set; }

        // наслучай если по одному виду работ нескольк озаписе 1 этапа
        // Например в 2 этапе 4 лифта, но вид работы только 1
        public List<VersionRecordStage1> Stage1List { get; set; }

        // неачльный год краткосрочной программы
        public int ShortYearStart { get; set; }

        public decimal Cost { get; set; }

        public decimal TotalCost { get; set; }

        public decimal ServiceCost { get; set; }

        public decimal Volume { get; set; }

        public long WorkId { get; set; }

        public string WorkName { get; set; }
    }

    public class WorkStrElInfo
    {
        public long WorkId { get; set; }
        public long StrElId { get; set; }
        public long CeoId { get; set; }
        public int CeoWeight { get; set; }
        public string CeoName { get; set; }
    }

    public class St1OldData
    {
        public long RoId { get; set; }
        public string WorkCode { get; set; }
        public string CeoNames { get; set; }
        public decimal Volume { get; set; }
        public decimal Sum { get; set; }
        public int Number { get; set; }
        public int Year { get; set; }
        public string Address { get; set; }
        public bool FixedYear { get; set; }
        public int NewYear { get; set; }
    }

    /// <summary>
    /// Дополнительная информация для записи лога актуализации версии
    /// </summary>
    public class VersionActualizeLogRecordAdditionInfo
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public RealityObject RealityObject { get; set; }

        /// <summary>
        /// Код работы
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// Год публикации
        /// </summary>
        public int PublishedYear { get; set; }
    }
}