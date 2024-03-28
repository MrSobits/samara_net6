/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionLotTypeWorkMap : BaseImportableEntityMap<CompetitionLotTypeWork>
///     {
///         public CompetitionLotTypeWorkMap() : base("CR_COMPETITION_LOT_TW")
///         {
///             References(x => x.Lot, "LOT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.TypeWork, "TYPE_WORK_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhCr.Entities.CompetitionLotTypeWork"</summary>
    public class CompetitionLotTypeWorkMap : BaseImportableEntityMap<CompetitionLotTypeWork>
    {
        
        public CompetitionLotTypeWorkMap() : 
                base("Bars.GkhCr.Entities.CompetitionLotTypeWork", "CR_COMPETITION_LOT_TW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Lot, "Лот конкурса").Column("LOT_ID").NotNull().Fetch();
            Reference(x => x.TypeWork, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
        }
    }
}
