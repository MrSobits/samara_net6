namespace Bars.Gkh.Entities.Dicts
{
    using System;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Работы
    /// </summary>
    public class AdditWork : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Норматив
        /// </summary>
        public virtual decimal? Percentage { get; set; }

        /// <summary>
        /// Очередность
        /// </summary>
        public virtual int? Queue { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }
    }
}
