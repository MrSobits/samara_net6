namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;

    using Entities;

    [Serializable]
    public class PeriodDto : IPeriod
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PeriodDto()
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period"></param>
        public PeriodDto(ChargePeriod period)
        {
            ArgumentChecker.NotNull(period, "period");

            Id = period.Id;
            Name = period.Name;
            StartDate = period.StartDate;
            EndDate = period.EndDate;
            IsClosed = period.IsClosed;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование периода
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата открытия периода
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата закрытия периода
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Флаг: период закрыт
        /// </summary>
        public bool IsClosed { get; set; }
    }
}