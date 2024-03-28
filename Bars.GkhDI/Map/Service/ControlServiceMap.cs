/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Entities;
///     using FluentNHibernate.Mapping;
/// 
///     public class ControlServiceMap : SubclassMap<ControlService>
///     {
///         public ControlServiceMap()
///         {
///             Table("DI_CONTROL_SERVICE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.ControlService"</summary>
    public class ControlServiceMap : JoinedSubClassMap<ControlService>
    {
        
        public ControlServiceMap() : 
                base("Bars.GkhDi.Entities.ControlService", "DI_CONTROL_SERVICE")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
