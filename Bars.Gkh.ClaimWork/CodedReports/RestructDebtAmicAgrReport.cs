namespace Bars.Gkh.ClaimWork.CodedReports
{
    using Modules.ClaimWork.Enums;

    /// <summary>
    /// Соглашение о погашении задолженности по мировому соглашению
    /// </summary>
    public class RestructDebtAmicAgrReport : RestructDebtReport
    {
        private const string ReportName = "Соглашение о погашении задолженности по мировому соглашению";

        public override string Id => "RestructDebtAmicAgr";

        public override string Name => RestructDebtAmicAgrReport.ReportName;
        
        public override string CodeForm => "RestructDebtAmicAgr";

        public override string OutputFileName { get; set; } = RestructDebtAmicAgrReport.ReportName;

        public override ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.RestructDebtAmicAgr;
    }
}