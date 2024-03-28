namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для списка контрагентов для расчета категории риска</summary>
    public class ROMCalcTaskManOrgMap : BaseEntityMap<ROMCalcTaskManOrg>
    {

        public ROMCalcTaskManOrgMap() :
                base("Контрегент для расчета категории риска", "GJI_CH_ROM_CALC_TASK_CONTRAGENT")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.ROMCalcTask, "Задача").Column("ROM_CALC_TASK_ID").NotNull().Fetch();
        }
    }
}
