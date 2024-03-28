namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using System.Collections.Generic;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    internal class ReportArgs
    {
        public string DocumentsPath { get; set; }
        public string FileName { get; set; }
        public IEnumerable<PaymentDocumentSnapshot> Snapshots { get; set; }
        public IEnumerable<AccountPaymentInfoSnapshot> AccountInfos { get; set; }
    }
}