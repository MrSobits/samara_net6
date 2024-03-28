namespace Bars.Gkh.Quartz.Scheduler
{
    using System;

    /// <summary>
    /// Инициатор выполнения задачи - система
    /// </summary>
    [Serializable]
    public class SystemExecutionOwner : IExecutionOwner
    {
        /// <summary>
        /// Тип инициатора выполнения задачи
        /// </summary>
        public ExecutionOwnerType Type
        {
            get
            {
                return ExecutionOwnerType.System;
            }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name {
            get
            {
                return "Система";
            }
        }
    }
}
