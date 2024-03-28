namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Проверки в отношении
    /// </summary>
    public enum PersonInspection
    {
        [Display("Физическое лицо")]
        PhysPerson = 10,

        [Display("Организация")]
        Organization = 20,

        [Display("Должностное лицо")]
        Official = 30,

        [Display("Жилой дом")]
        RealityObject = 40
    }
}