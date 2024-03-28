namespace Bars.GkhGji.Regions.Stavropol.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Время для акта проверки
    /// </summary>
    public class ActCheckTime : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Часы
        /// </summary>
        public virtual int Hour { get; set; }

        /// <summary>
        /// Минуты
        /// </summary>
        public virtual int Minute { get; set; }
    }
}
