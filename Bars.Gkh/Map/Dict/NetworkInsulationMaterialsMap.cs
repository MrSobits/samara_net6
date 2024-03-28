namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материалы теплоизоляции сети"</summary>
    public class NetworkInsulationMaterialsMap : BaseEntityMap<NetworkInsulationMaterials>
    {
        public NetworkInsulationMaterialsMap() : 
                base("Материалы теплоизоляции сети", "GKH_DICT_NETWORK_INSULATION_MATERIALS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
