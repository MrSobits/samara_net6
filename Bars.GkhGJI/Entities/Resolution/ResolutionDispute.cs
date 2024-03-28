namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Оспаривание постановления ГЖИ
    /// </summary>
    public class ResolutionDispute : BaseGkhEntity
    {
        /// <summary>
        /// постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// вид суда
        /// </summary>
        public virtual TypeCourtGji Court { get; set; }

        /// <summary>
        /// Инстанция
        /// </summary>
        public virtual InstanceGji Instance { get; set; }

        /// <summary>
        /// Решение суда
        /// </summary>
        public virtual CourtVerdictGji CourtVerdict { get; set; }

        /// <summary>
        /// дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Постановление обжаловано
        /// </summary>
        public virtual ResolutionAppealed Appeal { get; set; }

        /// <summary>
        /// Протест прокуратуры
        /// </summary>
        public virtual bool ProsecutionProtest { get; set; }

        /// <summary>
        /// Юрист
        /// </summary>
        public virtual Inspector Lawyer { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}