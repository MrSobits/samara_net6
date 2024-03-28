namespace Bars.GkhGji.Regions.Tyumen.Permissions
{
    using Bars.B4;

    public class TyumenPermissionMap : PermissionMap
    {
        public TyumenPermissionMap()
        {
            Namespace("Gkh.Dictionaries.Suggestion.ApplicantNotification", "Уведомления заявителю");
            Permission("Gkh.Dictionaries.Suggestion.ApplicantNotification.View", "Просмотр");
            Permission("Gkh.Dictionaries.Suggestion.ApplicantNotification.Create", "Создание");
            Permission("Gkh.Dictionaries.Suggestion.ApplicantNotification.Edit", "Редактирование");
            Permission("Gkh.Dictionaries.Suggestion.ApplicantNotification.Delete", "Удаление");

            Namespace("Gkh.Orgs.NetworkOperator", "Операторы связи");
            Permission("Gkh.Orgs.NetworkOperator.View", "Просмотр");

            Namespace("Gkh.Dictionaries.Techdecision", "Техническое решение");
            Permission("Gkh.Dictionaries.Techdecision.View", "Просмотр");

            Namespace("GkhGji", "Модуль ГЖИ");
            Namespace("GkhGji.LicenseGis", "Реестр лицензий с домами");
            CRUDandViewPermissions("GkhGji.LicenseGis");

            this.Namespace("GkhGji.SMEV", "СМЭВ");
            this.Namespace("GkhGji.SMEV.SMEVEGRN", "Запросы в ЕГРН");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRN");
        }
    }
}