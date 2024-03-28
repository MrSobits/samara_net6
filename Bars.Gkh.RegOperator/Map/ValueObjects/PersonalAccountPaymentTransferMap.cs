namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Маппинг <see cref="PersonalAccountPaymentTransfer"/>
    /// </summary>
    public class PersonalAccountPaymentTransferMap : TransferMap<PersonalAccountPaymentTransfer, BasePersonalAccount>
    {
        /// <inheritdoc />
        public PersonalAccountPaymentTransferMap()
            : base("Трансферы оплат ЛС", "REGOP_TRANSFER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Originator, "Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод").Column("ORIGINATOR_ID").Fetch();
        }
    }
}