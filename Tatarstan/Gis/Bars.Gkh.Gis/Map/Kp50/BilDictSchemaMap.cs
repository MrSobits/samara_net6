/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Kp50
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Kp50;
/// 
///     public class BilDictSchemaMap : PersistentObjectMap<BilDictSchema>
///     {
///         public BilDictSchemaMap()
///             : base("BIL_DICT_SCHEMA")
///         {
///             Map(x => x.CentralSchemaPrefix, "CENTRAL_SCHEMA_PREFIX");
///             Map(x => x.LocalSchemaPrefix, "LOCAL_SCHEMA_PREFIX");
///             Map(x => x.ConnectionString, "CONNECTION_STRING");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.IsActive, "IS_ACTIVE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Kp50
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Kp50;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilDictSchema"</summary>
    public class BilDictSchemaMap : PersistentObjectMap<BilDictSchema>
    {
        
        public BilDictSchemaMap() : 
                base("Bars.Gkh.Gis.Entities.Kp50.BilDictSchema", "BIL_DICT_SCHEMA")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.LocalSchemaPrefix, "LocalSchemaPrefix").Column("LOCAL_SCHEMA_PREFIX").Length(250);
            Property(x => x.CentralSchemaPrefix, "CentralSchemaPrefix").Column("CENTRAL_SCHEMA_PREFIX").Length(250);
            Property(x => x.ConnectionString, "ConnectionString").Column("CONNECTION_STRING").Length(250);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(250);
            Property(x => x.IsActive, "IsActive").Column("IS_ACTIVE");
            Property(x => x.ErcCode, "ErcCode").Column("ERC_CODE");
            Property(x => x.SenderLocalSchemaPrefix, "SenderLocalSchemaPrefix").Column("SENDER_LOCAL_SCHEMA_PREFIX").Length(250);
            Property(x => x.SenderCentralSchemaPrefix, "SenderCentralSchemaPrefix").Column("SENDER_CENTRAL_SCHEMA_PREFIX").Length(250);
        }
    }
}
