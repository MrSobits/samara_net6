namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус для Счета невыясненных сумм
    /// </summary>
    public enum SubsidyIncomeStatus
    {
        [Display("Подтвержден")]
        Distributed = 10,

        [Display("Не подтвержден")]
        NotDistributed = 20,

        [Display("Удален")]
        Deleted = 40,

        [Display("Частично подтвержден")]
        PartiallyDistributed = 50
    }
}