/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Поступление оплаты аренды"
///     /// </summary>
///     public class RentPaymentInMap : BaseImportableEntityMap<RentPaymentIn>
///     {
///         public RentPaymentInMap() : base("REGOP_RENT_PAYMENT_IN")
///         {
///             Map(x => x.Guid, "P_GUID").Not.Nullable().Length(40);
///             Map(x => x.OperationDate, "OPERATION_DATE");
///             Map(x => x.Sum, "PAYMENT_SUM");
/// 
///             References(x => x.Account, "ACCOUNT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Поступление оплат аренды"</summary>
    public class RentPaymentInMap : BaseImportableEntityMap<RentPaymentIn>
    {
        
        public RentPaymentInMap() : 
                base("Поступление оплат аренды", "REGOP_RENT_PAYMENT_IN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Guid, "Guid").Column("P_GUID").Length(40).NotNull();
            Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE");
            Property(x => x.Sum, "Сумма оплаты").Column("PAYMENT_SUM");
            Reference(x => x.Account, "Счет").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }
}
