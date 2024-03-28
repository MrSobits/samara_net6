namespace Bars.Gkh.Gis.Map.Kp50
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Kp50;
    
    /// <summary>
    /// Маппинг для "Bars.Gkh.Gis.Entities.Kp50.GisKp50Base"
    /// </summary>
    public class GisDataBankMap : BaseEntityMap<GisDataBank>
    {
        
        public GisDataBankMap() : 
                base("Bars.Gkh.Gis.Entities.Kp50.GisDataBank", "GIS_DATABANK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Property(x => x.Name, "Name").Column("NAME").Length(200);
            Property(x => x.Key, "Key").Column("KEY").Length(200);
        }
    }
}