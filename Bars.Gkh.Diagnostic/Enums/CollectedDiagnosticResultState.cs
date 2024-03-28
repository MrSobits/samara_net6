namespace Bars.Gkh.Diagnostic.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус дигностики системы
    /// </summary>
    public enum CollectedDiagnosticResultState
    {
        [Display("В работе")]
        Working = 10,

        [Display("Успешно")]
        Success = 20,

        [Display("Завершилась с ошибкой")]
        Fail = 30
    }
}
