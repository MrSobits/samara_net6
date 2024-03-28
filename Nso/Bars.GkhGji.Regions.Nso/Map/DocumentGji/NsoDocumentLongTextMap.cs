/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Type;
/// 
///     public class NsoDocumentLongTextMap : BaseEntityMap<NsoDocumentLongText>
///     {
///         public NsoDocumentLongTextMap() : base("GJI_ACTCHECK_LTEXT")
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

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.NsoDocumentLongText"</summary>
    public class NsoDocumentLongTextMap : BaseEntityMap<NsoDocumentLongText>
    {
        
        public NsoDocumentLongTextMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.NsoDocumentLongText", "GJI_ACTCHECK_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID").NotNull();
            Property(x => x.PersonViolationInfo, "PersonViolationInfo").Column("PERSON_VIOL");
            Property(x => x.PersonViolationActionInfo, "PersonViolationActionInfo").Column("PERSON_VIOL_ACTION");
			Property(x => x.ViolationDescription, "ViolationDescription").Column("VIOL_DESC");
		}
    }

    public class NsoDocumentLongTextNHibernateMapping : ClassMapping<NsoDocumentLongText>
    {
        public NsoDocumentLongTextNHibernateMapping()
        {
            Property(
                x => x.PersonViolationActionInfo,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            Property(
                x => x.PersonViolationInfo,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

	        Property(
		        x => x.ViolationDescription,
		        mapper =>
		        {
			        mapper.Type<BinaryBlobType>();
		        });
        }
    }
}
