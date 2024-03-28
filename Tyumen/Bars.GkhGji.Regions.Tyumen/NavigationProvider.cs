namespace Bars.GkhGji.Regions.Tyumen
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Сообщения граждан").Add("Сообщения граждан").Add("Уведомления заявителю", "applicantnotification").AddRequiredPermission("Gkh.Dictionaries.Suggestion.ApplicantNotification.View");

            root.Add("Участники процесса").Add("Роли контрагента").Add("Операторы связи", "networkoperator").WithIcon("tyumen-operator").AddRequiredPermission("Gkh.Orgs.NetworkOperator.View");

            root.Add("Справочники").Add("Общие").Add("Техническое решение", "techdecision").AddRequiredPermission("Gkh.Dictionaries.Techdecision.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения из ФГИС ЕГРН", "smevegrn").AddRequiredPermission("GkhGji.SMEV.SMEVEGRN.View");

            root.Add("Жилищная инспекция").Add("Лицензирование").Add("Реестр лицензий с домами", "manorglicenseregistergis").WithIcon("menuManorgLicenseRegister").AddRequiredPermission("GkhGji.LicenseGis.View");
        }
    }
}