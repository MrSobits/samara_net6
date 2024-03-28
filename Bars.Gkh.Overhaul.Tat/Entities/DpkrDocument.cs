namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Entities.Dict;

    /// <summary>
    /// Документ ДПКР
    /// </summary>
    public class DpkrDocument : BaseEntity
    {
        /// <summary>
        /// Вид документа
        /// </summary>
        public virtual BasisOverhaulDocKind DocumentKind { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Орган, принявший документ
        /// </summary>
        public virtual string DocumentDepartment { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public virtual DateTime PublicationDate { get; set; }
        
        /// <summary>
        /// Дата возникновения обязанности для домов, введенных в эксплуатацию до 2014 г.
        /// </summary>
        public virtual DateTime ObligationBefore2014 { get; set; }
        
        /// <summary>
        /// Дата возникновения обязанности для домов, введенных в эксплуатацию после 2014 г.
        /// </summary>
        public virtual DateTime ObligationAfter2014 { get; set; }
    }
}