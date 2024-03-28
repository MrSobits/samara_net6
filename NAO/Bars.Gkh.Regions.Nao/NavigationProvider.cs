using Bars.B4;
using Bars.Gkh.TextValues;

namespace Bars.Gkh.Regions.Nao
{
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
            var dictsRoot = root.Add("Справочники").WithIcon("gkh-dict");
            var commonDictsRoot = dictsRoot.Add("Общие");
            commonDictsRoot.Add("Настройки для выгрузки в Клиент-Сбербанк", "assberbankclient").AddRequiredPermission("Gkh.Dictionaries.ASSberbankClient.View");
        }
    }
}
