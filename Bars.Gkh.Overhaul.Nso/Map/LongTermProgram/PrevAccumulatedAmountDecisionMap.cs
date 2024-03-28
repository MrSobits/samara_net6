/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class PrevAccumulatedAmountDecisionMap : BaseJoinedSubclassMap<PrevAccumulatedAmountDecision>
///     {
///         public PrevAccumulatedAmountDecisionMap()
///             : base("OVRHL_PR_DEC_ACCUM_AMOUNT", "ID")
///         {
///             Map(x => x.AccumulatedAmount, "ACCUM_AMOUNT");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PrevAccumulatedAmountDecision"</summary>
    public class PrevAccumulatedAmountDecisionMap : JoinedSubClassMap<PrevAccumulatedAmountDecision>
    {
        
        public PrevAccumulatedAmountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PrevAccumulatedAmountDecision", "OVRHL_PR_DEC_ACCUM_AMOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AccumulatedAmount, "AccumulatedAmount").Column("ACCUM_AMOUNT");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
