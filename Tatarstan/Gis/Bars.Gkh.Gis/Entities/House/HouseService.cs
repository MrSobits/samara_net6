namespace Bars.Gkh.Gis.Entities.House
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Услуги по дому
    /// </summary>
    public class HouseService : PersistentObject
    {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long HouseId { get; set; }

        /// <summary>
        /// Количество лицевых счетов
        /// </summary>
        public virtual long LsCount { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// Формула
        /// </summary>
        public virtual string Formula { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateBegin { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }
    }
}
