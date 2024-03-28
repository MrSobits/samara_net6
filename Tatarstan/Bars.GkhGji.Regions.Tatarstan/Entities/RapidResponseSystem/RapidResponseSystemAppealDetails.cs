namespace Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Детализация обращения в СОПР
    /// </summary>
    public class RapidResponseSystemAppealDetails : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Обращение в СОПР
        /// </summary>
        public virtual RapidResponseSystemAppeal RapidResponseSystemAppeal { get; set; }

        /// <summary>
        /// Статус обращения
        /// </summary>
        public virtual State State { get; set; }
        
        /// <summary>
        /// Место возникновения проблемы
        /// </summary>
        public virtual AppealCitsRealityObject AppealCitsRealityObject { get; set; }
        
        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime ReceiptDate { get; set; }
        
        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime ControlPeriod { get; set; }
        
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }
    }
}