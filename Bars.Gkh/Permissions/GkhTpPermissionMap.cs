namespace Bars.Gkh.Permissions
{
    using Bars.B4;

    public class GkhTpPermissionMap : PermissionMap
    {
        public GkhTpPermissionMap()
        {
            this.Namespace("Gkh.RealityObject.Register.TechPassport", "Техпаспорт");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.TechPassport");

            #region Отчеты
            this.Namespace("Reports.GkhTp", "Модуль ЖКХ ТП");
            this.Permission("Reports.GkhTp.HouseTechPassportReport", "Отчет по техпаспорту дома");
            this.Permission("Reports.GkhTp.DevicesByRealityObject", "Отчет по приборам");
            this.Permission("Reports.GKH.RoTechPassportExport", "Экспорт технических паспортов домов");

            #endregion

            this.Permission("Import.TpElevatorsImport", "Импорт лифтов (техпаспорт)");
        }
    }
}