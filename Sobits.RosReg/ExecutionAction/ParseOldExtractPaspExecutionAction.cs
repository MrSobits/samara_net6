namespace Sobits.RosReg.ExecutionAction
{
    using System;
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.ExecutionAction;

    using Castle.Windsor;

    using Sobits.RosReg.Tasks.ExtractParse;

    /// <summary>
    /// Действие по созданию кошельков
    /// </summary>
    public class ParseOldExtractPaspExecutionAction : BaseExecutionAction
    {

        /// <inheritdoc />
        public override string Description => "Заполняет паспортв старых выписках";

        /// <inheritdoc />
        public override string Name => "РосРеестр - заполнение данных паспорта по старым выпискам";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;
        private readonly IWindsorContainer container;
        
        public ParseOldExtractPaspExecutionAction(IWindsorContainer container)
        {
            this.container = container;
        }
        

        private BaseDataResult Execute()
        {
            var taskManager = this.Container.Resolve<ITaskManager>();
            taskManager.CreateTasks(new ParseOldExtractsPaspTaskProvider(container), new BaseParams());

            return new BaseDataResult();
        }
    }
}