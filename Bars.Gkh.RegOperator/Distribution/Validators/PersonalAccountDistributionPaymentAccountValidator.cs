namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Distribution.Impl;

    /// <summary>
    /// Валидатор для проверки соответствия р/с получателя банковской выписки по ЛС
    /// </summary>
    public class PersonalAccountDistributionPaymentAccountValidator<TDistribution> : AbstractDistributionPaymentAccountValidator<TDistribution>
        where TDistribution : AbstractPersonalAccountDistribution
    {
        /// <inheritdoc />
        protected override long[] ExtractRealityObjectIds(BaseParams baseParams, IDistribution distribution)
        {
            var accountDistribution = (AbstractPersonalAccountDistribution)distribution;
            return accountDistribution.ExtractAccountsQuery(baseParams).Select(x => x.RoId).Distinct().ToArray();
        }
    }
}