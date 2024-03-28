/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// мап для FileExternal
///     /// </summary>
///     public class FileExternalMap : BaseGkhEntityMap<FileExternal>
///     {
///         public FileExternalMap()
///             : base("CONVERTER_FILE_EXTERNAL")
///         {
///             Map(x => x.FileInfoId, "FILE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации связь страого external_id с файлом в новой"</summary>
    public class FileExternalMap : BaseImportableEntityMap<FileExternal>
    {
        
        public FileExternalMap() : 
                base("Таблица для конвертации связь страого external_id с файлом в новой", "CONVERTER_FILE_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.FileInfoId, "Id файла в новой").Column("FILE_ID");
        }
    }
}
