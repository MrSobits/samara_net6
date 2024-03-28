// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhDi.Regions.Tatarstan
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
            var dict = root.Add("Справочники").Add("Раскрытие информации");
            dict.Add("Меры по снижению расходов", "measuresreducecosts").AddRequiredPermission("GkhDi.Dict.MeasuresReduceCosts.View");
        }
    }
}
