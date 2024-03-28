namespace Bars.GisIntegration.Gkh
{
    using Bars.B4;

    public class RisPermissionMap : PermissionMap
    {
        public RisPermissionMap()
        {
            this.Namespace("Administration.OutsideSystemIntegrations", "Интеграция с внешними системами");

            this.Namespace("Administration.OutsideSystemIntegrations.Gis", "Интеграция с ГИС");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.View", "Просмотр");

            this.Namespace("Administration.OutsideSystemIntegrations.Gis.Dictions", "Справочники");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Dictions.View", "Просмотр");

            this.Namespace("Administration.OutsideSystemIntegrations.Gis.Methods", "Методы");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Methods.ExecuteMethod", "Выполнить метод");

            // экспортеры данных
            this.Namespace("Administration.OutsideSystemIntegrations.Gis.Exporters", "Экспортеры данных");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.OrgRegistryExporter", "Импорт данных организации");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.DataProviderExporter", "Импорт сведений о поставщиках информации");           

            // задачи
            this.Namespace("Administration.OutsideSystemIntegrations.Gis.Tasks", "Задачи");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Tasks.View", "Просмотр");

            this.Namespace("Administration.OutsideSystemIntegrations.GisSettings", "Настройки интеграции с ГИС");
            this.Permission("Administration.OutsideSystemIntegrations.GisSettings.View", "Просмотр");

            // делегирование
            this.Namespace("Administration.OutsideSystemIntegrations.Delegacy", "Делегирование ГИС");
            this.Permission("Administration.OutsideSystemIntegrations.Delegacy.View", "Просмотр");

            // роли гис
            this.Namespace("Administration.OutsideSystemIntegrations.GisRole", "Роли ГИС");
            this.Permission("Administration.OutsideSystemIntegrations.GisRole.View", "Просмотр");

            // роли контрагентов рис
            this.Namespace("Administration.OutsideSystemIntegrations.RisContragentRole", "Роли контрагентов РИС");
            this.Permission("Administration.OutsideSystemIntegrations.RisContragentRole.View", "Просмотр");
        }
    }
}