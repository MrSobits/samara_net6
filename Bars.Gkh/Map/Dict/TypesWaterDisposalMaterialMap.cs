namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы материала водотведения"</summary>
    public class TypesWaterDisposalMaterialMap : BaseEntityMap<TypesWaterDisposalMaterial>
    {
        public TypesWaterDisposalMaterialMap() : 
                base("Типы материала водотведения", "GKH_DICT_TYPES_WATER_DISPOSAL_MATERIAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
