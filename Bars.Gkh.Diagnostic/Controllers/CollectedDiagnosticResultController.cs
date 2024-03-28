namespace Bars.Gkh.Diagnostic.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.Gkh.Diagnostic.DomainServices;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Enums;

    public class CollectedDiagnosticResultController : B4.Alt.DataController<CollectedDiagnosticResult>
    {
        public BaseDataResult RunDiagnostic(BaseParams baseParams)
        {
            var runDiagnosticService = Container.Resolve<IRunDiagnosticService>();

            try
            {
                var diagnosticRunTread = new Thread(
                    () => ExplicitSessionScope.CallInNewScope(runDiagnosticService.RunDiagnostic));

                diagnosticRunTread.Start();
            }
            finally
            {
                Container.Release(runDiagnosticService);
            }

            return new BaseDataResult(true);
        }
    }
}
