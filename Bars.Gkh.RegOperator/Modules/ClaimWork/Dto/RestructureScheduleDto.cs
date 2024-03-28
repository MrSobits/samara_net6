namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Dto
{
    using System;

    public class RestructureScheduleDto
    {
        public long Id { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public decimal Sum { get; set; }
        public decimal MainDebt { get; set; }
        public decimal MainPaid { get; set; }
        public decimal PenaltyDebt { get; set; }
        public decimal PenaltyPaid { get; set; }
        public decimal PaidSum { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string Status { get; set; }
    }
}