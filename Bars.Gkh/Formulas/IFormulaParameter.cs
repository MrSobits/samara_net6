namespace Bars.Gkh.Formulas
{
    using Bars.B4.Utils;

    /// <summary>
    /// Интерфейс для получения данных для параметра формулы
    /// </summary>
    public interface IFormulaParameter
    {
        /// <summary>
        /// Наименование параметра
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Код параметра для поиска
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Получить значение параметра
        /// </summary>
        object GetValue();

        /// <summary>
        /// Установить провадеры данных
        /// </summary>
        void SetDataProviders(DynamicDictionary providers);
    }
}