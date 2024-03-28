/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectSubsidyAccountOperationMap : BaseImportableEntityMap<RealityObjectSubsidyAccountOperation>
///     {
///         public RealityObjectSubsidyAccountOperationMap()
///             : base("REGOP_RO_SUBSIDY_ACC_OP")
///         {
///             Map(x => x.Date, "OPER_DATE", true);
///             Map(x => x.OperationSum, "OPER_SUM", true);
///             Map(x => x.OperationType, "OPER_TYPE", true);
/// 
///             References(x => x.Account, "ACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Фактическое поступление субсидий"</summary>
    public class RealityObjectSubsidyAccountOperationMap : BaseImportableEntityMap<RealityObjectSubsidyAccountOperation>
    {
        
        public RealityObjectSubsidyAccountOperationMap() : 
                base("Фактическое поступление субсидий", "REGOP_RO_SUBSIDY_ACC_OP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Счет субсидий").Column("ACC_ID").NotNull().Fetch();
            Property(x => x.OperationSum, "Сумма операции").Column("OPER_SUM").NotNull();
            Property(x => x.OperationType, "Тип операции").Column("OPER_TYPE").NotNull();
            Property(x => x.Date, "Дата операции").Column("OPER_DATE").NotNull();
        }
    }
}
