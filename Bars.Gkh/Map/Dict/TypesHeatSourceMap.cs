namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы теплоисточника или теплоносителя"</summary>
    public class TypesHeatSourceMap : BaseEntityMap<TypesHeatSource>
    {
        public TypesHeatSourceMap() : 
                base("Основание признания дома аварийным", "GKH_DICT_TYPES_HEAT_SOURCE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
