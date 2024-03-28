namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Муниципальное образование размера взноса на кр
    /// </summary>
    public class PaysizeRecord : BaseImportableEntity
    {
        /// <summary>
        /// Размер взноса на кр
        /// </summary>
        public virtual Paysize Paysize { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal? Value { get; set; }
    }
}