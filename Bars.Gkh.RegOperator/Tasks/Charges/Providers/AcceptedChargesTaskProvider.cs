namespace Bars.Gkh.RegOperator.Tasks.Charges.Providers
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;
    using Executors;

    /// <summary>
    /// Поставщик задачи подтвержденных начислений
    /// </summary>
    public class AcceptedChargesTaskProvider : ITaskProvider
    {
        #region Implementation of ITaskProvider

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[] { new TaskDescriptor("Подтверждение начислений", AcceptedChargesTaskExecutor.Id, @params) });
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode { get { return "ChargeExport"; }}

        #endregion
    }
}
