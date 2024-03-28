using Bars.B4.Utils;

namespace Bars.GkhGji.Enums
{
    /// <summary>
    /// Основание создания проверки
    /// </summary>
    public enum InspectionCreationBasis
    {
        /// <summary>
        /// Иное основание
        /// </summary>
        [Display("Иное основание")]
        AnotherBasis = 1,

        /// <summary>
        /// Обращение гражданина
        /// </summary>
        [Display("Обращение гражданина")]
        AppealCitizens = 2
    }
}
