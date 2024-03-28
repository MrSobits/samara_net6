namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Этап нарушения
    /// </summary>
    public class InspectionGjiViolStage : BaseGkhEntity, IEntityUsedInErp
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Нарушение проверки
        /// </summary>
        public virtual InspectionGjiViol InspectionViolation { get; set; }

        /// <summary>
        /// Тип этапа нарушения
        /// </summary>
        public virtual TypeViolationStage TypeViolationStage { get; set; }

        /// <summary>
        /// Плановая дата устранения
        /// </summary>
        public virtual DateTime? DatePlanRemoval { get; set; }

        /// <summary>
        /// фактическая дата устранения
        /// </summary>
        public virtual DateTime? DateFactRemoval { get; set; }

        /// <summary>
        /// дата уведомления
        /// </summary>
        public virtual DateTime? NotificationDate { get; set; }

        /// <summary>
        /// продленная дата устранения
        /// </summary>
        public virtual DateTime? DatePlanExtension { get; set; }

        /// <summary>
        /// сумма работ по устранению нарушений
        /// </summary>
        public virtual decimal? SumAmountWorkRemoval { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }
    }
}