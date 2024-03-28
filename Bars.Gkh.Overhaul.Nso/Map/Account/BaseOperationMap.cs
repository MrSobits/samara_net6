/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.Account
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class BaseOperationMap : BaseEntityMap<BaseOperation>
///     {
///         public BaseOperationMap()
///             : base("OVRHL_ACCOUNT_OPERATION")
///         {
///             Map(x => x.OperationDate, "OPERATION_DATE").Not.Nullable();
///             Map(x => x.Payer, "PAYER").Length(128);
///             Map(x => x.Purpose, "PURPOSE").Length(500);
///             Map(x => x.Receiver, "RECEIVER").Length(128);
///             Map(x => x.Sum, "SUM").Not.Nullable();
///             Map(x => x.Number, "OPER_NUMBER").Length(50);
/// 
///             References(x => x.BankStatement, "ACC_BANK_STAT_ID");
///             References(x => x.Operation, "OPERATION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.BaseOperation"</summary>
    public class BaseOperationMap : BaseEntityMap<BaseOperation>
    {
        
        public BaseOperationMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.BaseOperation", "OVRHL_ACCOUNT_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OperationDate, "OperationDate").Column("OPERATION_DATE").NotNull();
            Property(x => x.Payer, "Payer").Column("PAYER").Length(128);
            Property(x => x.Purpose, "Purpose").Column("PURPOSE").Length(500);
            Property(x => x.Receiver, "Receiver").Column("RECEIVER").Length(128);
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.Number, "Number").Column("OPER_NUMBER").Length(50);
            Reference(x => x.BankStatement, "BankStatement").Column("ACC_BANK_STAT_ID");
            Reference(x => x.Operation, "Operation").Column("OPERATION_ID");
        }
    }
}
