namespace Bars.Gkh.RegOperator.CodedReports.PayDoc
{
    using System.Collections.Generic;
    using System.Text;
    using B4.Application;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using DataProviders;
    using Entities.PersonalAccount.PayDoc;
    using Properties;

    internal class LoanDisposal : BaseCodedReport
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> _accounts;
        protected StringBuilder _logger;

        private long _loanId;

        public LoanDisposal(long loanId)
        {
            _loanId = loanId;
        }

        public LoanDisposal()
        {
        }

        protected override byte[] Template
        {
            get
            {
                return Resources.LoanDisposal;
            }
        }

        public override string Name
        {
            get
            {
                return "Распоряжение о проведении заимствований";
            }
        }

        public override string Description { get; }

        public override IEnumerable<IDataSource> GetDataSources()
        {


            return new[]
            {
                new CodedDataSource("Записи", new LoanDisposalDataProvider(ApplicationContext.Current.Container, _loanId))
            };
        }

    }
}