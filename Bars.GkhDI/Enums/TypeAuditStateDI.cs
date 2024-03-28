namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип состояния аудиторской проверки
    /// </summary>
    public enum TypeAuditStateDi
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Проведена")]
        Inspect = 20,

        [Display("Не проведена")]
        NotInspect = 30
    }
}
