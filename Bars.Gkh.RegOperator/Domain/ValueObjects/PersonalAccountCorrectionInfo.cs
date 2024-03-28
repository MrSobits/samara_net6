namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Данные по корректировке оплат
    /// </summary>
    public class PersonalAccountCorrectionInfo
    {
        /// <summary>
        /// Тип кошелька
        /// </summary>
        public WalletType PaymentType { get; set; }

        /// <summary>
        /// Сумма снятия
        /// </summary>
        public decimal TakeAmount { get; set; }

        /// <summary>
        /// Сумма зачисления
        /// </summary>
        public decimal EnrollAmount { get; set; }

        /// <summary>
        /// Сумма, на которую изменится баланс кошелька
        /// </summary>
        public decimal Amount => this.EnrollAmount - this.TakeAmount;
    }
}