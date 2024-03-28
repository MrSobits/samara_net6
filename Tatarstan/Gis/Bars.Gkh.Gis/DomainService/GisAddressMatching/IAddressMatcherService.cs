namespace Bars.Gkh.Gis.DomainService.GisAddressMatching
{
    using System.Collections.Generic;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;

    public interface IAddressMatcherService
    {
        /// <summary>
        /// Проверить адреса на существование в ФИАС
        /// </summary>
        /// <param name="adressesList">проверяемые адреса</param>
        /// <returns>Список несопоставленных адресов</returns>
        List<HouseRegister> MatchAddresses(List<HouseRegister> adressesList);

        /// <summary>
        /// Сопоставить адрес
        /// </summary>
        /// <param name="address">Сопоставляемый адрес</param>
        /// <returns>Адрес из ФИАС</returns>
        FiasAddress MatchAddress(HouseRegister address);
    }
}
