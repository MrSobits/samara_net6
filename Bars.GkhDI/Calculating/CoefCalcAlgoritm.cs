namespace Bars.GkhDi.Calculating
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhDi.Calculating.Configs;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Подсчёт процентов на основе коэффициентов
    /// </summary>
    public class CoefCalcAlgoritm : AbstractCalcPercentAlgoritm
    {
        private readonly BlockPercentConfigs configs;
        private readonly Func<BasePercent, ManorgType> manorgSelector;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="configs">Параметры коэффициентов</param>
        /// <param name="manorgSelector">Функция-селектор типа УО</param>
        public CoefCalcAlgoritm(BlockPercentConfigs configs, Func<BasePercent, ManorgType> manorgSelector)
        {
            this.configs = configs;
            this.manorgSelector = manorgSelector;
        }

        /// <inheritdoc />
        protected override decimal CalcPercentInternal(BasePercent parentPercent, IList<BasePercent> childElements)
        {
            var childCodes = childElements.Select(x => this.configs.GetPercent(this.GetType(x), x.Code)).SafeSum();
            if (childCodes != 1)
            {
                throw new ValidationException("Сумма коэффициентов по дочерним элементам не равно 1");
            }

            return childElements.AsParallel()
                .Select(x => x.Percent * this.configs.GetPercent(this.GetType(x), x.Code) ?? 0)
                .Sum();
        }

        /// <summary>
        /// Принудительная установка коэффициента для блока
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="value">Значение</param>
        public override void ForceBlockCoef(string code, decimal value)
        {
            this.configs.SetPercent(code, value);
        }

        private ManorgType GetType(BasePercent percent)
        {
            return this.manorgSelector.Invoke(percent);
        }
    }
}