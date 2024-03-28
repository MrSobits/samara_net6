namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Связи Расчета категории риска с коэффициентом Vпр"</summary>
    public class VprResolutionMap : BaseEntityMap<VprResolution>
    {
        
        public VprResolutionMap() : 
                base("Коэффициент Vпр", "GJI_CH_ROM_VPR_RESOLUTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ArtLaws, "Статьи закона").Column("ART_LAW_NAMES");
            Reference(x => x.Resolution, "Постановление").Column("RESOLUTION_ID").NotNull().Fetch();
            Reference(x => x.ROMCategory, "Расчет").Column("ROM_CATEGORY_ID").NotNull().Fetch();

        }
    }
}
