namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Сущность связи вида банковского распределения и распределения
    /// </summary>
    public class DistributionOperation : BaseGkhEntity
    {
        /// <summary>
        /// Банковское распределение
        /// </summary>
        public virtual BankAccountStatement BankAccountStatement { get; set; }

        /// <summary>
        /// Код распределения
        /// </summary>
        public virtual DistributionCode Code { get; set; }

        /// <summary>
        /// Операция, в рамках которой выполняется распределение (движение денег)
        /// </summary>
        public virtual MoneyOperation Operation { get; set; }
    }
}
