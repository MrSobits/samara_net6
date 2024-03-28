namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Утепляющие слои чердачных перекрытий"</summary>
    public class WarmingLayersAtticsMap : BaseEntityMap<WarmingLayersAttics>
    {
        public WarmingLayersAtticsMap() : 
                base("Утепляющие слои чердачных перекрытий", "GKH_DICT_WARMING_LAYERS_ATTICS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
