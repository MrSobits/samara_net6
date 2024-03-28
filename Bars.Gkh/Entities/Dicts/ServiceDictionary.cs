namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Справочник услуг ГИСа
    /// </summary>
    public class ServiceDictionary : BaseImportableEntity, IHaveExportId
    {
        /// <inheritdoc />
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual string Measure { get; set; }

        /// <summary>
        /// Тип услуги
        /// </summary>
        public virtual TypeServiceGis TypeService { get; set; }

        /// <summary>
        /// Вид коммунального ресурса
        /// </summary>
        public virtual TypeCommunalResource? TypeCommunalResource { get; set; }

        /// <summary>
        /// Услуга предоставляется на общедомовые нужды
        /// </summary>
        public virtual bool IsProvidedForAllHouseNeeds { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}