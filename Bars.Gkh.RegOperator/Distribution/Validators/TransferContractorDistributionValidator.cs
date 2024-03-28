namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using Bars.Gkh.RegOperator.Distribution.Impl;

    /// <summary>
    /// Проверка <see cref="TransferContractorDistribution"/>
    /// </summary>
    public class TransferContractorDistributionValidator : TransferCtrDistributionValidator
    {
        /// <summary>
        /// Код
        /// </summary>
        public override string Code => nameof(TransferContractorDistribution);
    }
}