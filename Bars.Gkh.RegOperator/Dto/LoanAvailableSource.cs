namespace Bars.Gkh.RegOperator.Dto
{
    using Enums;

    /// <summary>
    /// Доступный источник для займа
    /// </summary>
    public class LoanAvailableSource
    {
        /// <summary>
        /// Тип источника
        /// </summary>
        public TypeSourceLoan TypeSource { get; set; }

        /// <summary>
        /// Название источника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Доступные средства
        /// </summary>
        public decimal AvailableMoney { get; set; }
    }
}