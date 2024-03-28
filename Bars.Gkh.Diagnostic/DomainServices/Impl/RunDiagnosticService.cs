namespace Bars.Gkh.Diagnostic.DomainServices.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Enums;

    using Castle.Windsor;

    public class RunDiagnosticService : IRunDiagnosticService
    {
        public IWindsorContainer Container { get; set; }

        public void RunDiagnostic()
        {
            var diagnostics = Container.ResolveAll<IDiagnostic>();

            var dsDiagnosticResult = Container.Resolve<IDomainService<DiagnosticResult>>();

            var dsCollectedDiagnostic = Container.Resolve<IDomainService<CollectedDiagnosticResult>>();

            using (Container.Using(diagnostics, dsDiagnosticResult, dsCollectedDiagnostic))
            {
                var collectedDiagnostic = new CollectedDiagnosticResult { State = CollectedDiagnosticResultState.Working };

                dsCollectedDiagnostic.Save(collectedDiagnostic);

                var diagnosticDict = new Dictionary<IDiagnostic, DiagnosticResult>();

                foreach (var diagnostic in diagnostics)
                {
                    var diagnResult = new DiagnosticResult
                    {
                        CollectedDiagnostic = collectedDiagnostic,
                        Name = diagnostic.Name,
                        State = DiagnosticResultState.Waiting
                    };

                    dsDiagnosticResult.Save(diagnResult);

                    diagnosticDict.Add(diagnostic, diagnResult);
                }

                foreach (var diagnosticResultPair in diagnosticDict)
                {
                    var diagnostic = diagnosticResultPair.Key;

                    var diagnResult = diagnosticResultPair.Value;

                    diagnResult.State = DiagnosticResultState.Working;

                    dsDiagnosticResult.Update(diagnResult);

                    diagnostic.Run(diagnResult);
                }

                if (diagnosticDict.Values.Any(x => x.State == DiagnosticResultState.Fail))
                {
                    collectedDiagnostic.State = CollectedDiagnosticResultState.Fail;
                    dsCollectedDiagnostic.Save(collectedDiagnostic);
                }
                else
                {
                    collectedDiagnostic.State = CollectedDiagnosticResultState.Success;
                    dsCollectedDiagnostic.Save(collectedDiagnostic);
                }
            }
        }
    }
}
