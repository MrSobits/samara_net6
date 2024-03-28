namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using GkhCr.Entities;

    /// <summary>
    /// Связь записи счета НВС с оплатой акта
    /// </summary>
    public class SuspenseAccountPerformedWorkActPayment : BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenseAccount { get; set; }

        public virtual PerformedWorkActPayment PerformedWorkActPayment { get; set; }
    }
}