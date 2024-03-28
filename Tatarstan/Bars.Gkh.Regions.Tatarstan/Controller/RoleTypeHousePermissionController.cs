namespace Bars.Gkh.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class RoleTypeHousePermissionController : B4.Alt.DataController<RoleTypeHousePermission>
    {
        public IRoleTypeHousePermissionService Service { get; set; }

        [ActionPermission("Gkh.RealityObject.RoleTypeHousePermission.Edit")]
        public override ActionResult Update(BaseParams baseParams)
        {
            var result = Service.UpdatePermissions(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRoleTypeHouses(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.GetRoleTypeHouses(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        } 
    }
}