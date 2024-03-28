namespace Bars.Gkh.Diagnostic.Entities
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Diagnostic.Enums;

    public class CollectedDiagnosticResult : BaseEntity
    {
        private IList<DiagnosticResult> diagnosticResults;

        public virtual CollectedDiagnosticResultState State { get; set; }

        public CollectedDiagnosticResult()
        {
            this.diagnosticResults = new List<DiagnosticResult>();
        }

        public virtual IList<DiagnosticResult> DiagnosticResults
        {
            get
            {
                return diagnosticResults; 
            }

            set
            {
                diagnosticResults = value;
            }
        }
    }
}
