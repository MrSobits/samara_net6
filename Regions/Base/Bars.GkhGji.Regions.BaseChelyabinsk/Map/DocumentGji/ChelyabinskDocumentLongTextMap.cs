/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Type;
/// 
///     public class ChelyabinskDocumentLongTextMap : BaseEntityMap<ChelyabinskDocumentLongText>
///     {
///         public ChelyabinskDocumentLongTextMap() : base("GJI_ACTCHECK_LTEXT")
///         {
///             References(x => x.DocumentGji, "DOCUMENT_ID", ReferenceMapConfig.NotNull);
///             Property(x => x.PersonViolationActionInfo,
///                 mapper =>
///                 {
///                     mapper.Column("PERSON_VIOL_ACTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
/// 
///             Property(x => x.PersonViolationInfo,
///                 mapper =>
///                 {
///                     mapper.Column("PERSON_VIOL");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.DocumentGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskDocumentLongText"</summary>
    public class ChelyabinskDocumentLongTextMap : BaseEntityMap<ChelyabinskDocumentLongText>
    {
        
        public ChelyabinskDocumentLongTextMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskDocumentLongText", "GJI_ACTCHECK_LTEXT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID").NotNull();
            this.Property(x => x.PersonViolationInfo, "PersonViolationInfo").Column("PERSON_VIOL");
            this.Property(x => x.PersonViolationActionInfo, "PersonViolationActionInfo").Column("PERSON_VIOL_ACTION");
			this.Property(x => x.ViolationDescription, "ViolationDescription").Column("VIOL_DESC");
		}
    }

    public class ChelyabinskDocumentLongTextNHibernateMapping : ClassMapping<ChelyabinskDocumentLongText>
    {
        public ChelyabinskDocumentLongTextNHibernateMapping()
        {
            this.Property(
                x => x.PersonViolationActionInfo,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            this.Property(
                x => x.PersonViolationInfo,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

	        this.Property(
		        x => x.ViolationDescription,
		        mapper =>
		        {
			        mapper.Type<BinaryBlobType>();
		        });
        }
    }
}
