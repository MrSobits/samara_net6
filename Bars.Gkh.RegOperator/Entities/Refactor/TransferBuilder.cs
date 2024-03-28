namespace Bars.Gkh.RegOperator.Entities.Refactor
{
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    /// <summary>
    /// Билдер трансфера
    /// </summary>
    public class TransferBuilder
    {
        private string sourceGuid;
        private string targetGuid;
        
        /// <summary>
        /// Поток средств
        /// </summary>
        public MoneyStream MoneyStream { get; protected set; }

        /// <summary>
        /// Владелец трансфера
        /// </summary>
        public ITransferOwner Owner { get; protected set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="owner">Владелец</param>
        /// <param name="moneyStream">Поток средств</param>
        protected TransferBuilder(ITransferOwner owner, MoneyStream moneyStream)
        {
            this.MoneyStream = moneyStream;
            this.Owner = owner;
        }

        /// <summary>
        /// Метод создаёт типизированный билдер трансфера
        /// </summary>
        /// <typeparam name="TOwner">Тип владельца</typeparam>
        /// <param name="owner">Владелец</param>
        /// <param name="moneyStream">Поток средств</param>
        /// <returns><see cref="TransferBuilder{T}"/></returns>
        public static TransferBuilder<TOwner> Create<TOwner>(TOwner owner, MoneyStream moneyStream)
            where TOwner : ITransferOwner
        {
            return TransferBuilder<TOwner>.Create(owner, moneyStream);
        }

        /// <summary>
        /// Установить целевое направление трансфера
        /// </summary>
        /// <param name="guid">Гуид</param>
        /// <returns><see cref="TransferBuilder"/></returns>
        public TransferBuilder SetTargetGuid(string guid)
        {
            this.targetGuid = guid;
            this.sourceGuid = this.MoneyStream.SourceOrTargetGuid;

            return this;
        }

        /// <summary>
        /// Установить источник направления трансфера
        /// </summary>
        /// <param name="guid">Гуид</param>
        /// <returns><see cref="TransferBuilder"/></returns>
        public TransferBuilder SetSourceGuid(string guid)
        {
            this.targetGuid = this.MoneyStream.SourceOrTargetGuid;
            this.sourceGuid = guid;

            return this;
        }

        /// <summary>
        /// Установить целевое направление трансфера
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <returns><see cref="TransferBuilder"/></returns>
        public TransferBuilder SetTargetGuid(IWallet wallet)
        {
            return this.SetTargetGuid(wallet.TransferGuid);
        }

        /// <summary>
        /// Установить источник направления трансфера
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <returns><see cref="TransferBuilder"/></returns>
        public TransferBuilder SetSourceGuid(IWallet wallet)
        {
            return this.SetSourceGuid(wallet.TransferGuid);
        }

        /// <summary>
        /// Получить трансфер
        /// </summary>
        /// <returns><see cref="Transfer"/></returns>
        public Transfer Build()
        {
            if (this.MoneyStream.Amount == 0)
            {
                return null;
            }

            return this.Owner.CreateTransfer(this.sourceGuid, this.targetGuid, this.MoneyStream);
        }   
    }

    /// <summary>
    /// Типизированный билдер трансфера
    /// </summary>
    /// <typeparam name="TOwner">Тип владельца</typeparam>
    public class TransferBuilder<TOwner> : TransferBuilder
        where TOwner : ITransferOwner
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="owner">Владелец</param>
        /// <param name="moneyStream">Поток средств</param>
        public TransferBuilder(ITransferOwner owner, MoneyStream moneyStream) : base(owner, moneyStream)
        {
        }

        /// <summary>
        /// Метод создаёт типизированный билдер трансфера
        /// </summary>
        /// <param name="owner">Владелец</param>
        /// <param name="moneyStream">Поток средств</param>
        /// <returns><see cref="TransferBuilder{T}"/></returns>
        public static TransferBuilder<TOwner> Create(TOwner owner, MoneyStream moneyStream)
        {
            return new TransferBuilder<TOwner>(owner, moneyStream);
        }
    }
}