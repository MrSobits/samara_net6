namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FIAS;

    /// <summary>
    /// Учебное заведение
    /// </summary>
    public class Institutions : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Аббревиатура
        /// </summary>
        public virtual string Abbreviation { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual FiasAddress Address { get; set; }
    }
}
