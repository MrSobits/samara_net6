namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Entities;

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
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}