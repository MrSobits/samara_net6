namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IRoleTypeHousePermissionService
    {
        IDataResult UpdatePermissions(BaseParams baseParams); 

        IDataResult GetRoleTypeHouses(BaseParams baseParams); 
    }
}