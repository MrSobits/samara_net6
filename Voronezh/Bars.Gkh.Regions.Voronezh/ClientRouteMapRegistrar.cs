namespace Bars.Gkh.Regions.Voronezh
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("dateareaownerrecordimport",
                 "B4.controller.Import.DateAreaOwner",
                 requiredPermission: "Import.DateAreaOwner"));
             map.AddRoute(new ClientRoute("dateareaowner", "B4.controller.DateAreaOwner", requiredPermission: "Rosreg.AllRosreg.Import"));
            map.AddRoute(new ClientRoute("owneraccountroomcomparison", "B4.controller.OwnerAccountRoomComparison"));
            map.AddRoute(new ClientRoute("rosregextract", "B4.controller.RosRegExtract", requiredPermission: "Rosreg.AllRosreg.Import"));
          //  map.AddRoute(new ClientRoute("viewrosregextract", "B4.controller.ViewRosRegExtract"));
            map.AddRoute(new ClientRoute("rosregextractdesc", "B4.controller.RosRegExtractDesc", requiredPermission: "Rosreg.AllRosreg.RoomEGRN"));
        }
    }
}