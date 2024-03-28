namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Период сведений о наличии и расходе топлива
    /// </summary>
    public class FuelInfoPeriod : BaseEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual byte Month { get; set; }
    }
}