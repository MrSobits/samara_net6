namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Utils;

    /// <summary>
    /// Тестовое действие
    /// </summary>
    [HiddenAction]
    public class TestAction : BaseExecutionAction, IMandatoryExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "[Тестовое действие]";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <inheritdoc />
        public override string Description => "Тестовое действие. Скрыто от пользователя";

        public bool IsNeedAction()
        {
            return true;
        }

        /// <inheritdoc />
        private BaseDataResult Execute()
        {
            var sw = new Stopwatch();
            sw.Start();
            var count = 10;
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(1000);
                this.CancellationToken.ThrowIfCancellationRequested();
            }

            var v = new Random(DateTime.Now.Ticks.ToInt()).Next(0, 100) < 30 ? 0 : 1;
            int a = 10 / v;
            sw.Stop();

            return new BaseDataResult(new
            {
                sw.Elapsed,
                this.Name,
                this.Description
            });
        }
    }
}