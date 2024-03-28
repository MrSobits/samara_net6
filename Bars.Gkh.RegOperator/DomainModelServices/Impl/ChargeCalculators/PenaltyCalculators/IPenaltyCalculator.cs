namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Интерфейс калькулятора пени
    /// </summary>
    public interface IPenaltyCalculator
    {
        /// <summary>
        /// Расчитать
        /// </summary>
        /// <returns></returns>
        CalculationResult<PenaltyResult> Calculate();

        /// <summary>
        /// Получить кэш для ЛС
        /// </summary>
        /// <param name="account"></param>
        /// <param name="period"></param>
        /// <param name="recalcHistory"></param>
        /// <returns></returns>
        IPenaltyCalculator Init(BasePersonalAccount account, IPeriod period, List<RecalcHistory> recalcHistory);
    }
}