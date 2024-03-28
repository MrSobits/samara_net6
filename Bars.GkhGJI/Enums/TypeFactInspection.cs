namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Факт проверки ЮЛ
    /// </summary>
    public enum TypeFactInspection
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Проведена")]
        Done = 20,

        [Display("Не проведена")]
        NotDone = 30,

        [Display("Отменена")]
        Cancelled = 40,

        [Display("Перенесена")]
        Changed = 50
    }
}