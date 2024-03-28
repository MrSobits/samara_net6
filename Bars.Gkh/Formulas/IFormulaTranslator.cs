namespace Bars.Gkh.Formulas
{
    /// <summary>
    /// Переводчик условий формулы
    /// </summary>
    public interface IFormulaTranslator
    {
        /// <summary>
        /// Перевести
        /// </summary>
        /// <param name="formulaText">Текст формулы</param>
        string Translate(string formulaText);
    }
}