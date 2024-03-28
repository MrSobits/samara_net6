namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип операции по счету
    /// </summary>

    public enum AccountOperationType
    {
        [Display("Приход")]
        Income = 10,

        [Display("Расход")]
        Outcome = 20
    }
}