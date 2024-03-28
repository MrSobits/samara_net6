namespace Bars.Gkh.RegOperator.Dto
{
    using Enums;

    /// <summary>
    /// Сумма денег, которые нужно взять по типу источника займа
    /// </summary>
    public class LoanTakenMoney
    {
        /// <summary>
        /// Тип источника
        /// </summary>
        public TypeSourceLoan TypeSource { get; set; }

        /// <summary>
        /// Взятые деньги
        /// </summary>
        public decimal TakenMoney { get; set; }
    }
}