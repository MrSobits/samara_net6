namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Муниципальное образование типа дома
    /// </summary>
    public class RealEstateTypeMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}