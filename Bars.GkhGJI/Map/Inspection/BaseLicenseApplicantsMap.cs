/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки ГЖИ по обращению граждан"
///     /// </summary>
///     public class BaseLicenseApplicantsMap : SubclassMap<BaseLicenseApplicants>
///     {
///         public BaseLicenseApplicantsMap()
///         {
///             Table("GJI_INSPECTION_LIC_APP");
///             KeyColumn("ID");
/// 
///             Map(x => x.FormCheck, "FORM_CHECK").Not.Nullable().CustomType<FormCheck>();
///            
///             References(x => x.ManOrgLicenseRequest, "MAN_ORG_LIC_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание проверки соискателей лицензии"</summary>
    public class BaseLicenseApplicantsMap : JoinedSubClassMap<BaseLicenseApplicants>
    {
        
        public BaseLicenseApplicantsMap() : 
                base("Основание проверки соискателей лицензии", "GJI_INSPECTION_LIC_APP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.InspectionType, "Тип проверки").Column("INSPECTION_TYPE").NotNull();
            Property(x => x.TypeForm, "Форма проверки").Column("FORM_CHECK").NotNull();
            Reference(x => x.ManOrgLicenseRequest, "Обращение за выдачей лицензии").Column("MAN_ORG_LIC_ID").Fetch();
        }
    }
}
