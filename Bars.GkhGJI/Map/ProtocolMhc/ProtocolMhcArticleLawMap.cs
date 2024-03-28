/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статьи закона постановления прокуратуры"
///     /// </summary>
///     public class ProtocolMhcArticleLawMap : BaseGkhEntityMap<ProtocolMhcArticleLaw>
///     {
///         public ProtocolMhcArticleLawMap()
///             : base("GJI_PROTOCOLMHC_ARTLAW")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ProtocolMhc, "PROTOCOLMHC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статьи закона в протокола МЖК"</summary>
    public class ProtocolMhcArticleLawMap : BaseEntityMap<ProtocolMhcArticleLaw>
    {
        
        public ProtocolMhcArticleLawMap() : 
                base("Статьи закона в протокола МЖК", "GJI_PROTOCOLMHC_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProtocolMhc, "Протокол МЖК").Column("PROTOCOLMHC_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
