namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Закрытие задолженности
    /// </summary>
    public enum DebtCalc
    {
        /// <summary>
        /// Начиная с самого раннего начисления
        /// </summary>
        [Display("Начиная с самого раннего начисления")]
        Early = 10,

        /// <summary>
        /// Начиная с самого позднего начисления
        /// </summary>
        [Display("Начиная с самого позднего начисления")]
        Late = 20
    }
}