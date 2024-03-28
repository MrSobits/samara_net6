using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.LogMap
{
    public class RealityObjectStructuralElementLogMap : AuditLogMap<RealityObjectStructuralElement>
    {
        public RealityObjectStructuralElementLogMap()
        {
            Name("Конструктивный элемент дома");

            Description(x => x.ReturnSafe(y => string.Format("{0}-{1}", y.RealityObject.Address, y.StructuralElement.Name)));

            MapProperty(x => x.RealityObject, "Address", "Объект", x => x.Return(y => y.Address));
            MapProperty(x => x.LastOverhaulYear, "LastOverhaulYear", "Год установки последнего кап. ремонта");
            MapProperty(x => x.Volume, "Volume", "Объем");
            MapProperty(x => x.Wearout, "Wearout", "Износ");
            MapProperty(x => x.StructuralElement, "UnitMeasure", "Единица измерения", x => x.Return(y => y.UnitMeasure).Return(y => y.Name));
            MapProperty(x => x.State, "State", "Статус", x => x.Return(y => y.Name));
        }
    }
}
