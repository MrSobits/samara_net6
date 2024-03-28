namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using System.Collections.Generic;

    /// <summary>
    /// Результат создание задач
    /// </summary>
    internal class DocumentTaskCreateResult
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DocumentTaskCreateResult()
        {
            this.SnapshotTaskDescriptors = new List<TaskDescriptor>();
            this.DocumentTaskDescriptors = new List<TaskDescriptor>();
        }

        /// <summary>
        /// Задачи на создание слепков данных
        /// </summary>
        public List<TaskDescriptor> SnapshotTaskDescriptors { get; set; }

        /// <summary>
        /// Задачи на печать документов
        /// </summary>
        public List<TaskDescriptor> DocumentTaskDescriptors { get; set; }

        /// <summary>
        /// Можно ли выполнять задачи
        /// </summary>
        public bool CanExecuteTasks { get; set; }
    }
}
