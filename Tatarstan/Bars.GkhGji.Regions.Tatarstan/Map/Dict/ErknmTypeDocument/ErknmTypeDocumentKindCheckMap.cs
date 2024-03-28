namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.ErknmTypeDocument
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

    /// <summary>
    /// Маппинг сущности <see cref="ErknmTypeDocumentKindCheck"/>
    /// </summary>
    public class ErknmTypeDocumentKindCheckMap : BaseEntityMap<ErknmTypeDocumentKindCheck>
    {
        public ErknmTypeDocumentKindCheckMap()
            : base(nameof(ErknmTypeDocumentKindCheck), "GJI_DICT_ERKNM_TYPE_DOCUMENT_KIND_CHECK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ErknmTypeDocument, nameof(ErknmTypeDocumentKindCheck.ErknmTypeDocument))
                .Column("TYPE_DOCUMENT_ID").Fetch();
            this.Reference(x => x.KindCheck, nameof(ErknmTypeDocumentKindCheck.KindCheck))
                .Column("KIND_CHECK_ID").Fetch();
        }
    }
}