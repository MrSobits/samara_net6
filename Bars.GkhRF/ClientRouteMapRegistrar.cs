namespace Bars.GkhRf
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("limitcheck", "B4.controller.LimitCheck", requiredPermission: "GkhRf.LimitCheck.View"));
            map.AddRoute(new ClientRoute("requesttransferrf", "B4.controller.RequestTransferRf", requiredPermission: "GkhRf.RequestTransferRfViewCreate.View"));
            map.AddRoute(new ClientRoute("payment", "B4.controller.Payment", requiredPermission: "GkhRf.Payment.View"));
            map.AddRoute(new ClientRoute("transferrf", "B4.controller.TransferRf", requiredPermission: "GkhRf.TransferRf.View"));
            map.AddRoute(new ClientRoute("contractrf", "B4.controller.ContractRf", requiredPermission: "GkhRf.ContractRf.View"));
        }
    }
}