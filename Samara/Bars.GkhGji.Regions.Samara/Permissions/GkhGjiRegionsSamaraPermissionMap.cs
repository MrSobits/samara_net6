namespace Bars.GkhGji.Regions.Samara.Permissions
{
    using Bars.B4;

    class GkhGjiRegionsSamaraPermissionMap : PermissionMap
    {
        public GkhGjiRegionsSamaraPermissionMap()
        {
            #region Отчеты

            Permission("Reports.GJI.Form123Samara", "Отчет \"Форма 123_2\"");
            Permission("Reports.GJI.ProtocolResponsibility_2", "Отчет по протоколам-2");
            Permission("Reports.GJI.PrescriptionViolationRemoval", "Устранение нарушений по предписаниям");
            Permission("Reports.GJI.ControlAppealsExecution", "Контроль исполнения обращений");
            #endregion
        }
    }
}
