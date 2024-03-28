namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

    public interface ITransferService
    {
        IDataResult ListTransferForPaymentAccount(BaseParams baseParams);

        IDataResult ListTransferForSubsidyAccount(BaseParams baseParams);
    }
}