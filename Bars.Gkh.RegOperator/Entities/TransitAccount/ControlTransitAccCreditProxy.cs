namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    public  class ControlTransitAccCreditProxy : BaseImportableEntity
    {
        public virtual DateTime? Date { get; set; }

        public virtual string CreditOrgName { get; set; }

        public virtual string CalcAccount { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual decimal ConfirmSum { get; set; }

        public virtual decimal Divergence { get; set; }
    }
}