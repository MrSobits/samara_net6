namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип эксперта
    /// </summary>
    public enum ExpertType
    {
        /// <summary>
        /// Эксперт
        /// </summary>
        [Display("Эксперт")]
        Expert = 1,

        /// <summary>
        /// Экспертная организация
        /// </summary>
        [Display("Экспертная организация")]
        ExpertOrganization = 2,

        /// <summary>
        /// Специалист
        /// </summary>
        [Display("Специалист")]
        Specialist = 3,

        /// <summary>
        /// Независимый орган инспекции
        /// </summary>
        [Display("Независимый орган инспекции")]
        IndependentInspection = 4,

        /// <summary>
        /// СРО
        /// </summary>
        [Display("СРО")]
        Sro = 5,

        /// <summary>
        /// Иные лица
        /// </summary>
        [Display("Иные лица")]
        OtherPersons = 6
    }
}