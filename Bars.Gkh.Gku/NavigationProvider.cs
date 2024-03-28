// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.Gkh.Gku
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
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
            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Сведения о ЖКУ", "gkuinfo").AddRequiredPermission("Gkh.GkuInfo.View");

            root.Add("Справочники").Add("Жилищно-коммунальное хозяйство").Add("Тарифы ЖКУ", "gkutarif").AddRequiredPermission("GkhGji.Dict.GkuTariff.View");
        }
    }
}