namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник расчетов пеней
    /// </summary>
    public class PaymentPenaltiesExcludePersAcc : BaseImportableEntity
    {
        /// <summary>
        /// Количество дней
        /// </summary>
        public virtual PaymentPenalties PaymentPenalties { get; set; }

        /// <summary>
        /// Количество дней
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }
    }
}