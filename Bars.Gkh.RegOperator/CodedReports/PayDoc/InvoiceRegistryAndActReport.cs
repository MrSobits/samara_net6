namespace Bars.Gkh.RegOperator.CodedReports
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Properties;
    using System.Collections.Generic;

    internal class InvoiceRegistryAndActReport : BaseInvoiceReport
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> snapshots;
        private readonly IEnumerable<AccountPaymentInfoSnapshot> accountInfos;

        public InvoiceRegistryAndActReport(
            IEnumerable<PaymentDocumentSnapshot> snapshots,
            IEnumerable<AccountPaymentInfoSnapshot> accountInfos)
            : base(null)
        {
            this.snapshots = snapshots;
            this.accountInfos = accountInfos;
        }

        protected override byte[] Template
        {
            get { return Resources.PaymentDocumentInvoiceRegistryAndAct; }
        }

        public override string Name
        {
            get { return "Счет (с реестром помещений)"; }
        }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new[]
            {
                new CodedDataSource("Записи",
                    new InvoiceRegistryAndActDataProvider(ApplicationContext.Current.Container, this.snapshots)),

                new CodedDataSource("ЛицевойСчет", 
                    new InvoiceAccountInfoDataProvider(ApplicationContext.Current.Container, this.accountInfos))
            };
        }
    }
}