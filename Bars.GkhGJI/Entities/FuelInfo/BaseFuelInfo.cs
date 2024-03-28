namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Базовый класс сведений наличии и расходе топлива
    /// </summary>
    public class BaseFuelInfo : BaseEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        protected BaseFuelInfo()
        {
        }

        /// <summary>
        /// Период сведенияй о наличии и расходе топлива
        /// </summary>
        public virtual FuelInfoPeriod FuelInfoPeriod { get; set; }

        /// <summary>
        /// Показатель
        /// </summary>
        public virtual string Mark { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public virtual int RowNumber { get; set; }
    }
}