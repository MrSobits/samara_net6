namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Distribution.Impl;

    /// <summary>
    /// Валидатор для проверки соответствия р/с получателя банковской выписки по ЛС
    /// </summary>
    public class RealityAccountDistributionPaymentAccountValidator<TDistribution> : AbstractDistributionPaymentAccountValidator<TDistribution>
            where TDistribution : AbstractRealtyAccountDistribution
    {
        /// <inheritdoc />
        protected override long[] ExtractRealityObjectIds(BaseParams baseParams, IDistribution distribution)
        {
            var accountDistribution = (AbstractRealtyAccountDistribution)distribution;
            return accountDistribution.GetRealityObjectPaymentAccounts(baseParams).Select(x => x.RealityObject.Id).Distinct().ToArray();
        }
    }
}