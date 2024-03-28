namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipalityRecord"</summary>
    public class SubsidyMunicipalityRecordMap : BaseEntityMap<SubsidyMunicipalityRecord>
    {
        
        public SubsidyMunicipalityRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipalityRecord", "OVRHL_SUBSIDY_MU_REC")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.SubsidyMunicipality, "SubsidyMunicipality").Column("SUBSIDY_MU_ID").NotNull().Fetch();
            this.Property(x => x.SubsidyYear, "SubsidyYear").Column("SUBCIDY_YEAR").NotNull();
            this.Property(x => x.BudgetFcr, "BudgetFcr").Column("BUDGET_FCR").NotNull();
            this.Property(x => x.BudgetRegion, "BudgetRegion").Column("BUDGET_REGION").NotNull();
            this.Property(x => x.BudgetMunicipality, "BudgetMunicipality").Column("BUDGET_MUNICIPALITY").NotNull();
            this.Property(x => x.OwnerSource, "OwnerSource").Column("OWNER_SOURCE").NotNull();
            this.Property(x => x.BudgetCr, "BudgetCr").Column("BUDGET_CR").NotNull();
        }
    }
}
