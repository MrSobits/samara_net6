/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Type;
/// 
///     public class ActCheckViolationLongTextMap : BaseEntityMap<ActCheckViolationLongText>
///     {
///         public ActCheckViolationLongTextMap()
///             : base("GJI_ACTCHECKVIOL_LTEXT")
///         {
///             References(x => x.Violation, "ACTVIOL_ID", ReferenceMapConfig.NotNull);
/// 
///             Property(x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.ActCheckViolationLongText"</summary>
    public class ActCheckViolationLongTextMap : BaseEntityMap<ActCheckViolationLongText>
    {
        
        public ActCheckViolationLongTextMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.ActCheckViolationLongText", "GJI_ACTCHECKVIOL_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Violation, "Violation").Column("ACTVIOL_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ActCheckViolationLongTextNHibernateMapping : ClassMapping<ActCheckViolationLongText>
    {
        public ActCheckViolationLongTextNHibernateMapping()
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
