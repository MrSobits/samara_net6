namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Нарушение в служебной записке
    /// </summary>
    public class PrescriptionOfficialReportViolation : BaseEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual PrescriptionOfficialReport PrescriptionOfficialReport { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual PrescriptionViol PrescriptionViol { get; set; }       

    }
}