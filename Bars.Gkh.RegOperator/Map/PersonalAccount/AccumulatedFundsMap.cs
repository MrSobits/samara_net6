/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Ранее накопленные средства"
///     /// </summary>
///     public class AccumulatedFundsMap : BaseImportableEntityMap<AccumulatedFunds>
///     {
///         public AccumulatedFundsMap() : base("REGOP_ACCUMULATED_FUNDS")
///         {
///             Map(x => x.Guid, "P_GUID", true, 40);
///             Map(x => x.OperationDate, "OPERATION_DATE");
///             Map(x => x.Sum, "ACCUMULATED_SUM");
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Ранее накопленные средства"</summary>
    public class AccumulatedFundsMap : BaseImportableEntityMap<AccumulatedFunds>
    {
        
        public AccumulatedFundsMap() : 
                base("Ранее накопленные средства", "REGOP_ACCUMULATED_FUNDS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Счет").Column("ACCOUNT_ID").NotNull().Fetch();
            Property(x => x.OperationDate, "Дата оплаты").Column("OPERATION_DATE");
            Property(x => x.Guid, "Guid").Column("P_GUID").Length(40).NotNull();
            Property(x => x.Sum, "Сумма").Column("ACCUMULATED_SUM");
        }
    }
}
