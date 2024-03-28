namespace Bars.GkhDi.Calculating
{
    using System;

    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерфейс алгоритма подсчёта процетов
    /// </summary>
    public interface ICalcPercentAlgoritm
    {
        /// <summary>
        /// Принудительная установка процентов для указанного блока
        /// </summary>
        /// <param name="percent">Блок</param>
        /// <param name="selector">Процент</param>
        void ForcePercent(BasePercent percent, Func<BasePercent, decimal?> selector);

        /// <summary>
        /// Принудительная установка коэффициента для блока
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="value">Значение</param>
        void ForceBlockCoef(string code, decimal value);

        /// <summary>
        /// Добавил дочерний элемент к текущему и посчитать его процент
        /// </summary>
        /// <param name="parentPercent">Родительский хранитель процентов</param>
        /// <param name="childPercent">Дочерний хранитель процето</param>
        void AddElement(BasePercent parentPercent, BasePercent childPercent);

        /// <summary>
        /// Посчитать итоговый процент согласно алгоритма
        /// </summary>
        /// <param name="parentPercent">Родительский блок</param>
        void CalcPercent(BasePercent parentPercent);
    }
}