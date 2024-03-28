namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class GASUDataMap : BaseEntityMap<GASUData>
    {
        
        public GASUDataMap() : 
                base("Нарушения в ГИС ЕРП", "GJI_CH_CMEV_GASU_DATA")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GASU, "ЗАПРОС").Column("GASU_ID").NotNull().Fetch();
            Property(x => x.Indexname, "CODE").Column("INDEX_NAME");
            Property(x => x.IndexUid, "INDEX_UID").Column("INDEX_UID");
            Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_ID");
            Property(x => x.Value, "Value").Column("VALUE");
        }
    }
}
