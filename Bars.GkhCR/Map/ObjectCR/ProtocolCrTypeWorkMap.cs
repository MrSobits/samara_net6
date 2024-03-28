/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Gkh.Map;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Виды работ протокола(акта) КР"
///     /// </summary>
///     public class ProtocolCrTypeWorkMap : BaseGkhEntityByCodeMap<ProtocolCrTypeWork>
///     {
/// 		public ProtocolCrTypeWorkMap()
/// 			: base("CR_OBJ_PROTOCOL_TW")
/// 		{
/// 			References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.TypeWork, "TYPE_WORK_ID", ReferenceMapConfig.Fetch);
/// 		}
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ протокола(акта) КР"</summary>
    public class ProtocolCrTypeWorkMap : BaseImportableEntityMap<ProtocolCrTypeWork>
    {
        
        public ProtocolCrTypeWorkMap() : 
                base("Виды работ протокола(акта) КР", "CR_OBJ_PROTOCOL_TW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.Protocol, "Протокол КР").Column("PROTOCOL_ID").Fetch();
            Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
        }
    }
}
