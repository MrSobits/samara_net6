namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using Bars.B4.Utils;

    /// <summary>
    /// Трансфер на дом
    /// </summary>
    public class RealityObjectTransfer : TransferWithOwner<RealityObjectPaymentAccount>
    {
        /// <inheritdoc />
        public RealityObjectTransfer(RealityObjectPaymentAccount owner, string sourceGuid, string targetGuid, decimal amount, MoneyOperation operation)
            : base(owner, sourceGuid, targetGuid, amount, operation)
        {
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected RealityObjectTransfer()
        {
        }

        /// <summary>
        /// Трансфер, на основании которого было произведено копирование на дом
        /// </summary>
        public virtual PersonalAccountPaymentTransfer CopyTransfer
        {
            get { return base.Originator as PersonalAccountPaymentTransfer; }
            set { base.Originator = value; }
        }

        /// <summary>
        /// Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод
        /// </summary>
        public new virtual RealityObjectTransfer Originator
        {
            // производим soft-приведение типа, т.к. Оригинатором может быть оригинальный трансфер на ЛС
            get { return base.Originator as RealityObjectTransfer; }
            set { base.Originator = value; }
        }
    }
}