namespace Bars.GkhDi.Calculating
{
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Методы расширения для калькулятора
    /// </summary>
    public static class CalcPercentAlgoritmExtensions
    {
        /// <summary>
        /// Добавить неизменяемый процент
        /// </summary>
        /// <typeparam name="T">Тип процентоного блока</typeparam>
        /// <param name="element">Объект</param>
        /// <param name="algoritm">Интерфейс алгоритма</param>
        /// <returns>Исходный объект</returns>
        public static T AddForcePercent<T>(this T element, ICalcPercentAlgoritm algoritm) where T : BasePercent
        {
            algoritm.ForcePercent(element, x => x.Percent);
            return element;
        }

        /// <summary>
        /// Добавить неизменяемый процент
        /// </summary>
        /// <typeparam name="T">Тип процентоного блока</typeparam>
        /// <param name="element">Объект</param>
        /// <param name="condition">Условие включения</param>
        /// <param name="algoritm">Интерфейс алгоритма</param>
        /// <returns>Исходный объект</returns>
        public static T AddForcePercentIf<T>(this T element, bool condition, ICalcPercentAlgoritm algoritm) where T : BasePercent
        {
            if (condition)
            {
                return element.AddForcePercent(algoritm);
            }

            return element;
        }
    }
}