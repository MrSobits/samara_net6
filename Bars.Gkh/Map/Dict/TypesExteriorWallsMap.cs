namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы наружных стен"</summary>
    public class TypesExteriorWallsMap : BaseEntityMap<TypesExteriorWalls>
    {
        public TypesExteriorWallsMap() : 
                base("Типы наружных стен", "GKH_DICT_TYPES_EXTERIOR_WALLS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
