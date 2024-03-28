namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

	/// <summary>Маппинг для "НПА проверки"</summary>
	public class TypeSurveyInspFoundationCheckGjiMap : BaseEntityMap<TypeSurveyInspFoundationCheckGji>
    {
        /// <summary>
		/// Конструктор
		/// </summary>
        public TypeSurveyInspFoundationCheckGjiMap() : 
                base("НПА проверки", "GJI_DICT_LEGFOUND_INSPECTCHECK")
        {
        }
        
		/// <summary>
		/// Маппинг
		/// </summary>
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORM_DOC_ID").NotNull().Fetch();
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
        }
    }
}
