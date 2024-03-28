namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус оплаты графика оплат реструктуризации по мировому соглашению
    /// </summary>
    public enum RestructAmicAgrPaymentState
    {
        /// <summary>
        /// Оплаты по графику
        /// </summary>
        [Display("Оплаты по графику")]
        OnSchedule = 0,

        /// <summary>
        /// Просрочен
        /// </summary>
        [Display("Просрочен")]
        Overdue = 10
    }
}