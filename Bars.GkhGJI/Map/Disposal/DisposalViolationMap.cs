/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение в акте ГЖИ"
///     /// </summary>
///     public class DisposalViolationMap : SubclassMap<DisposalViolation>
///     {
///         public DisposalViolationMap()
///         {
///             Table("GJI_DISPOSAL_VIOLAT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап нарушения в приказе Используется в томске но поскольку есть большая вероятност ьчто дальеш в других регионах тоже могут нарушения создават ьв приказе то делаю"</summary>
    public class DisposalViolationMap : JoinedSubClassMap<DisposalViolation>
    {
        
        public DisposalViolationMap() : 
                base("Этап нарушения в приказе Используется в томске но поскольку есть большая вероятно" +
                        "ст ьчто дальеш в других регионах тоже могут нарушения создават ьв приказе то дел" +
                        "аю", "GJI_DISPOSAL_VIOLAT")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
