/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
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

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActCheck
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ActCheckRoLongDescription"</summary>
    public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
    {
        
        public ActCheckRoLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ActCheckRoLongDescription", "GJI_ACTCHECKRO_LTEXT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ActCheckRo, "ActCheckRo").Column("ACTCHECK_RO_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.NotRevealedViolations, "NotRevealedViolations").Column("NOTREVEALEDVIOL");
            this.Property(x => x.AdditionalChars, "AdditionalChars").Column("ADDITIONALCHARS");
        }
    }

    public class ActCheckRoLongDescriptionNHibernateMapping : ClassMapping<ActCheckRoLongDescription>
    {
        public ActCheckRoLongDescriptionNHibernateMapping()
        {
            this.Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            this.Property(
                x => x.NotRevealedViolations,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            this.Property(
                x => x.AdditionalChars,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
