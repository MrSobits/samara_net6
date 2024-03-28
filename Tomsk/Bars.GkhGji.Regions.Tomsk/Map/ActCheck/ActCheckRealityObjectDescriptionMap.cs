/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Type;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Описание Дома акта проверки"
///     /// </summary>
///     public class ActCheckRealityObjectDescriptionMap  : BaseEntityMap<ActCheckRealityObjectDescription>
///     {
///         public ActCheckRealityObjectDescriptionMap()
///             : base("GJI_ACTCHECK_RO_DISCR")
///         {
///             Property(x => x.Description, m =>
///             {
///                 m.Column("DESCRIPTION");
///                 m.Type<BinaryBlobType>();
///             });
/// 
///             References(x => x.ActCheckRealityObject, "ACTCHECK_RO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ActCheckRealityObjectDescription"</summary>
    public class ActCheckRealityObjectDescriptionMap : BaseEntityMap<ActCheckRealityObjectDescription>
    {
        
        public ActCheckRealityObjectDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ActCheckRealityObjectDescription", "GJI_ACTCHECK_RO_DISCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActCheckRealityObject, "ActCheckRealityObject").Column("ACTCHECK_RO_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ActCheckRealityObjectDescriptionNHibernateMapping : ClassMapping<ActCheckRealityObjectDescription>
    {
        public ActCheckRealityObjectDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                m =>
                    {
                        m.Type<BinaryBlobType>();
                    });
        }
    }
}
