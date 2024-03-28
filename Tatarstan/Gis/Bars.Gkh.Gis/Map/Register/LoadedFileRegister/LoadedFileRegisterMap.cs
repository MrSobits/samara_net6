/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.LoadedFileRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.LoadedFileRegister;
/// 
///     /// <summary>
///     /// Маппинг для сущности LoadedFileRegister
///     /// </summary>
///     public class LoadedFileRegisterMap : BaseEntityMap<LoadedFileRegister>
///     {
///         /// <summary>
///         /// Конструктор
///         /// </summary>
///         public LoadedFileRegisterMap()
///             : base("GIS_LOADED_FILE_REGISTER")
///         {
///             Map(x => x.FileName, "FILENAME", true);
///             Map(x => x.Size, "SIZE", true);
///             Map(x => x.TypeStatus, "TYPESTATUS", true);
///             Map(x => x.Format, "FORMAT");
///             Map(x => x.ImportResult, "IMPORT_RESULT");
///             Map(x => x.ImportName, "IMPORT_NAME");
/// 
///             References(x => x.File, "B4_FILE_INFO_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Log, "B4_LOG_INFO_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Supplier, "SUPPLIER");
///             References(x => x.B4User, "B4_USER_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.LoadedFileRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.LoadedFileRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.LoadedFileRegister.LoadedFileRegister"</summary>
    public class LoadedFileRegisterMap : BaseEntityMap<LoadedFileRegister>
    {
        
        public LoadedFileRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.LoadedFileRegister.LoadedFileRegister", "GIS_LOADED_FILE_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.FileName, "FileName").Column("FILENAME").Length(250).NotNull();
            Reference(x => x.B4User, "B4User").Column("B4_USER_ID").Fetch();
            Property(x => x.Size, "Size").Column("SIZE").NotNull();
            Property(x => x.TypeStatus, "TypeStatus").Column("TYPESTATUS").NotNull();
            Reference(x => x.File, "File").Column("B4_FILE_INFO_ID").Fetch();
            Reference(x => x.Log, "Log").Column("B4_LOG_INFO_ID").Fetch();
            Property(x => x.SupplierName, "SupplierName").Column("SUPPLIER_NAME");
            Property(x => x.Format, "Format").Column("FORMAT");
            Property(x => x.ImportResult, "ImportResult").Column("IMPORT_RESULT");
            Property(x => x.ImportName, "ImportName").Column("IMPORT_NAME").Length(250);
            Property(x => x.CalculationDate, "CalculationDate").Column("CALCULATION_DATE");
        }
    }
}
