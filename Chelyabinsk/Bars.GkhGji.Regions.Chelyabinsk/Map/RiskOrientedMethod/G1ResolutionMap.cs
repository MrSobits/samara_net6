namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Связи Расчета категории риска с коэффициентом Vпр"</summary>
    public class G1ResolutionMap : BaseEntityMap<G1Resolution>
    {
        
        public G1ResolutionMap() : 
                base("Коэффициент Vпр", "GJI_CH_ROM_G1_RESOLUTION")
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
