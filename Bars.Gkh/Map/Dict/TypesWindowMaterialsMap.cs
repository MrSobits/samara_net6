namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы материалов окон"</summary>
    public class TypesWindowMaterialsMap : BaseEntityMap<TypesWindowMaterials>
    {
        public TypesWindowMaterialsMap() : 
                base("Типы материалов окон", "GKH_DICT_TYPES_WINDOW_MATERIALS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
