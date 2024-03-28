/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ActSurveyLongDescriptionMap : BaseEntityMap<ActSurveyLongDescription>
///     {
///         public ActSurveyLongDescriptionMap()
///             : base("GJI_ACT_SURV_LONGDESC")
///         {
///             References(x => x.ActSurvey, "ACT_SURV_ID", ReferenceMapConfig.NotNull);
///             Property(
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

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ActSurveyLongDescription"</summary>
    public class ActSurveyLongDescriptionMap : BaseEntityMap<ActSurveyLongDescription>
    {
        
        public ActSurveyLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ActSurveyLongDescription", "GJI_ACT_SURV_LONGDESC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActSurvey, "ActSurvey").Column("ACT_SURV_ID").NotNull();
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
