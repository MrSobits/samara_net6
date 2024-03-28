namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Виды несущей части крыши"</summary>
    public class TypesBearingPartRoofMap : BaseEntityMap<TypesBearingPartRoof>
    {
        public TypesBearingPartRoofMap() : 
                base("Виды несущей части крыши", "GKH_DICT_TYPES_BEARING_PART_ROOF")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
