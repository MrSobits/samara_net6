namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь расчетно-кассвого центра с МО
    /// </summary>
    public class CashPaymentCenterMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Расчетно-кассовый центр
        /// </summary>
        public virtual CashPaymentCenter CashPaymentCenter { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}