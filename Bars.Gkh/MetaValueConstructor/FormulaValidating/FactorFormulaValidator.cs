namespace Bars.Gkh.MetaValueConstructor.FormulaValidating
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Валидатор формулы фактора
    /// </summary>
    public class FactorFormulaValidator : AbstractEfficiencyRatingFormulaValidator
    {
        /// <summary>
        /// Тип элемента
        /// </summary>
        protected override ElementLevel Level => ElementLevel.Factor;

        /// <summary>
        /// Проверить валидность формулы
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>Результат валидации</returns>
        public override IDataResult Validate(DataMetaInfo metaInfo)
        {
            // у фактора всегда одна и та же формула
            // Value = (Результат коэффициента 1 * Вес коэффициента 1) + (Результат коэффициента 2 * Вес коэффициента 2)+ (Результат коэффициента N * Вес коэффициента N)
            // поэтому в формулу пишем сумму коэффициентов, а когда будем считать, то каждый параметр умножим на его вес перед суммированием
            var childCodes = metaInfo.GetChildrenCodes();

            metaInfo.Required = true;
            metaInfo.Formula = childCodes.Keys.AggregateWithSeparator(" + ");

            if (metaInfo.Children.Any(x => !x.Weight.HasValue))
            {
                return BaseDataResult.Error("Не у всех коэффициентов фактора заполнен вес коэффициента");
            }

            return base.Validate(metaInfo);
        }
    }
}