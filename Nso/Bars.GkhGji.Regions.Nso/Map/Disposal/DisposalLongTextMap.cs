/// <mapping-converter-backup>
/// using Bars.GkhGji.Regions.Nso.Entities.Disposal;
/// 
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Nso.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class DisposalLongTextMap : BaseEntityMap<DisposalLongText>
///     {
///         public DisposalLongTextMap()
///             : base("GJI_NSO_DISP_LTEXT")
///         {
///             this.References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.NoticeDescription,
///                 mapper =>
///                 {
///                     mapper.Column("NOTICE_DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalLongText"</summary>
    public class DisposalLongTextMap : BaseEntityMap<DisposalLongText>
    {
        
        public DisposalLongTextMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalLongText", "GJI_NSO_DISP_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull();
            Property(x => x.NoticeDescription, "NoticeDescription").Column("NOTICE_DESCRIPTION");
        }
    }

    public class DisposalLongTextNHibernateMapping : ClassMapping<DisposalLongText>
    {
        public DisposalLongTextNHibernateMapping()
        {
            Property(
                x => x.NoticeDescription,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
