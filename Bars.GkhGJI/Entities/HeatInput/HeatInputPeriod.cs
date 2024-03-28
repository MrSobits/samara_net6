namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Период подачи тепла
    /// </summary>
    public class HeatInputPeriod : BaseEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>а
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual byte Month { get; set; }
    }
}