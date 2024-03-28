namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы наружного утепления фасада"</summary>
    public class TypesExternalFacadeInsulationMap : BaseEntityMap<TypesExternalFacadeInsulation>
    {
        public TypesExternalFacadeInsulationMap() : 
                base("Типы наружного утепления фасада", "GKH_DICT_TYPES_EXTERNAL_FACADE_INSULATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
