namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Маппинг т<see cref="RealityObjectTransfer"/>
    /// </summary>
    public class RealityObjectTransferMap : TransferMap<RealityObjectTransfer, RealityObjectPaymentAccount>
    {
        /// <inheritdoc />
        public RealityObjectTransferMap()
            : base("Трансферы дома", "REGOP_REALITY_TRANSFER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.CopyTransfer, "Трансфер, на основании которого было произведено копирование на дом").Column("COPY_TRANSFER_ID");
            this.Reference(x => x.Originator, "Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод").Column("ORIGINATOR_ID").Fetch();
        }
    }
}