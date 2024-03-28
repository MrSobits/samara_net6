namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тип дома
    /// </summary>
    public class RealEstateType: BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? MarginalRepairCost { get; set; }
    }
}