namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для сущности "Цель проверки приказа ГЖИ"</summary>
	public class DisposalSurveyPurposeMap : BaseEntityMap<DisposalSurveyPurpose>
    {
        public DisposalSurveyPurposeMap() : 
                base("Цель проверки приказа ГЖИ", "GJI_NSO_DISPOSAL_SURVEY_PURP")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.SurveyPurpose, "Цель проверки").Column("SURVEY_PURP_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }
}
