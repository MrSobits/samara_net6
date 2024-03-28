namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;
    using Bars.GkhCr.Entities;

    public interface IPerformedWorkActPaymentService
    {
        IDataResult SaveWithDetails(BaseParams baseParams);

        IDataResult SaveWithDetails(PerformedWorkActPayment payment, PaymentOrderDetailSource[] details);

        IDataResult ExportToTxt(BaseParams baseParams);
    }
}