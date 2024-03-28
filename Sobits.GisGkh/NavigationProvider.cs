namespace Sobits.GisGkh
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <inheritdoc />
    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }
#pragma warning disable 618
        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;

        public void Init(MenuItem root)
        {
            root.Add("Администрирование").Add("Интеграция с внешними системами").Add("Интеграция с ГИС ЖКХ", "gisgkhintegration").AddRequiredPermission("Administration.OutsideSystemIntegrations.GisGkh.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Интеграция с ГИС ЖКХ", "gisgkhintegrationinsmev").AddRequiredPermission("GkhGji.GisGkh.View");
        }
    }
}