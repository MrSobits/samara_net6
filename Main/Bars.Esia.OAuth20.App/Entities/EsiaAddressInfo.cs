namespace Bars.Esia.OAuth20.App.Entities
{
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Enums;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация об адресе в ЕСИА
    /// </summary>
    public class EsiaAddressInfo
    {
        /// <summary>
        /// Тип адреса
        /// </summary>
        public AddressType AddressType { get; }

        /// <summary>
        /// Индекс
        /// </summary>
        public string ZipCode { get; }

        /// <summary>
        /// Идентификатор страны
        /// </summary>
        public string CountryId { get; }

        /// <summary>
        /// Адрес в виде строки
        /// (без дома, строения, корпуса и 6номера квартиры)
        /// </summary>
        public string AddressStr { get; }

        /// <summary>
        /// Строение
        /// </summary>
        public string Building { get; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Frame { get; }

        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat { get; }

        /// <summary>
        /// Код по ФИАС (код КЛАДР)
        /// </summary>
        public string FiasCode { get; }

        /// <summary>
        /// Регион
        /// </summary>
        public string Region { get; }

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Внутригородской район
        /// </summary>
        public string District { get; }

        /// <summary>
        /// Район
        /// </summary>
        public string Area { get; }

        /// <summary>
        /// Поселение
        /// </summary>
        public string Settlement { get; }

        /// <summary>
        /// Доп. территория
        /// </summary>
        public string AdditionArea { get; }

        /// <summary>
        /// Улица на доп.территории
        /// </summary>
        public string AdditionAreaStreet { get; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; }

        public EsiaAddressInfo()
        {
        }

        public EsiaAddressInfo(JObject addrInfo)
        {
            if (addrInfo == null)
                return;

            this.AddressType = addrInfo.Get("type")?.ToString() == "PLV"
                ? AddressType.Residential
                : AddressType.Registration;
            this.ZipCode = addrInfo.Get("zipCode")?.ToString();
            this.CountryId = addrInfo.Get("countryId")?.ToString();
            this.AddressStr = addrInfo.Get("addressStr")?.ToString();
            this.Building = addrInfo.Get("building")?.ToString();
            this.Frame = addrInfo.Get("frame")?.ToString();
            this.House = addrInfo.Get("house")?.ToString();
            this.Flat = addrInfo.Get("flat")?.ToString();
            this.FiasCode = addrInfo.Get("fiasCode")?.ToString();
            this.Region = addrInfo.Get("region")?.ToString();
            this.City = addrInfo.Get("city")?.ToString();
            this.District = addrInfo.Get("district")?.ToString();
            this.Area = addrInfo.Get("area")?.ToString();
            this.Settlement = addrInfo.Get("settlement")?.ToString();
            this.AdditionArea = addrInfo.Get("additionArea")?.ToString();
            this.AdditionAreaStreet = addrInfo.Get("additionAreaStreet")?.ToString();
            this.Street = addrInfo.Get("street")?.ToString();
        }
    }
}