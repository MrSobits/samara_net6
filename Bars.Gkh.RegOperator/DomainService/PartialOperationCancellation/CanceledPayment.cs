namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation
{
    using System;

    public interface ICancelablePayment
    {
        decimal Sum { get; }

        DateTime PaymentDate { get; }
    }
}