namespace Bars.Gkh.RegOperator.Domain
{
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    /// <summary>
    /// Интерфейс хранилища оперируемых в сессии траснферов
    /// </summary>
    public interface IWalletOperationHistoryRepository
    {
        /// <summary>
        /// Добавить в кэш созданный трансфер
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Созданный трансфер для кошелька</param>
        void AddTransfer(IWallet wallet, Transfer transfer);

        /// <summary>
        /// Метод проверяет, была ли уже првоедена отмена данного трансфера в текущей сессии
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Отменяемый трансфер</param>
        /// <returns>Результат проверки</returns>
        bool IsTransferCancelled(IWallet wallet, Transfer transfer);
    }
}