using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    public class CostLimitTypeWorkCrMap : BaseEntityMap<CostLimitTypeWorkCr>
    {
        public CostLimitTypeWorkCrMap() : base("Связь Предельная стоимость и Работа", "COST_LIMIT_TYPE_WORK_CR")
        {
        }

        protected override void Map()
        {
            Reference(x => x.CostLimit, "Предельная стоимость").Column("COST_LIMIT_ID").NotNull().Fetch();
            Reference(x => x.TypeWorkCr, "Работа").Column("TYPE_WORK_CR_ID").NotNull().Fetch();
            Property(x => x.Year, "Год").Column("YEAR").NotNull();
            Property(x => x.Cost, "Стоимость").Column("COST").NotNull();
            Property(x => x.Volume, "Объем").Column("VOLUME").NotNull();
            Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").NotNull().Fetch();
        }
    }
}
