
namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    using System;

    public class CourtPracticeFile : BaseEntity
    {
        /// <summary>
        /// CourtPractice
        /// </summary>
        public virtual CourtPractice CourtPractice { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocDate { get; set; }
    }
}
