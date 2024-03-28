/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Аудиторские проверки финансовой деятельности"
///     /// </summary>
///     public class FinActivityAuditMap : BaseGkhEntityMap<FinActivityAudit>
///     {
///         public FinActivityAuditMap(): base("DI_DISINFO_FINACT_AUDIT")
///         {
///             Map(x => x.TypeAuditStateDi, "TYPE_AUDIT_STATE").Not.Nullable().CustomType<TypeAuditStateDi>();
///             Map(x => x.Year, "YEAR").Not.Nullable();
/// 
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityAudit"</summary>
    public class FinActivityAuditMap : BaseImportableEntityMap<FinActivityAudit>
    {
        
        public FinActivityAuditMap() : 
                base("Bars.GkhDi.Entities.FinActivityAudit", "DI_DISINFO_FINACT_AUDIT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeAuditStateDi, "TypeAuditStateDi").Column("TYPE_AUDIT_STATE").NotNull();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAGING_ORG_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
