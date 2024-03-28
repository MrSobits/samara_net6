namespace Bars.Gkh.Dto
{
    /// <summary>
    /// DTO для сохранения информации об адресе для сопоставления
    /// </summary>
    public class NotMatchedAddressInfo
    {
        /// <summary>
        /// Код в сторонней системе
        /// </summary>
        public string BillingId { get; set; }
        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Перегруженный метод сравнения
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим</param>
        /// <returns>True если код сторонней системы и адресные поля объектов равны</returns>
        protected bool Equals(NotMatchedAddressInfo other)
        {
            return string.Equals(BillingId, other.BillingId) && string.Equals(City, other.City) && string.Equals(Street, other.Street) && string.Equals(House, other.House);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NotMatchedAddressInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (BillingId != null ? BillingId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (House != null ? House.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
