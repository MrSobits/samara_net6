namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;
    using GkhRf.Entities;

    public class TransferObject : BaseImportableEntity
    {
        public virtual TransferRfRecord TransferRecord { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual decimal? TransferredSum { get; set; }

        public virtual bool Transferred { get; set; }
    }
}