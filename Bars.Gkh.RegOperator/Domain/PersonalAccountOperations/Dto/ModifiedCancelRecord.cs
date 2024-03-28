namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Dto
{
    public class ModifiedCancelRecord
    {
        public long Id { get; set; }
        public decimal CancelBaseTariffSum { get; set; }
        public decimal CancelDecisionTariffSum { get; set; }
        public decimal CancelPenaltySum { get; set; }
    }
}
