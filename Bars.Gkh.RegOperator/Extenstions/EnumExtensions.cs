namespace Bars.Gkh.RegOperator.Extenstions
{
    using System;

    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Расширения для enum
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Привести экземпляр <see cref="RecalcType"/> к <see cref="WalletType"/>
        /// </summary>
        /// <param name="recalcType">Тип перерасчёта</param>
        /// <returns>Тип кошелька</returns>
        public static WalletType ToWalletType(this RecalcType recalcType)
        {
            switch (recalcType)
            {
                case RecalcType.BaseTariffCharge:
                    return WalletType.BaseTariffWallet;

                case RecalcType.DecisionTariffCharge:
                    return WalletType.DecisionTariffWallet;

                case RecalcType.Penalty:
                    return WalletType.PenaltyWallet;

                default:
                    throw new ArgumentOutOfRangeException(nameof(recalcType), recalcType, null);
            }
        }
    }
}