namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "статей закона в справочнике видов КНД"</summary>
    public class KindKNDDictArtLawMap : BaseEntityMap<KindKNDDictArtLaw>
    {
        
        public KindKNDDictArtLawMap() : 
                base("Статьи закона для типа КНД", "GJI_CH_TYPE_KND_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.KindKNDDict, "Тип КНД").Column("TYPE_KND_ID").NotNull().Fetch();
            Reference(x => x.ArticleLawGji, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
            Property(x => x.Koefficients, "Коэффициент").Column("KOEFFICIENT");
        }
    }
}
