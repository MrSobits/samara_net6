namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Решение судебного участка
    /// </summary>
    public enum ZVSPCourtDecision
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Вынесен судебный приказ
        /// </summary>
        [Display("Вынесен судебный приказ")]
        CourtOrder = 10,

        /// <summary>
        /// Определение о возврате ЗВСП
        /// </summary>
        [Display("Определение о возврате ЗВСП")]
        ReturnВefinition = 20,

        /// <summary>
        /// Определение об отказе в принятии ЗВСП
        /// </summary>
        [Display("Определение об отказе в принятии ЗВСП")]
        Denied = 30
    }
}