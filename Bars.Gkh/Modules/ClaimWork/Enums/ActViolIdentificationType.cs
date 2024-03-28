namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип акта выявления нарушений
    /// </summary>
    public enum ActViolIdentificationType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Двусторонний")]
        Bilateral = 10,

        [Display("Односторонний")]
        Unilateral = 20
    }
}