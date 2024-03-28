namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Показатели качества
    /// </summary>
    public class PublicOrgServiceQualityLevel : BaseImportableEntity
    {
        /// <summary>
        /// Наименование показателя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Установленное значение
        /// </summary>
        public virtual decimal Value { get; set; }

        /// <summary>
        /// Едининца измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Услуга по договору поставщика ресурсов
        /// </summary>
        public virtual PublicServiceOrgContractService ServiceOrg { get; set; }
    }
}
