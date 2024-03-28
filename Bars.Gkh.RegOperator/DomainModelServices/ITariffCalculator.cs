namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    using Domain.ProxyEntity;
    using Entities;

    /// <summary>
    /// Интерфейс для расчета значения тарифа
    /// </summary>
    public interface ITariffCalculator
    {
        CalculationResult<Dictionary<DateTime, TariffWithOverplus>> Calculate(BasePersonalAccount account,
            IPeriod period,
            UnacceptedCharge unAccepted);
    }
}