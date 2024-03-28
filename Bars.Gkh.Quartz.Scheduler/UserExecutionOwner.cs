namespace Bars.Gkh.Quartz.Scheduler
{
    using System;

    /// <summary>
    /// Инициатор выполнения задачи - пользователь
    /// </summary>
    [Serializable]
    public class UserExecutionOwner: IExecutionOwner
    {
        /// <summary>
        /// Тип инициатора выполнения задачи
        /// </summary>
        public ExecutionOwnerType Type
        {
            get
            {
                return ExecutionOwnerType.User;
            }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>Идентификатор пользователя</summary>
        public long UserId { get; set; }

        /// <summary>
        /// IP-адрес запроса от пользователя.
        /// Равен null, если текущее действие не инициировано пользователем.
        /// </summary>
        public string RequestIpAddress { get; set; }
    }
}
