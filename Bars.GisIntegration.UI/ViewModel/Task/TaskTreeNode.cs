namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using System;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Узел дерева задач
    /// </summary>
    public class TaskTreeNode: BaseNode
    {
        /// <summary>
        /// Конструктор узла дерева задач
        /// </summary>
        public TaskTreeNode()
        {
        }

        /// <summary>
        /// Конструктор узла дерева задач
        /// </summary>
        /// <param name="task">Экземпляр сущности Задача</param>
        public TaskTreeNode(RisTask task)
        {
            this.Id = task.Id;
            this.Type = "Task";
            this.Name = task.Description;
            this.StartTime = task.StartTime == DateTime.MinValue ? string.Empty : task.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
            this.EndTime = task.EndTime == DateTime.MinValue ? string.Empty : task.EndTime.ToString("dd.MM.yyyy HH:mm:ss");
            this.Author = task.UserName;
            this.IconCls = "icon_task";
        }

        /// <summary>
        /// Конструктор узла дерева задач
        /// </summary>
        /// <param name="packageTrigger">Экземпляр сущности, связывающей триггер и пакет</param>
        public TaskTreeNode(RisPackageTrigger packageTrigger)
        {
            this.Id = packageTrigger.Id;
            this.Type = "Package";
            this.Name = packageTrigger.Package.Name;
            this.Author = packageTrigger.Package.UserName;
            this.State = packageTrigger.State.GetDisplayName();
            this.Result = packageTrigger.State == PackageState.SuccessProcessed || packageTrigger.State == PackageState.ProcessedWithErrors;
            this.Protocol = this.Result;
            this.Message = packageTrigger.Message;
            this.Leaf = true;
            this.IconCls = "icon_package";
        }

        /// <summary>
        /// Конструктор узла дерева задач
        /// </summary>
        /// <param name="taskTrigger">Экземпляр сущности, связывающей триггер и задачу</param>
        public TaskTreeNode(RisTaskTrigger taskTrigger)
        {
            string name;

            switch (taskTrigger.TriggerType)
            {
                case TriggerType.PreparingData:
                    name = "Подготовка данных";
                    this.Leaf = true;
                    this.IconCls = "icon_prepareData";
                    this.Type = "PreparingDataTrigger";
                    break;
                case TriggerType.SendingData:
                    name = "Отправка данных";
                    this.Leaf = false;
                    this.IconCls = "icon_send";
                    this.Type = "SendingDataTrigger";
                    break;
                default:
                    name = taskTrigger.Trigger.ClassName;
                    this.Type = "Trigger";
                    break;
            }

            this.Id = taskTrigger.Trigger.Id;
            this.Name = name;
            this.Author = taskTrigger.Trigger.UserName;
            this.StartTime = taskTrigger.Trigger.StartTime == DateTime.MinValue ? string.Empty : taskTrigger.Trigger.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
            this.EndTime = taskTrigger.Trigger.EndTime == DateTime.MinValue ? string.Empty : taskTrigger.Trigger.EndTime.ToString("dd.MM.yyyy HH:mm:ss");            
        }

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип узла, сущности ему соответствующей
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Время начала выполнения
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Время окончания выполнения
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public string Percent { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Результат доступен для просмотра
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Протокол доступен для просмотра
        /// </summary>
        public bool Protocol { get; set; }
    }
}
