namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// Справочник Производственный календарь
    /// </summary>
    public class ProdCalendar : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual DateTime ProdDate { get; set; }
    }
}