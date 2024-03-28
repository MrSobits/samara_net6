namespace Bars.Gkh.Modules.ClaimWork.Entities
{

    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Исковое зявление
    /// </summary>
    public class LawsuitClwCourt : BaseEntity
    {
        /// <summary>
        /// ссылка на докуент
        /// </summary>
        public virtual DocumentClw DocumentClw { get; set; }

        /// <summary>
        /// тип суда
        /// </summary>
        public virtual LawsuitCourtType LawsuitCourtType { get; set; }

        /// <summary>
        /// Дата 
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Номер 
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Описание - навсякий слцчай добавляю описание
        /// </summary>

        public virtual string Description { get; set; }
        public virtual PretensionType PretensionType { get; set; }
        public virtual string PretensionReciever { get; set; }
        public virtual DateTime? PretensionDate { get; set; }
        public virtual string PretensionResult { get; set; }
        public virtual DateTime? PretensionReviewDate { get; set; }
        public virtual string PretensionNote { get; set; }

    }
}