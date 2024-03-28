/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ActSurveyLongDescriptionMap : BaseEntityMap<ActSurveyLongDescription>
///     {
///         public ActSurveyLongDescriptionMap()
///             : base("GJI_ACTSURVEY_LONGDESC")
///         {
///             this.References(x => x.ActSurvey, "ACT_SURVEY_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.ActSurveyLongDescription"</summary>
    public class ActSurveyLongDescriptionMap : BaseEntityMap<ActSurveyLongDescription>
    {
        
        public ActSurveyLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.ActSurveyLongDescription", "GJI_ACTSURVEY_LONGDESC")
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
