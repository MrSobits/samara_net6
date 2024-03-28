namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материал кровли"</summary>
    public class MaterialRoofMap : BaseEntityMap<MaterialRoof>
    {
        public MaterialRoofMap() : 
                base("Материал кровли", "GKH_DICT_MATERIAL_ROOF")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
