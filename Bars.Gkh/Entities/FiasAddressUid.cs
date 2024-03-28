namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    public class FiasAddressUid : BaseImportableEntity
    {
        public virtual FiasAddress Address { get; set; }

        public virtual string Uid { get; set; }

        public virtual string BillingId { get; set; }
    }
}
