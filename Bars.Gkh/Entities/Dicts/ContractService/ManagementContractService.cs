namespace Bars.Gkh.Entities.Dicts.ContractService
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Справочник "Услуги по договорам управления"
    /// </summary>
    public class ManagementContractService : BaseImportableEntity, IHaveExportId
    {
        /// <inheritdoc />
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Вид услуги
        /// </summary>
        public virtual ManagementContractServiceType ServiceType { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
