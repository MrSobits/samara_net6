namespace Bars.Gkh.Overhaul.Utils
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Возвращает 0, если значение меньше 0
        /// </summary>
        public static decimal ZeroIfBelowZero(this decimal value)
        {
            if (value < 0)
            {
                return 0M;
            }

            return value;
        }

        /// <summary>
        /// Перевод в проценты / 100
        /// </summary>
        public static decimal ToDivisional(this decimal val)
        {
            if (val > 1)
            {
                return val / 100;
            }

            return val;
        }
    }
}