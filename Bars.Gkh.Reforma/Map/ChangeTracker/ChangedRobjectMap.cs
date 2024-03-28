/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map.ChangeTracker
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities.ChangeTracker;
/// 
///     public class ChangedRobjectMap : PersistentObjectMap<ChangedRobject>
///     {
///         public ChangedRobjectMap()
///             : base("RFRM_CHANGED_ROBJECT")
///         {
///             this.References(x => x.RealityObject, "ROBJECT_ID", ReferenceMapConfig.NotNull);
///             this.References(x => x.PeriodDi, "PERIOD_DI_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map.ChangeTracker
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    
    
    /// <summary>Маппинг для "Изменившийся дом"</summary>
    public class ChangedRobjectMap : PersistentObjectMap<ChangedRobject>
    {
        
        public ChangedRobjectMap() : 
                base("Изменившийся дом", "RFRM_CHANGED_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("ROBJECT_ID").NotNull();
            Reference(x => x.PeriodDi, "Период раскрытия").Column("PERIOD_DI_ID");
        }
    }
}
