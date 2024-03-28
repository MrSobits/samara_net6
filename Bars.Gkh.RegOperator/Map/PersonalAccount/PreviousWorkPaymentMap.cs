/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class PreviousWorkPaymentMap : BaseImportableEntityMap<PreviousWorkPayment>
///     {
///         public PreviousWorkPaymentMap() : base("REGOP_PREV_WORK_PAY")
///         {
/// 
///             Map(x => x.Guid, "P_GUID").Not.Nullable().Length(40);
///             Map(x => x.OperationDate, "OPERATION_DATE");
///             Map(x => x.Sum, "SUM");
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
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PreviousWorkPayment"</summary>
    public class PreviousWorkPaymentMap : BaseImportableEntityMap<PreviousWorkPayment>
    {
        
        public PreviousWorkPaymentMap() : 
                base("Bars.Gkh.RegOperator.Entities.PreviousWorkPayment", "REGOP_PREV_WORK_PAY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Guid, "Guid").Column("P_GUID").Length(40).NotNull();
            Property(x => x.OperationDate, "Дата оплаты").Column("OPERATION_DATE");
            Property(x => x.Sum, "Сумма").Column("SUM");
            Reference(x => x.Account, "Счет").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }
}
