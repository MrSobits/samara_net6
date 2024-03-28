/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "протокол смоленска"
///     /// </summary>
///     public class ActRemovalSmolMap : SubclassMap<ActRemovalSmol>
///     {
///         public ActRemovalSmolMap()
///         {
///             Table("GJI_ACTREMOVAL_SMOL");
///             KeyColumn("ID");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ActRemovalSmol"</summary>
    public class ActRemovalSmolMap : JoinedSubClassMap<ActRemovalSmol>
    {
        
        public ActRemovalSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ActRemovalSmol", "GJI_ACTREMOVAL_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateNotification, "DateNotification").Column("DATE_NOTIFICATION");
            Property(x => x.NumberNotification, "NumberNotification").Column("NUMBER_NOTIFICATION");
        }
    }
}
