using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;

    public class PerformedWorkActLogMap : AuditLogMap<PerformedWorkAct>
    {
        public PerformedWorkActLogMap()
        {
            Name("Реестр актов выполненных работ");

            Description(x => x.DocumentNum);

            MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа");
            MapProperty(x => x.ObjectCr.ProgramCr, "ProgramCr", "Программа", x => x.Return(y => y.Name));
            MapProperty(
                x => x.TypeWorkCr,
                "TypeWorkCr",
                "Работа",
                obj =>
                string.Format(
                    "{0} ({1})",
                    obj.Return(x => x.Work).Return(x => x.Name),
                    obj.Return(x => x.FinanceSource).Return(x => x.Name)));

            MapProperty(x => x.DateFrom, "DateFrom", "От");
            MapProperty(x => x.Volume, "Volume", "Объем");
            MapProperty(x => x.Sum, "Sum", "Сумма");
        }
    }
}
