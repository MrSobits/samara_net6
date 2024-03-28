namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Водоразборные устройства"</summary>
    public class WaterDispensersMap : BaseEntityMap<WaterDispensers>
    {
        public WaterDispensersMap() : 
                base("Водоразборные устройства", "GKH_DICT_WATER_DISPENSERS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
