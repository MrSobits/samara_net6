namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Маппинг <see cref="PersonalAccountChargeTransfer"/>
    /// </summary>
    public class PersonalAccountChargeTransferMap : TransferMap<PersonalAccountChargeTransfer, BasePersonalAccount>
    {
        /// <inheritdoc />
        public PersonalAccountChargeTransferMap()
            : base("Трансферы начислений ЛС", "REGOP_CHARGE_TRANSFER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Originator, "Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод").Column("ORIGINATOR_ID").Fetch();
        }
    }
}