namespace Bars.Gkh.RegOperator.Enums
{
    /// <summary>
    /// Направление долга
    /// </summary>
    public enum LoanDirection
    {
        /// <summary>
        /// Шаблон долга. Используется для создания болванки в управлении займами
        /// </summary>
        BlankLoan,

        /// <summary>
        /// Дому должны
        /// </summary>
        IncomingLoan,

        /// <summary>
        /// Дом должен
        /// </summary>
        OutgoingLoan
    }
}