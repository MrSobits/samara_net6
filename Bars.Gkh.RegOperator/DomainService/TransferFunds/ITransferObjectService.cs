namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    /// <summary>
    /// Интерфейс сервиса перечислений средств
    /// </summary>
    public interface ITransferObjectService
    {
        /// <summary>
        /// Расчитать начисления
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат расчёта</returns>
        IDataResult Calc(BaseParams baseParams);

        /// <summary>
        /// Посчитать начисления по домам за период, исключая начисления по помещениям с типом собственности "Муниципальная"
        /// </summary>
        /// <param name="date"> Период начислений </param>
        /// <param name="chargeAccountRoIds"> Список идентификаторов домов </param>
        /// <returns> Словарь: Ключ - ИД дома; Значение - начисления </returns>
        Dictionary<long, decimal> GetPaids(DateTime date, IQueryable<long> chargeAccountRoIds);
    }
}