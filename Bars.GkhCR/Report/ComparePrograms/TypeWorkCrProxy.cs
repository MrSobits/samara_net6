namespace Bars.GkhCr.Report.ComparePrograms
{
    using Bars.GkhCr.Enums;

    internal sealed class TypeWorkCrAndFinanceSourceProxy
    {
        public long ObjectCrId { get; set; }

        public string FinanceSourceId { get; set; }

        public decimal? Volume { get; set; }

        public decimal? Sum { get; set; }

        public string Code { get; set; }

        public long RealtyObjectId { get; set; }

        public long ProgramCrId { get; set; }

        public string FinanceSource { get; set; }

        public decimal FundResource { get; set; }

        public decimal BudgetSubject { get; set; }

        public decimal BudgetMu { get; set; }

        public decimal OwnerResource { get; set; }

        public TypeFinanceGroup TypeFinanceGroup { get; set; }
    }
}
