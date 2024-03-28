/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
///     {
///         public ActCheckRoLongDescriptionMap()
///             : base("GJI_ACTCHECKRO_LTEXT")
///         {
///             this.References(x => x.ActCheckRo, "ACTCHECK_RO_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.NotRevealedViolations,
///                 mapper =>
///                 {
///                     mapper.Column("NOT_REV_VIOL");
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.ActCheckRoLongDescription"</summary>
    public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
    {
        
        public ActCheckRoLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.ActCheckRoLongDescription", "GJI_ACTCHECKRO_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActCheckRo, "ActCheckRo").Column("ACTCHECK_RO_ID").NotNull();
            Property(x => x.NotRevealedViolations, "NotRevealedViolations").Column("NOT_REV_VIOL");
        }
    }

    public class ActCheckRoLongDescriptionNHibernateMapping : ClassMapping<ActCheckRoLongDescription>
    {
        public ActCheckRoLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.NotRevealedViolations,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
