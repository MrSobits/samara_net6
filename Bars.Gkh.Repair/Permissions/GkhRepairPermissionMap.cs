namespace Bars.Gkh.Repair.Permissions
{
    using B4;
    using Entities;

    public class GkhRepairPermissionMap : PermissionMap
    {
        public GkhRepairPermissionMap()
        {
            this.Namespace("GkhRepair", "Модуль текущий ремонт");

            this.Namespace("GkhRepair.RepairProgram", "Реестр программ текущего ремонта");
            this.CRUDandViewPermissions("GkhRepair.RepairProgram");

            #region Объекты текущего ремонта
            this.Namespace("GkhRepair.RepairObjectViewCreate", "Объекты текущего ремонта: Просмотр, создание");
            this.Permission("GkhRepair.RepairObjectViewCreate.View", "Просмотр");
            this.Permission("GkhRepair.RepairObjectViewCreate.Create", "Создание записей");

            this.Namespace<RepairObject>("GkhRepair.RepairObject", "Объекты текущего ремонта: Изменение, удаление");
            this.Permission("GkhRepair.RepairObject.Edit", "Изменение записей");
            this.Permission("GkhRepair.RepairObject.Delete", "Удаление записей");

            #region Паспорт объекта
            this.Namespace("GkhRepair.RepairObject.RepairObjectViewEdit", "Паспорт объекта: Просмотр, изменение");
            this.Permission("GkhRepair.RepairObject.RepairObjectViewEdit.View", "Просмотр");
            this.Permission("GkhRepair.RepairObject.RepairObjectViewEdit.Edit", "Изменение записи");
            #region  Поля
            this.Namespace("GkhRepair.RepairObject.RepairObjectViewEdit.Field", "Поля");
            this.Permission("GkhRepair.RepairObject.RepairObjectViewEdit.Field.Document", "Документ-основание для проведения работ");
            this.Permission("GkhRepair.RepairObject.RepairObjectViewEdit.Field.Comment", "Примечание");
            #endregion
            #endregion

            #region Виды работ
            this.Namespace("GkhRepair.RepairObject.RepairWork", "Виды работ");
            this.CRUDandViewPermissions("GkhRepair.RepairObject.RepairWork");
            #region  Поля
            this.Namespace("GkhRepair.RepairObject.RepairWork.Field", "Поля");
            this.Permission("GkhRepair.RepairObject.RepairWork.Field.Work_Edit", "Вид работы");
            this.Permission("GkhRepair.RepairObject.RepairWork.Field.Volume_Edit", "Объем");
            this.Permission("GkhRepair.RepairObject.RepairWork.Field.Sum_Edit", "Сумма");
            this.Permission("GkhRepair.RepairObject.RepairWork.Field.Builder_Edit", "Подрядчик");
            #endregion
            #endregion

            #region Ход выполнения работ
            this.Namespace("GkhRepair.RepairObject.ProgressExecutionWork", "Акт выполнения работ");
            this.Permission("GkhRepair.RepairObject.ProgressExecutionWork.View", "Просмотр");
            this.Permission("GkhRepair.RepairObject.ProgressExecutionWork.Edit", "Изменение");
            #region  Поля
            this.Namespace("GkhRepair.RepairObject.ProgressExecutionWork.Field", "Поля");
            this.Permission("GkhRepair.RepairObject.ProgressExecutionWork.Field.VolumeOfCompletion_Edit", "Объем выполнения");
            this.Permission("GkhRepair.RepairObject.ProgressExecutionWork.Field.CostSum_Edit", "Сумма расходов");
            this.Permission("GkhRepair.RepairObject.ProgressExecutionWork.Field.PercentOfCompletion_Edit", "Процент выполнения");
            #endregion
            #endregion Ход выполнения работ

            this.Namespace("GkhRepair.RepairObject.PerformedRepairWorkAct", "Ход выполнения работ");
            this.Permission("GkhRepair.RepairObject.PerformedRepairWorkAct.View", "Просмотр");
            this.Permission("GkhRepair.RepairObject.PerformedRepairWorkAct.Edit", "Изменение");

            #region График выполнения работ
            this.Namespace("GkhRepair.RepairObject.ScheduleExecutionWork", "График выполнения работ");
            this.Permission("GkhRepair.RepairObject.ScheduleExecutionWork.Edit", "Изменение");
            #endregion График выполнения работ

            #endregion Объекты текущего ремонта

            this.Namespace("GkhRepair.RepairControlDate", "Контрольные сроки работ по текущему ремонту");
            this.CRUDandViewPermissions("GkhRepair.RepairControlDate");

            this.Namespace("GkhRepair.RepairControlDate.Field", "Поля");
            this.Permission("GkhRepair.RepairControlDate.Field.Date", "Контрольный срок");

            this.Namespace("Reports.GkhRepair", "Модуль текущий ремонт");
            this.Permission("Reports.GkhRepair.MkdRepairInfoReport", "Информация по текущему ремонту МКД");

            this.Namespace("GkhRepair.RepairObjectMassStateChange", "Массовая смена статусов объектов ТР");
            this.Permission("GkhRepair.RepairObjectMassStateChange.View", "Просмотр");
        }
    }
}