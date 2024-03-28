/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.ImportedAddressMatching
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Gis.Entities.ImportAddressMatching;
/// 
///     /// <summary>
///     /// Маппинг для сущности Сопоставление импортированного адреса и адреса из жилфонда
///     /// </summary>
///     public class ImportedAddressMatchMap : BaseEntityMap<ImportedAddressMatch>
///     {
///         /// <summary>
///         /// .ctor
///         /// </summary>
///         public ImportedAddressMatchMap()
///             : base("BIL_IMPORT_ADDRESS")
///         {
///             Map(x => x.ImportType, "IMPORT_TYPE", false, 256);
///             Map(x => x.ImportFilename, "IMPORT_FILENAME", false, 256);
///             Map(x => x.AddressCode, "ADDRESS_CODE", false, 20);
///             Map(x => x.City, "CITY", false, 200);
///             Map(x => x.Street, "STREET", false, 200);
///             Map(x => x.House, "HOUSE", false, 200);
///             Map(x => x.ImportDate, "IMPORT_DATE", false);
///             References(x => x.FiasAddress, "FIAS_ADDRESS_UID_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.ImportAddressMatching
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.ImportAddressMatching;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.ImportAddressMatching.ImportedAddressMatch"</summary>
    public class ImportedAddressMatchMap : BaseEntityMap<ImportedAddressMatch>
    {
        
        public ImportedAddressMatchMap() : 
                base("Bars.Gkh.Gis.Entities.ImportAddressMatching.ImportedAddressMatch", "BIL_IMPORT_ADDRESS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.FiasAddress, "FiasAddress").Column("FIAS_ADDRESS_UID_ID").Fetch();
            Property(x => x.ImportType, "ImportType").Column("IMPORT_TYPE").Length(256);
            Property(x => x.ImportFilename, "ImportFilename").Column("IMPORT_FILENAME").Length(256);
            Property(x => x.AddressCode, "AddressCode").Column("ADDRESS_CODE").Length(20);
            Property(x => x.City, "City").Column("CITY").Length(200);
            Property(x => x.Street, "Street").Column("STREET").Length(200);
            Property(x => x.House, "House").Column("HOUSE").Length(200);
            Property(x => x.ImportDate, "ImportDate").Column("IMPORT_DATE");
            Property(x => x.HouseCode, "HouseCode").Column("NZP_DOM").Length(200);
            Property(x => x.DataBankId, "DataBankId").Column("DATA_BANK_ID").Length(200);
        }
    }
}
