namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние проверки перед закрытием месяца
    /// </summary>
    public enum PeriodCloseCheckStateType
    {
        [Display("Ожидание")]
        Pending = 0,

        [Display("Выполняется")]
        Running = 10,
        
        [Display("Успешно")]
        Success = 20,

        [Display("Ошибка")]
        Error = 30,

        [Display("Внутренняя ошибка")]
        Exception = 40
    }
}