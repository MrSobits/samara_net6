using System.Collections.Generic;

namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using Gkh.Entities;
    using Impl;

    /// <summary>
    /// Интерфейс для получения данных банка
    /// </summary>
    public interface IBankAccountDataProvider
    {
        /// <summary>
        /// Получить номер расчетного счета банка, к которому привязан дом
        /// </summary>
        /// <param name="ro">МКД</param>
        /// <returns>Номер р/с</returns>
        string GetBankAccountNumber(RealityObject ro);

        /// <summary>
        /// Получить инф-цию о расчетном счете банка, к которому привязан дом
        /// </summary>
        /// <param name="roId">id МКД</param>
        /// <returns>Номер р/с</returns>
        AccInfoProxy GetBankAccountInfo(long roId);

        Dictionary<long, string> GetBankNumbersForCollection(IEnumerable<RealityObject> ros);
    }
}