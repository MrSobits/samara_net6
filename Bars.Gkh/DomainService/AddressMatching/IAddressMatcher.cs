namespace Bars.Gkh.DomainService.AddressMatching
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сопоставлятель адресов
    /// </summary>
    public interface IAddressMatcher
    {
        /// <summary>
        /// Сопоставить адреса автоматически
        /// </summary>
        /// <param name="addreses">Адреса</param>
        /// <returns>Результат сопоставления</returns>
        IDataResult<IEnumerable<AddressMatch>> MatchAddresses(params AddressMatchDto[] addreses);
    }

    /// <summary>
    /// Класс DTO для сопоставления адресов
    /// </summary>
    public class AddressMatchDto
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Гуид из ФИАС
        /// </summary>
        public string HouseGuid { get; set; }
    }
}