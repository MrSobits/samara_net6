using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using Bars.B4.Utils;

    public class EmergencyObjectLogMap : AuditLogMap<EmergencyObject>
    {
        public EmergencyObjectLogMap()
        {
            Name("Реестр аварийных домов");

            Description(x => x.RealityObject.Return(y => y.Address));

            MapProperty(x => x.RealityObject, "RealityObject", "Адрес", x => x.Return(y => y.Address));
        }
    }
}
