/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// using Bars.Gkh.Enums;
/// 
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности Лицензия  "Управляющая организация"
///     /// </summary>
///     public class ManOrgLicenseDocMap : BaseImportableEntityMap<ManOrgLicenseDoc>
///     {
///         public ManOrgLicenseDocMap()
///             : base("GKH_MANORG_LIC_DOC")
///         {
///             Map(x => x.DocDate, "DOC_DATE");
///             Map(x => x.DocNumber, "DOC_NUMBER").Length(100);
///             Map(x => x.DocType, "DOC_TYPE").CustomType<TypeManOrgTypeDocLicense>();
/// 
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.ManOrgLicense, "LIC_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Документ Лицензии управляющей организации"</summary>
    public class ManOrgLicenseDocMap : BaseImportableEntityMap<ManOrgLicenseDoc>
    {
        
        public ManOrgLicenseDocMap() : 
                base("Документ Лицензии управляющей организации", "GKH_MANORG_LIC_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocDate, "Дата").Column("DOC_DATE");
            Property(x => x.DocNumber, "Номер").Column("DOC_NUMBER").Length(100);
            Property(x => x.DocType, "Основание прекращения деятельности").Column("DOC_TYPE");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.ManOrgLicense, "Лицензия").Column("LIC_ID").NotNull();
        }
    }
}
