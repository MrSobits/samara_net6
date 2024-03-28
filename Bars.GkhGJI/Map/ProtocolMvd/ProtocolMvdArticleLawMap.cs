/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статьи закона протокола МВД"
///     /// </summary>
///     public class ProtocolMvdArticleLawMap : BaseGkhEntityMap<ProtocolMvdArticleLaw>
///     {
///         public ProtocolMvdArticleLawMap()
///             : base("GJI_PROT_MVD_ARTLAW")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ProtocolMvd, "PROT_MVD_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статьи закона в протоколе МВД"</summary>
    public class ProtocolMvdArticleLawMap : BaseEntityMap<ProtocolMvdArticleLaw>
    {
        
        public ProtocolMvdArticleLawMap() : 
                base("Статьи закона в протоколе МВД", "GJI_PROT_MVD_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProtocolMvd, "Протокол МВД").Column("PROT_MVD_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
