namespace Bars.Gkh.RegOperator.Regions.Tyumen
{
    using B4;
    using System.Linq;
    using TextValues;

    /// <summary>
    ///     Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        /// <summary>
        /// Текстовое обозначение пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
        
        /// <summary>
        /// Инициализация меню
        /// </summary>
        /// <param name="root"></param>
        public void Init(MenuItem root)
        {
            root.Add("Справочники").Add("Капитальный ремонт").Add("Получатели запросов на редактирование", "requeststateperson").AddRequiredPermission("Gkh.Dictionaries.RequestStatePerson.View");
            root.Add("Администрирование").Add("Логи").Add("Запросы на редактирование", "requeststate").AddRequiredPermission("Gkh.Dictionaries.RequestStatePerson.View");
        }
    }
}