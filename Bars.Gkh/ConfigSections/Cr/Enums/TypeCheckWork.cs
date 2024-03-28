namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип проверки видов работ
    /// </summary>
    public enum TypeCheckWork
    {
        [Display("На основе дефектной ведомости")]
        WithDefectList = 0,

        [Display("Без учета дефектной ведомости")]
        WithoutDefectList = 10
    }
}
