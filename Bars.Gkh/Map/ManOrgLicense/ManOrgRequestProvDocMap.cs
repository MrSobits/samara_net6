/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Предоставляемый документ заявки на получение лицензии
///     /// </summary>
///     public class ManOrgRequestProvDocMap : BaseImportableEntityMap<ManOrgRequestProvDoc>
///     {
///         public ManOrgRequestProvDocMap()
///             : base("GKH_MANORG_REQ_PROVDOC")
///         {
///             References(x => x.LicProvidedDoc, "LIC_PROVDOC_ID", ReferenceMapConfig.NotNull);
///             References(x => x.LicRequest, "LIC_REQUEST_ID", ReferenceMapConfig.NotNull);
///             Map(x => x.Number, "LIC_PROVDOC_NUMBER", false, 100);
///             Map(x => x.Date, "LIC_PROVDOC_DATE");
///             References(x => x.File, "LIC_PROVDOC_FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемый документ заявки на лицензию"</summary>
    public class ManOrgRequestProvDocMap : BaseImportableEntityMap<ManOrgRequestProvDoc>
    {
        
        public ManOrgRequestProvDocMap() : 
                base("Предоставляемый документ заявки на лицензию", "GKH_MANORG_REQ_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicRequest, "Заявка на лицензию").Column("LIC_REQUEST_ID").NotNull();
            Reference(x => x.LicProvidedDoc, "Предосталвяемы документ заявки на лицензию").Column("LIC_PROVDOC_ID").NotNull();
            Property(x => x.Number, "Номер предоставляемого документа").Column("LIC_PROVDOC_NUMBER").Length(100);
            Property(x => x.Date, "Дата предоставляемого документа").Column("LIC_PROVDOC_DATE");
            Reference(x => x.File, "Файл предоставляемого документа").Column("LIC_PROVDOC_FILE_ID").Fetch();
            Property(x => x.SignedInfo, "SignedInfo").Column("SERT_INFO").Length(1500);
        }
    }
}
