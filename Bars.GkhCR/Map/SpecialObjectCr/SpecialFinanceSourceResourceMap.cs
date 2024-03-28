namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Средства источника финансирования"</summary>
    public class SpecialFinanceSourceResourceMap : BaseImportableEntityMap<SpecialFinanceSourceResource>
    {
        
        public SpecialFinanceSourceResourceMap() : 
                base("Средства источника финансирования", "CR_SPECIAL_OBJ_FIN_SOURCE_RES")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
            this.Property(x => x.BudgetMu, "Бюджет МО").Column("BUDGET_MU");
            this.Property(x => x.BudgetSubject, "Бюджет субъекта").Column("BUDGET_SUB");
            this.Property(x => x.OwnerResource, "Средства собственника").Column("OWNER_RES");
            this.Property(x => x.FundResource, "Средства фонда").Column("FUND_RES");
            this.Property(x => x.BudgetMuIncome, "Поступило из Бюджет МО").Column("BUDGET_MU_INCOME");
            this.Property(x => x.BudgetSubjectIncome, "Поступило из Бюджет субъекта").Column("BUDGET_SUB_INCOME");
            this.Property(x => x.FundResourceIncome, "Поступило из Средства фонда").Column("FUND_RES_INCOME");
            this.Reference(x => x.TypeWorkCr, "Вид работ").Column("TYPE_WORK_ID").Fetch();
            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
        }
    }
}
