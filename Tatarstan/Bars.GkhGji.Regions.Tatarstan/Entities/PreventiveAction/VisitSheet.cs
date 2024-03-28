namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Лист визита
    /// </summary>
    public class VisitSheet : DocumentGji
    {
        /// <summary>
        /// Визит проведен
        /// </summary>
        public virtual Inspector ExecutingInspector { get; set; }

        /// <summary>
        /// Экземпляр листа визита получен
        /// </summary>
        public virtual YesNoNotSet HasCopy { get; set; }

        /// <summary>
        /// Дата начала проведения визита
        /// </summary>
        public virtual DateTime? VisitDateStart { get; set; }

        /// <summary>
        /// Дата окончания проведения визита
        /// </summary>
        public virtual DateTime? VisitTimeStart { get; set; }

        /// <summary>
        /// Время проведения визита с
        /// </summary>
        public virtual DateTime? VisitDateEnd { get; set; }

        /// <summary>
        /// Время проведения визита по
        /// </summary>
        public virtual DateTime? VisitTimeEnd { get; set; }
    }
}
