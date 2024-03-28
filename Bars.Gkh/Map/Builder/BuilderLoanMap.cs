/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Займ подрядчика"
///     /// </summary>
///     public class BuilderLoanMap : BaseGkhEntityMap<BuilderLoan>
///     {
///         public BuilderLoanMap()
///             : base("GKH_BUILDER_LOAN")
///         {
///             Map(x => x.DateIssue, "DATE_ISSUE");
///             Map(x => x.DatePlanReturn, "DATE_PLAN_RETURN");
///             Map(x => x.Amount, "AMOUNT");
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(100);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
/// 
///             References(x => x.Lender, "LENDER_ID").Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Займ подрядчика"</summary>
    public class BuilderLoanMap : BaseImportableEntityMap<BuilderLoan>
    {
        
        public BuilderLoanMap() : 
                base("Займ подрядчика", "GKH_BUILDER_LOAN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateIssue, "Дата выдачи").Column("DATE_ISSUE");
            Property(x => x.DatePlanReturn, "Плановая дата возврата").Column("DATE_PLAN_RETURN");
            Property(x => x.Amount, "Сумма выданного").Column("AMOUNT");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(100);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Reference(x => x.Lender, "Заемщик").Column("LENDER_ID").Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
