namespace Bars.Gkh.RegOperator.Domain.Repository.Wallets
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.Repositories;

    /// <summary>
    /// Репозиторий кошельков
    /// </summary>
    public interface IWalletRepository : IDomainRepository<Wallet>
    {
        /// <summary>
        /// Вернуть кошельки, с которых пришло
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Поздапрос</returns>
        IQueryable<IWallet> GetSourceWalletsFor(IEnumerable<Transfer> transfers);

        /// <summary>
        /// Вернуть кошельки, на которые пришло
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Поздапрос</returns>
        IQueryable<IWallet> GetTargetWalletsFor(IEnumerable<Transfer> transfers);

        /// <summary>
        /// Обновить баланс кошельков
        /// </summary>
        /// <param name="walletGuids">Идентификаторы кошельков</param>
        /// <param name="realityObject">Обновление баланса дома</param>
        void UpdateWalletBalance(List<string> walletGuids, bool realityObject = false);
    }
}
