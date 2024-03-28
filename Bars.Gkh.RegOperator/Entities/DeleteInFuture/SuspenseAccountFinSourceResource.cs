namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Связка записи из счета невясненных сумм и средства источника финансирования
    /// </summary>
    public class SuspenseAccountFinSourceResource : BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenseAccount { get; set; }

        public virtual FinanceSourceResource FinSourceResource { get; set; }

        public virtual SuspenseAccountDistributionParametersType DistributionType { get; set; }
    }
}