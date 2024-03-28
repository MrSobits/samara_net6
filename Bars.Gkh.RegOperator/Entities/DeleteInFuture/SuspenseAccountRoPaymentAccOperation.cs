namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    public class SuspenseAccountRoPaymentAccOperation: BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenseAccount { get; set; }

        public virtual RealityObjectPaymentAccountOperation Operation { get; set; }
    }
}