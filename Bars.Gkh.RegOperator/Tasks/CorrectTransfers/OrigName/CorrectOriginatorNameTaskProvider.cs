namespace Bars.Gkh.RegOperator.Tasks.CorrectTransfers.OrigName
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Провайдер задач
    /// </summary>
    public class CorrectOriginatorNameTaskProvider : ITaskProvider
    {
        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[]
            {
                new TaskDescriptor("Корректировка поля \"Плательщик\" для трансферов оплат", CorrectPaymentOriginatorNameTaskExecutor.Id, @params),
                new TaskDescriptor("Корректировка поля \"Получатель/Основание\" для трансферов оплат актов", CorrectPwaOriginatorNameTaskExecutor.Id, @params),
                new TaskDescriptor("Корректировка поля \"Получатель/Основание\" для трансферов оплат подрядчикам", CorrectTransferCtrOriginatorNameTaksExecutor.Id, @params)
            });
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode
        {
            get
            {
                return "CorrectTransferOriginatorName";
            }
        }
    }
}