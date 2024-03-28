namespace Bars.GkhEdoInteg.Permissions
{
    using B4;

    public class GkhEdoIntegPermissionMap : PermissionMap
    {
        public GkhEdoIntegPermissionMap()
        {
            Namespace("GkhEdoInteg", "Модуль интеграция с ЭДО");

            Namespace("GkhEdoInteg.SendEdo", "Отправка данных в ЭДО: Просмотр");
            Permission("GkhEdoInteg.SendEdo.View", "Просмотр");

            Namespace("GkhEdoInteg.LogEdo", "Данные по интеграции с ЭДО");
            Permission("GkhEdoInteg.LogEdo.View", "Просмотр");

            Namespace("GkhEdoInteg.LogEdoRequests", "Лог интеграции с ЭДО");
            Permission("GkhEdoInteg.LogEdoRequests.View", "Просмотр");

            Namespace("GkhEdoInteg.Compare.RevenueSource", "Сопоставление справочника источник поступления: Просмотр");
            Permission("GkhEdoInteg.Compare.RevenueSource.View", "Просмотр");

            Namespace("GkhEdoInteg.Compare.RevenueForm", "Сопоставление справочника форма поступления: Просмотр");
            Permission("GkhEdoInteg.Compare.RevenueForm.View", "Просмотр");

            Namespace("GkhEdoInteg.Compare.KindStatement", "Сопоставление справочника вид обращения: Просмотр");
            Permission("GkhEdoInteg.Compare.KindStatement.View", "Просмотр");

            Namespace("GkhEdoInteg.Compare.Inspector", "Сопоставление справочника инспекторы: Просмотр");
            Permission("GkhEdoInteg.Compare.Inspector.View", "Просмотр");
        }
    }
}
