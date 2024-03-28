namespace Bars.GkhGji.Regions.Tula
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

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
            root.Add("Справочники").Add("ГЖИ").Add("Правовые основания", "legalreason").AddRequiredPermission("GkhGji.Dict.LegalReason.View");
            root.Add("Справочники").Add("ГЖИ").Add("Предметы проверки", "surveysubject").AddRequiredPermission("GkhGji.Dict.SurveySubject.View");
        }
    }
}