namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ActSurveyLongDescription"</summary>
    public class ActSurveyLongDescriptionMap : BaseEntityMap<ActSurveyLongDescription>
    {
        
        public ActSurveyLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ActSurveyLongDescription", "GJI_ACTSURVEY_LONGDESC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActSurvey, "ActSurvey").Column("ACT_SURVEY_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ActSurveyLongDescriptionNHibernateMapping : ClassMapping<ActSurveyLongDescription>
    {
        public ActSurveyLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
