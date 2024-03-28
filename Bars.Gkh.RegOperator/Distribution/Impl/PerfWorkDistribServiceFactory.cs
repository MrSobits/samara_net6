namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Фабрика для <see cref="IPerformedWorkDistribution" />
    /// </summary>
    public static class PerfWorkDistribServiceFactory
    {
        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <param name="container">IoC</param>
        public static IPerformedWorkDistribution Create(IWindsorContainer container)
        {
            var configProvider = container.Resolve<IGkhConfigProvider>();

            var perfWorkChargeType = configProvider.Get<RegOperatorConfig>().GeneralConfig.PerfWorkChargeConfig.PerfWorkChargeType;

            return perfWorkChargeType == PerfWorkChargeType.ForExistingCharges
                ? container.Resolve<PerformedWorkDistribution>(nameof(PerformedWorkDistribution))
                : container.Resolve<PerformedWorkDeferredDistribution>(nameof(PerformedWorkDeferredDistribution));
        }
    }
}