namespace Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType
{
    using B4.DataAccess;

    /// <summary>
    /// Сущность группа типов домов
    /// </summary>
    public class GisRealEstateTypeGroup : BaseEntity
    {
        /// <summary>
        /// Наименование группы
        /// </summary>
        public virtual string Name { get; set; }
    }
}