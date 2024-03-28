namespace Bars.Gkh.RegOperator.DomainModelServices
{
    /// <summary>
    /// Фабрика для получения реализаций калькуляторов начисления
    /// </summary>
    public interface IChargeCalculationImplFactory
    {
        /// <summary>
        /// Создать калькулятор всего
        /// </summary>
        IPersonalAccountChargeCaculator CreateCalculator();
    }
}