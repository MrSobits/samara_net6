namespace Bars.GkhGji.Regions.Tomsk.Entities
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
        /// Час
        /// </summary>
        public virtual int Hour { get; set; }

        /// <summary>
        /// Минута
        /// </summary>
        public virtual int Minute { get; set; }
    }
}