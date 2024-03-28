/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class LongTermObjectLoanMap : BaseImportableEntityMap<LongTermObjectLoan>
///     {
///         public LongTermObjectLoanMap() : base("OVRHL_LONG_PROG_OBJ_LOAN")
///         {
///             Map(x => x.DateIssue, "DATE_ISSUE");
///             Map(x => x.DateRepayment, "DATE_REPAYMENT");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(300);
///             Map(x => x.LoanAmount, "LOAN_AMOUNT");
///             Map(x => x.PeriodLoan, "PERIOD_LOAN");
/// 
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.Object, "OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ObjectIssued, "OBJECT_ISSUED_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map; 
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.LongTermObjectLoan"</summary>
    public class LongTermObjectLoanMap : BaseImportableEntityMap<LongTermObjectLoan>
    {
        
        public LongTermObjectLoanMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.LongTermObjectLoan", "OVRHL_LONG_PROG_OBJ_LOAN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateIssue, "Дата займа").Column("DATE_ISSUE");
            Property(x => x.DateRepayment, "Дата погашения").Column("DATE_REPAYMENT");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(300);
            Property(x => x.LoanAmount, "Сумма займа").Column("LOAN_AMOUNT");
            Property(x => x.PeriodLoan, "Периода займа (мес.)").Column("PERIOD_LOAN");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Object, "Объект долгосрочной программы").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ObjectIssued, "Объект долгосрочной программы, выдавший займ").Column("OBJECT_ISSUED_ID").NotNull().Fetch();
        }
    }
}
