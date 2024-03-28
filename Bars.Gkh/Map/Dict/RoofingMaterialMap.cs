namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Материал кровли"</summary>
    public class RoofingMaterialMap : BaseImportableEntityMap<RoofingMaterial>
    {
        
        public RoofingMaterialMap() : 
                base("Материал кровли", "GKH_DICT_ROOFING_MATERIAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
