using Bars.B4.Utils;

namespace Bars.Gkh.RegOperator.Enums
{
    public enum IdentityType
    {
        /// <summary>
        /// Паспорт
        /// </summary>
        [Display("Паспорт")]
        Passport = 10,

        /// <summary>
        /// Свидетельство о рождении
        /// </summary>
        [Display("Свидетельство о рождении")]
        BirthCertificate = 20,

        /// <summary>
        /// СНИЛС
        /// </summary>
        [Display("СНИЛС")]
        InsuranceNumber = 30
    }
}
