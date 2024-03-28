namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Периоды программ капремонта
    /// </summary>
    public class CrPeriod : BaseGkhEntity
    {
        /// <summary>
        /// Начальный год
        /// </summary>
        public virtual int YearStart { get; set; }

        /// <summary>
        /// Конечный год
        /// </summary>
        public virtual int YearEnd { get; set; }
    }
}