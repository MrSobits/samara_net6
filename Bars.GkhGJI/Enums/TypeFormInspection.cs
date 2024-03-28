namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма проверки ЮЛ
    /// </summary>
    public enum TypeFormInspection
    {
        /// <summary>
        /// Выездная
        /// </summary>
        [Display("Выездная")]
        Exit = 10,

        /// <summary>
        /// Документарная
        /// </summary>
        [Display("Документарная")]
        Documentary = 20,

        /// <summary>
        /// Выездная и документарная
        /// </summary>
        [Display("Выездная и документарная")]
        ExitAndDocumentary = 30,
        
        /// <summary>
        /// Визуальное обследование
        /// </summary>
        [Display("Визуальное обследование")]
        Visual = 40,
        
        /// <summary>
        /// Инспекционный визит
        /// </summary>
        [Display("Инспекционный визит")]
        InspectionVisit = 50
    }
}