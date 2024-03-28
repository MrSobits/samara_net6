namespace Bars.Gkh.Formulas
{
    /// <summary>
    /// Мета, для хранения информации о параметре формулы
    /// </summary>
    public class FormulaParameterMeta
    {
        /// <summary>
        /// Наименование параметра (a, b, c, x)
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Код параметра <see cref="IFormulaParameter.Code"/>
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Понятное имя параметра <see cref="IFormulaParameter.DisplayName"/>
        /// </summary>
        public string DisplayName { get; set; }
    }
}