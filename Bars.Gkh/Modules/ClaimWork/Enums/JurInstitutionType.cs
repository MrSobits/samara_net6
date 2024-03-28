namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип учреждения в судебной практике
    /// </summary>
    public enum JurInstitutionType
    {
        /// <summary>
        /// Суд
        /// </summary>
        [Display("Суд")]
        Court = 10,

        /// <summary>
        /// Служба судебных приставов
        /// </summary>
        [Display("Служба судебных приставов")]
        Bailiffs = 20
    }
}