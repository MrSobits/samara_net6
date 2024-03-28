namespace Bars.Gkh.Repair.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип состояния программы текущего ремонта
    /// </summary>
    public enum TypeProgramRepairState
    {
        [Display("Новая")]
        New = 10,

        [Display("Активна")]
        Active = 20,

        [Display("Закрыта")]
        Close = 30
    }
}
