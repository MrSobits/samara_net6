namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материалы фундамента"</summary>
    public class FoundationMaterialsMap : BaseEntityMap<FoundationMaterials>
    {
        public FoundationMaterialsMap():
            base("Материалы фундамента", "GKH_DICT_FOUNDATION_MATERIALS")
        { 
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}