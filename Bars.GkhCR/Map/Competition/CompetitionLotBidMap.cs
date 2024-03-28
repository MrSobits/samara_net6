/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionLotBidMap : BaseImportableEntityMap<CompetitionLotBid>
///     {
///         public CompetitionLotBidMap() : base("CR_COMPETITION_LOT_BID")
///         {
///             References(x => x.Lot, "LOT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Builder, "BUILDER_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.IncomeDate, "BDATE", true);
///             Map(x => x.Points, "POINTS");
///             Map(x => x.Price, "PRICE");
///             Map(x => x.PriceNds, "PRICE_NDS");
///             Map(x => x.IsWinner, "IS_WINNER", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Заявка лота"</summary>
    public class CompetitionLotBidMap : BaseImportableEntityMap<CompetitionLotBid>
    {
        
        public CompetitionLotBidMap() : 
                base("Заявка лота", "CR_COMPETITION_LOT_BID")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Lot, "Лот").Column("LOT_ID").NotNull().Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
            Property(x => x.IncomeDate, "Дата поступления").Column("BDATE").NotNull();
            Property(x => x.Points, "Количество баллов").Column("POINTS");
            Property(x => x.Price, "Цена заявки (без НДС)").Column("PRICE");
            Property(x => x.PriceNds, "Цена заявки (с НДС)").Column("PRICE_NDS");
            Property(x => x.IsWinner, "Является победителем").Column("IS_WINNER").NotNull();
        }
    }
}
