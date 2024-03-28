namespace Bars.Gkh.Gis.NavigationMenu
{
    using B4;

    /// <summary>
    /// Меню карточки управляющей организации
    /// </summary>
    public class ManOrgMenuProvider : INavigationProvider
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key
        {
            get
            {
                return "ManOrg";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get
            {
                return "Меню карточки управляющей организации";
            }
        }

        /// <summary>
        /// Инициализация меню
        /// </summary>
        /// <param name="root">Пункт меню</param>
        public void Init(MenuItem root)
        {
        }
    }
}