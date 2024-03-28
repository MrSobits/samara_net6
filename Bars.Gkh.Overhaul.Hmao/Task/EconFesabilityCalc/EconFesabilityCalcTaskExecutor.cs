using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.Overhaul.Hmao.Task
{
    public class EconFesabilityCalcTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        public IDomainService<EconFeasibilityCalcResult> EconCalcResults { get; set; }
        public IDomainService<EconFeasibilitiWork> EconResultsWorks { get; set; }
        public IRepository<VersionRecord> VersionRecordRep { get; set; }
        //public IDomainService<Room> RoomDomain { get; set; }
        public IRepository<LivingSquareCost> CostsRep { get; set; }
        //public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try            
            {
                var roIds = @params.Params.GetAs<List<long>>("roIds");
                var yearStart = @params.Params.GetAs<int>("yearStart");
                var yearEnd = @params.Params.GetAs<int>("yearEnd");
                var maxPercent = 70;
                //собираем суммы работ по домам в заданном интервале лет
                var works = VersionRecordRep.GetAll()
                    .Where(x => x.Show)
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .Where(x => x.Year >= yearStart && x.Year <= yearEnd)
              //      .Where(x=> x.RealityObject.MoSettlement!=null)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => new { works = y.Select(t => t).ToArray(),
                        summ = y.Sum(t => t.Sum),
                        MoSettId = y.Select(t => t.RealityObject.MoSettlement != null? t.RealityObject.MoSettlement.Id: 0).FirstOrDefault(),
                        MuId = y.Select(t => t.RealityObject.Municipality.Id).FirstOrDefault(),
                        AreaMkd = y.Select(t => t.RealityObject.AreaMkd).FirstOrDefault()
                    }
                   );

                //собираем площади всех помещений дома
                //var squares = RoomDomain.GetAll()
                //    .Where(x => roIds.Contains(x.RealityObject.Id))
                //    .GroupBy(x => x.RealityObject.Id)
                //    .ToDictionary(x => x.Key, y => y.Sum(x => x.Area));

                //var squares = RealityObjectDomain.GetAll()
                //   .Where(x => roIds.Contains(x.Id))
               //    .GroupBy(x => x.Id)
                 //  .ToDictionary(x => x.Id, y=> y.AreaMkd);

                //тянем последнюю стоимость кв.метра в МО
                var costs = CostsRep.GetAll()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.Year)
                                                     .FirstOrDefault());
                var counter = 0;

                foreach (var work in works)
                {
                    EconFeasibilityCalcResult result = new EconFeasibilityCalcResult();
                    List<EconFeasibilitiWork> resultWorks = new List<EconFeasibilitiWork>();

                  
                    if (costs.ContainsKey(work.Value.MoSettId))
                    {
                        result.SquareCost = costs[work.Value.MoSettId];
                        result.TotatRepairSumm = work.Value.summ;
                        result.TotalSquareCost = work.Value.AreaMkd * costs[work.Value.MoSettId].Cost;
                        foreach (var repairWork in work.Value.works)
                        {
                            resultWorks.Add(new EconFeasibilitiWork { RecorWorkdId = repairWork });
                        }
                    }
                    else if (costs.ContainsKey(work.Value.MuId))
                    {
                        result.SquareCost = costs[work.Value.MuId];
                        result.TotatRepairSumm = work.Value.summ;
                        result.TotalSquareCost = work.Value.AreaMkd * costs[work.Value.MuId].Cost;
                        foreach (var repairWork in work.Value.works)
                        {
                            resultWorks.Add(new EconFeasibilitiWork { RecorWorkdId = repairWork });
                        }
                    }
                    else
                    {
                        continue;
                    }
                    if (result.TotatRepairSumm != null && result.TotalSquareCost != null)
                    {
                        result.CostPercent = Math.Round((decimal)(result.TotatRepairSumm / result.TotalSquareCost) * 100);
                        result.Decision = result.CostPercent < maxPercent
                                                    ? Enum.EconFeasibilityResult.Economical
                                                    : Enum.EconFeasibilityResult.NotEconomical;

                        result.RoId = RealityObjectRepository.Get(work.Key);

                        result.YearStart = yearStart;
                        result.YearEnd = yearEnd;

                        SaveResults(result, resultWorks);
                    }

                    indicator?.Report(null, (uint)(++counter / 5), "Выполнение расчета");
                    
                }

                return new BaseDataResult(true, $@"выполнено {counter}"); 
        
            }
             catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
        
        private bool SaveResults (EconFeasibilityCalcResult result, List<EconFeasibilitiWork> works)
        {
            try
            {
                var ro = RealityObjectRepository.Get(result.RoId.Id);
                ro.IsRepairInadvisable = result.Decision.Equals(Enum.EconFeasibilityResult.NotEconomical);
                RealityObjectRepository.Update(ro);
                
                var existResult = EconCalcResults.GetAll()
                    .Where(x => x.RoId == result.RoId
                              && x.YearStart == result.YearStart
                              && x.YearEnd == result.YearEnd).ToArray();
                if (existResult.Length != 0)
                {
                    foreach (var exRes in existResult)
                    {
                        var existsWorks = EconResultsWorks.GetAll().Where(x => x.ResultId == exRes).ToArray();
                        if (existsWorks.Length != 0)
                        {
                            existsWorks.ForEach(EconResultsWorks.Delete);
                        }
                        EconCalcResults.Delete(exRes);

                    }
                }

                EconCalcResults.Save(result);
              

                foreach (var work in works)
                {
                    work.ResultId = result;
                    EconResultsWorks.Save(work);
                }

                return true;
            }
            catch (Exception e)
            {
                var ex = e.Message;
                return false;
                
            }

        }
      


    }
        

}
