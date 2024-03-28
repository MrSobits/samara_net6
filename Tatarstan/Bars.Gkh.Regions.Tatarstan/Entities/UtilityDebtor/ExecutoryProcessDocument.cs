namespace Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Документы для исполнительного производства
    /// </summary>
    public class ExecutoryProcessDocument : BaseEntity
    {
        /// <summary>
        /// Исполнительное производство
        /// </summary>
        public virtual ExecutoryProcess ExecutoryProcess { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual ExecutoryProcessDocumentType ExecutoryProcessDocumentType { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Notation { get; set; }
    }
}