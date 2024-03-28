using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;

    public class EstimateLogMap : AuditLogMap<Estimate>
    {
        public EstimateLogMap()
        {
            Name("Реестр смет");

            Description(x => x.ReturnSafe(y => y.EstimateCalculation.ObjectCr.RealityObject.Address));

            MapProperty(x => x.EstimateCalculation.ObjectCr.RealityObject, "RealityObject", "Объект КР", x => x.Return(y => y.Address));
            MapProperty(x => x.EstimateCalculation.ObjectCr.ProgramCr, "ProgramCr", "Программа КР", x => x.Return(y => y.Name));
            MapProperty(x => x.EstimateCalculation.TypeWorkCr.Work, "Work", "Вид работ", x => x.Return(y => y.Name));
        }
    }
}
