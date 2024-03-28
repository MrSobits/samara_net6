namespace Bars.Gkh.MetaValueConstructor.FormulaValidating
{
    /// <summary>
    /// Валидатор коэффициента
    /// </summary>
    public class CoefficientFormulaValidator : AbstractEfficiencyRatingFormulaValidator
    {
        /// <summary>
        /// Тип элемента
        /// </summary>
        protected override ElementLevel Level => ElementLevel.Coefficient;
    }
}