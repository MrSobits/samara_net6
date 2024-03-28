namespace Bars.Gkh.RegOperator.CodedReports.PayDoc
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Properties;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class InvoiceAndActReport : BaseInvoiceReport
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> snapshots;

        public InvoiceAndActReport(IEnumerable<PaymentDocumentSnapshot> snapshots)
            : base(null)
        {
            this.snapshots = snapshots;
        }

        protected override byte[] Template
        {
            get { return Resources.PaymentDocumentInvoiceAndAct; }
        }

        public override string Name
        {
            get { return "Счет (с адресом)"; }
        }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("Записи", 
                    new InvoiceAndActDataProvider(ApplicationContext.Current.Container, this.snapshots))
            };
        }
    }
}