namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Адрес ПГМУ
    /// </summary>
    public class PgmuAddress : BaseEntity
    {
        /// <summary>
        /// Код расчетного центра
        /// </summary>
        public virtual int ErcCode { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        [AddressObjectLevel(0)]
        public virtual string District { get; set; }
        
        /// <summary>
        /// Город
        /// </summary>
        [AddressObjectLevel(1)]
        [AddressObjectPrefix("г.")]
        public virtual string Town { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [AddressObjectLevel(2)]
        [AddressObjectPrefix("ул.")]
        public virtual string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        [AddressObjectLevel(3)]
        [AddressObjectPrefix("д.")]
        public virtual string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        [AddressObjectLevel(4)]
        [AddressObjectPrefix("корп.")]
        public virtual string Building { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        [AddressObjectLevel(5)]
        [AddressObjectPrefix("кв.")]
        public virtual string Apartment { get; set; }

        /// <summary>
        /// Комната
        /// </summary>
        [AddressObjectLevel(6)]
        [AddressObjectPrefix("ком.")]
        public virtual string Room { get; set; }
    }
}