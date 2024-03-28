/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.LogImportExport
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.ImportExport;
/// 
///     public class ImportExportLogMap : BaseEntityMap<ImportExport>
///     {
///         public ImportExportLogMap()
///             : base("GKH_IMPORT_EXPORT")
///         {
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.HasErrors, "HAS_ERRORS");
///             Map(x => x.HasMessages, "HAS_MESSAGES");
///             Map(x => x.Type, "IE_TYPE");
/// 
///             References(x => x.FileInfo, "FILE_ID", ReferenceMapConfig.CascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.ImportExport
{
    using Bars.B4.Modules.Mapping.Mappers;

    /// <summary>Маппинг для "Bars.Gkh.ImportExport.ImportExport"</summary>
    public class ImportExportMap : BaseEntityMap<ImportExport>
    {
        
        public ImportExportMap() : 
                base("Bars.Gkh.ImportExport.ImportExport", "GKH_IMPORT_EXPORT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Type, "Type").Column("IE_TYPE");
            Reference(x => x.FileInfo, "FileInfo").Column("FILE_ID");
            Property(x => x.HasErrors, "HasErrors").Column("HAS_ERRORS");
            Property(x => x.HasMessages, "HasMessages").Column("HAS_MESSAGES");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
        }
    }
}
