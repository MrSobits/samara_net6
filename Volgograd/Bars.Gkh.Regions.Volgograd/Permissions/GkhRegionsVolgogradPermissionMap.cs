namespace Bars.Gkh.Regions.Volgograd.Permissions
{
    using Bars.B4;

    class GkhRegionsVolgogradPermissionMap : PermissionMap
    {
        public GkhRegionsVolgogradPermissionMap()
        {
            #region Отчеты

            Permission("Reports.GKH.RepairPlanning", "Планирование ремонта");
            #endregion
        }
    }
}