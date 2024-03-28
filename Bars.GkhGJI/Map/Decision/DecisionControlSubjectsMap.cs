namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures"</summary>
    public class DecisionControlSubjectsMap : BaseEntityMap<DecisionControlSubjects>
    {
        
        public DecisionControlSubjectsMap() : 
                base("Bars.GkhGji.Entities", "GJI_DECISION_CON_SUBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Disposal").Column("DECISION_ID").NotNull().Fetch();
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");          
            this.Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            this.Property(x => x.PhysicalPersonINN, "Физическое лицо").Column("PHYSICAL_PERSON_INN").Length(15);
            this.Property(x => x.PhysicalPersonPosition, "Должность ФЛ").Column("PHYSICAL_PERSON_POSITION").Length(300);
            this.Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION").NotNull();
            this.Reference(x => x.Contragent, "Контрагент проверки").Column("CONTRAGENT_ID");
      
        }
    }
}
