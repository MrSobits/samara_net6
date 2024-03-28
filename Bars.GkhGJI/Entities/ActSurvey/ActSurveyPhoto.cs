namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// фото в акте обследования
    /// </summary>
    public class ActSurveyPhoto : BaseEntity
    {
        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual ActSurvey ActSurvey { get; set; }

        /// <summary>
        /// Выводить на печать?
        /// </summary>
        public virtual bool IsPrint { get; set; }

        /// <summary>
        /// Дата изображения
        /// </summary>
        public virtual DateTime? ImageDate { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual ImageGroupSurveyGji Group { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}