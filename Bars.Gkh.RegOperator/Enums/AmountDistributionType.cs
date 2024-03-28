namespace Bars.Gkh.RegOperator.Enums
{
    using System;

    /// <summary>
    /// Тип распределения суммы оплаты
    /// </summary>
    [Flags]
    public enum AmountDistributionType
    {
        /// <summary>
        /// Распределять на задолженность по базовому тарифу и сверх базового
        /// </summary>
        Tariff = 0x1,

        /// <summary>
        /// Распределять на задолженность по пени
        /// </summary>
        Penalty = 0x2,

        /// <summary>
        /// Распределять на задолженности по тарифам и по пени
        /// </summary>
        TariffAndPenalty = Tariff | Penalty
    }
}