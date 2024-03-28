namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Услуги по дому
    /// </summary>
    public class PersonalAccountService : PersistentObject
    {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long ApartmentId { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual string Tariff { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateBegin { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? TariffDateBegin { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? TariffDateEnd { get; set; }
    }
}
