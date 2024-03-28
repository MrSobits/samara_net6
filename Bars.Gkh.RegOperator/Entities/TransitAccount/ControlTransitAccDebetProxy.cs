namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    public class ControlTransitAccDebetProxy : BaseImportableEntity
    {
        public virtual string Number { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual string PaymentAgentName { get; set; }

        public virtual string PaymentAgentCode { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual decimal ConfirmSum { get; set; }

        public virtual decimal Divergence { get; set; }
    }
}