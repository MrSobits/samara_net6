namespace Bars.Gkh.DomainService.AddressMatching
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс сервиса сопоставления адресов внешних систем
    /// </summary>
    public interface IImportAddressMatchService
    {
        /// <summary>
        /// Сопоставить автоматически
        /// </summary>
        /// <returns>Результат сопоставления</returns>
        IDataResult<IEnumerable<AddressMatch>> MatchAutomatically(params AddressMatchDto[] addreses);
    }
}