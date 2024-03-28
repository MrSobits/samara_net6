namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связка записи из счета невясненных сумм и оплатой по ЛС
    /// </summary>
    public class SuspenseAccountPersonalAccountPayment : BaseImportableEntity
    {
        public virtual SuspenseAccount SuspenseAccount { get; set; }

        public virtual PersonalAccountPayment Payment { get; set; }
    }
}