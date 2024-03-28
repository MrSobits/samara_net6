namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Метод расчета эталонного начисления
    /// </summary>
    public enum DebtCalcMethod
    {
        /// <summary>
        /// Суд
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// По дате открытия ЛС

        /// </summary>
        [Display("По дате открытия ЛС")]
        OpenDate = 10,

        /// <summary>
        /// Служба судебных приставов
        /// По первому периоду начисления
        /// </summary>
        [Display("По первому периоду начисления")]
        FirstPeriod = 20,

        /// <summary>
        /// Служба судебных приставов
        /// </summary>
        [Display("Указаны фактические начисления")]
        None = 30,
            
        /// <summary>
        /// Указаны фактические начисления
        /// </summary>
        [Display("Указаны фактические начисления")]
        Fact = 31
    }
}