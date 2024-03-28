namespace Bars.GkhGjiCr.Permissions
{
    using B4;

    public class GkhGjiCrPermissionMap : PermissionMap
    {
        public GkhGjiCrPermissionMap()
        {
            Permission("Reports.GJI.InformationCrByResolution", "Информация по капремонту по постановлениям КМ РТ");
            Permission("Reports.GJI.Form1StateHousingInspection", "Отчет \"Форма 1 - госжилинспекция\"");
        }
    }
}