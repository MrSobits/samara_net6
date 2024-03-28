namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class GISERPResultViolationsMap : BaseEntityMap<GISERPResultViolations>
    {
        
        public GISERPResultViolationsMap() : 
                base("Нарушения в ГИС ЕРП", "GJI_CH_GIS_ERP_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GISERP, "ЗАПРОС").Column("GIS_ERP_ID").NotNull().Fetch();
            Property(x => x.CODE, "CODE").Column("CODE");
            Property(x => x.DATE_APPOINTMENT, "DATE_APPOINTMENT").Column("DATE_APPOINTMENT");
            Property(x => x.EXECUTION_DEADLINE, "EXECUTION_DEADLINE").Column("EXECUTION_DEADLINE");
            Property(x => x.EXECUTION_NOTE, "EXECUTION_NOTE").Column("EXECUTION_NOTE");
            Property(x => x.NUM_GUID, "NUM_GUID").Column("NUM_GUID");
            Property(x => x.TEXT, "TEXT").Column("TEXT");
            Property(x => x.VIOLATION_ACT, "VIOLATION_ACT").Column("VIOLATION_ACT");
            Property(x => x.VIOLATION_NOTE, "VIOLATION_NOTE").Column("VIOLATION_NOTE");
            Property(x => x.VLAWSUIT_TYPE_ID, "VLAWSUIT_TYPE_ID").Column("VLAWSUIT_TYPE_ID");
        }
    }
}
