/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class SuspenseAccountAccumFundsMap : BaseImportableEntityMap<SuspenseAccountAccumFunds>
///     {
///         public SuspenseAccountAccumFundsMap()
///             : base("REGOP_SUSPACC_ACCFUNDS")
///         {
///             References(x => x.AccumFunds, "ACCUMFUNDS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.SuspenceAccount, "SUSPACC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.SuspenseAccountAccumFunds"</summary>
    public class SuspenseAccountAccumFundsMap : BaseImportableEntityMap<SuspenseAccountAccumFunds>
    {
        
        public SuspenseAccountAccumFundsMap() : 
                base("Bars.Gkh.RegOperator.Entities.SuspenseAccountAccumFunds", "REGOP_SUSPACC_ACCFUNDS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AccumFunds, "AccumFunds").Column("ACCUMFUNDS_ID").NotNull().Fetch();
            Reference(x => x.SuspenceAccount, "SuspenceAccount").Column("SUSPACC_ID").NotNull().Fetch();
        }
    }
}
