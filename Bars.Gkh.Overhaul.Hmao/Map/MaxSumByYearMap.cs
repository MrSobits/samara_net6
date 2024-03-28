using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    public class MaxSumByYearMap : BaseEntityMap<MaxSumByYear>
    {
        public MaxSumByYearMap() : base("Предельные стоимости в разрезе МО", "OVRHL_MAX_SUM_BY_YEAR")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").Fetch();
            Reference(x => x.Program, "Программа").Column("PROGRAM_ID").Fetch();
            Property(x => x.Year, "Год").Column("YEAR").NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
        }
    }
}
