namespace Bars.Gkh.RegOperator.Dto
{
    using Enums;

    /// <summary>
    /// Отмененные начисления 
    /// </summary>
    public class CancelChargeDto
    {
        /// <summary>
        /// Операция отмены начислений
        /// </summary>
        public long ChargeOperationId { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// Период, за который отменяем начисления
        /// </summary>
        public long CancelPeriodId { get; set; }

        /// <summary>
        /// Тип отмененного начисления (по базовому, по тарифу решений, пени)
        /// </summary>
        public CancelType CancelType { get; set; }

        /// <summary>
        /// Сумма отмененного начисления
        /// </summary>
        public decimal CancelSum { get; set; }
    }
}
