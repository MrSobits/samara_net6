namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using Bars.Gkh.RegOperator.Distribution.Impl;

    /// <summary>
    /// Проверка <see cref="DEDPaymentDistribution"/>
    /// </summary>
    public class DEDPaymentDistributionValidator : TransferCtrDistributionValidator
    {
        /// <summary>
        /// Код
        /// </summary>
        public override string Code => nameof(DEDPaymentDistribution);
    }
}