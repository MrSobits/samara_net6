
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Эксперт рапоряжения ГЖИ"</summary>
    public class DecisionExpertMap : BaseEntityMap<DecisionExpert>
    {
        
        public DecisionExpertMap() : 
                base("Эксперт решения ГЖИ", "GJI_DECISION_EXPERT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Decision, "Распоряжение").Column("DECISION_ID").NotNull().Fetch();
            Reference(x => x.Expert, "Эксперт").Column("EXPERT_ID").NotNull().Fetch();
        }
    }
}
