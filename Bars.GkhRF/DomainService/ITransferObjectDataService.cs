namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Интерфейс реализуется в модуле RegOperator
    /// </summary>
    public interface ITransferObjectDataService
    {
        /// <summary>
        /// Посчитать итого оплачено по домам
        /// </summary>
        /// <param name="date">
        /// Дата перечисления.
        /// </param>
        /// <param name="chargeAccountRoIds">
        /// Список идентификаторов домов.
        /// </param>
        /// <returns>
        /// Словарь: Ключ - ИД дома; Значение - начисления
        /// </returns>
        Dictionary<long, decimal> GetPaids(DateTime date, IQueryable<long> chargeAccountRoIds);
    }
}
