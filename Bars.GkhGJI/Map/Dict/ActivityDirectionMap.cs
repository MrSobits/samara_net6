namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.ActivityDirection"</summary>
    public class ActivityDirectionMap : BaseEntityMap<ActivityDirection>
    {
        
        public ActivityDirectionMap() : 
                base("Bars.GkhGji.Entities.Dict.ActivityDirection", "GJI_ACTIVITY_DIRECTION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Code, "Code").Column("CODE").Length(100).NotNull();
        }
    }
}
