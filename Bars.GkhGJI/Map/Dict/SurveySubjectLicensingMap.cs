namespace Bars.GkhGji.Map
{
    using B4.Modules.Mapping.Mappers;
    using GkhGji.Entities.Dict;

    public class SurveySubjectLicensingMap : BaseEntityMap<SurveySubjectLicensing>
    {
        public SurveySubjectLicensingMap() :  base("Справочники - ГЖИ - Предметы проверки Лицензирование", "GJI_DICT_SURVEY_SUBJ_LICENSING")
        {
        }
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(2000);
            Property(x => x.Code, "Code").Column("CODE").Length(300);
        }
    }
}