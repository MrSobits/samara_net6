namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Адрес ФССП
    /// </summary>
    public class FsspAddress : BaseEntity
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Адрес ПГМУ
        /// </summary>
        public virtual PgmuAddress PgmuAddress { get; set; }
    }
}