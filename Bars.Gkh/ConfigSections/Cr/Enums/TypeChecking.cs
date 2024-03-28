namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Проверка объема с объемом в паспорте  дома
    /// </summary>
    public enum TypeChecking
    {
        [Display("Не проверять")]
        NoCheck = 0,

        [Display("Проверять")]
        Check = 10
    }
}
