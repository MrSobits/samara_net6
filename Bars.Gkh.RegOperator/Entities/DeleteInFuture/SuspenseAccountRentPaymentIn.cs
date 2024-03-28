namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    public class SuspenseAccountRentPaymentIn : BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenceAccount { get; set; }

        public virtual RentPaymentIn Payment { get; set; }
    }
}
