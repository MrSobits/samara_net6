namespace Bars.Gkh.Diagnostic.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Diagnostic.Enums;

    public class DiagnosticResult : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual CollectedDiagnosticResult CollectedDiagnostic { get; set; }

        public virtual DiagnosticResultState State { get; set; }

        public virtual string Message { get; set; }

        public virtual void AddMessage(string additionalMessage)
        {
            this.Message = this.Message + additionalMessage;
        }
    }
}
