/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectPaymentAccountOperationMap : BaseImportableEntityMap<RealityObjectPaymentAccountOperation>
///     {
///         public RealityObjectPaymentAccountOperationMap() : base("REGOP_RO_PAYMENT_ACC_OP")
///         {
///             Map(x => x.Date, "CDATE", true);
///             Map(x => x.OperationSum, "COPER_SUM", true);
///             Map(x => x.OperationType, "COPER_TYPE", true);
///             Map(x => x.OperationStatus, "COPER_STATUS", true);
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
    
    
    /// <summary>Маппинг для "Îïåðàöèÿ ïî ñ÷åòó îïëàò äîìà"</summary>
    public class RealityObjectPaymentAccountOperationMap : BaseImportableEntityMap<RealityObjectPaymentAccountOperation>
    {
        
        public RealityObjectPaymentAccountOperationMap() : 
                base("Îïåðàöèÿ ïî ñ÷åòó îïëàò äîìà", "REGOP_RO_PAYMENT_ACC_OP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Äàòà îïåðàöèè").Column("CDATE").NotNull();
            Reference(x => x.Account, "Ñ÷åò îïëàò äîìà").Column("ACC_ID").NotNull().Fetch();
            Property(x => x.OperationSum, "Ñóììà îïåðàöèè").Column("COPER_SUM").NotNull();
            Property(x => x.OperationType, "Òèï îïåðàöèè").Column("COPER_TYPE").NotNull();
            Property(x => x.OperationStatus, "Ñòàòóñ îïåðàöèè").Column("COPER_STATUS").NotNull();
        }
    }
}
