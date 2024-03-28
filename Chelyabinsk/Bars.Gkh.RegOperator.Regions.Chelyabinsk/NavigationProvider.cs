namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
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
            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Долевые ПИР", "partialclaimwork").AddRequiredPermission("Clw.FlattenedClaimWork.View");
            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Агенты ПИР", "agentpir").AddRequiredPermission("Clw.ClaimWork.AgentPIR.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт документов агента ПИР", "agentpirdocument");
            root.Add("Администрирование").Add("Импорты").Add("Импорт файлов документов ПИР", "agentpirfileimport");
            root.Add("Администрирование").Add("Импорты").Add("Импорт зачислений агента ПИР", "agentpirdebtorcreditimport").AddRequiredPermission("Import.AgentPIRDebtorCreditImport.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт погашений пошлины должника ПИР", "agentpirdutyimport");
            root.Add("Администрирование").Add("Импорты").Add("Импорт выписок должников агента ПИР", "agentpirdebtororderingimport").AddRequiredPermission("Import.AgentPIRDebtorOrderingImport.View");
        }
    }
}