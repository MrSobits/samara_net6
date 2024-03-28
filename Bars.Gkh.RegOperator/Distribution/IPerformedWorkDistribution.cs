namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Linq;

    using B4;

    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Распределение средств за ранее выполненные работы
    /// </summary>
    public interface IPerformedWorkDistribution
    {
        /// <summary>
        /// Применить распределение
        /// </summary>
        IDataResult Apply(BaseParams baseParams);

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetAccountsInfo(BaseParams baseParams);

        /// <summary>
        /// Получить остаток за ранее выполненные работы
        /// </summary>
        /// <param name="accountId">ЛС</param>
        /// <returns></returns>
        decimal GetPerformedWorkChargeBalance(long accountId);

        /// <summary>
        /// Распределить зачеты заранее выполненные работы для пакета ЛС
        /// </summary>
        /// <param name="personalAccounts">Список ЛС</param>
        void DistributeForAccountPacket(IQueryable<BasePersonalAccount> personalAccounts);
    }
}