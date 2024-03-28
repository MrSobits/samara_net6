using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bars.Gkh.Tasks.ExecutorTest
{
    public class ExecutorTestTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var sw = new Stopwatch();
                var report = new StringBuilder();               
                
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
