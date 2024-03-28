namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using Bars.Gkh.RegOperator.Distribution.Impl;

    /// <summary>
    /// Проверка <see cref="BuildControlPaymentDistribution"/>
    /// </summary>
    public class BuildControlPaymentDistributionValidator : TransferCtrDistributionValidator
    {
        /// <summary>
        /// Код
        /// </summary>
        public override string Code => nameof(BuildControlPaymentDistribution);
    }
}