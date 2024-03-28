namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class ROMCalcTaskMap : BaseEntityMap<ROMCalcTask>
    {
        
        public ROMCalcTaskMap() : 
                base("Расчет категории риска", "GJI_CH_ROM_CALC_TASK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Протокол расчета").Column("FILE").Fetch();
            Property(x => x.KindKND, "Вид КНД").Column("KIND_KND").NotNull();
            Property(x => x.YearEnums, "Год расчета").Column("YEAR").NotNull();
            Property(x => x.CalcDate, "Дата расчета").Column("CALC_DATE").NotNull();
            Property(x => x.CalcState, "Статус расчета").Column("CALC_STATE");
        }
    }
}
