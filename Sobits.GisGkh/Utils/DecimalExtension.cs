using Bars.B4.Utils;

namespace Sobits.GisGkh.Utils
{
    /// <summary>
    /// Статический класс, содержащий методы работы с decimal
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// Преобразовать число к формату дополненному нулями
        /// </summary>
        public static decimal ToMagic(this decimal value, int round)
        {
            decimal value1 = 0.1m;

            for (int i = 0; i < round - 1; i++)
            {
                value1 *= 0.1m;
            }

            value = value + value1 * 1m - value1 * 1m;

            return value.RoundDecimal(round);
        }
    }
}