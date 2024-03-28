/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "статьи закона протокола"
///     /// </summary>
///     public class ProtocolArticleLawMap : BaseEntityMap<ProtocolArticleLaw>
///     {
///         public ProtocolArticleLawMap()
///             : base("GJI_PROTOCOL_ARTLAW")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.Protocol, "PROTOCOL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для "Статьи закона в протоколе ГЖИ"</summary>
    public class ProtocolArticleLawMap : BaseEntityMap<ProtocolArticleLaw>
    {
        
        public ProtocolArticleLawMap() : 
                base("Статьи закона в протоколе ГЖИ", "GJI_PROTOCOL_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            this.Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
            this.Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);
        }
    }
}
