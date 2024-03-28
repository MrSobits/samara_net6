/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Организационно правовая форма"
///     /// </summary>
///     public class OrganizationFormGjiMap : BaseGkhEntityMap<OrganizationForm>
///     {
///         public OrganizationFormGjiMap()
///             : base("GKH_DICT_ORG_FORM")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.OkopfCode, "OKOPF_CODE").Length(50);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Организационно правовая форма"</summary>
    public class OrganizationFormMap : BaseImportableEntityMap<OrganizationForm>
    {
        
        public OrganizationFormMap() : 
                base("Организационно правовая форма", "GKH_DICT_ORG_FORM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(50);
            Property(x => x.OkopfCode, "Код ОКОПФ").Column("OKOPF_CODE").Length(50);
        }
    }
}
