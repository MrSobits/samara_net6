namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг справочника "Документы подрядных организаций"
    /// </summary>
    public class BuilderDocumentTypeMap : BaseImportableEntityMap<BuilderDocumentType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BuilderDocumentTypeMap() : base("Документы подрядных организаций", "GKH_DICT_BUILDER_DOCUMENT_TYPE")
        {
        }

        /// <summary>
        /// Маппинг свойств
        /// </summary>
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").NotNull();
            Property(x => x.Name, "Наименование типа документа").Column("NAME").Length(250).NotNull();
        }
    }
}
