namespace Bars.Gkh.RegOperator.DomainService.PaymentDocument
{
    using Bars.B4;

    public interface ISberbankPaymentDocService
    {
        IDataResult CreateReestr(BaseParams baseParams);
    }
}