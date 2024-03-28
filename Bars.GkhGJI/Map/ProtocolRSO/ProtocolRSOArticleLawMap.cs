namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статьи закона в протоколе прокуратуры"</summary>
    public class ProtocolRSOArticleLawMap : BaseEntityMap<ProtocolRSOArticleLaw>
    {
        
        public ProtocolRSOArticleLawMap() : 
                base("Статьи закона в протоколе РСО", "GJI_PROTOCOLRSO_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProtocolRSO, "Протокол РСО").Column("PROTOCOLRSO_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
