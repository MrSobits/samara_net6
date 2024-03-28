namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Entities;

    using Gkh.Entities;

    public class TransitAccount : BaseImportableEntity
    {
        public virtual PaymentAgent PaymentAgent { get; set; }

        public virtual string Number { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual decimal Sum { get; set; }
    }
}