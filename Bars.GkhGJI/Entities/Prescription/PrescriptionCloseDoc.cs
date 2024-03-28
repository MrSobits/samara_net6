namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Документ по закрытию предписани
    /// </summary>
    public class PrescriptionCloseDoc : BaseEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Prescription Prescription { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual PrescriptionDocType DocType { get; set; }

        /// <summary>
        /// Дата предоставления
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
