namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип действия для истории
    /// </summary>
    public enum TypeWorkCrHistoryAction
    {
        [Display("Добавление")]
        Creation = 10,

        [Display("Изменение")]
        Modification = 20,

        [Display("Удаление")]
        Removal = 30
    }
}
