using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Utils;
using Bars.GkhRf.Entities;

namespace Bars.GkhRF.LogMap
{
    public class LimitCheckLogMap : AuditLogMap<LimitCheck>
    {
        public LimitCheckLogMap()
        {
            Name("Настройки проверки на наличие лимитов");

            Description(x => "Настройки проверки на наличие лимитов");

            MapProperty(x => x.TypeProgram, "Тип программы", "Тип программы", x => x.Return(y => y.GetEnumMeta().Display));
        }
    }
}
