/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.RealityObject
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
///     using Bars.Gkh.Overhaul.Tat.Enum;
/// 
///     public class DpkrCorrectionStage2Map : BaseEntityMap<DpkrCorrectionStage2>
///     {
///         public DpkrCorrectionStage2Map()
///             : base("OVRHL_DPKR_CORRECT_ST2")
///         {
///             Map(x => x.PlanYear, "PLAN_YEAR").Not.Nullable();
///             Map(x => x.TypeResult, "TYPE_RESULT").Not.Nullable().CustomType<TypeResultCorrectionDpkr>();
/// 
///             References(x => x.RealityObject, "REALITYOBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Stage2, "ST2_VERSION_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.DpkrCorrectionStage2"</summary>
    public class DpkrCorrectionStage2Map : BaseEntityMap<DpkrCorrectionStage2>
    {
        
        public DpkrCorrectionStage2Map() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.DpkrCorrectionStage2", "OVRHL_DPKR_CORRECT_ST2")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PlanYear, "PlanYear").Column("PLAN_YEAR").NotNull();
            Property(x => x.TypeResult, "TypeResult").Column("TYPE_RESULT").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITYOBJECT_ID").NotNull().Fetch();
            Reference(x => x.Stage2, "Stage2").Column("ST2_VERSION_ID").NotNull().Fetch();
        }
    }
}
