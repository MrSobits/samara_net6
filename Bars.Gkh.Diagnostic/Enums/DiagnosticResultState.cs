namespace Bars.Gkh.Diagnostic.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус диагностики
    /// </summary>
    public enum DiagnosticResultState
    {
        [Display("Ожидает запуска")]
        Waiting = 10,

        [Display("В работе")]
        Working = 20,

        [Display("Успешно")]
        Success = 30,

        [Display("Завершилась с ошибкой")]
        Fail = 40
    }
}
