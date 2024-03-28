/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Этап проверки ГЖИ"
///     /// </summary>
///     public class InspectionGjiStageMap : BaseGkhEntityMap<InspectionGjiStage>
///     {
///         public InspectionGjiStageMap() : base("GJI_INSPECTION_STAGE")
///         {
///             Map(x => x.TypeStage, "TYPE_STAGE").Not.Nullable().CustomType<TypeStage>();
///             Map(x => x.Position, "POSITION").Not.Nullable();
/// 
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().LazyLoad();
///             References(x => x.Parent, "PARENT_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап проверки ГЖИ"</summary>
    public class InspectionGjiStageMap : BaseEntityMap<InspectionGjiStage>
    {
        
        public InspectionGjiStageMap() : 
                base("Этап проверки ГЖИ", "GJI_INSPECTION_STAGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeStage, "Тип этапа").Column("TYPE_STAGE").NotNull();
            Property(x => x.Position, "Позиция этапа в дереве то есть последовательность от меньшего к большему").Column("POSITION").NotNull();
            Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            Reference(x => x.Parent, "Родительский этап").Column("PARENT_ID").Fetch();
        }
    }
}
