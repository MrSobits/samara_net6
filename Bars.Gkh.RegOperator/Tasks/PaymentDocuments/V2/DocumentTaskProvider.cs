namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;

    /// <summary>
    /// Провайдер создания задач выгрузки документов на оплату
    /// </summary>
    internal class DocumentTaskProvider : ITaskProvider
    {
        public const string TaskCodeName = "DocumentTaskProvider";
        private readonly TaskDescriptor[] taskDescriptors;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="taskDescriptors">Массив описателей задач</param>
        public DocumentTaskProvider(TaskDescriptor[] taskDescriptors)
        {
            this.taskDescriptors = taskDescriptors;
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode { get { return DocumentTaskProvider.TaskCodeName; } }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат создания задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return this.taskDescriptors.IsEmpty()
                ? new CreateTasksResult(new TaskDescriptor[0])
                : new CreateTasksResult(this.taskDescriptors);
        }
    }
}