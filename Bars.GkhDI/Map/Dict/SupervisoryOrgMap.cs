/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для справочника "Контролирующие органы"
///     /// </summary>
///     public class SupervisoryOrgMap : BaseGkhEntityMap<SupervisoryOrg>
///     {
///         public SupervisoryOrgMap() : base("DI_DICT_SUPERVISORY_ORG")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.SupervisoryOrg"</summary>
    public class SupervisoryOrgMap : BaseImportableEntityMap<SupervisoryOrg>
    {
        
        public SupervisoryOrgMap() : 
                base("Bars.GkhDi.Entities.SupervisoryOrg", "DI_DICT_SUPERVISORY_ORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(300);
        }
    }
}
