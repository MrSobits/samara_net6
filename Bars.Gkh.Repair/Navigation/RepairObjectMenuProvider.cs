namespace Bars.Gkh.Repair.Navigation
{
    using Bars.B4;

    public class RepairObjectMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "RepairObject";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки объекта текущего ремонта";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "B4.controller.repairobject.Edit").AddRequiredPermission("GkhRepair.RepairObject.RepairObjectViewEdit.View").WithIcon("icon-book-addresses");
            root.Add("Виды работ", "B4.controller.repairobject.RepairWork").WithIcon("icon-wrench-orange");
            root.Add("График выполнения работ", "B4.controller.repairobject.ScheduleExecutionWork").WithIcon("icon-text-list-numbers");
            root.Add("Ход выполнения работ", "B4.controller.repairobject.ProgressExecutionWork").WithIcon("icon-chart-curve");
            root.Add("Акты выполненных работ", "B4.controller.repairobject.PerformedRepairWorkAct").WithIcon("icon-script-edit");
        }
    }
}