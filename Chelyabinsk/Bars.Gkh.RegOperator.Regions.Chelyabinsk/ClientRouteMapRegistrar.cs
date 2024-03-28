namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("agentpir", "B4.controller.agentpir.AgentPIR"));
            map.AddRoute(new ClientRoute("agentpirdocument", "B4.controller.AgentPIRDocumentImport"));
            map.AddRoute(new ClientRoute("agentpirfileimport", "B4.controller.AgentPIRFileImport"));
            map.AddRoute(new ClientRoute("agentpirdebtorcreditimport", "B4.controller.AgentPIRDebtorCreditImport", requiredPermission: "Import.AgentPIRDebtorCreditImport.View"));
            map.AddRoute(new ClientRoute("agentpirdutyimport", "B4.controller.AgentPIRDutyImport"));
            map.AddRoute(new ClientRoute("agentpirdebtororderingimport", "B4.controller.AgentPIRDebtorOrderingImport", requiredPermission: "Import.AgentPIRDebtorOrderingImport.View"));
        }
    }
}