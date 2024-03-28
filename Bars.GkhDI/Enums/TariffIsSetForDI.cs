namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тариф установлен для
    /// </summary>
    public enum TariffIsSetForDi
    {
        [Display("Для собственников жилых и нежилых помещений")]
        ForOwnersResidentialAndNonResidentialPremises = 10,

        [Display("Для собственников жилых помещений")]
        ForOwnersResidentialPremises = 20,

        [Display("Для собственников нежилых помещений (арендаторов)")]
        ForOwnersNonResidentialPremises = 30
    }
}
