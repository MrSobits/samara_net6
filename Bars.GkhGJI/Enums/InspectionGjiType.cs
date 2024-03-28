namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип проверки
    /// </summary>
    public enum InspectionGjiType
    {
        /// <summary>
        /// Соблюдение обязательных требований
        /// </summary>
        [Display("Соблюдение обязательных требований")]
        ComplianceWithRequirements = 10,

        /// <summary>
        /// Соответствие сведений
        /// </summary>
        [Display("Соответствие сведений")]
        MatchInformation = 20
    }
}