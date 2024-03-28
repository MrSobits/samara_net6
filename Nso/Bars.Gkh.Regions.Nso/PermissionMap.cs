namespace Bars.Gkh.Regions.Nso
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Permission("Import.ManOrgRobjectImport", "Импорт УО и привязка их к домам");

            Namespace("Gkh.RealityObject.Register.Document", "Документы");
            CRUDandViewPermissions("Gkh.RealityObject.Register.Document");

            Permission("Reports.GJI.LocalGovernmentReport", "Реестр органов МЖК");
            Permission("Reports.GJI.TsjProvidedInfoReport", "Реестр сведений, предоставляемых ТСЖ");
        }
    }
}