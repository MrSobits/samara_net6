/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол собрания жильцов"
///     /// </summary>
///     public class RealityObjectProtocolMap : BaseGkhEntityMap<RealityObjectProtocol>
///     {
///         public RealityObjectProtocolMap()
///             : base("GKH_OBJ_PROTOCOL_MT")
///         {
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
/// 
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.CouncilResult, "COUNCIL_RESULT").Not.Nullable().CustomType<CouncilResult>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Протокол собрания жильцов"</summary>
    public class RealityObjectProtocolMap : BaseImportableEntityMap<RealityObjectProtocol>
    {
        
        public RealityObjectProtocolMap() : 
                base("Протокол собрания жильцов", "GKH_OBJ_PROTOCOL_MT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.CouncilResult, "CouncilResult").Column("COUNCIL_RESULT").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
