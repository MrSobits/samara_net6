namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид проверки
    /// </summary>
    public enum TypeCheck
    {
        /// <summary>
        /// Плановая выездная
        /// </summary>
        [Display("Плановая выездная")]
        PlannedExit = 1,

        /// <summary>
        /// Внеплановая выездная
        /// </summary>
        [Display("Внеплановая выездная")]
        NotPlannedExit = 2,

        /// <summary>
        /// Плановая документарная
        /// </summary>
        [Display("Плановая документарная")]
        PlannedDocumentation = 3,

        /// <summary>
        /// Внеплановая документарная
        /// </summary>
        [Display("Внеплановая документарная")]
        NotPlannedDocumentation = 4,

        /// <summary>
        /// Инспекционное обследование
        /// </summary>
        [Display("Инспекционное обследование")]
        InspectionSurvey = 5,

        /// <summary>
        /// Мониторинг
        /// </summary>
        [Display("Мониторинг")]
        Monitoring = 6,

        /// <summary>
        /// Плановая документарная и выездная
        /// </summary>
        [Display("Плановая документарная и выездная")]
        PlannedDocumentationExit = 7,

        /// <summary>
        /// Визуальное обследование
        /// </summary>
        [Display("Визуальное обследование")]
        VisualSurvey = 8,

        /// <summary>
        /// Внеплановая документарная и выездная
        /// </summary>
        [Display("Внеплановая документарная и выездная")]
        NotPlannedDocumentationExit = 9,
        
        [Display("Инспекционный визит")]
        InspectVisit = 10,

        [Display("Инструментальное обследование")]
        Instrumental = 11,
        
        /// <summary>
        /// Плановый инспекционный визит
        /// </summary>
        [Display("Плановый инспекционный визит")]
        PlannedInspectionVisit = 12,

        /// <summary>
        /// Внеплановый инспекционный визит
        /// </summary>
        [Display("Внеплановый инспекционный визит")]
        NotPlannedInspectionVisit = 13
    }
}