namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма источников финансирования 
    /// </summary>
    public enum FormFinanceSource
    {
        [Display("С учетом вида работ")]
        WithTypeWork = 0,

        [Display("Без учета вида работ")]
        WithoutTypeWork = 10
    }
}
