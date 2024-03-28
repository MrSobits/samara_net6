namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// Маппинг для сущности "статьи закона протокола 19.7"
    /// </summary>
    public class Protocol197ArticleLawMap : BaseEntityMap<Protocol197ArticleLaw>
    {
		public Protocol197ArticleLawMap() : 
                base("Статьи закона в протоколе ГЖИ", "GJI_PROTOCOL197_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            this.Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}