/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.ImportIncrementalData
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.ImportIncrementalData;
/// 
///     /// <summary>
///     /// Маппинг для сущности LoadedFileRegister
///     /// </summary>
///     public class OpenTatarstanDataMap : BaseEntityMap<OpenTatarstanData>
///     {
///         /// <summary>
///         /// Конструктор
///         /// </summary>
///         public OpenTatarstanDataMap()
///             : base("GIS_OPEN_TATARSTAN")
///         {
///             Map(x => x.ImportResult, "IMPORT_RESULT");
///             Map(x => x.ImportName, "IMPORT_NAME");
///             Map(x => x.ResponseInfo, "RESPONSE_INFO");
///             Map(x => x.ResponseCode, "RESPONSE_CODE");
/// 
///             References(x => x.File, "B4_FILE_INFO_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Log, "B4_LOG_INFO_ID", ReferenceMapConfig.Fetch);
///             References(x => x.B4User, "B4_USER_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.ImportIncrementalData
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.ImportIncrementalData;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.ImportIncrementalData.OpenTatarstanData"</summary>
    public class OpenTatarstanDataMap : BaseEntityMap<OpenTatarstanData>
    {
        
        public OpenTatarstanDataMap() : 
                base("Bars.Gkh.Gis.Entities.ImportIncrementalData.OpenTatarstanData", "GIS_OPEN_TATARSTAN")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.B4User, "B4User").Column("B4_USER_ID").Fetch();
            Reference(x => x.File, "File").Column("B4_FILE_INFO_ID").Fetch();
            Reference(x => x.Log, "Log").Column("B4_LOG_INFO_ID").Fetch();
            Property(x => x.ImportResult, "ImportResult").Column("IMPORT_RESULT");
            Property(x => x.ImportName, "ImportName").Column("IMPORT_NAME").Length(250);
            Property(x => x.ResponseInfo, "ResponseInfo").Column("RESPONSE_INFO").Length(250);
            Property(x => x.ResponseCode, "ResponseCode").Column("RESPONSE_CODE").Length(250);
        }
    }
}
