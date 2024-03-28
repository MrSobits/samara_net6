
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемые документы рапоряжения ГЖИ"</summary>
    public class DecisionProvidedDocMap : BaseEntityMap<DecisionProvidedDoc>
    {
        
        public DecisionProvidedDocMap() : 
                base("Предоставляемые документы рапоряжения ГЖИ", "GJI_DECISION_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.Decision, "Распоряжение").Column("DECISION_ID").NotNull().Fetch();
            Reference(x => x.ProvidedDoc, "Предоставляемый документа").Column("PROVIDED_DOC_ID").NotNull().Fetch();
        }
    }
}
