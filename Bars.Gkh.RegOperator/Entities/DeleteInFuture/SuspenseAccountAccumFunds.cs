namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    public class SuspenseAccountAccumFunds : BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenceAccount { get; set; }

        public virtual AccumulatedFunds AccumFunds { get; set; }
    }
}
