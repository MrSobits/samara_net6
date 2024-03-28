namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

    public interface IUnacceptedPaymentService
    {
        IDataResult AcceptPayments(BaseParams baseParams);

        IDataResult CancelPayments(BaseParams baseParams);

        IDataResult CancelPayments(long[] packetIds = null);

        IDataResult RemovePayments(BaseParams baseParams);
    }
}