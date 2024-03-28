namespace Bars.GkhGji.Regions.Chelyabinsk
{

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("GkhGji.AppealCitizens", "Обращения: Просмотр, создание");
            this.Permission("GkhGji.AppealCitizens.ShowOnlyFromEais", @"Показать только обращения ЕАИС ""Обращения граждан""");
        }
    }
}