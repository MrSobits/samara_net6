namespace Bars.Gkh.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    ///  Период начислений
    /// </summary>
    [Serializable]
    public class ChargePeriod : BaseImportableEntity, IEquatable<ChargePeriod>, IPeriod
    {
        public ChargePeriod()
        {
            this.IsClosed = false;
        }

        public ChargePeriod(string name, DateTime startDate, DateTime? endDate = null) : this()
        {
            this.Name = name;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        /// <summary>
        /// Наименование периода
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата открытия периода
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата закрытия периода
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Флаг: период закрыт
        /// </summary>
        public virtual bool IsClosed { get; set; }

        /// <summary>
        /// Признак, что период закрывается
        /// </summary>
        public virtual bool IsClosing { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(ChargePeriod other)
        {
            if (other == null)
                return false;

            if (this.Id == 0 && other.Id == 0)
                return this.StartDate == other.StartDate
                       && this.EndDate == other.EndDate;

            return this.Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            return this.Equals(obj as ChargePeriod);
        }

        public override int GetHashCode()
        {
            return this.Id > 0 ? this.Id.GetHashCode() : this.StartDate.GetHashCode();
        }

        public virtual bool Contains(DateTime dateTime)
        {
            return this.StartDate <= dateTime && (!this.EndDate.HasValue || dateTime <= this.EndDate);
        }
    }

    public interface IPeriod
    {
        long Id { get; }

        /// <summary>
        /// Наименование периода
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Дата открытия периода
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// Дата закрытия периода
        /// </summary>
        DateTime? EndDate { get; set; }

        /// <summary>
        /// Флаг: период закрыт
        /// </summary>
        bool IsClosed { get; set; }
    }

    public static class PeriodExtensions
    {
        public static bool Contains(this IPeriod period, DateTime date)
        {
            return period.StartDate <= date
                   && (period.EndDate.HasValue
                       ? period.EndDate.Value
                       : period.StartDate.AddMonths(1).AddDays(-1)) >= date;
        }
    }
}