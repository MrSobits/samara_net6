namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тим файла импорта
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Неизвестный
        /// </summary>
        [Display("Неизвестный")]
        Unknown = -1,

        /// <summary>
        /// Лицевые счета
        /// </summary>
        [Display("Лицевые счета")]
        Account,

        /// <summary>
        /// Начисления
        /// </summary>
        [Display("Начисления")]
        Calc,

        /// <summary>
        /// Протоколы расчета начислений
        /// </summary>
        [Display("Протоколы расчета начислений")]
        CalcProt,

        /// <summary>
        /// Изменения сальдо
        /// </summary>
        [Display("Изменения сальдо")]
        SaldoChange,

        /// <summary>
        /// Перерасчеты начислений
        /// </summary>
        [Display("Перерасчеты начислений")]
        Recalc,

        /// <summary>
        /// Оплаты
        /// </summary>
        [Display("Оплаты")]
        Pay
    }
}