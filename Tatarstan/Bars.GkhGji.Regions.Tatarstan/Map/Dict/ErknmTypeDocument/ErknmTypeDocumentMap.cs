namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.ErknmTypeDocument
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

    /// <summary>
    /// Маппинг сущности <see cref="Dict.ErknmTypeDocument"/>
    /// </summary>
    public class ErknmTypeDocumentMap : BaseEntityMap<ErknmTypeDocument>
    {
        public ErknmTypeDocumentMap()
            : base(nameof(Dict.ErknmTypeDocument), "GJI_DICT_ERKNM_TYPE_DOCUMENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentType, nameof(ErknmTypeDocument.DocumentType)).Column("DOCUMENT_TYPE");
            this.Property(x => x.Code, nameof(ErknmTypeDocument.Code)).Column("CODE");
            this.Property(x => x.IsBasisKnm, nameof(ErknmTypeDocument.IsBasisKnm)).Column("IS_BASIS_KNM").DefaultValue(false).NotNull();
        }
    }
}