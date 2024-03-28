namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Период
    /// </summary>
    public class Period : BaseGkhEntity
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
