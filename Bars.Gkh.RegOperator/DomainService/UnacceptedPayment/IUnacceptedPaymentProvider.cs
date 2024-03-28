namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;

    using Bars.Gkh.Import;
    using Wcf.Contracts.PersonalAccount;
    using Impl;

    public interface IUnacceptedPaymentProvider
    {
        Entities.BankDocumentImport CreateUnacceptedPayments(
            ILogImport logImport,
            IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo,
            string source,
            TransitAccountProxy transitAccountProxy = null);
    }
}