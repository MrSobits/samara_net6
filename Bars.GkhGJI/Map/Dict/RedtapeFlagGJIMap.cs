/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class RedtapeFlagGjiMap : BaseGkhEntityMap<RedtapeFlagGji>
///     {
///         public RedtapeFlagGjiMap()
///             : base("GJI_DICT_ACREDT_FLAG")
///         {
///             Map(x => x.Name, "NAME").Length(250);
///             Map(x => x.Code, "CODE");
///             Map(x => x.SystemValue, "SYSTEM_VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Признак волокиты"</summary>
    public class RedtapeFlagGjiMap : BaseEntityMap<RedtapeFlagGji>
    {
        
        public RedtapeFlagGjiMap() : 
                base("Признак волокиты", "GJI_DICT_ACREDT_FLAG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.SystemValue, "Системное Значение").Column("SYSTEM_VALUE");
        }
    }
}
