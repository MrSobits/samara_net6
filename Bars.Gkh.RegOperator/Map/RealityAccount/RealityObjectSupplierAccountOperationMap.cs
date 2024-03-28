/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class RealityObjectSupplierAccountOperationMap : BaseImportableEntityMap<RealityObjectSupplierAccountOperation>
///     {
///         public RealityObjectSupplierAccountOperationMap()
///             : base("REGOP_RO_SUPP_ACC_OP")
///         {
///             Map(x => x.Date, "OP_DATE");
///             Map(x => x.Credit, "CREDIT");
///             Map(x => x.Debt, "DEBT");
///             Map(x => x.OperationType, "OP_TYPE");
///             References(x => x.Account, "ACC_ID", ReferenceMapConfig.CascadeDelete);
///             References(x => x.Work, "WORK_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Операции счета по рассчета с поставщиками"</summary>
    public class RealityObjectSupplierAccountOperationMap : BaseImportableEntityMap<RealityObjectSupplierAccountOperation>
    {
        
        public RealityObjectSupplierAccountOperationMap() : 
                base("Операции счета по рассчета с поставщиками", "REGOP_RO_SUPP_ACC_OP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Дата").Column("OP_DATE");
            Property(x => x.OperationType, "Вид опреации").Column("OP_TYPE");
            Property(x => x.Debt, "Обороты по дебету").Column("DEBT");
            Property(x => x.Credit, "Обороты по кредиту").Column("CREDIT");
            Reference(x => x.Account, "Счет").Column("ACC_ID");
            Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
        }
    }
}
