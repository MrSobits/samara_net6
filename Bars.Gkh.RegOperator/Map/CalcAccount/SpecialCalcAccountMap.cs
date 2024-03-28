/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SpecialCalcAccountMap : BaseJoinedSubclassMap<SpecialCalcAccount>
///     {
///         public SpecialCalcAccountMap() : base("REGOP_CALC_ACC_SPEC", "ID")
///         {
///             Map(x => x.IsActive, "IS_ACTIVE", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Специальный расчетный счет"</summary>
    public class SpecialCalcAccountMap : JoinedSubClassMap<SpecialCalcAccount>
    {
        
        public SpecialCalcAccountMap() : 
                base("Специальный расчетный счет", "REGOP_CALC_ACC_SPEC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IsActive, "Признак счет активен/не активен").Column("IS_ACTIVE").NotNull();
        }
    }
}
