namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Основание признания дома аварийным"</summary>
    public class BaseHouseEmergencyMap : BaseEntityMap<BaseHouseEmergency>
    {
        public BaseHouseEmergencyMap() : 
                base("Основание признания дома аварийным", "GKH_DICT_BASE_HOUSE_EMERGENCY")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
