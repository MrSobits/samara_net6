/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "График погашения займов подрядчиков"
///     /// </summary>
///     public class BuilderLoanRepaymentMap : BaseGkhEntityMap<BuilderLoanRepayment>
///     {
///         public BuilderLoanRepaymentMap()
///             : base("GKH_BUILDER_LOAN_REPAY")
///         {
///             Map(x => x.RepaymentDate, "DATE_REPAYMENT");
///             Map(x => x.RepaymentAmount, "AMOUNT");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.BuilderLoan, "BUILDER_LOAN_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "График погашения займа подрядчика"</summary>
    public class BuilderLoanRepaymentMap : BaseImportableEntityMap<BuilderLoanRepayment>
    {
        
        public BuilderLoanRepaymentMap() : 
                base("График погашения займа подрядчика", "GKH_BUILDER_LOAN_REPAY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RepaymentDate, "Дата погашения").Column("DATE_REPAYMENT");
            Property(x => x.RepaymentAmount, "Сумма погашения").Column("AMOUNT");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.BuilderLoan, "Займ подрядчика").Column("BUILDER_LOAN_ID").NotNull().Fetch();
        }
    }
}
