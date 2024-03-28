namespace Bars.Gkh.Regions.Perm
{
    using Bars.B4;

    /// <summary>
    /// Роуты
    /// </summary>
    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        /// <inheritdoc />
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gkh2winterlist", "B4.controller.integrations.ExternalRis"));

            map.AddRoute(new ClientRoute("manorglicense/{type}/{id}/requestlist", "B4.controller.manorglicense.RequestList"));
        }
    }
}