namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;


    /// <summary>Маппинг для "Bars.GkhCr.Entities.CompetitionLotSpecialTypeWork"</summary>
    public class CompetitionLotSpecialTypeWorkMap : BaseImportableEntityMap<CompetitionLotSpecialTypeWork>
    {
        
        public CompetitionLotSpecialTypeWorkMap() : 
                base("Bars.GkhCr.Entities.CompetitionLotTypeWork", "CR_COMPETITION_LOT_SPECIAL_TW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Lot, "Лот конкурса").Column("LOT_ID").NotNull().Fetch();
            Reference(x => x.TypeWork, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
        }
    }
}
