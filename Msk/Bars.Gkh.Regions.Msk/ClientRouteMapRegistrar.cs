namespace Bars.Gkh.Regions.Msk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("mskdpkrimport", "B4.controller.import.MskDpkrImport"));
            map.AddRoute(new ClientRoute("mskceoimport", "B4.controller.import.MskCeoStateImport"));
            map.AddRoute(new ClientRoute("mskceoserviceimport", "B4.controller.import.MskCeoStateServiceImport"));
            map.AddRoute(new ClientRoute("mskceopointimport", "B4.controller.import.MskCeoPointImport"));
            map.AddRoute(new ClientRoute("mskoverhaulimport", "B4.controller.import.MskOverhaulImport"));
        }
    }
}
