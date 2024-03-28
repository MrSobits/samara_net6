namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;

    /// <summary>
    /// Тестовое обязательное действие
    /// </summary>
    public class TestMandatoryAction : BaseMandatoryExecutionAction
    {
        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "[Тестовое обязательное действие]";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Тестовое действие для проверки работоспособности планировщика.\r\n"
            + "Генерирует исключение в случае ошибки.\r\n"
            + "Не позволяет запускать себя чаще чем раз в 2 минуты.";

        /// <summary>
        /// Количество итераций
        /// </summary>
        public int WorkIterationCount { get; set; } = 10;

        /// <summary>
        /// Вероятность ошибки (%)
        /// </summary>
        public int ErrorProbability { get; set; } = 50;

        /// <summary>
        /// Проверка необходимости выполнения действия
        /// </summary>
        public override bool IsNeedAction()
        {
            var results = this.Container.ResolveRepository<ExecutionActionResult>();

            using (this.Container.Using(results))
            {
                //Нет задач запущенных ранее чем 2 минуты назад
                var now = DateTime.Now.AddMinutes(-2);
                return !results.GetAll()
                    .Any(j => j.Task.ActionCode == this.Code && j.EndDate.HasValue &&  now < j.StartDate);
            }
        }

        private BaseDataResult Execute()
        {
            var sw = new Stopwatch();
            sw.Start();
            var count = this.ExecutionParams.Params.GetAs("WorkIterationCount", this.WorkIterationCount);
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(1000);
                this.CancellationToken.ThrowIfCancellationRequested();
            }
            var error = this.ExecutionParams.Params.GetAs("ErrorProbability", this.ErrorProbability);
            var v = new Random(DateTime.Now.Ticks.ToInt()).Next(0, 100) < error ? 0 : 1;
            int a = 10 / v;
            sw.Stop();

            this.ActionCodeList.Add(nameof(TestAction));

            return new BaseDataResult(
                new
                {
                    sw.Elapsed,
                    this.Name,
                    this.Description
                });
        }
    }
}