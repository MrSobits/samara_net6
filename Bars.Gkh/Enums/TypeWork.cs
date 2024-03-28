namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип работ
    /// </summary>
    public enum TypeWork
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Работа")]
        Work = 10,

        [Display("Услуга")]
        Service = 20
    }
}
