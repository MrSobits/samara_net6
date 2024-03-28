namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

	/// <summary>
	/// Фильтр Наличие начислений в карточке ЛС
	/// </summary>
    public enum AccountFilterHasCharges
	{
		/// <summary>
		/// Начисление по базовому тарифу > 0
		/// </summary>
		[Display("Начисление по базовому тарифу > 0")]
		BaseTariffCharge = 0,

		/// <summary>
		/// Начисление по тарифу решения > 0
		/// </summary>
		[Display("Начисление по тарифу решения > 0")]
		DecisionTariffCharge = 10,

		/// <summary>
		/// Начисление пени > 0
		/// </summary>
		[Display("Начисление пени > 0")]
		PenaltyCharge = 20,

		/// <summary>
		/// Начисление пени = 0
		/// </summary>
		[Display("Начисление пени = 0")]
		PenaltyZeroCharge = 30,

        /// <summary>
        /// Существует переплата (общая)
        /// </summary>
        [Display("Существует переплата (общая)")]
		Overpayment = 40,

		/// <summary>
		/// Начисление по базовому тарифу = 0
		/// </summary>
		[Display("Начисление по базовому тарифу = 0")]
		BaseTariffZeroCharge = 50,

        /// <summary>
        /// Переплата по базовому тарифу
        /// </summary>
        [Display("Переплата по базовому тарифу")]
        BaseTariffOverpayment = 60,

        /// <summary>
        /// Переплата по тарифу решения
        /// </summary>
        [Display("Переплата по тарифу решения")]
        DecisionTariffOverpayment = 70,

        /// <summary>
        /// Переплата по пени
        /// </summary>
        [Display("Переплата по пени")]
        PenaltyOverpayment = 80,

        /// <summary>
        /// Долг по базовому тарифу
        /// </summary>
        [Display("Долг по базовому тарифу")]
        BaseTariffDebt = 90,

        /// <summary>
        /// Долг по тарифу решения
        /// </summary>
        [Display("Долг по тарифу решения")]
        DecisionDebt = 100,

        /// <summary>
        /// Долг по пени
        /// </summary>
        [Display("Долг по пени")]
        PenaltyDebt = 110
    }
}
