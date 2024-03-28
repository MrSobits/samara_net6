/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities.Refactor;
/// 
///     public class MoneyLockMap : BaseImportableEntityMap<MoneyLock>
///     {
///         public MoneyLockMap()
///             : base("REGOP_MONEY_LOCK")
///         {
///             Map(x => x.Amount, "AMOUNT");
///             Map(x => x.IsActive, "IS_ACTIVE");
///             Map(x => x.LockGuid, "LOCK_GUID");
///             Map(x => x.TargetGuid, "TARGET_GUID");
///             Map(x => x.LockDate, "LOCK_DATE");
///             Map(x => x.UnlockDate, "UNLOCK_DATE");
///             Map(x => x.SourceName, "SOURCE_NAME");
/// 
///             References(x => x.Wallet, "WALLET_ID");
///             References(x => x.Operation, "OPERATION_ID");
///             References(x => x.CancelOperation, "CANCEL_OPERATION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Refactor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    
    
    /// <summary>Маппинг для "Блокирование денег"</summary>
    public class MoneyLockMap : BaseImportableEntityMap<MoneyLock>
    {
        
        public MoneyLockMap() : 
                base("Блокирование денег", "REGOP_MONEY_LOCK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Amount, "Количество заблокированных денег").Column("AMOUNT");
            Property(x => x.IsActive, "Блокировка снята").Column("IS_ACTIVE");
            Property(x => x.LockGuid, "Уникальный guid блокировки").Column("LOCK_GUID");
            Property(x => x.TargetGuid, "Получатель денег").Column("TARGET_GUID");
            Property(x => x.LockDate, "Времы создания блокировки").Column("LOCK_DATE");
            Property(x => x.UnlockDate, "Время снятия блокировки").Column("UNLOCK_DATE");
            Property(x => x.SourceName, "Наименование источника").Column("SOURCE_NAME");
            Reference(x => x.Wallet, "Кошелек, на котором блокируются деньги").Column("WALLET_ID");
            Reference(x => x.Operation, "Операция, в рамках которой была произведена блокировка").Column("OPERATION_ID");
            Reference(x => x.CancelOperation, "Операция, в рамках которой была снята блокировка").Column("CANCEL_OPERATION_ID");
        }
    }
}
