namespace Bars.Gkh.Overhaul.Regions.Saratov
{
    public class PermissionMap: B4.PermissionMap
    {
        public PermissionMap()
        {
            #region Отчеты
            
            Namespace("Reports.Saratov.GkhOverhaul", "Модуль капитальный ремонт (ДПКР)");

            Permission("Reports.Saratov.GkhOverhaul.RealtyObjectCertificationControl", "Контроль паспортизации домов (Саратов)");
            
            #endregion
        }
    }
}