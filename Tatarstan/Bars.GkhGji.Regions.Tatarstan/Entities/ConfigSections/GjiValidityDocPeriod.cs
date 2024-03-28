namespace Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;
    using System;

    /// <summary>
    /// Период действия документа ГЖИ
    /// </summary>
    public class GjiValidityDocPeriod : BaseEntity
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual TypeDocumentGji TypeDocument { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}
