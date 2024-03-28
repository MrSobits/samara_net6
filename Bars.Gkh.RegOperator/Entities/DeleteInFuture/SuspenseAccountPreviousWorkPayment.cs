namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    public class SuspenseAccountPreviousWorkPayment: BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenceAccount { get; set; }

        public virtual PreviousWorkPayment PreviousWorkPayment { get; set; }
    }
}
