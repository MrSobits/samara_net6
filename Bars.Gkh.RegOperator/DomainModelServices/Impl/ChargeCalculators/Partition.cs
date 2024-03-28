namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators.Impl;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Причина разделения периода
    /// </summary>
    public class Partition
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public Partition()
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="partitionType">Тип</param>
        /// <param name="debtChange">Сумма изменения задолженности</param>
        /// <param name="restructSum">Сумма, для погашения реструктуризации</param>
        public Partition(PeriodPartitionType partitionType, decimal debtChange, decimal restructSum = 0)
        {
            // берем с противоположным знаком, т.к. приходит изменение задолженности
            this.Amount = -debtChange;
            this.PartitionType = partitionType;
            this.RestructSum = restructSum;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="partitionType">Тип</param>
        /// <param name="dayChange">Параметры смены количества дней допустимой просрочки для протокола</param>
        public Partition(PeriodPartitionType partitionType, PeriodDaysChange dayChange)
        {
            this.PeriodDaysChange = dayChange;
            this.PartitionType = partitionType;
        }

        /// <summary>
        /// Сумма оплаты/возврата
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public PeriodPartitionType PartitionType { get; set; }

        /// <summary>
        /// Параметры смены количества дней допустимой просрочки для протокола
        /// </summary>
        public PeriodDaysChange PeriodDaysChange { get; set; }

        /// <summary>
        /// Сумма, для погашения реструктуризации
        /// </summary>
        public decimal RestructSum { get; set; }
    }
}

