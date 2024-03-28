namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    ///  Конструктивный элемент дома
    /// </summary>
    public class RealityObjectMissingCeo : BaseImportableEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual CommonEstateObject MissingCommonEstateObject { get; set; }
    }
}