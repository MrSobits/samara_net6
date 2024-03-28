// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhEdoInteg
{
    using B4;

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
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Отправка данных в ЕМСЭД", "sendemsed").AddRequiredPermission("GkhEdoInteg.SendEdo.View");
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Данные по интеграции с ЭДО", "edolog").AddRequiredPermission("GkhEdoInteg.LogEdo.View");
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Лог интеграции с ЭДО", "edologrequests").AddRequiredPermission("GkhEdoInteg.LogEdoRequests.View");

            root.Add("Справочники").Add("Обращения").Add("Интеграция c ЭДО: Источник поступления", "revenuesourceedointeg").AddRequiredPermission("GkhEdoInteg.Compare.RevenueSource.View");
            root.Add("Справочники").Add("Обращения").Add("Интеграция c ЭДО: Форма поступления", "revenueformedointeg").AddRequiredPermission("GkhEdoInteg.Compare.RevenueForm.View");
            root.Add("Справочники").Add("Обращения").Add("Интеграция c ЭДО: Вид обращения", "kindstatementedointeg").AddRequiredPermission("GkhEdoInteg.Compare.KindStatement.View");
            root.Add("Справочники").Add("Обращения").Add("Интеграция c ЭДО: Инспекторы", "inspectoredointeg").AddRequiredPermission("GkhEdoInteg.Compare.Inspector.View");
        }
    }
}