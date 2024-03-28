namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Voronezh.Enums;
    using System;

    /// <summary>
    /// Предварительная проверка
    /// </summary>
    public class PreliminaryCheck : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string PreliminaryCheckNumber { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string Result { get; set; }

        /// <summary>
        /// Номер ответа
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public virtual PreliminaryCheckResult PreliminaryCheckResult { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}