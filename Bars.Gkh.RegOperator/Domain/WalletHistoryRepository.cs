namespace Bars.Gkh.RegOperator.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    /// <summary>
    /// Хранилище оперируемых в сессии траснферов
    /// </summary>
    public class WalletOperationHistoryRepository : IWalletOperationHistoryRepository
    {
        private static readonly object syncRoot = new object();

        private readonly IDictionary<string, HashSet<Transfer>> inTransferContainer;
        private readonly IDictionary<string, HashSet<Transfer>> outTransferContainer;

        /// <summary>
        /// .ctor
        /// </summary>
        public WalletOperationHistoryRepository()
        {
            this.inTransferContainer = new Dictionary<string, HashSet<Transfer>>();
            this.outTransferContainer = new Dictionary<string, HashSet<Transfer>>();
        }

        /// <summary>
        /// Добавить в кэш созданный трансфер
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Созданный трансфер для кошелька</param>
        public void AddTransfer(IWallet wallet, Transfer transfer)
        {
            if (transfer.Originator.IsNull())
            {
                return;
            }

            lock (WalletOperationHistoryRepository.syncRoot)
            {
                var isInTransfer = transfer.TargetGuid == wallet.WalletGuid;

                if (isInTransfer)
                {
                    this.GetTransfers(wallet, this.inTransferContainer).Add(transfer);
                }
                else
                {
                    this.GetTransfers(wallet, this.outTransferContainer).Add(transfer);
                }
            }
        }

        /// <summary>
        /// Метод проверяет, была ли уже првоедена отмена данного трансфера в текущей сессии
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Отменяемый трансфер</param>
        /// <returns></returns>
        public bool IsTransferCancelled(IWallet wallet, Transfer transfer)
        {
            var cancelled = false;

            lock (WalletOperationHistoryRepository.syncRoot)
            {
                var transfers = this.GetTransfers(wallet, this.inTransferContainer);
                if (transfers.Any(t => t.Originator.Equals(transfer)))
                {
                    cancelled = true;
                }

                transfers = this.GetTransfers(wallet, this.outTransferContainer);
                if (transfers.Any(t => t.Equals(transfer)))
                {
                    cancelled = true;
                }
            }

            return cancelled;
        }

        private HashSet<Transfer> GetTransfers(IWallet wallet, IDictionary<string, HashSet<Transfer>> transferContainer)
        {
            HashSet<Transfer> result;
            if (!transferContainer.TryGetValue(wallet.WalletGuid, out result))
            {
                result = new HashSet<Transfer>();
                transferContainer[wallet.WalletGuid] = result;
            }

            return result;
        }
    }
}