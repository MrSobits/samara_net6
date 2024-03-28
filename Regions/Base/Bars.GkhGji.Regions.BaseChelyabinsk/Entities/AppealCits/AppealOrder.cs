namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Поручение отработки обращения нарушителю
    /// </summary>
    public class  AppealOrder: BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Contragent Executant { get; set; }

        /// <summary>
        /// должностное лицо
        /// </summary>
        public virtual string Person { get; set; }

        /// <summary>
        /// должностное лицо телефон
        /// </summary>
        public virtual string PersonPhone { get; set; }

        /// <summary>
        /// Дата поручения
        /// </summary>
        public virtual DateTime OrderDate { get; set; }

        /// <summary>
        /// Срок исполнения
        /// </summary>
        public virtual DateTime? PerformanceDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Текст обращения
        /// </summary>
        public virtual string AppealText { get; set; }

        /// <summary>
        /// Исполнено
        /// </summary>
        public virtual YesNoNotSet YesNoNotSet { get; set; }

        /// <summary>
        /// Подтверждено
        /// </summary>
        public virtual YesNoNotSet Confirmed { get; set; }

        /// <summary>
        /// Подтверждено
        /// </summary>
        public virtual YesNoNotSet ConfirmedGJI { get; set; }

    }
}
