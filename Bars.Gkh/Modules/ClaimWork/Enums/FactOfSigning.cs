namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Факт подписания
    /// </summary>
    public enum FactOfSigning
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Подписан всеми сторонами")]
        Signed = 10,

        [Display("Отказ от подписи одной стороной")]
        RefusalOneSide = 20,

        [Display("Отказ от подписи всеми сторонами")]
        RefusalAllSide = 30
    }
}