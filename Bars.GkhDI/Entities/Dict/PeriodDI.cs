namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Период раскрытия информации
    /// </summary>
    public class PeriodDi : BaseGkhEntity
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Не учитывать дома, введенные в эксплуатацию позже следующей даты 
        /// (Процент не будет расчитываться если дата ввода в эксплуатацию дома больше, чем данная дата)
        /// </summary>
        public virtual DateTime? DateAccounting { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
