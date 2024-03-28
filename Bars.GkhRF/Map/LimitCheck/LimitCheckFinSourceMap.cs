/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using B4.DataAccess;
/// 
///     using Entities;
/// 
///     public class LimitCheckFinSourceMap : BaseEntityMap<LimitCheckFinSource>
///     {
///         public LimitCheckFinSourceMap()
///             : base("RF_LIMIT_CHECK_FINSOURCE")
///         {
///             References(x => x.FinanceSource, "FINANCE_SOURCE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.LimitCheck, "LIMIT_CHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Разрез финансирования проверки лимитов по заявке"</summary>
    public class LimitCheckFinSourceMap : BaseEntityMap<LimitCheckFinSource>
    {
        
        public LimitCheckFinSourceMap() : 
                base("Разрез финансирования проверки лимитов по заявке", "RF_LIMIT_CHECK_FINSOURCE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.FinanceSource, "Разрез финансирования").Column("FINANCE_SOURCE_ID").NotNull().Fetch();
            Reference(x => x.LimitCheck, "Проверка лимитов").Column("LIMIT_CHECK_ID").NotNull().Fetch();
        }
    }
}
