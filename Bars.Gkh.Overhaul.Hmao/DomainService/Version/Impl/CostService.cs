using Bars.B4;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities.Version;
using Bars.Gkh.Overhaul.Hmao.Enum;
using Bars.B4.Modules.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Bars.Gkh.Enums;
using NHibernate.Linq.Visitors.ResultOperatorProcessors;
using Bars.GkhCr.Entities;
using Bars.B4.Utils;
using NHibernate.Dialect;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.B4.DataAccess;
using Castle.Windsor;
using Bars.Gkh.Modules.ClaimWork.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    internal class CostService : ICostService
    {
        public IFileManager FileManager { get; set; }
        public IWindsorContainer Container { get; set; }
        public IDomainService<CostLimit> CostLimitDomain { get; set; }
        public IDomainService<CostLimitTypeWorkCr> CostLimitTypeWorkCrDomain { get; set; }
        public IDomainService<CapitalGroup> CapitalGroupDomain { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }
        public IDomainService<Work> WorkDomain { get; set; }
        public IDomainService<CostLimitOOI> CostLimitOOIDomain { get; set; }
        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }
        public IDomainService<VersionRecordStage2> VersionRecordStage2Domain { get; set; }
        public IDomainService<VersionRecord> VersionRecordStage3Domain { get; set; }
        public IDomainService<VersionActualizeLog> VersionActualizeLogDomain { get; set; }

        public IUserIdentity UserIdentity { get; set; }

        /// <summary>
        /// Сервис ДПКР
        /// </summary>
        public ILongProgramService LongProgramService { get; set; }

        public IDataResult CalculateCostLimit(BaseParams baseParams)
        {
            DeleteAllCosts();

            var calcYear = baseParams.Params.GetAs<int>("calcYear");
            var calcIndex = baseParams.Params.GetAs<decimal>("calcIndex");
            if (calcYear > 0 && calcIndex > 0)
            {
                var capitalList = CapitalGroupDomain.GetAll().OrderBy(x => x.Code).ToList();
                foreach (CapitalGroup cg in capitalList)
                {
                    try
                    {
                        ProcessGroup(cg, calcYear, calcIndex);
                    }
                    catch (Exception ex)
                    {
                        return new BaseDataResult(false, ex.Message);
                    }

                }
                return new BaseDataResult(true, "Рассчет завершен");
            }
            else
            {
                return new BaseDataResult(false, "Не удалось получить параметры для расчета");
            }


        }

        public IDataResult TypeWorksForSelect(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var year = baseParams.Params.GetAs<int>("year");
            var workId = baseParams.Params.GetAs<long>("workId");
            var capGroupId = baseParams.Params.GetAs<long>("capGroup");

            var typeWorksForSelect = TypeWorkCrDomain.GetAll()
                .Where(x => x.Work.Id == workId)
                .Where(x => x.ObjectCr.RealityObject.CapitalGroup.Id == capGroupId)
                .Where(x => x.YearRepair.HasValue && x.YearRepair == year)
                .Where(x => x.Sum.HasValue && x.Sum > 0)
                .Where(x => x.Volume.HasValue && x.Volume > 0)
                .Select(x => new
                {
                    x.Id,
                    x.Work.Name,
                    x.ObjectCr.RealityObject.Address
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .Paging(loadParams)
                .ToList();

            return new ListDataResult(typeWorksForSelect, typeWorksForSelect.Count);
        }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            var costLimId = baseParams.Params.GetAs<long>("costLimId");
            var recordIds = baseParams.Params.GetAs<long[]>("workIds");

            var costLimit = CostLimitDomain.Get(costLimId);

            var oldCostLimTypeWorkList = CostLimitTypeWorkCrDomain.GetAll()
                .Where(x => x.CostLimit == costLimit)
                .Select(x => x.TypeWorkCr)
                .AsQueryable();

            var newWorkList = TypeWorkCrDomain.GetAll()
                .Where(x => recordIds.Contains(x.Id))
                .Where(x => !oldCostLimTypeWorkList.Contains(x))
                .ToList();

            foreach (var work in newWorkList)
            {
                var costLimTypeWork = new CostLimitTypeWorkCr
                {
                    CostLimit = costLimit,
                    TypeWorkCr = work,
                    Year = costLimit.Year.ToString(),
                    Cost = work.Sum ?? 0,
                    Volume = work.Volume ?? 0,
                    UnitMeasure = work.Work.UnitMeasure
                };

                CostLimitTypeWorkCrDomain.Save(costLimTypeWork);
            }

            var costLimTypeWorkList = CostLimitTypeWorkCrDomain.GetAll()
                .Where(x => x.CostLimit == costLimit)
                .ToList();

            var costList = new List<decimal>();
            foreach (var costWork in costLimTypeWorkList)
            {
                var costByWork = costWork.Volume != 0 ? costWork.Cost / costWork.Volume : 0;
                costList.Add(costByWork);
            }

            var costLim = (costList.SafeSum() / costList.Count) * (1 + costLimit.Rate / 100);

            try
            {
                costLimit.Cost = decimal.Round(costLim, 2);
            }
            catch
            {
                costLimit.Cost = costLim;
            }

            CostLimitDomain.Update(costLimit);

            return new BaseDataResult(new { costLimit.Cost });
        }

        /// <summary>
        /// Пересчитать стоимости всех элементов версии
        /// </summary>
        public void CalculateVersion(ProgramVersion version, int calcfromyear, int calctoyear)
        {
            StringBuilder log = new StringBuilder();
            int count = 0;
            log.Append($"Тип сущности;Id;Адрес;ООИ;Старая сумма;Новая сумма;\n");

            var allStages3 = VersionRecordStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.Stage2Version.Stage3Version != null && x.Stage2Version.Stage3Version.Show)
                .Where(x => x.Stage2Version.Stage3Version.Year >= calcfromyear && x.Stage2Version.Stage3Version.Year <= calctoyear)
                .GroupBy(x => x.Stage2Version.Stage3Version)
                .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var stage3 in allStages3)
            {
                //пересчитываем стоимости всех stage1
                decimal stage3cost = 0;
                foreach (var stage1 in stage3.Value)
                {

                    var sum = this.LongProgramService.GetDpkrCost(
                        stage1.RealityObject.Municipality.Id,
                        0,
                        (int)stage3.Key.Year,
                        stage1.StructuralElement.StructuralElement.Id,
                        stage1.RealityObject.Id,
                        stage1.RealityObject.CapitalGroup != null ? stage1.RealityObject.CapitalGroup.Id : 0,
                        stage1.StructuralElement.StructuralElement.CalculateBy,
                        stage1.StructuralElement.Volume,
                        stage1.RealityObject.AreaLiving,
                        stage1.RealityObject.AreaMkd,
                        stage1.RealityObject.AreaLivingNotLivingMkd);

                    // var sum = GetCost(stage1.RealityObject, stage1.Stage2Version.CommonEstateObject, (short)stage1.Year, stage1.StructuralElement.Volume, stage1.StructuralElement.StructuralElement.CalculateBy);
                    if (sum.HasValue && stage1.Sum != sum.Value)
                    {
                        log.Append($"Stage1;{stage1.Id};{stage1.RealityObject.Address};{stage1.Stage2Version.CommonEstateObject.Name};{stage1.Sum};{sum.Value};\n");

                        stage1.Sum = sum.Value;
                        VersionRecordStage1Domain.Save(stage1);

                        count++;
                    }

                    stage3cost += stage1.Sum;
                }

                //пересчитываем стоимости всех stage3
                if (stage3.Key.Sum != stage3cost)
                {
                    log.Append($"Stage3;{stage3.Key.Id};{stage3.Key.RealityObject.Address};{stage3.Key.CommonEstateObjects};{stage3.Key.Sum};{stage3cost};\n");

                    stage3.Key.Sum = stage3cost;
                    VersionRecordStage3Domain.Save(stage3.Key);

                    count++;
                }
            }

            //Сохраняем лог
            VersionActualizeLogDomain.Save(new VersionActualizeLog
            {
                ProgramVersion = version,
                Municipality = version.Municipality,
                UserName = UserIdentity.Name,
                DateAction = DateTime.Now,
                ActualizeType = VersionActualizeType.ActualizeSum,
                CountActions = count,
                LogFile = FileManager.SaveFile($"CalculateCostLog_{DateTime.Now.ToString("dd_MMM_yyyy-HH_mm_ss")}.csv", Encoding.UTF8.GetBytes(log.ToString())),
            });
        }


        List<CostLimitOOI> cache = null;
        public decimal? GetCost(RealityObject house, CommonEstateObject ooi, short yearRepair, decimal volume, PriceCalculateBy calcBy)
        {
            if (calcBy == PriceCalculateBy.Volume && volume == 0)
                return 0;

            if (cache == null)
                cache = CostLimitOOIDomain.GetAll().ToList();

            var cost = cache
                .Where(x => x.CommonEstateObject.Id == ooi.Id)
                .Where(x => x.Municipality == null || x.Municipality.Id == house.Municipality.Id)
                .Where(x => x.DateStart == null || x.DateStart.Value.Year <= yearRepair)
                .Where(x => x.DateEnd == null || x.DateEnd.Value.Year >= yearRepair)
                .Where(x => x.FloorStart == null || x.FloorStart <= house.Floors)
                .Where(x => x.FloorEnd == null || x.FloorEnd >= house.Floors)
                .Select(x => x.Cost)
                .ToList();

            if (cost.Count > 1)
                throw new ApplicationException($"Под условия отбора предельной стоимости {house.Address}:{ooi.Name}:{yearRepair} подошли несколько стоимостей. Пожалуйста удалите лишние");
            else if (cost.Count == 0)
                return null;
            else
                return cost.First() * volume;
        }

        public decimal? GetCost(RealityObject house, Work work, short yearRepair, decimal volume)
        {
            if (volume == 0)
                return 0;

            var cost = CostLimitDomain.GetAll()
                .Where(x => x.Work.Id == work.Id)
                .Where(x => x.Municipality == null || x.Municipality.Id == house.Municipality.Id)
                .Where(x => x.DateStart == null || x.DateStart.Value.Year <= yearRepair)
                .Where(x => x.DateEnd == null || x.DateEnd.Value.Year >= yearRepair)
                .Where(x => x.FloorStart == null || x.FloorStart <= house.Floors)
                .Where(x => x.FloorEnd == null || x.FloorEnd >= house.Floors)
                .Select(x => x.Cost)
                .ToList();

            if (cost.Count > 1)
                throw new ApplicationException($"Под условия отбора предельной стоимости {house.Address}:{work.Name}:{yearRepair} подошли несколько стоимостей. Пожалуйста удалите лишние");
            else if (cost.Count == 0)
                return null;
            else
                return cost.First() * volume;
        }

        private IEnumerable<VersionRecordStage1> GetAllStages1(VersionRecord stage3)
        {
            return VersionRecordStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.Id == stage3.Id)
                .ToList();
        }
        private void ProcessGroup(CapitalGroup cg, int calcYear, decimal calcIndex)
        {
            var worksList = TypeWorkCrDomain.GetAll()
                .Where(x => x.YearRepair.HasValue && x.YearRepair.Value == calcYear && x.ObjectCr.RealityObject.CapitalGroup != null && x.ObjectCr.RealityObject.CapitalGroup == cg)
                .Where(x => x.Work != null)
                .Select(x => x.Work).Distinct().ToList();
            foreach (var work in worksList)
            {
                CostLimit newCostLimit = new CostLimit
                {
                    DateStart = new DateTime(calcYear + 1, 1, 1),
                    DateEnd = new DateTime(calcYear + 1, 12, 31),
                    Work = work,
                    CapitalGroup = cg,
                    Cost = 0,
                    Year = calcYear,
                    Rate = calcIndex,
                    CostForCapGroup = 0,
                    UnitCostForCapGroup = 0
                };
                List<decimal> costs = new List<decimal>();
                var typeWorkIds = TypeWorkCrDomain.GetAll()
                    .Where(x => x.Work == work)
                    .Where(x => x.YearRepair.HasValue && x.YearRepair.Value == calcYear && x.ObjectCr.RealityObject.CapitalGroup != null && x.ObjectCr.RealityObject.CapitalGroup == cg)
                    .Where(x => x.Sum.HasValue && x.Sum.Value != 0 && x.Volume.HasValue && x.Volume.Value != 0)
                    .Select(x => x.Id).ToList();

                if (typeWorkIds.Any()) 
                {
                    var sumForCapGroup = TypeWorkCrDomain.GetAll()
                    .Where(x => x.Work == work)
                    .Where(x => x.YearRepair.HasValue && x.YearRepair.Value == calcYear && x.ObjectCr.RealityObject.CapitalGroup == cg)
                    .Where(x => x.Sum.HasValue && x.Sum.Value != 0 && x.Volume.HasValue && x.Volume.Value != 0)
                    .SafeSum(x => x.Sum.Value);

                    var unitSumForCapGroup = TypeWorkCrDomain.GetAll()
                        .Where(x => x.Work == work)
                        .Where(x => x.YearRepair.HasValue && x.YearRepair.Value == calcYear && x.ObjectCr.RealityObject.CapitalGroup == cg)
                        .Where(x => x.Sum.HasValue && x.Sum.Value != 0 && x.Volume.HasValue && x.Volume.Value != 0)
                        .SafeSum(x => (x.Sum.Value / x.Volume.Value));

                    newCostLimit.CostForCapGroup = sumForCapGroup / typeWorkIds.Count();
                    newCostLimit.UnitCostForCapGroup = unitSumForCapGroup / typeWorkIds.Count();
                }

                List<long> takenList = GetTWList(typeWorkIds);
                List<CostLimitTypeWorkCr> costLimitTypeWorksToAdd = new List<CostLimitTypeWorkCr>();
                if (takenList.Count > 0)
                {
                    var typeWorks = TypeWorkCrDomain.GetAll()
                        .Where(x => takenList.Contains(x.Id)).ToList();
                    foreach (var tw in typeWorks)
                    {
                        costLimitTypeWorksToAdd.Add(new CostLimitTypeWorkCr
                        {
                            CostLimit = newCostLimit,
                            Cost = tw.Sum ?? 0,
                            UnitMeasure = tw.Work.UnitMeasure,
                            TypeWorkCr = tw,
                            Volume = tw.Volume ?? 0,
                            Year = calcYear.ToString(),
                        });
                        try
                        {
                            var costByWork = tw.Sum.Value / tw.Volume;
                            costs.Add(costByWork ?? 1);
                        }
                        catch (Exception)
                        {
                            throw new Exception($"Не указан объем работ: {tw.Work.Name}, {tw.ObjectCr.RealityObject.Address}");
                        };
                    }
                    newCostLimit.Cost = (costs.SafeSum() / costs.Count) * (1 + calcIndex / 100);
                    try
                    {
                        newCostLimit.Cost = decimal.Round(newCostLimit.Cost, 2);
                        newCostLimit.CostForCapGroup = decimal.Round(newCostLimit.CostForCapGroup, 2);
                        newCostLimit.UnitCostForCapGroup = decimal.Round(newCostLimit.UnitCostForCapGroup, 2);
                    }
                    catch
                    { }


                    #region Сохранение

                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            CostLimitDomain.Save(newCostLimit);
                            costLimitTypeWorksToAdd.ForEach(x =>
                            {
                                CostLimitTypeWorkCrDomain.Save(x);
                            });

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }

                    #endregion
                }



            }
        }

        private List<long> GetTWList(List<long> typeWorkIds)
        {
            List<long> takenList = new List<long>();
            if (typeWorkIds.Count >= 5)
            {
                for (int i = 0; i < 6; i++)
                {
                    var rand = new Random();
                    var recentWorks = typeWorkIds.WhereIf(takenList.Count > 0, x => !takenList.Contains(x));
                    takenList.Add(recentWorks.ElementAt(rand.Next(recentWorks.Count())));
                }

            }
            else if (typeWorkIds.Count > 2)
            {
                takenList.AddRange(typeWorkIds);
            }
            return takenList;
        }

        private void DeleteAllCosts()
        {
            CostLimitTypeWorkCrDomain.GetAll().Select(x => x.Id).ToList().ForEach(x => { CostLimitTypeWorkCrDomain.Delete(x); });

            CostLimitDomain.GetAll().Select(x => x.Id).ToList().ForEach(x => { CostLimitDomain.Delete(x); });
        }

    }
}
