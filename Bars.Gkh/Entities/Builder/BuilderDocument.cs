namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Документы подрядчиков
    /// </summary>
    public class BuilderDocument : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наличие документа
        /// </summary>
        public virtual YesNoNotSet DocumentExist { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        [Obsolete("Оставили только для проведения миграции, использовать BuilderDocumentType")]
        public virtual TypeDocument TypeDocument { get; set; }

        /// <summary>
        /// Тип документа из справочника
        /// </summary>
        public virtual BuilderDocumentType BuilderDocumentType { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
