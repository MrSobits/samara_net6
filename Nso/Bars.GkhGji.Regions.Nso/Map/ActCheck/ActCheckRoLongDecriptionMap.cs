/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
/// 
///     using Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
///     {
///         public ActCheckRoLongDescriptionMap()
///             : base("GJI_ACTCHECKRO_LTEXT")
///         {
///             References(x => x.ActCheckRo, "ACTCHECK_RO_ID", ReferenceMapConfig.NotNull);
///             Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
/// 
///             Property(
///                 x => x.NotRevealedViolations,
///                 mapper =>
///                 {
///                     mapper.Column("NOTREVEALEDVIOL");
///                     mapper.Type<BinaryBlobType>();
///                 });
/// 
///             Property(
///                 x => x.AdditionalChars,
///                 mapper =>
///                 {
///                     mapper.Column("ADDITIONALCHARS");
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.ActCheckRoLongDescription"</summary>
    public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
    {
        
        public ActCheckRoLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.ActCheckRoLongDescription", "GJI_ACTCHECKRO_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActCheckRo, "ActCheckRo").Column("ACTCHECK_RO_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.NotRevealedViolations, "NotRevealedViolations").Column("NOTREVEALEDVIOL");
            Property(x => x.AdditionalChars, "AdditionalChars").Column("ADDITIONALCHARS");
        }
    }

    public class ActCheckRoLongDescriptionNHibernateMapping : ClassMapping<ActCheckRoLongDescription>
    {
        public ActCheckRoLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            Property(
                x => x.NotRevealedViolations,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            Property(
                x => x.AdditionalChars,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
