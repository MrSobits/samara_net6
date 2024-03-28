/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Общие сведения финансовой деятельности"
///     /// </summary>
///     public class FinActivityMap : BaseGkhEntityMap<FinActivity>
///     {
///         public FinActivityMap() : base("DI_DISINFO_FIN_ACTIVITY")
///         {
///             Map(x => x.ValueBlankActive, "VALUE_BLANK_ACTIVE");
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.ClaimDamage, "CLAIM_DAMAGE");
///             Map(x => x.FailureService, "FAILURE_SERVICE");
///             Map(x => x.NonDeliveryService, "NON_DELIVERY_SERVICE");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TaxSystem, "TAX_SYSTEM_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivity"</summary>
    public class FinActivityMap : BaseImportableEntityMap<FinActivity>
    {
        
        public FinActivityMap() : 
                base("Bars.GkhDi.Entities.FinActivity", "DI_DISINFO_FIN_ACTIVITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ValueBlankActive, "ValueBlankActive").Column("VALUE_BLANK_ACTIVE");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Property(x => x.ClaimDamage, "ClaimDamage").Column("CLAIM_DAMAGE");
            Property(x => x.FailureService, "FailureService").Column("FAILURE_SERVICE");
            Property(x => x.NonDeliveryService, "NonDeliveryService").Column("NON_DELIVERY_SERVICE");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.TaxSystem, "TaxSystem").Column("TAX_SYSTEM_ID").Fetch();
        }
    }
}
