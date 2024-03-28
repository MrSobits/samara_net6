namespace Bars.Gkh.Gis.Entities.RealEstate
{
    using B4.DataAccess;

    /// <summary>
    /// Сущность индикатора дома
    /// </summary>
    public class RealEstateIndicator : PersistentObject
    {
        /// <summary>
        /// Наименование индикатора
        /// </summary>
        public virtual string Name { get; set; }
    }
}
