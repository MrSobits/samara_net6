namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;

    /// <summary>Маппинг для "Спровочник настроек расчета пени с отсрочкой"</summary>
    public class PenaltiesWithDeferredMap : BaseImportableEntityMap<PenaltiesWithDeferred>
    {

        public PenaltiesWithDeferredMap() :
                base("Спровочник настроек расчета пени с отсрочкой", "REGOP_PENALTIES_DEFERRED")
        {
        }

        protected override void Map()
        {
            Property(x => x.DateStartCalc, "Дата начала").Column("DATE_START");
            Property(x => x.DateEndCalc, "Дата окончания").Column("DATE_END");
        }
    }
}