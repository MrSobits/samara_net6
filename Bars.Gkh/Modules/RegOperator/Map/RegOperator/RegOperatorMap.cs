/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.RegOperator.Map.RegOperator
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
/// 
///     /// <summary>
///     /// Маппинг сущности "Региональный оператор"
///     /// </summary>
///     public class RegOperatorMap : BaseEntityMap<RegOperator>
///     {
///         public RegOperatorMap()
///             : base("OVRHL_REG_OPERATOR")
///         {
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.RegOperator.Map.RegOperator
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    
    
    /// <summary>Маппинг для "Региональный оператор"</summary>
    public class RegOperatorMap : BaseImportableEntityMap<RegOperator>
    {
        
        public RegOperatorMap() : 
                base("Региональный оператор", "OVRHL_REG_OPERATOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
