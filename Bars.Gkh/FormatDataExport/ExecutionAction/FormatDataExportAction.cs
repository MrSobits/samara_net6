namespace Bars.Gkh.FormatDataExport.ExecutionAction
{
    using System;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.FormatDataExport.Tasks;

    /// <summary>
    /// Экспорт данных системы по формату на стороне Web-приложения
    /// </summary>
    [HiddenAction]
    public class FormatDataExportAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Code => nameof(FormatDataExportAction);

        /// <inheritdoc />
        public override string Name => "Экспорт данных системы по формату на стороне Web-приложения";

        /// <inheritdoc />
        public override string Description => this.Name;

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var taskExecutor = this.Container.Resolve<ITaskExecutor>(FormatDataExportTaskExecutor.Id);

            return taskExecutor.Execute(this.ExecutionParams, null, null, CancellationToken.None) as BaseDataResult;
        }
    }
}