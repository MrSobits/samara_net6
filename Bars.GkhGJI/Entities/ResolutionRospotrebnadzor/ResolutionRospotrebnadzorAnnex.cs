namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    /// <summary>
    /// Приложение постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorAnnex : BaseGkhEntity
    {
        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public virtual ResolutionRospotrebnadzor Resolution { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }

        /// <summary>
        /// Статус файла 
        /// </summary>
        public virtual MessageCheck MessageCheck { get; set; }
    }
}