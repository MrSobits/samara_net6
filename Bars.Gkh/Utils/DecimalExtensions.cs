namespace Bars.Gkh.Utils
{
    using System;

    public static class DecimalExtensions
    {
        /// <summary>
        /// Округлить число до указанного кол-ва знаков в большую сторону
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="decimals">Кол-во знаков после запятой</param>
        /// <returns>
        /// Округленное число
        /// </returns>
        public static Decimal RegopRoundDecimal(this Decimal value, int decimals)
        {
            return Decimal.Round(value, decimals, MidpointRounding.AwayFromZero);
        }
    }
}
