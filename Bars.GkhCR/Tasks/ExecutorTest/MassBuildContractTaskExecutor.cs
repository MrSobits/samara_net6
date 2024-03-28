using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhCr.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bars.GkhCR.Tasks
{
    public class MassBuildContractTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDomainService<MassBuildContract> MassBuildContractDomain { get; set; }

        public IDomainService<MassBuildContractWork> MassBuildContractWorkDomain { get; set; }

        public IDomainService<MassBuildContractObjectCrWork> MassBuildContractObjectCrWorkDomain { get; set; }

        public IDomainService<MassBuildContractObjectCr> MassBuildContractObjectCrDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<BuildContract> BuildContractDomain { get; set; }

        public IDomainService<BuildContractTypeWork> BuildContractTypeWorkDomain { get; set; }

        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            MassBuildContract massBK = MassBuildContractDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if (massBK == null)
                return new BaseDataResult(false, $"Договор с ID {@params.Params["taskId"]} не найден");

            try
            {
                var sw = new Stopwatch();
                var report = new StringBuilder();
                MassBuildContractObjectCrDomain.GetAll()
                    .Where(x => x.MassBuildContract == massBK)
                    .Select(x => x.ObjectCr).ToList().ForEach(y =>
                     {
                         try
                         {
                             var enabledWorkIds = MassBuildContractObjectCrWorkDomain.GetAll()
                             .Where(x=> x.MassBuildContractObjectCr.MassBuildContract == massBK)
                             .Where(x => x.MassBuildContractObjectCr.ObjectCr == y)
                             .Select(x => x.Work.Id).ToList();

                             var enabledWorks = MassBuildContractObjectCrWorkDomain.GetAll()
                            .Where(x => x.MassBuildContractObjectCr.MassBuildContract == massBK)
                            .Where(x => x.MassBuildContractObjectCr.ObjectCr == y)
                            .Select(x=> new
                             {
                                x.Work.Id,
                                Sum = x.Sum != null? x.Sum.Value:0m
                             }).AsEnumerable()
                             .GroupBy(x=> x.Id)
                             .ToDictionary(obj=> obj.Key, obj => obj.SafeSum(z=> z.Sum));

                             var workList = TypeWorkCrDomain.GetAll()
                             .Where(x => x.ObjectCr.ProgramCr == massBK.ProgramCr)
                             .Where(x => x.ObjectCr == y)
                             .Where(x => enabledWorkIds.Contains(x.Work.Id))
                             .ToList();

                             var objectSum = MassBuildContractObjectCrWorkDomain.GetAll()
                               .Where(x => x.MassBuildContractObjectCr.MassBuildContract == massBK)
                                 .Where(x => x.MassBuildContractObjectCr.ObjectCr == y)
                                 .Sum(x => x.Sum);


                             var existingContract = BuildContractDomain.GetAll()
                             .Where(x => x.ObjectCr == y && x.DocumentName == massBK.DocumentName && x.DocumentNum == massBK.DocumentNum && x.DocumentDateFrom == massBK.DocumentDateFrom).FirstOrDefault();
                             if (existingContract == null && enabledWorks.Count>0)
                             {
                                 BuildContract newBk = new BuildContract
                                 {
                                     BudgetMo = massBK.BudgetMo,
                                     Builder = massBK.Builder,
                                     BudgetSubject = massBK.BudgetSubject,
                                     Contragent = massBK.Contragent,
                                     DateAcceptOnReg = massBK.DateAcceptOnReg,
                                     DateCancelReg = massBK.DateCancelReg,
                                     DateEndWork = massBK.DateEndWork,
                                     DateInGjiRegister = massBK.DateInGjiRegister,
                                     DateStartWork = massBK.DateStartWork,
                                     Description = massBK.Description,
                                     DocumentDateFrom = massBK.DocumentDateFrom,
                                     DocumentFile = massBK.DocumentFile,
                                     DocumentName = massBK.DocumentName,
                                     DocumentNum = massBK.DocumentNum,
                                     ExternalId = massBK.ExternalId,
                                     FundMeans = massBK.FundMeans,
                                     GuaranteePeriod = massBK.GuaranteePeriod,
                                     Inspector = massBK.Inspector,
                                     ObjectCr = y,
                                     ObjectCreateDate = DateTime.Now,
                                     ObjectEditDate = DateTime.Now,
                                     ObjectVersion = 1,
                                     OwnerMeans = massBK.OwnerMeans,
                                     ProtocolDateFrom = massBK.ProtocolDateFrom,
                                     ProtocolFile = massBK.ProtocolFile,
                                     ProtocolName = massBK.ProtocolName,
                                     ProtocolNum = massBK.ProtocolNum,
                                     Sum = objectSum.HasValue? objectSum.Value:0,
                                     StartSum = massBK.StartSum,
                                     // Sum = massBK.Sum,
                                     TerminationDate = massBK.TerminationDate,
                                     TerminationDictReason = massBK.TerminationDictReason,
                                     TerminationDocumentFile = massBK.TerminationDocumentFile,
                                     TerminationDocumentNumber = massBK.TerminationDocumentNumber,
                                     TerminationReason = massBK.TerminationReason,
                                     TypeContractBuild = massBK.TypeContractBuild,
                                     UrlResultTrading = massBK.UrlResultTrading,
                                     UsedInExport = massBK.UsedInExport

                                 };
                                 BuildContractDomain.Save(newBk);

                                 foreach (var tw in workList)
                                 {
                                     BuildContractTypeWork newTw = new BuildContractTypeWork
                                     {
                                         BuildContract = newBk,
                                         ObjectCreateDate = DateTime.Now,
                                         ObjectEditDate = DateTime.Now,
                                         TypeWork = tw,
                                         Sum = enabledWorks.ContainsKey(tw.Work.Id)? enabledWorks[tw.Work.Id]: tw.Sum
                                     };
                                     BuildContractTypeWorkDomain.Save(newTw);
                                 }

                             }

                         }
                         catch (Exception e)
                         {
                           //  return new BaseDataResult(false, $"Ошибка создания договора с ID {@params.Params["taskId"]} {e.Message} в {e.StackTrace}");
                         }
                     });


                try
                {
                    indicator?.Report(null, 10, $"Загрузка CPU");
                    PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    report.Append($"Процессор загружен на {cpuCounter.NextValue()}%; ");
                    cpuCounter.Dispose();

                    indicator?.Report(null, 50, $"Оперативная память");
                    PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                    report.Append($"Свободно памяти: {ramCounter.NextValue()} МБ; ");
                    ramCounter.Dispose();
                }
                catch(UnauthorizedAccessException)
                {
                    report.Append($"У процесса нет прав на чтение счетчиков производительности; ");
                }

                indicator?.Report(null, 80, $"Тест связи с базой");

                var tasks = TaskEntryDomain.GetAll().Where(x => x.Parent != null);
                var mSPerTick = (float)(1000L) / Stopwatch.Frequency;

                sw.Start();

                int InProgress = tasks.Where(x => x.Status == TaskStatus.InProgress).Count();
                int Initial = tasks.Where(x => x.Status == TaskStatus.Initial).Count();
                int Zombied = tasks.Where(x => x.Status == TaskStatus.Zombied).Count();
                int Queued = tasks.Where(x => x.Status == TaskStatus.Queued).Count();
                int Handling = tasks.Where(x => x.Status == TaskStatus.Handling).Count();

                sw.Stop();

                report.Append($"Запрос к базе занял {(sw.ElapsedTicks * mSPerTick / 5).ToString("N2")} мс; ");
                report.Append($"Активных задач: {InProgress}, в начальном статусе: {Initial}, в обработке: {Handling}, блокированных: {Queued}, зомби: {Zombied}; ");

                return new BaseDataResult(report.ToString());
            }
            catch(Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
