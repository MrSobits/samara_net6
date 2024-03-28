/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "протокол смоленска"
///     /// </summary>
///     public class ActCheckSmolMap : SubclassMap<ActCheckSmol>
///     {
///         public ActCheckSmolMap()
///         {
///             Table("GJI_ACTCHECK_SMOL");
///             KeyColumn("ID");
///             Map(x => x.HaveViolation, "HAVE_VIOLATION").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.DateNotification, "DATE_NOTIFICATION");
///             Map(x => x.NumberNotification, "NUMBER_NOTIFICATION");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ActCheckSmol"</summary>
    public class ActCheckSmolMap : JoinedSubClassMap<ActCheckSmol>
    {
        
        public ActCheckSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ActCheckSmol", "GJI_ACTCHECK_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.HaveViolation, "HaveViolation").Column("HAVE_VIOLATION").NotNull();
            Property(x => x.DateNotification, "DateNotification").Column("DATE_NOTIFICATION");
            Property(x => x.NumberNotification, "NumberNotification").Column("NUMBER_NOTIFICATION");
        }
    }
}
