using Bars.B4;
namespace Bars.Gkh.Repair
{
    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("repairprogram", "B4.controller.dict.RepairProgram", requiredPermission: "GkhRepair.RepairProgram.View"));
            map.AddRoute(new ClientRoute("repairobject", "B4.controller.RepairObject", requiredPermission: "GkhRepair.RepairObjectViewCreate.View"));
            map.AddRoute(new ClientRoute("repaircontroldate", "B4.controller.RepairControlDate", requiredPermission: "GkhRepair.RepairControlDate.View"));
            map.AddRoute(new ClientRoute("repairobjectmassstatechange", "B4.controller.RepairObjectMassStateChange", requiredPermission: "GkhRepair.RepairObjectMassStateChange.View"));
        }
    }
}