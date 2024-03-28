namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using Bars.Gkh.Repositories.ChargePeriod;

    using ChargeCalculators;
    using Domain.Interfaces;
    using Domain.ParametersVersioning;
    using Domain.Repository;

    /// <summary>
    /// Фабрика для получения реализаций калькуляторов начисления
    /// </summary>
    public class ChargeCalculationImplFactory : IChargeCalculationImplFactory
    {
        private readonly IParameterTracker paramTracker;
        private readonly ICalculationGlobalCache globalCache;
        private readonly IChargePeriodRepository chargePeriodRepo;

        public ChargeCalculationImplFactory(
            IParameterTracker paramTracker,
            ICalculationGlobalCache globalCache,
            IChargePeriodRepository periodRepo)
        {
            this.paramTracker = paramTracker;
            this.globalCache = globalCache;
            this.chargePeriodRepo = periodRepo;
        }

        public IPersonalAccountChargeCaculator CreateCalculator()
        {
            return new PersonalAccountChargeCalculator(this.paramTracker, this.globalCache, this.chargePeriodRepo);
        }
    }
}