namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Риск проверки ГЖИ
    /// </summary>
    public class InspectionRisk : BaseEntity
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Категория риска
        /// </summary>
        public virtual RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}