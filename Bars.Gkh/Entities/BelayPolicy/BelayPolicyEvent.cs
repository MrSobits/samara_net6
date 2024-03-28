namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Страховой случай
    /// </summary>
    public class BelayPolicyEvent : BaseGkhEntity
    {
        /// <summary>
        /// Страховой полис
        /// </summary>
        public virtual BelayPolicy BelayPolicy { get; set; }

        /// <summary>
        /// Дата наступления страхового случая
        /// </summary>
        public virtual DateTime? EventDate { get; set; }

        /// <summary>
        /// Описание страхового случая
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}
