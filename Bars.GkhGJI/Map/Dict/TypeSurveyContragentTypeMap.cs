namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.TypeSurveyContragentType"</summary>
    public class TypeSurveyContragentTypeMap : BaseEntityMap<TypeSurveyContragentType>
    {
		/// <summary>
		/// Конструктор
		/// </summary>
		public TypeSurveyContragentTypeMap() : 
                base("Bars.GkhGji.Entities.Dict.TypeSurveyContragentType", "GJI_DICT_TYPESURV_CONTRTYPE")
        {
        }
        
		/// <summary>
		/// Замапить
		/// </summary>
        protected override void Map()
        {
            this.Reference(x => x.TypeSurveyGji, "TypeSurveyGji").Column("TYPESURVEY_ID").NotNull();
            this.Property(x => x.TypeJurPerson, "TypeJurPerson").Column("TYPE_JUR_PERSON").NotNull();
        }
    }
}
