using Bars.B4;
namespace Bars.Gkh.Diagnostic
{
    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("diagnostic", "B4.controller.diagnostic.CollectedDiagnosticResult"));
        }
    }
}
