namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Тип поквартирной разводки внутридомовой системы отопления"</summary>
    public class TypeInterHouseHeatingSystemMap : BaseEntityMap<TypeInterHouseHeatingSystem>
    {
        public TypeInterHouseHeatingSystemMap() : 
                base("Тип поквартирной разводки внутридомовой системы отопления", "GKH_DICT_TYPE_INTER_HOUSE_HEATING_SYSTEM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
