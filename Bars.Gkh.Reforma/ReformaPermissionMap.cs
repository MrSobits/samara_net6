namespace Bars.Gkh.Reforma
{
    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class ReformaPermissionMap : PermissionMap
    {
        public ReformaPermissionMap()
        {
            this.Namespace("Gkh.Orgs.Managing.Register.Reforma", "Реформа ЖКХ");
            this.Permission("Gkh.Orgs.Managing.Register.Reforma.View", "Просмотр");

            this.Namespace("Gkh.RealityObject.Register.Reforma", "Реформа ЖКХ");
            this.Permission("Gkh.RealityObject.Register.Reforma.View", "Просмотр");


            this.Namespace("GkhDi.Reforma", "Интеграция с Реформой ЖКХ");

            this.Permission("GkhDi.Reforma.ChangeParams", "Настройка интеграции");
            this.Permission("GkhDi.Reforma.SyncLog", "Просмотр логов синхронизации");
            this.Permission("GkhDi.Reforma.Restore", "Восстановление данных");

            this.Namespace("GkhDi.Reforma.Dictionaries", "Справочники");
            this.Namespace("GkhDi.Reforma.Dictionaries.ReportingPeriod", "Отчетные периоды");
            this.Permission("GkhDi.Reforma.Dictionaries.ReportingPeriod.View", "Просмотр");
            this.Permission("GkhDi.Reforma.Dictionaries.ReportingPeriod.Edit", "Изменение");

            this.Namespace<DisclosureInfo>("GkhDi.Reforma.ManualIntegration", "Выборочная интеграция");
            this.Permission("GkhDi.Reforma.ManualIntegration.RealityObj", "Провести интеграцию по домам - показать");
            this.Permission("GkhDi.Reforma.ManualIntegration.ManagingOrganization", "Провести интеграцию по УО - показать");
        }
    }
}