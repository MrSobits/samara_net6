namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Документ конкурса
    /// </summary>
    public class CompetitionDocument : BaseImportableEntity
    {
        /// <summary>
        /// Конкурс
        /// </summary>
        public virtual Competition Competition { get; set; }

        /// <summary>
        /// Наименование докуммента
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}