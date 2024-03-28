namespace Bars.Gkh.RegOperator.Exceptions
{
    using Bars.B4;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    /// <summary>
    /// Ошибка нехватки средств на кошельке
    /// </summary>
    public class WalletBalanceException : ValidationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">Владелец трансфера</param>
        /// <param name="wallet">Кошелек</param>
        /// <param name="message">Сообщение</param>
        public WalletBalanceException(ITransferOwner owner, IWallet wallet, string message) : base(message)
        {
            ArgumentChecker.NotNull(owner, nameof(owner));
            ArgumentChecker.NotNull(wallet, nameof(wallet));

            this.Owner = owner;
            this.Wallet = wallet;
        }

        /// <summary>
        /// Владелец трансфера
        /// </summary>
        public ITransferOwner Owner { get; }

        /// <summary>
        /// Кошелек
        /// </summary>
        public IWallet Wallet { get; }
    }
}
