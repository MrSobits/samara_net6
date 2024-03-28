/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class PrescriptionViolDescriptionMap : BaseEntityMap<PrescriptionViolDescription>
///     {
///         public PrescriptionViolDescriptionMap()
///             : base("GJI_PRESCRIPTION_VIOLAT_SMOL_DESCR")
///         {
///             this.References(x => x.PrescriptionViol, "PRESCR_VIOLAT_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                     {
///                         mapper.Column("DESCRIPTION");
///                         mapper.Type<BinaryBlobType>();
///                     });
///             this.Property(
///                 x => x.Action,
///                 mapper =>
///                     {
///                         mapper.Column("ACTION");
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.PrescriptionViolDescription"</summary>
    public class PrescriptionViolDescriptionMap : BaseEntityMap<PrescriptionViolDescription>
    {
        
        public PrescriptionViolDescriptionMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.PrescriptionViolDescription", "GJI_PRESCRIPTION_VIOLAT_SMOL_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PrescriptionViol, "PrescriptionViol").Column("PRESCR_VIOLAT_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.Action, "Action").Column("ACTION");
        }
    }

    public class PrescriptionViolDescriptionNHibernateMapping : ClassMapping<PrescriptionViolDescription>
    {
        public PrescriptionViolDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.Action,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
