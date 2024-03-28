namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Оспаривание постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorDispute : BaseGkhEntity
    {
        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public virtual ResolutionRospotrebnadzor Resolution { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Вид суда
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
        /// Постановление обжаловано
        /// </summary>
        public virtual ResolutionRospotrebnadzorAppealed Appeal { get; set; } = ResolutionRospotrebnadzorAppealed.HeadInspection;

        /// <summary>
        /// Юрист
        /// </summary>
        public virtual Inspector Lawyer { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Протест прокуратуры
        /// </summary>
        public virtual bool ProsecutionProtest { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}