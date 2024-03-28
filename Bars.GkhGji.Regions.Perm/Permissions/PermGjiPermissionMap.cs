namespace Bars.GkhGji.Regions.Perm.Permissions
{
    using Bars.B4;

    /// <summary>
    /// Права доступа
    /// </summary>
    public class PermGjiPermissionMap : PermissionMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PermGjiPermissionMap()
        {
            this.Permission("GkhGji.HeatSeason.AvailabilityAndConsumptionFuel", "Сведения о наличии и расходе топлива");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.ControlType_View", "Вид контроля");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.ControlType_View", "Вид контроля");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.ControlType_View", "Вид контроля");

            this.Permission("Reports.GJI.MotivatedProposalForLicenseReport", "Мотивированное предложение о выдачи лицензии");
            this.Permission("Reports.GJI.MotivatedProposalForRefusalLicenseReport", "Мотивированное предложение об отказе в выдачи лицензии");
        }
    }
}