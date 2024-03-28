namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип архива значений в мониторинге СМР
    /// </summary>
    public enum TypeArchiveSmr
    {
        [Display("Ход выполнения работ")]
        ProgressExecutionWork = 10,

        [Display("Численность рабочих")]
        WorkersCount = 20
    }
}
