/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.Account
/// {
///     using Bars.B4.DataAccess;
///     using Entities;
/// 
///     public class BaseOperationMap : BaseImportableEntityMap<BaseOperation>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.BaseOperation"</summary>
    public class BaseOperationMap : BaseImportableEntityMap<BaseOperation>
    {
        
        public BaseOperationMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.BaseOperation", "OVRHL_ACCOUNT_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE").NotNull();
            Property(x => x.Payer, "Плательщик").Column("PAYER").Length(128);
            Property(x => x.Purpose, "Назначение").Column("PURPOSE").Length(500);
            Property(x => x.Receiver, "Получатель").Column("RECEIVER").Length(128);
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            Property(x => x.Number, "Номер П/П").Column("OPER_NUMBER").Length(50);
            Reference(x => x.BankStatement, "Специальный счет").Column("ACC_BANK_STAT_ID");
            Reference(x => x.Operation, "Наименование операции").Column("OPERATION_ID");
        }
    }
}
