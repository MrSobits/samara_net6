namespace Bars.Gkh.Regions.Tatarstan.Entities.Egso
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Задача интеграции с ЕГСО ОВ
    /// </summary>
    public class EgsoIntegration : BaseEntity
    {
        /// <summary>
        /// Тип задачи
        /// </summary>
        public virtual EgsoTaskType TaskType { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public virtual EgsoTaskStateType StateType { get; set; }
        
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }
        
        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public virtual FileInfo Log { get; set; }

        /// <summary>
        /// Время выполнения
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Год за который отправляются сведения
        /// </summary>
        public virtual short Year { get; set; }
    }
}