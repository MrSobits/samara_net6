namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материалы отделки фасада"</summary>
    public class FacadeDecorationMaterialsMap : BaseEntityMap<FacadeDecorationMaterials>
    {
        public FacadeDecorationMaterialsMap() : 
                base("Материалы отделки фасада", "GKH_DICT_FACADE_DECORATION_MATERIALS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
