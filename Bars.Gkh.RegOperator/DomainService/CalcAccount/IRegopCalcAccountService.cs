namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Linq;

    using B4;

    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Сервис расчетных счетов регоператора
    /// </summary>
    public interface IRegopCalcAccountService
    {
        /// <summary>
        /// Получение счетов реестр домов регионального оператора
        /// </summary>
        /// <param name="ro">Жилой дом</param>
        RegopCalcAccount GetRegopAccount(RealityObject ro);

        /// <summary>
        /// Отображения суммы счетов реестр домов регионального оператора
        /// </summary>
        IDataResult ListRegister(BaseParams baseParams);

        /// <summary>
        /// Формирования выборки 
        /// </summary>
        IQueryable<RegopCalcAccountRoProxy> GetProxyQueryable(BaseParams baseParams);

        /// <summary>
        /// Список расчетных счетов по регоператору
        /// </summary>
        IDataResult ListByRegop(BaseParams baseParams);
    }
}