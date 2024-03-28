/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class MinAmountDecisionMap : BaseJoinedSubclassMap<MinAmountDecision>
///     {
///         public MinAmountDecisionMap()
///             : base("OVRHL_PR_DEC_MIN_AMOUNT", "ID")
///         {
///             Map(x => x.SizeOfPaymentOwners, "PAY_SIZE", true);
///             Map(x => x.PaymentDateStart, "PAY_DATE_START", true);
///             Map(x => x.PaymentDateEnd, "PAY_DATE_END", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.MinAmountDecision"</summary>
    public class MinAmountDecisionMap : JoinedSubClassMap<MinAmountDecision>
    {
        
        public MinAmountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.MinAmountDecision", "OVRHL_PR_DEC_MIN_AMOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SizeOfPaymentOwners, "SizeOfPaymentOwners").Column("PAY_SIZE").NotNull();
            Property(x => x.PaymentDateStart, "PaymentDateStart").Column("PAY_DATE_START").NotNull();
            Property(x => x.PaymentDateEnd, "PaymentDateEnd").Column("PAY_DATE_END");
        }
    }
}
