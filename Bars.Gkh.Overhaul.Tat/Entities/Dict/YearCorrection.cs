namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Ограничение на выбор скорректированного года
    /// </summary>
    public class YearCorrection : BaseEntity
    {
        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }
    }
}