/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess;
///     using Entities;
///     using Bars.Gkh.Overhaul.Enum;
/// 
///     public class BankAccountMap : BaseImportableEntityMap<BankAccount>
///     {
///         public BankAccountMap()
///             : base("OVRHL_ACCOUNT")
///         {
///             Map(x => x.Number, "ACC_NUMBER").Length(50);
///             Map(x => x.OpenDate, "OPEN_DATE");
///             Map(x => x.CloseDate, "CLOSE_DATE");
///             Map(x => x.TotalIncome, "TOTAL_INCOME");
///             Map(x => x.TotalOut, "TOTAL_OUT");
///             Map(x => x.Balance, "BALANCE");
///             Map(x => x.LastOperationDate, "LAST_OPERATION_DATE");
///             Map(x => x.CreditLimit, "CREDIT_LIMIT");
///             Map(x => x.AccountType, "ACCOUNT_TYPE").Not.Nullable().CustomType<AccountType>();
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Базовый класс Счет"</summary>
    public class BankAccountMap : BaseImportableEntityMap<BankAccount>
    {
        
        public BankAccountMap() : 
                base("Базовый класс Счет", "OVRHL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Номер счета").Column("ACC_NUMBER").Length(50);
            Property(x => x.OpenDate, "Дата открытия").Column("OPEN_DATE");
            Property(x => x.CloseDate, "Дата закрытия").Column("CLOSE_DATE");
            Property(x => x.TotalIncome, "Итого по приходу").Column("TOTAL_INCOME");
            Property(x => x.TotalOut, "Итого по расходу").Column("TOTAL_OUT");
            Property(x => x.Balance, "Сальдо по счету").Column("BALANCE");
            Property(x => x.LastOperationDate, "Дата последней операции по счету").Column("LAST_OPERATION_DATE");
            Property(x => x.CreditLimit, "Лимит по кредиту").Column("CREDIT_LIMIT");
            Property(x => x.AccountType, "Тип счета").Column("ACCOUNT_TYPE").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
        }
    }
}
