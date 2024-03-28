namespace Bars.GkhGji.Regions.Tula.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    using Gkh.Entities;

    /// <summary>
    /// Заключение о техническом состоянии
    /// </summary>
    public class ActSurveyConclusion : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual Bars.GkhGji.Entities.ActSurvey ActSurvey { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// ДЛ, вынесшее заключение
        /// </summary>
        public virtual Inspector Official { get; set; }

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
