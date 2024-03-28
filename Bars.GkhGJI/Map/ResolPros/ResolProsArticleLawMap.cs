/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статьи закона постановления прокуратуры"
///     /// </summary>
///     public class ResolProsArticleLawMap : BaseGkhEntityMap<ResolProsArticleLaw>
///     {
///         public ResolProsArticleLawMap()
///             : base("GJI_RESOLPROS_ARTLAW")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ResolPros, "RESOLPROS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статьи закона в постановлении прокуратуры ГЖИ"</summary>
    public class ResolProsArticleLawMap : BaseEntityMap<ResolProsArticleLaw>
    {
        
        public ResolProsArticleLawMap() : 
                base("Статьи закона в постановлении прокуратуры ГЖИ", "GJI_RESOLPROS_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ResolPros, "постановление прокуратуры").Column("RESOLPROS_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
