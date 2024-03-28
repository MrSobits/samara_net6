namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид проверки
    /// </summary>
    public enum DecisionTypeCheck
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
        /// Плановый инспекционный визит
        /// </summary>
        [Display("Плановый инспекционный визит")]
        PlannedInspectionVisit = 10,

        /// <summary>
        /// Внеплановый инспекционный визит
        /// </summary>
        [Display("Внеплановый инспекционный визит")]
        NotPlannedInspectionVisit = 11
    }
}