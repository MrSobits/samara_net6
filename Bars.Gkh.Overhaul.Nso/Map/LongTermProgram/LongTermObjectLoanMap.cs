/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class LongTermObjectLoanMap : BaseEntityMap<LongTermObjectLoan>
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

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.LongTermObjectLoan"</summary>
    public class LongTermObjectLoanMap : BaseEntityMap<LongTermObjectLoan>
    {
        
        public LongTermObjectLoanMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.LongTermObjectLoan", "OVRHL_LONG_PROG_OBJ_LOAN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateIssue, "DateIssue").Column("DATE_ISSUE");
            Property(x => x.DateRepayment, "DateRepayment").Column("DATE_REPAYMENT");
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER").Length(300);
            Property(x => x.LoanAmount, "LoanAmount").Column("LOAN_AMOUNT");
            Property(x => x.PeriodLoan, "PeriodLoan").Column("PERIOD_LOAN");
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.Object, "Object").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ObjectIssued, "ObjectIssued").Column("OBJECT_ISSUED_ID").NotNull().Fetch();
        }
    }
}
