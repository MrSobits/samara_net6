/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ActCheckRealityObjectDescriptionMap : BaseEntityMap<ActCheckRealityObjectDescription>
///     {
///         public ActCheckRealityObjectDescriptionMap()
///             : base("GJI_ACTCHECK_SMOL_ROBJECT_DESCR")
///         {
///             this.References(x => x.ActCheckRealityObject, "ACT_CHECK_ROBJECT_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                     {
///                         mapper.Column("DESCRIPTION");
///                         mapper.Type<BinaryBlobType>();
///                     });
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ActCheckRealityObjectDescription"</summary>
    public class ActCheckRealityObjectDescriptionMap : BaseEntityMap<ActCheckRealityObjectDescription>
    {
        
        public ActCheckRealityObjectDescriptionMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ActCheckRealityObjectDescription", "GJI_ACTCHECK_SMOL_ROBJECT_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActCheckRealityObject, "ActCheckRealityObject").Column("ACT_CHECK_ROBJECT_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ActCheckRealityObjectDescriptionNHibernateMapping : ClassMapping<ActCheckRealityObjectDescription>
    {
        public ActCheckRealityObjectDescriptionNHibernateMapping()
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
