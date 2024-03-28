namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник ЦТП
    /// </summary>
    public class CentralHeatingStation : BaseImportableEntity
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