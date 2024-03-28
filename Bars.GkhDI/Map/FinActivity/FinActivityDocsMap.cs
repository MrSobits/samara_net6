/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы фин деятельности"
///     /// </summary>
///     public class FinActivityDocsMap : BaseGkhEntityMap<FinActivityDocs>
///     {
///         public FinActivityDocsMap() : base("DI_DISINFO_FINACT_DOCS")
///         {
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BookkepingBalance, "BOOKKEEP_BALANCE").Fetch.Join();
///             References(x => x.BookkepingBalanceAnnex, "BOOKKEEP_BALANCE_ANNEX").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityDocs"</summary>
    public class FinActivityDocsMap : BaseImportableEntityMap<FinActivityDocs>
    {
        
        public FinActivityDocsMap() : 
                base("Bars.GkhDi.Entities.FinActivityDocs", "DI_DISINFO_FINACT_DOCS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.BookkepingBalance, "BookkepingBalance").Column("BOOKKEEP_BALANCE").Fetch();
            Reference(x => x.BookkepingBalanceAnnex, "BookkepingBalanceAnnex").Column("BOOKKEEP_BALANCE_ANNEX").Fetch();
        }
    }
}
