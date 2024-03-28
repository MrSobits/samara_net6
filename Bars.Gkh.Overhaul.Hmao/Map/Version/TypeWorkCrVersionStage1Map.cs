/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.Version
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class TypeWorkCrVersionStage1Map : BaseImportableEntityMap<TypeWorkCrVersionStage1>
///     {
///         public TypeWorkCrVersionStage1Map()
///             : base("OVRHL_TYPE_WORK_CR_ST1")
///         {
///             References(x => x.Stage1Version, "ST1_ID", ReferenceMapConfig.Fetch);
///             this.References(x => x.TypeWorkCr, "TYPE_WORK_CR_ID", ReferenceMapConfig.NotNullAndFetch);
///             this.References(x => x.UnitMeasure, "UNIT_MEASURE_ID", ReferenceMapConfig.NotNullAndFetch);
///             this.Map(x => x.CalcBy, "CALCULATE_BY", true, (object)0);
///             this.Map(x => x.Volume, "VOLUME", true, 0);
///             this.Map(x => x.Sum, "SUM", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Связь записи первого этапа версии и видов работ"</summary>
    public class TypeWorkCrVersionStage1Map : BaseImportableEntityMap<TypeWorkCrVersionStage1>
    {
        
        public TypeWorkCrVersionStage1Map() : 
                base("Связь записи первого этапа версии и видов работ", "OVRHL_TYPE_WORK_CR_ST1")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Stage1Version, "Stage1Version").Column("ST1_ID").Fetch();
            Reference(x => x.TypeWorkCr, "TypeWorkCr").Column("TYPE_WORK_CR_ID").NotNull().Fetch();
            Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE_ID").NotNull().Fetch();
            Property(x => x.CalcBy, "CalcBy").Column("CALCULATE_BY").DefaultValue(PriceCalculateBy.Volume).NotNull();
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
        }
    }
}
