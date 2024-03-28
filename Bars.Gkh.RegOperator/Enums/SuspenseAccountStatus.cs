namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус для Счета невыясненных сумм
    /// </summary>
    public enum SuspenseAccountStatus
    {
        [Display("Распределен")]
        Distributed = 10,

        [Display("Не распределен")]
        NotDistributed = 20,

        [Display("Отменен")]
        Canceled = 30
    }
}