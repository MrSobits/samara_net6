namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип проведения корректировки
    /// </summary>
    public enum TypeCorrection
    {
        [Display("Корректировка не выполнена")]
        NotDone = 10,

        [Display("Корректировка выполнена")]
        Done = 20
    }
}