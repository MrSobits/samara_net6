/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предоставляемые документы, для получения лицензии"
///     /// </summary>
///     public class LicenseProvidedDocMap : BaseImportableEntityMap<LicenseProvidedDoc>
///     {
///         public LicenseProvidedDocMap()
///             : base("GKH_DICT_LIC_PROVDOC")
///         {
///             Map(x => x.Name, "NAME").Length(2000).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемые документы для выдачи лицензии"</summary>
    public class LicenseProvidedDocMap : BaseImportableEntityMap<LicenseProvidedDoc>
    {
        
        public LicenseProvidedDocMap() : 
                base("Предоставляемые документы для выдачи лицензии", "GKH_DICT_LIC_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
