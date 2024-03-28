namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;

    /// <summary>
    /// Документ Искового зявления
    /// </summary>
    public class LawsuitClwDocument : BaseEntity
    {
        /// <summary>
        /// ссылка на докуент
        /// </summary>
        public virtual DocumentClw DocumentClw { get; set; }

        /// <summary>
        /// Наименвоание документа
        /// </summary>
        public virtual string DocName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Описание 
        /// </summary>
        public virtual string Description { get; set; }
    }
}