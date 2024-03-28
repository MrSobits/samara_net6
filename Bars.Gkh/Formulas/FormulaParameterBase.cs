namespace Bars.Gkh.Formulas
{
    using Bars.B4.Utils;

    public abstract class FormulaParameterBase : IFormulaParameter
    {
        #region Implementation of IFormulaParameter

        /// <summary>
        /// Наименование параметра
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Код параметра для поиска
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Получить значение параметра
        /// </summary>
        public abstract object GetValue();

        /// <summary>
        /// Установить провадеры данных
        /// </summary>
        public virtual void SetDataProviders(DynamicDictionary providers) {}

        #endregion
    }
}