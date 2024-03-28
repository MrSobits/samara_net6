namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum ComputingProcessStatus
    {
        [Display("Запущен")]
        Started = 10,
        [Display("Успешно выполнен")]
        Succeed = 20,
        [Display("Завершен с ошибками")]
        Failed = 30
    }
}
