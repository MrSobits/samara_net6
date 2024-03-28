namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ протокола(акта) КР"</summary>
    public class SpecialProtocolCrTypeWorkMap : BaseImportableEntityMap<SpecialProtocolCrTypeWork>
    {
        
        public SpecialProtocolCrTypeWorkMap() : 
                base("Виды работ протокола(акта) КР", "CR_SPECIAL_OBJ_PROTOCOL_TW")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);

            this.Reference(x => x.Protocol, "Протокол КР").Column("PROTOCOL_ID").Fetch();
            this.Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
        }
    }
}
