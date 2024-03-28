namespace Bars.Gkh.Repair
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
            root.Add("Капитальный ремонт").Add("Текущий ремонт").Add("Программы текущего ремонта", "repairprogram").AddRequiredPermission("GkhRepair.RepairProgram.View");
            root.Add("Капитальный ремонт").Add("Текущий ремонт").Add("Контрольные сроки работ по текущему ремонту", "repaircontroldate").AddRequiredPermission("GkhRepair.RepairControlDate.View");
            root.Add("Капитальный ремонт").Add("Текущий ремонт").Add("Объекты текущего ремонта", "repairobject").AddRequiredPermission("GkhRepair.RepairObjectViewCreate.View");
            root.Add("Капитальный ремонт").Add("Текущий ремонт").Add("Массовая смена статусов объектов ТР", "repairobjectmassstatechange").AddRequiredPermission("GkhRepair.RepairObjectMassStateChange.View").WithIcon("objCrMassChangeStatus");
            
        }
    }
}