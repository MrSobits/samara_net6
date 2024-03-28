namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сущность сопоставления внешнего адреса и адреса ЖКХ.Комплекс
    /// </summary>
    public class AddressMatch : BaseEntity
    {
        /// <summary>
        /// Внешний адрес
        /// </summary>
        public virtual string ExternalAddress { get; set; }

        /// <summary>
        /// Гуид дома
        /// </summary>
        public virtual string HouseGuid { get; set; }

        /// <summary>
        /// Дом, сопоставленный с указанным адресом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}