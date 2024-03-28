namespace Bars.GkhCr.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class WorksCrInspectionLogMap : AuditLogMap<WorksCrInspection>
    {
        public WorksCrInspectionLogMap()
        {
            Name("Обследование объекта");

            Description(x =>
                string.Format("{0}, Программа КР: {1}, Вид работы {2}",
                    x.TypeWork.Return(z => z.ObjectCr).Return(z => z.RealityObject).Return(z => z.Address),
                    x.TypeWork.Return(z => z.ObjectCr).Return(z => z.ProgramCr).Return(z => z.Name),
                    x.TypeWork.Work.Return(z => z.Name)));

            MapProperty(x => x.Description, "Description", "Описание");
            MapProperty(x => x.TypeWork, "Work", "Вид работы", x => x.Return(y => y.Work).Return(z => z.Name));
            MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер");
            MapProperty(x => x.FactDate, "FactDate", "Фактическая дата");
            MapProperty(x => x.InspectionState, "InspectionState", "Факт обследования");
            MapProperty(x => x.Official, "Official", "Ответственный", x => x.Return(z => z.Fio));
            MapProperty(x => x.PlanDate, "PlanDate", "Плановая дата");
            MapProperty(x => x.Reason, "Reason", "Причина");
        }
    }
}
