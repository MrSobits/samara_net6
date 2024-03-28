namespace Bars.Gkh.Formulas
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с формулами
    /// </summary>
    public interface IFormulaService
    {
        /// <summary>
        /// Получить список параметров формулы
        /// </summary>
        IDataResult GetParamsList(string formula);

        /// <summary>
        /// Проверить корректность формулы
        /// </summary>
        IDataResult CheckFormula(string formula);
    }
}