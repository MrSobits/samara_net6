namespace Bars.Gkh.Diagnostic.DomainServices
{
    using Bars.Gkh.Diagnostic.Entities;

    public interface IDiagnostic
    {
        string Name { get; }

        string Id { get; }

        void Run(DiagnosticResult diagnostic);
    }
}
