/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Объект расчета процентов"
///     /// </summary>
///     public class BasePercentMap : BaseEntityMap<BasePercent>
///     {
///         public BasePercentMap()
///             : base("DI_PERC_CALC")
///         {
///             Map(x => x.TypeEntityPercCalc, "TYPE_ENTITY_PERC_CALC").CustomType<TypeEntityPercCalc>().Not.Nullable();
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.Percent, "PERCENT");
///             Map(x => x.CalcDate, "CALC_DATE");
///             Map(x => x.ActualVersion, "ACTUAL_VERSION");
///             Map(x => x.PositionsCount, "POSITION_CNT");
///             Map(x => x.CompletePositionsCount, "COMPLETE_POSIT_CNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.BasePercent"</summary>
    public class BasePercentMap : BaseEntityMap<BasePercent>
    {
        
        public BasePercentMap() : 
                base("Bars.GkhDi.Entities.BasePercent", "DI_PERC_CALC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeEntityPercCalc, "TypeEntityPercCalc").Column("TYPE_ENTITY_PERC_CALC").NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(50);
            Property(x => x.Percent, "Percent").Column("PERCENT");
            Property(x => x.CalcDate, "CalcDate").Column("CALC_DATE");
            Property(x => x.ActualVersion, "ActualVersion").Column("ACTUAL_VERSION");
            Property(x => x.PositionsCount, "PositionsCount").Column("POSITION_CNT");
            Property(x => x.CompletePositionsCount, "CompletePositionsCount").Column("COMPLETE_POSIT_CNT");
        }
    }
}
