namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// Маппинг для сущности "Нарушение в Протоколе 19.7"
    /// </summary>
	public class Protocol197ViolationMap : JoinedSubClassMap<Protocol197Violation>
    {
		public Protocol197ViolationMap() :
			base("Этап наказания за нарушение в протоколе 19.7", "GJI_PROTOCOL197_VIOLAT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(1000);
        }
    }
}