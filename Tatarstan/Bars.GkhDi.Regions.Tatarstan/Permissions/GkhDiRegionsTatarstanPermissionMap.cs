namespace Bars.GkhDi.Regions.Tatarstan.Permissions
{
    using B4;
    
    public class GkhDiRegionsTatarstanPermissionMap : PermissionMap
    {
        public GkhDiRegionsTatarstanPermissionMap()
        {
            this.Namespace("GkhDi.Dict.MeasuresReduceCosts", "Меры по снижению расходов");
            this.CRUDandViewPermissions("GkhDi.Dict.MeasuresReduceCosts");
            this.Permission("GkhDi.DisinfoRealObj.Actions.CopyUninhabitablePremises", "Копирование сведений об использовании нежилых помещений из периода в период");
            this.Permission("GkhDi.DisinfoRealObj.Actions.CopyCommonAreas", "Копирование сведений об использовании мест общего пользования из периода в период");
        }
    }
}