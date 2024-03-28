/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class ContragentAuditPurposeMap : BaseEntityMap<ContragentAuditPurpose>
///     {
///         public ContragentAuditPurposeMap()
///             : base("GKH_CONTRAGENT_AUDIT_PURP")
///         {
///             Map(x => x.LastInspDate, "LAST_INSP_DATE");
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.AuditPurpose, "AUDIT_PURPOSE_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Цели контрагента"</summary>
    public class ContragentAuditPurposeMap : BaseEntityMap<ContragentAuditPurpose>
    {
        
        public ContragentAuditPurposeMap() : 
                base("Цели контрагента", "GKH_CONTRAGENT_AUDIT_PURP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.AuditPurpose, "Цель").Column("AUDIT_PURPOSE_ID").NotNull().Fetch();
            Property(x => x.LastInspDate, "Дата прошлой проверки").Column("LAST_INSP_DATE");
        }
    }
}
