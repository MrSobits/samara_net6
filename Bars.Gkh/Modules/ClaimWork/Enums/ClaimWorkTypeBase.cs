namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания претензионно-исковой работы
    /// </summary>
    public enum ClaimWorkTypeBase
    {
        /// <summary>
        /// Работа с неплательщиками
        /// </summary>
        [Display("Работа с неплательщиками")]
        Debtor = 10,

        /// <summary>
        /// Работа с подрядчиками, нарушившими условия договора подряда
        /// </summary>
        [Display("Работа с подрядчиками, нарушившими условия договора подряда")]
        BuildContract = 20,

        /// <summary>
        /// Работа с неплательщиками ЖКУ
        /// </summary>
        [Display("Работа с неплательщиками ЖКУ")]
        UtilityDebtor = 30
    }
}