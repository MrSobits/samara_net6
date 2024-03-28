/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class MinAmountDecisionMap : BaseJoinedSubclassMap<MinAmountDecision>
///     {
///         public MinAmountDecisionMap()
///             : base("OVRHL_PR_DEC_MIN_AMOUNT", "ID")
///         {
///             this.Map(x => x.SizeOfPaymentOwners, "PAY_SIZE", true);
///             this.Map(x => x.PaymentDateStart, "PAY_DATE_START", true);
///             this.Map(x => x.PaymentDateEnd, "PAY_DATE_END", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Решение собственников помещений МКД (Установление минимального размера фонда кап.ремонта)"</summary>
    public class MinAmountDecisionMap : JoinedSubClassMap<MinAmountDecision>
    {
        
        public MinAmountDecisionMap() : 
                base("Решение собственников помещений МКД (Установление минимального размера фонда кап." +
                        "ремонта)", "OVRHL_PR_DEC_MIN_AMOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SizeOfPaymentOwners, "Размер вноса, установленный собственниками (руб.)").Column("PAY_SIZE").NotNull();
            Property(x => x.PaymentDateStart, "Дата начала действия взноса").Column("PAY_DATE_START").NotNull();
            Property(x => x.PaymentDateEnd, "Дата окончания действия взноса").Column("PAY_DATE_END");
        }
    }
}
