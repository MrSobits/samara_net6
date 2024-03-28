namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.TypeSurveyLegalReason"</summary>
    public class TypeSurveyLegalReasonMap : BaseEntityMap<TypeSurveyLegalReason>
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public TypeSurveyLegalReasonMap() : 
                base("Bars.GkhGji.Entities.Dict.TypeSurveyLegalReason", "GJI_DICT_TYPESURV_LEGREAS")
        {
        }
        
		/// <summary>
		/// Замапить
		/// </summary>
        protected override void Map()
        {
            this.Reference(x => x.TypeSurveyGji, "TypeSurveyGji").Column("TYPESURVEY_ID").NotNull();
            this.Reference(x => x.LegalReason, "LegalReason").Column("LEGAL_REASON_ID").NotNull();
        }
    }
}
