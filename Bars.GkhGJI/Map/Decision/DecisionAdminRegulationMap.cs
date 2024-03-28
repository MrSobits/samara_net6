namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для сущности "Административный регламент  ГЖИ"</summary>
	public class DecisionAdminRegulationMap : BaseEntityMap<DecisionAdminRegulation>
    {
        public DecisionAdminRegulationMap() : 
                base("Административный регламент приказа ГЖИ", "GJI_DECISION_ADMREG")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Decision, "Распоряжение ГЖИ").Column("DECISION_ID").NotNull().Fetch();
            this.Reference(x => x.AdminRegulation, "Административный регламент").Column("ADMREG_ID").NotNull().Fetch();
        }
    }
}