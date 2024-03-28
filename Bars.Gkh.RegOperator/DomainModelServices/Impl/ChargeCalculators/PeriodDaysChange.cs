namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    /// <summary>
    /// Параметры смены количества дней допустимой просрочки для протокола
    /// </summary>
    public class PeriodDaysChange
    {
        /// <summary>
        /// Старое значение
        /// </summary>
        public int OldDays { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public int NewDays { get; set; }
    }
}