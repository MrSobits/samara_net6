namespace Bars.Gkh.Regions.Nso
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("manorgrobjectimport", "B4.controller.import.ManOrgRobjectImport", requiredPermission: "Import.ManOrgRobjectImport"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/document", "B4.controller.realityobj.Document", requiredPermission: "Gkh.RealityObject.Register.Document.View"));
        }
    }
}