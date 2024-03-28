/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Банковская выписка"
///     /// </summary>
///     public class BankStatementMap : BaseGkhEntityMap<BankStatement>
///     {
///         public BankStatementMap() : base("CR_OBJ_BANK_STATEMENT")
///         {
///             Map(x => x.TypeFinanceGroup, "TYPE_FIN_GROUP").Not.Nullable().CustomType<TypeFinanceGroup>();
///             Map(x => x.BudgetYear, "BUDGET_YEAR");
///             Map(x => x.IncomingBalance, "INCOMING_BALANCE");
///             Map(x => x.OutgoingBalance, "OUTGOING_BALANCE");
///             Map(x => x.PersonalAccount, "PERSONAL_ACCOUNT").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.OperLastDate, "OPER_LAST_DATE");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
/// 
///             References(x => x.ObjectCr, "OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ManagingOrganization, "MANAG_ORG_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Fetch.Join();
///             References(x => x.Period, "PERIOD_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Банковская выписка"</summary>
    public class BankStatementMap : BaseImportableEntityMap<BankStatement>
    {
        
        public BankStatementMap() : 
                base("Банковская выписка", "CR_OBJ_BANK_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeFinanceGroup, "Группа финансирования").Column("TYPE_FIN_GROUP").NotNull();
            Property(x => x.BudgetYear, "Бюджетный год").Column("BUDGET_YEAR");
            Property(x => x.IncomingBalance, "Входящий остаток").Column("INCOMING_BALANCE");
            Property(x => x.OutgoingBalance, "Исходящий остаток").Column("OUTGOING_BALANCE");
            Property(x => x.PersonalAccount, "Лицевой счет").Column("PERSONAL_ACCOUNT").Length(300);
            Property(x => x.DocumentNum, "Номер").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.OperLastDate, "Последний день операции по счету").Column("OPER_LAST_DATE");
            Property(x => x.DocumentDate, "Дата выписки").Column("DOCUMENT_DATE");
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAG_ORG_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Period, "Контрагент").Column("PERIOD_ID").Fetch();
        }
    }
}
