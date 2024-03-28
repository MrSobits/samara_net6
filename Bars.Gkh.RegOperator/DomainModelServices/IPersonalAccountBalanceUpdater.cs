namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Linq;
    using Entities;

    /// <summary>
    /// Интерфейс сервисс обновления баланса ЛС
    /// </summary>
    public interface IPersonalAccountBalanceUpdater
    {
        /// <summary>
        /// Обновить баланс
        /// </summary>
        /// <param name="accounts">Запрос по лицевым счетам</param>
        void UpdateBalance(IQueryable<BasePersonalAccount> accounts);
    }
}