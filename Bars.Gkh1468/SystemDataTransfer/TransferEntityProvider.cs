namespace Bars.Gkh1468.SystemDataTransfer
{
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh1468.Entities;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Add<PublicService>("Коммунальные услуги").AddComparer(x => x.Name);
        }
    }
}