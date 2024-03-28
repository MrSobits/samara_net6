namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Источник поступления обращения
    /// </summary>
    public class AppealCitsSource : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime? RevenueDate { get; set; }

        /// <summary>
        /// Источник поступления
        /// </summary>
        public virtual RevenueSourceGji RevenueSource { get; set; }

        /// <summary>
        /// Исх. № источника поступления
        /// </summary>
        public virtual string RevenueSourceNumber { get; set; }

        /// <summary>
        /// Форма поступления
        /// </summary>
        public virtual RevenueFormGji RevenueForm { get; set; }

        /// <summary>
        /// Дата ССТУ
        /// </summary>
        public virtual DateTime? SSTUDate { get; set; }
    }
}