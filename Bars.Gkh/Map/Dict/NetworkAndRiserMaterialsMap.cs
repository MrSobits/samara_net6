namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материалы сети и стояков"</summary>
    public class NetworkAndRiserMaterialsMap : BaseEntityMap<NetworkAndRiserMaterials>
    {
        public NetworkAndRiserMaterialsMap() : 
                base("Материалы сети и стояков", "GKH_DICT_NETWORK_AND_RISER_MATERIALS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
