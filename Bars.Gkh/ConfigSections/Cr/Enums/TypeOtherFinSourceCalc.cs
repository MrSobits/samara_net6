namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Рассчитывать/Не рассчитывать другие источники финансирования
    /// </summary>
    public enum TypeOtherFinSourceCalc
    {
        [Display("Не рассчитывать")]
        No = 0,

        [Display("Рассчитывать")]
        Yes = 10
    }
}
