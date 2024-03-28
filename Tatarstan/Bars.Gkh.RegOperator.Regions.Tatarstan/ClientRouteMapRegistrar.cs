namespace Bars.Gkh.RegOperator.Regions.Tatarstan
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("confirmContributionDoc", "B4.controller.ConfirmContribution", requiredPermission: "GkhRegOp.RegionalFundUse.ConfirmContributionDocs.View"));
        }
    }
}