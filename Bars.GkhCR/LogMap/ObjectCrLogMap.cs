using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;

    public class ObjectCrLogMap : AuditLogMap<ObjectCr>
    {
        public ObjectCrLogMap()
        {
            Name("Объект КР");

            Description(x => x.RealityObject.Return(y => y.Address));

            MapProperty(x => x.RealityObject, "RealityObject", "Объект недвижимости", x => x.Return(y => y.Address));
			MapProperty(x => x.ProgramCr, "ProgramCr", "Программа", x => x.Return(y => y.Name));
        }
    }
}
