namespace Bars.GkhDi.Calculating
{
    using System;
    using System.Collections.Generic;
    
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Абстрактный алгоритм подсчёта процентов
    /// </summary>
    public abstract class AbstractCalcPercentAlgoritm : ICalcPercentAlgoritm
    {
        protected readonly IDictionary<BasePercent, IList<BasePercent>> PercentDict;
        protected readonly IDictionary<BasePercent, Func<BasePercent, decimal?>> ForcePercentDict = new Dictionary<BasePercent, Func<BasePercent, decimal?>>();

        /// <summary>
        /// .ctor
        /// </summary>
        protected AbstractCalcPercentAlgoritm()
        {
            this.PercentDict = new Dictionary<BasePercent, IList<BasePercent>>();
        }

        /// <inheritdoc />
        public void AddElement(BasePercent parentPercent, BasePercent childPercent)
        {
            IList<BasePercent> blockPercents;
            if (!this.PercentDict.TryGetValue(parentPercent, out blockPercents))
            {
                blockPercents = new List<BasePercent>();
                this.PercentDict[parentPercent] = blockPercents;
            }

            blockPercents.Add(childPercent);
            this.CalcPercent(childPercent);
        }

        /// <inheritdoc />
        public void CalcPercent(BasePercent percent)
        {
            if (this.ForcePercentDict.ContainsKey(percent))
            {
                percent.Percent = this.ForcePercentDict[percent].Invoke(percent);
            }
            else if (this.PercentDict.ContainsKey(percent))
            {
                var children = this.PercentDict[percent];
                this.EnsurePositionCount(percent, children);

                percent.Percent = this.CalcPercentInternal(percent, children).RoundDecimal(2);
            }
            else
            {
                percent.Percent = this.CalcLeafPercentInternal(percent)?.RoundDecimal(2);
            }
        }

        /// <inheritdoc />
        public void ForcePercent(BasePercent percent, Func<BasePercent, decimal?> selector)
        {
            this.ForcePercentDict[percent] = selector;
        }

        /// <summary>
        /// Принудительная установка коэффициента для блока
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="value">Значение</param>
        public virtual void ForceBlockCoef(string code, decimal value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Пересчитать количество заполненных полей
        /// </summary>
        /// <param name="parentPercent">Родительский элемент</param>
        /// <param name="childElements">Его потомки</param>
        protected virtual void EnsurePositionCount(BasePercent parentPercent, IList<BasePercent> childElements)
        {
            parentPercent.PositionsCount = 0;
            parentPercent.CompletePositionsCount = 0;

            foreach (var childElement in childElements)
            {
                parentPercent.PositionsCount += childElement.PositionsCount;
                parentPercent.CompletePositionsCount += childElement.CompletePositionsCount;
            }
        }

        /// <summary>
        /// Посчитать проценты согласно алгоритма для элемента, не имеющего потомков
        /// </summary>
        /// <param name="basePercent">Блок, для которого производится расчёт</param>
        /// <returns>Сумма процентов</returns>
        protected virtual decimal? CalcLeafPercentInternal(BasePercent basePercent)
        {
            if (basePercent.PositionsCount != 0)
            {
                return decimal.Divide(basePercent.CompletePositionsCount, basePercent.PositionsCount) * 100;
            }

            return basePercent.Percent;
        }

        /// <summary>
        /// Посчитать проценты согласно алгоритма для элемента, имеющего потомков
        /// </summary>
        /// <param name="parentPercent">Блок, для которого производится расчёт</param>
        /// <param name="childElements">Список потомков</param>
        /// <returns>Сумма процентов</returns>
        protected abstract decimal CalcPercentInternal(BasePercent parentPercent, IList<BasePercent> childElements);
    }
}