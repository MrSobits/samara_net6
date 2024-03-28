namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип формирования расчёта потребности
    /// </summary>
    public enum LoanFormationType
    {
        /// <summary>
        /// На основании актов выполненных работ
        /// </summary>
        [Display("На основании актов выполненных работ")]
        ByPerformedWorkAct = 0x1,

        /// <summary>
        /// На основании заявки на перечисление средств подрядчикам
        /// </summary>
        [Display("На основании заявки на перечисление средств подрядчикам")]
        ByTransferCtr= 0x2,
    }
}