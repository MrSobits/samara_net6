namespace Bars.Gkh.ExecutionAction.Impl
{
    using Bars.B4;
    // TODO: Расскомментировать
    //using Bars.B4.Modules.FIAS.AutoUpdater;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Tasks.FiasUpdater;
    using System;

    [Repeatable]
    public class FiasAutoUpdaterTaskAction : BaseExecutionAction
    {
        public override string Description => "Автоматическое обновление ФИАС (задачей)";

        public override string Name => "Автоматическое обновление ФИАС (задачей)";

        public override Func<IDataResult> Action => this.Execute;

           //public IFiasAutoUpdater FiasAutoUpdater { get; set; }

        public ITaskManager TaskManager { get; set; }

        public BaseDataResult Execute()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new FiasUpdaterTaskProvider(), ExecutionParams);
                return new BaseDataResult(true, "Задача успешно поставлена");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }

        private void StartSetHouseGuidFromFiasHouseAction()
        {
            if (this.ExecutionParams.Params.GetAs("FixHouseGuid", false, true))
            {
                this.ActionCodeList.Add(nameof(SetHouseGuidFromFiasHouseAction));
            }
        }
    }
}