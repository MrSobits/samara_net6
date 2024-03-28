namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "справочника видов КНД"</summary>
    public class KindKNDDictMap : BaseEntityMap<KindKNDDict>
    {
        
        public KindKNDDictMap() : 
                base("Тип КНД", "GJI_CH_TYPE_KND")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.KindKND, "Тип КНД").Column("TYPE_KND");
            Property(x => x.DateFrom, "Дата с").Column("DATE_FROM");
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
         
        }
    }
}
