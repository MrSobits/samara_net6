namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид действия
    /// </summary>
    public enum ActCheckActionType
    {
        /// <summary>
        /// Осмотр
        /// </summary>
        [Display("Осмотр")]
        Inspection = 1,

        /// <summary>
        /// Опрос
        /// </summary>
        [Display("Опрос")]
        Survey = 2,

        /// <summary>
        /// Инструментальное обследование
        /// </summary>
        [Display("Инструментальное обследование")]
        InstrumentalExamination = 3,

        /// <summary>
        /// Истребование документов
        /// </summary>
        [Display("Истребование документов")]
        RequestingDocuments = 4,

        /// <summary>
        /// Получение письменных объяснений
        /// </summary>
        [Display("Получение письменных объяснений")]
        GettingWrittenExplanations = 5,
        
        /// <summary>
        /// Сбор и анализ данных об объектах контроля
        /// </summary>
        [Display("Сбор и анализ данных об объектах контроля")]
        CollectAnalyzeDataOfControlObjects = 6,
        
        /// <summary>
        /// Инструментальное обследование с применением видеозаписи (при необходимости)
        /// </summary>
        [Display("Инструментальное обследование с применением видеозаписи (при необходимости)")]
        InstrumentalExaminationWithVideoRecording = 7
    }
}